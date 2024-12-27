using System.Collections.Generic;
using _Project.Scripts.Data;
using _Project.Scripts.Systems;
using DrumRhythmGame.Data;
using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.Field
{
    /// <summary>
    /// Handles the selection of different drum kits, and also changing
    /// the hand of the selected partner.
    /// </summary>
    public class DrumManager : SingletonMonoBehaviour<DrumManager>
    {
        private static readonly string Prefix = "[<b>DrumManager</b>]";

        #region Serialized Fields

        [SerializeField] private List<GameObject> drummingKits;

        [SerializeField] private Transform instantiationPosition;

        #endregion

        private GameObject _currentDrum;

        // public PartnerBehaviourType CurrentBehaviourPartnerOne { get; private set; } = PartnerBehaviourType.None;

        #region Event Functions

        private void OnEnable()
        {
            _currentDrum = drummingKits[0];
            EventManager.DrumSelected += InstantiateDrum;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            var position = transform.position;
            Handles.DrawDottedLine(position, instantiationPosition.position, 4f);
        }
#endif

        #endregion

        /// <summary>
        ///     Instantiates the prefab of the selected drum.
        /// </summary>
        /// <param name="drum"></param>
        private void InstantiateDrum(DrumSO drum)
        {
            // If we already have a partner, and its the same one, we double clicked the same agent so remove it
            if (_currentDrum != null && _currentDrum == drum.prefab)
            {
                Destroy(_currentDrum);
                return;
            }

            // If we selected a different agent, destroy the old one and instantiate the new one
            if (_currentDrum != null) Destroy(_currentDrum);
            // SaveData.Instance.avatarData.partnerOneAvatarIndex = drum.index;
            _currentDrum = Instantiate(drum.prefab, instantiationPosition);
            // _currentDrum.SetActive(CurrentBehaviourPartnerOne != PartnerBehaviourType.None);
            Debug.Log($"{Prefix} Instantiated drum <color=green>{drum.index}</color>");
        }

        private void SelectAvatar(int drumIndex, int altDrumIndex)
        {
            if (drumIndex < 0) drumIndex = 0;

            if (drumIndex > drummingKits.Count && altDrumIndex == 0)
                drumIndex = 0;

            // If we already have a partner
            if (_currentDrum != null)
                Destroy(_currentDrum);

            // SET SKIN INDEX
            SaveData.Instance.avatarData.partnerOneAvatarIndex = drumIndex;

            // INSTANTIATE THE MODEL
            _currentDrum = Instantiate(drummingKits[drumIndex], instantiationPosition);

            // GET PARTNER SCRIPT ON OBJECT
            //var partnerBehaviour = CurrentPartnerOne.GetComponentInChildren<Partner>
            // SET PARTNER AS ACTIVE
            // _currentDrum.SetActive(CurrentBehaviourPartnerOne != PartnerBehaviourType.None);

            Debug.Log($"{Prefix} Updated the drum: <color=green>{altDrumIndex}</color>");
        }

        public void DestroyDrum()
        {
            if (_currentDrum == null) return;
            Destroy(_currentDrum);
            _currentDrum = null;
        }

        // public void SelectNextDrum(int drumIndex)
        // {
        //     var skinIndex = SaveData.Instance.avatarData.partnerOneAvatarIndex;

        //     SelectAvatar((skinIndex + 1) % drummingKits.Count, partnerIndex);
        // }

        // public void SelectPreviousAvatar(int partnerIndex)
        // {
        //     var skinIndex = SaveData.Instance.avatarData.partnerOneAvatarIndex;
        //     SelectAvatar((drummingAvatars.Count + skinIndex - 1) % drummingAvatars.Count, partnerIndex);
        // }

        // public void SwitchBehaviour(PartnerBehaviourType type)
        // {
        //     Debug.Log($"[PartnerManager] {CurrentBehaviourPartnerOne} => {type}");

        //     CurrentBehaviourPartnerOne = type;
        //     if (_currentPartnerOne != null)
        //     {
        //         _currentPartnerOne.SetActive(CurrentBehaviourPartnerOne != PartnerBehaviourType.None);
        //         _currentPartnerOne.GetComponentInChildren<Partner.Partner>().SwitchType(type);
        //     }
        // }
    }
}