using System;
using UnityEngine;

namespace _Project.Scripts.Data
{
    /// <summary>
    /// Represents a track that can be played in the experiment with relevant
    /// data, including:.
    /// <list type="bullet">BPM</list>
    /// <list type="bullet">Score</list>
    /// <list type="bullet">Initial delay of drumming rhythm</list>
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "Playlist", menuName = "Playlist", order = 1)]
    public class Playlist : ScriptableObject
    {
        public PlaylistItem[] playlistItems;
    }


    [Serializable]
    public struct PlaylistItem
    {
        public MusicSetting track;
        public float duration;
    }
}