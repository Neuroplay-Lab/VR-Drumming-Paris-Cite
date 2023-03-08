/*
UniBpmAnalyzer
Copyright (c) 2016 WestHillApps (Hironari Nishioka)
This software is released under the MIT License.
http://opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace _Project.Scripts.Systems
{
    public class UniBpmAnalyzer
    {
        #region CONST

        // BPM search range
        private const int MinBpm = 60;
        private const int MaxBpm = 400;
        // Base frequency (44.1kbps)
        private const int BaseFrequency = 44100;
        // Base channels (2ch)
        private const int BaseChannels = 2;
        // Base split size of sample data (case of 44.1kbps & 2ch)
        private const int BaseSplitSampleSize = 2205;

        #endregion

        public struct BpmMatchData
        {
            public int bpm;
            public float match;
        }

        private static readonly BpmMatchData[] BpmMatchDatas = new BpmMatchData[MaxBpm - MinBpm + 1];

        /// <summary>
        /// Analyze BPM from an audio clip
        /// </summary>
        /// <param name="clip">target audio clip</param>
        /// <returns>bpm</returns>
        public static int AnalyzeBpm(AudioClip clip)
        {
            for (var i = 0; i < BpmMatchDatas.Length; i++)
            {
                BpmMatchDatas[i].match = 0f;
            }
            if (clip == null)
            {
                return -1;
            }
            var frequency = clip.frequency;

            var channels = clip.channels;

            var splitFrameSize = Mathf.FloorToInt(((float)frequency / (float)BaseFrequency) * ((float)channels / (float)BaseChannels) * (float)BaseSplitSampleSize);

            // Get all sample data from audioclip
            var allSamples = new float[clip.samples * channels];
            clip.GetData(allSamples, 0);

            // Create volume array from all sample data
            var volumeArr = CreateVolumeArray(allSamples, frequency, channels, splitFrameSize);

            // Search bpm from volume array
            var bpm = SearchBpm(volumeArr, frequency, splitFrameSize);

            var strBuilder = new StringBuilder("BPM Match Data List\n");
            for (var i = 0; i < BpmMatchDatas.Length; i++)
            {
                strBuilder.Append("bpm : " + BpmMatchDatas[i].bpm + ", match : " + Mathf.FloorToInt(BpmMatchDatas[i].match * 10000f) + "\n");
            }
            //Debug.Log(strBuilder.ToString());

            return bpm;
        }

        /// <summary>
        /// Create volume array from all sample data
        /// </summary>
        private static float[] CreateVolumeArray(IReadOnlyList<float> allSamples, int frequency, int channels, int splitFrameSize)
        {
            // Initialize volume array
            var volumeArr = new float[Mathf.CeilToInt((float)allSamples.Count / (float)splitFrameSize)];
            var powerIndex = 0;

            // Sample data analysis start
            for (var sampleIndex = 0; sampleIndex < allSamples.Count; sampleIndex += splitFrameSize)
            {
                var sum = 0f;
                for (var frameIndex = sampleIndex; frameIndex < sampleIndex + splitFrameSize; frameIndex++)
                {
                    if (allSamples.Count <= frameIndex)
                    {
                        break;
                    }
                    // Use the absolute value, because left and right value is -1 to 1
                    var absValue = Mathf.Abs(allSamples[frameIndex]);
                    if (absValue > 1f)
                    {
                        continue;
                    }

                    // Calculate the amplitude square sum
                    sum += (absValue * absValue);
                }

                // Set volume value
                volumeArr[powerIndex] = Mathf.Sqrt(sum / splitFrameSize);
                powerIndex++;
            }

            // Representing a volume value from 0 to 1
            var maxVolume = volumeArr.Max();
            for (var i = 0; i < volumeArr.Length; i++)
            {
                volumeArr[i] = volumeArr[i] / maxVolume;
            }

            return volumeArr;
        }

        /// <summary>
        /// Search bpm from volume array
        /// </summary>
        private static int SearchBpm(IReadOnlyList<float> volumeArr, int frequency, int splitFrameSize)
        {
            // Create volume diff list
            var diffList = new List<float>();
            for (var i = 1; i < volumeArr.Count; i++)
            {
                diffList.Add(Mathf.Max(volumeArr[i] - volumeArr[i - 1], 0f));
            }

            // Calculate the degree of coincidence in each BPM
            var index = 0;
            var splitFrequency = (float)frequency / (float)splitFrameSize;
            for (var bpm = MinBpm; bpm <= MaxBpm; bpm++)
            {
                var sinMatch = 0f;
                var cosMatch = 0f;
                var bps = bpm / 60f;

                if (diffList.Count > 0)
                {
                    for (var i = 0; i < diffList.Count; i++)
                    {
                        sinMatch += (diffList[i] * Mathf.Cos(i * 2f * Mathf.PI * bps / splitFrequency));
                        cosMatch += (diffList[i] * Mathf.Sin(i * 2f * Mathf.PI * bps / splitFrequency));
                    }

                    sinMatch *= (1f / (float)diffList.Count);
                    cosMatch *= (1f / (float)diffList.Count);
                }

                var match = Mathf.Sqrt((sinMatch * sinMatch) + (cosMatch * cosMatch));

                BpmMatchDatas[index].bpm = bpm;
                BpmMatchDatas[index].match = match;
                index++;
            }

            // Returns a high degree of coincidence BPM
            var matchIndex = Array.FindIndex(BpmMatchDatas, x => x.match == BpmMatchDatas.Max(y => y.match));

            return BpmMatchDatas[matchIndex].bpm;
        }
    }
}
