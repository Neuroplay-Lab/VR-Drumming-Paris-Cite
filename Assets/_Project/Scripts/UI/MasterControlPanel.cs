using _Project.Scripts.Field;
using _Project.Scripts.Systems;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    // Invoke Awake() after the end of PartnerBehaviour settings
    [DefaultExecutionOrder(2)]
    public class MasterControlPanel : MonoBehaviour
    {
        #region Serialized Fields

        public static MasterControlPanel Instance { get; private set; }

        [SerializeField] private Button playMusicButton;
        [SerializeField] private Button resetMusicButton;

        [SerializeField] private Toggle allowParticipantStart;
        [SerializeField] private Toggle enableRecordingToggle;

        // [SerializeField] private Toggle recordPerUnitToggle;
        [SerializeField] private Toggle enableLoggingToggle;
        [SerializeField] private Toggle muteAgentDrumSoundsToggle;
        [SerializeField] private Toggle muteParticipantDrumSoundsToggle;
        [SerializeField] private Toggle muteMusicToggle;
        [SerializeField] private Toggle hideDrumsToggle;

        // [SerializeField] private Slider volumeSlider;
        [SerializeField] private Toggle toggleCue;
        // [SerializeField] private InputField promptEntryTime;

        [SerializeField] private Button applicationQuitButton;

        [SerializeField] private AudioMixer _audio;

        #endregion

        private bool _isPlaying;
        private Coroutine _timerCoroutine;

        #region Event Functions

        private void OnEnable()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                SetupListeners();
                SetupSavedPanelData();
            }
        }

        private void OnDisable()
        {
            playMusicButton.onClick.RemoveAllListeners();
            resetMusicButton.onClick.RemoveAllListeners();
            applicationQuitButton.onClick.RemoveAllListeners();

            enableRecordingToggle.onValueChanged.RemoveAllListeners();
        }

        #endregion

        private void onStartClick()
        {
            if (GameData.Instance.currentPlayType == PlayType.SingleTrack)
            {
                MusicSequence.Instance.Play();
            }
            else
            {
                // link to playlist
                PlaylistController.Instance.Play();
            }
        }

        private void onResetClick()
        {
            if (GameData.Instance.currentPlayType == PlayType.SingleTrack)
            {
                MusicSequence.Instance.Reset();
            }
            else
            {
                // stop playlist
                PlaylistController.Instance.Reset();
            }
        }

        private void SetupListeners()
        {
            playMusicButton.onClick.AddListener(onStartClick);
            resetMusicButton.onClick.AddListener(onResetClick);
            applicationQuitButton.onClick.AddListener(Quit);

            allowParticipantStart.onValueChanged.AddListener(value =>
            {
                SaveData.Instance.preferenceData.allowParticipantStart = value;
                MusicSequence.Instance.SetParticipantCanStart(value);
            });
            enableRecordingToggle.onValueChanged.AddListener(value =>
                SaveData.Instance.preferenceData.enableRecording = value);
            muteAgentDrumSoundsToggle.onValueChanged.AddListener(value =>
            {
                SaveData.Instance.preferenceData.muteAgentDrumSounds = value;
                _audio.SetFloat("Partner Drums Volume", value ? -80 : 0);
            });
            muteParticipantDrumSoundsToggle.onValueChanged.AddListener(value =>
            {
                SaveData.Instance.preferenceData.muteParticipcantDrumSounds = value;
                _audio.SetFloat("Player Drums Volume", value ? -80 : 0);
            });
            muteMusicToggle.onValueChanged.AddListener(value =>
            {
                SaveData.Instance.preferenceData.muteMusicSounds = value;
                _audio.SetFloat("Music Volume", value ? -80 : 0);
            });
            hideDrumsToggle.onValueChanged.AddListener(value =>
            {
                SaveData.Instance.preferenceData.hideDrums = value;
                DrumManager.Instance.HideDrums(value);
            });

            toggleCue.onValueChanged.AddListener(value =>
            {
                EventManager.InvokeCueStateChanged(value);
                SaveData.Instance.preferenceData.displayVisualCue = value;
            });
            enableLoggingToggle.onValueChanged.AddListener(value =>
            {
                SaveData.Instance.preferenceData.enableLogging = value;
                EventManager.InvokeLoggingStateChanged(value);
            });
        }

        /// <summary>
        ///     Sets the UI elements to the saved values
        /// </summary>
        private void SetupSavedPanelData()
        {
            allowParticipantStart.isOn = SaveData.Instance.preferenceData.allowParticipantStart;
            enableRecordingToggle.isOn = SaveData.Instance.preferenceData.enableRecording;
            enableLoggingToggle.isOn = SaveData.Instance.preferenceData.enableLogging;
            toggleCue.isOn = SaveData.Instance.preferenceData.displayVisualCue;
            muteAgentDrumSoundsToggle.isOn = SaveData.Instance.preferenceData.muteAgentDrumSounds;
            muteMusicToggle.isOn = SaveData.Instance.preferenceData.muteMusicSounds;
            hideDrumsToggle.isOn = SaveData.Instance.preferenceData.hideDrums;
            muteParticipantDrumSoundsToggle.isOn = SaveData.Instance.preferenceData.muteParticipcantDrumSounds;
        }

        private static void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
            UnityEngine.Application.Quit();
#endif
        }
    }
}