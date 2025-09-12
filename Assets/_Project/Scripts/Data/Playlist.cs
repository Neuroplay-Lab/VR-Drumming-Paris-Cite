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
        public Playlist(PlaylistItem[] playlistItems)
        {
            this.playlistItems = playlistItems;
        }
        public PlaylistItem[] playlistItems;

        public static Playlist CreatePlaylist(PlaylistItem[] playlistItems)
        {
            Playlist obj = CreateInstance<Playlist>();
            obj.playlistItems = playlistItems;
            return obj;
        }
    }


    [Serializable]
    public struct PlaylistItem
    {
        public PlaylistItem(MusicSetting musicSetting, float duration, bool hidePartner)
        {
            track = musicSetting;
            this.duration = duration;
            this.hidePartner = hidePartner;
        }
        public MusicSetting track;
        public float duration;
        public bool hidePartner;
    }
}