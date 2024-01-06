using System;
using System.Collections.Generic;
using UnityEngine;

namespace DrumRhythmGame.Data
{
    [Serializable]
    public class PartnerErrorData
    {
        [Serializable]
        public class Data
        {
            public float missHitFrequencyRate = 0f;
            public float latencyFrequencyRate = 0f;
            public float maxLatencyTime = 0f;
        }
        
        [SerializeField] private int presetLength = 5;
        [SerializeField] private int index = 0;
        [SerializeField] private List<Data> presets;
        
        public int PresetLength => presetLength;
        public int CurrentIndex => index;
        public Data Current => presets[index];

        public PartnerErrorData()
        {
            presets = new List<Data>();
            for (var i = 0; i < presetLength; i++)
            {
                presets.Add(new Data());
            }
        }

        public void UpdateCurrentIndex(int targetIndex)
        {
            if (targetIndex < 0 || presetLength <= targetIndex)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (presets.Count != presetLength)
            {
                var previous = presets.ToArray();
                presets = new List<Data>();
                for (var i = 0; i < presetLength; i++)
                {
                    presets.Add(i < previous.Length ? previous[i] : new Data());
                }
            }

            index = targetIndex;
        }
    }
}