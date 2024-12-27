using System.Collections.Generic;
using _Project.Scripts.Field;
using DrumRhythmGame.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Scripts.UI
{
    /// <summary>
    /// Handles the different options available for the drumming partner agent.
    /// </summary>
    public class PartnerHandPreferenceController : MonoBehaviour
    {
        [SerializeField] private Dropdown partnerHandednessDropdown;

        [SerializeField] private List<string> labels;

        private void Awake()
        {
            if (partnerHandednessDropdown == null) return;
            partnerHandednessDropdown.AddOptions(labels);
            partnerHandednessDropdown.onValueChanged.AddListener(OnDropdownChanged);

            OnDropdownChanged(0);
            partnerHandednessDropdown.SetValueWithoutNotify(0);
        }

        private void OnDestroy()
        {
            partnerHandednessDropdown?.onValueChanged.RemoveAllListeners();
        }

        private void OnDropdownChanged(int value)
        {
            switch (value)
            {
                case 0:
                    Debug.Log("Handedness: Both");
                    break;
                case 1:
                    Debug.Log("Handedness: Left");
                    break;
                case 2:
                    Debug.Log("Handedness: Right");
                    break;
            }
        }
    }
}