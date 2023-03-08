using _Project.Scripts.Systems;
using DrumRhythmGame.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class PlayerSettingPanel : MonoBehaviour
    {
        [SerializeField] private Toggle visibleCueToggle;
        [SerializeField] private GameObject cueBoard;

        private void Awake()
        {
            // Initialize value and register event handler
            visibleCueToggle.onValueChanged.AddListener((value) =>
            {
                SaveData.Instance.preferenceData.displayVisualCue = value;
                cueBoard.SetActive(value);
            });
            visibleCueToggle.isOn = SaveData.Instance.preferenceData.displayVisualCue;
        }

        private void OnDestroy()
        {
            visibleCueToggle.onValueChanged.RemoveAllListeners();
        }
    }
}