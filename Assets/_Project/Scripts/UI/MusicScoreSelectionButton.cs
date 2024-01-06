using _Project.Scripts.Data;
using _Project.Scripts.Systems;
using UnityEngine;

namespace _Project.Scripts.UI
{
    /// <summary>
    /// Attached to the music score selection button to invoke a track change
    /// event when pressed.
    /// </summary>
    public class MusicScoreSelectionButton : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private string title;
        [SerializeField] private string artist;
        [Space] [SerializeField] private MusicSetting musicSetting;

        #endregion

        public void MusicSequenceSelected()
        {
            EventManager.InvokeMusicSettingChangeEvent(musicSetting);
            //MusicSequence.Instance.GetComponent<AudioSource>().clip = musicSetting.bgm;
            Debug.Log($"Selected: {title} by {artist}");
        }
        // TODO: Convert this to use a scriptable object instead, so its easier to create new music scores.
    }
}