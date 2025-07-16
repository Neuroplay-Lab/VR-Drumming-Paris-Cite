using _Project.Scripts.Data;
using _Project.Scripts.Systems;
using UnityEngine;

namespace _Project.Scripts.UI
{
    /// <summary>
    /// Attached to the music score selection button to invoke a track change
    /// event when pressed.
    /// </summary>
    public class PlaylistSelectionButton : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private string title;
        [SerializeField] private string artist;
        [Space][SerializeField] private Playlist playlist;

        #endregion

        public void PlaylistSelected()
        {
            GameData.Instance.SetUseMusicTrack(false);
            PlaylistController.Instance.SetCurrentPlaylist(playlist);
        }
    }
}