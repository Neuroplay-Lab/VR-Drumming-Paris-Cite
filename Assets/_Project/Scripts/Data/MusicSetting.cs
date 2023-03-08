using System;
using UnityEngine;

namespace _Project.Scripts.Data
{
    [Serializable]
    [CreateAssetMenu(fileName = "MusicSettingSample", menuName = "MusicSetting", order = 0)]
    public class MusicSetting : ScriptableObject
    {
        #region Serialized Fields

        [Header("BGM Setting")] public AudioClip bgm;

        public int beatNumber;

        [Header("Music Score Setting")] [Tooltip("CSV file of a music score.")]
        public TextAsset scoreAsset;

        [Tooltip("Count of instruments.")] public int instrumentCount = 4;

        [Tooltip("Initial delay time of the music score.")]
        public float initialDelayTime;

        [Tooltip(
            "How many seconds before the timing of the strike the note appears. If this time is shortened, the notes move faster.")]
        public float noteMarginTime;

        [Range(0.999f, 1.001f)] public float speedMagnification = 1f;

        [Tooltip("Whether to loop when the score reaches the end.")]
        public bool loopScore = true;

        [Header("Extras")] public int customBpm;

        #endregion
    }
}