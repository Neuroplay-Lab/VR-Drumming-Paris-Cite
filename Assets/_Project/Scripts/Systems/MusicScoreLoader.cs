using System;
using DrumRhythmGame.Data;
using UnityEngine;

namespace _Project.Scripts.Systems
{
    public class MusicScoreLoader
    {
        public static MusicScore Load(string csvText, int columnCount)
        {
            var musicScore = new MusicScore();
            
            var lines = csvText.Split('\n');
            if (int.TryParse(lines[1], out var result))
            {
                musicScore.beatCount = result;
            }
            else
            {
                throw new Exception("[MusicScoreLoader] Format of the music score is invalid. " +
                                    "You have to write beat of the music at second line.");
            }

            musicScore.rowCount = lines.Length - 2;
            musicScore.columnCount = columnCount;
            musicScore.score = new bool[musicScore.rowCount, columnCount];
            
            for (var i = 0; i < musicScore.rowCount; i++)
            {
                // First two lines are reserved for the header and the instrument count.
                var elements = lines[i+2].Split(',');

                for (var j = 0; j < columnCount; j++)
                {
                    musicScore.score[i, j] = elements[j].Contains("1");
                }
            }

            return musicScore;
        }

        void Start()
        {

        }

        private void OnDisable()
        {
            EventManager.MusicResetEvent -= PatternReset;
        }

        private void PatternReset()
        {

        }


        public static MusicScore Load(TextAsset csvTextAsset, int columnCount)
        {
            return Load(csvTextAsset.text, columnCount);
        }
    }
}
