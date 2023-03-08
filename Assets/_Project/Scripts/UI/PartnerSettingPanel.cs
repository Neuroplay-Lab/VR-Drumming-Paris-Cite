using System.Collections.Generic;
using _Project.Scripts.Field;
using DrumRhythmGame.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Scripts.UI
{
    public class PartnerSettingPanel : MonoBehaviour
    {
        [SerializeField] private int partnerIndex;
        [SerializeField] private Dropdown partnerBehaviourDropdown;
        [SerializeField] private PartnerBehaviourType defaultBehaviourType = PartnerBehaviourType.None;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button previousButton;

        private void Awake()
        {
            nextButton?.onClick.AddListener(() =>
            {
                PartnerManager.Instance.SelectNextAvatar(partnerIndex);
            });
            previousButton?.onClick.AddListener(() =>
            {
                PartnerManager.Instance.SelectPreviousAvatar(partnerIndex);
            });

            var labels = new List<string> { "No partner", "Leader - Follower", "Joint goal" };
            
            if(partnerBehaviourDropdown == null) return;
            partnerBehaviourDropdown.AddOptions(labels);
            partnerBehaviourDropdown.onValueChanged.AddListener(OnDropdownChanged);

            OnDropdownChanged((int)defaultBehaviourType);
            partnerBehaviourDropdown.SetValueWithoutNotify((int)defaultBehaviourType);
        }

        private void OnDestroy()
        {
            nextButton?.onClick.RemoveAllListeners();
            previousButton?.onClick.RemoveAllListeners();
            partnerBehaviourDropdown?.onValueChanged.RemoveAllListeners();
        }
        
        private void OnDropdownChanged(int value)
        {
            switch (value)
            {
                case 0:
                    PartnerManager.Instance.SwitchBehaviour(PartnerBehaviourType.None);
                    break;
                case 1:
                    PartnerManager.Instance.SwitchBehaviour(PartnerBehaviourType.Follow);
                    break;
                case 2:
                    PartnerManager.Instance.SwitchBehaviour(PartnerBehaviourType.Rhythm);
                    break;
            }
        }
    }
}