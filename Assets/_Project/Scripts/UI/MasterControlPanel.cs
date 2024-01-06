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

        [SerializeField] private Button playMusicButton;
        [SerializeField] private Button resetMusicButton;

        [SerializeField] private Toggle enableRecordingToggle;

        // [SerializeField] private Toggle recordPerUnitToggle;
        [SerializeField] private Toggle enableLoggingToggle;
        [SerializeField] private Toggle muteAgentDrumSoundsToggle;

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
            SetupListeners();
            SetupSavedPanelData();
        }

        private void OnDisable()
        {
            playMusicButton.onClick.RemoveAllListeners();
            resetMusicButton.onClick.RemoveAllListeners();
            applicationQuitButton.onClick.RemoveAllListeners();

            enableRecordingToggle.onValueChanged.RemoveAllListeners();
        }

        #endregion

        private void SetupListeners()
        {
            playMusicButton.onClick.AddListener(MusicSequence.Instance.Play);
            resetMusicButton.onClick.AddListener(MusicSequence.Instance.Reset);
            applicationQuitButton.onClick.AddListener(Quit);

            enableRecordingToggle.onValueChanged.AddListener(value =>
                SaveData.Instance.preferenceData.enableRecording = value);
            muteAgentDrumSoundsToggle.onValueChanged.AddListener(value =>
            {
                SaveData.Instance.preferenceData.muteAgentDrumSounds = value;
                _audio.SetFloat("AgentDrumVolume", value ? -80 : 1);
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
            enableRecordingToggle.isOn = SaveData.Instance.preferenceData.enableRecording;
            enableLoggingToggle.isOn = SaveData.Instance.preferenceData.enableLogging;
            toggleCue.isOn = SaveData.Instance.preferenceData.displayVisualCue;
            muteAgentDrumSoundsToggle.isOn = SaveData.Instance.preferenceData.muteAgentDrumSounds;
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