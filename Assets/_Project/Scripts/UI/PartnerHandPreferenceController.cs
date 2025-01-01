using System.Collections.Generic;
using _Project.Scripts.Field;
using _Project.Scripts.Field.Partner;
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
                    PartnerManager.Instance.SwitchHandPreference(PartnerHandPreference.Both);
                    break;
                case 1:
                    PartnerManager.Instance.SwitchHandPreference(PartnerHandPreference.Left);
                    break;
                case 2:
                    PartnerManager.Instance.SwitchHandPreference(PartnerHandPreference.Right);
                    break;
            }
        }
    }
}