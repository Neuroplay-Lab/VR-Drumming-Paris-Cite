using System.Collections.Generic;
using _Project.Scripts.Data;
using _Project.Scripts.Systems;
using _Project.Scripts;
using DrumRhythmGame.Data;
using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.Field
{
    /// <summary>
    /// Handles the selection of different drumming partners, and also changing
    /// the behaviour of the selected partner.
    /// </summary>
    public class PartnerManager : SingletonMonoBehaviour<PartnerManager>
    {
        private static readonly string Prefix = "[<b>PartnerManager</b>]";

        #region Serialized Fields

        [SerializeField] private List<GameObject> drummingAvatars;

        [SerializeField] private Transform instantiationPositionPartnerOne;

        #endregion

        private GameObject _currentPartnerOne;

        public PartnerBehaviourType CurrentBehaviourPartnerOne { get; private set; } = PartnerBehaviourType.None;

        private PartnerHandPreference partnerHandPreference = PartnerHandPreference.Both;

        #region Event Functions

        private void OnEnable()
        {
            EventManager.AgentSelected += InstantiateAvatar;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            var position = transform.position;
            Handles.DrawDottedLine(position, instantiationPositionPartnerOne.position, 4f);
        }
#endif

        #endregion

        /// <summary>
        ///     Instantiates the prefab of the selected agent.
        /// </summary>
        /// <param name="agent"></param>
        private void InstantiateAvatar(AgentSO agent)
        {

            GameObject agentPrefab;

            switch (partnerHandPreference)
            {
                case PartnerHandPreference.Left:
                    agentPrefab = agent.leftHandedVariant;
                    break;
                case PartnerHandPreference.Right:
                    agentPrefab = agent.rightHandedVariant;
                    break;
                default:
                    agentPrefab = agent.prefab;
                    break;
            }

            // If we already have a partner, and its the same one, we double clicked the same agent so remove it
            if (_currentPartnerOne != null && _currentPartnerOne == agentPrefab)
            {
                Destroy(_currentPartnerOne);
                return;
            }

            // If we selected a different agent, destroy the old one and instantiate the new one
            if (_currentPartnerOne != null) Destroy(_currentPartnerOne);
            SaveData.Instance.avatarData.partnerOneAvatarIndex = agent.index;
            _currentPartnerOne = Instantiate(agent.prefab, instantiationPositionPartnerOne);
            _currentPartnerOne.SetActive(CurrentBehaviourPartnerOne != PartnerBehaviourType.None);
            Debug.Log($"{Prefix} Instantiated agent <color=green>{agent.index}</color>");
        }

        private void SelectAvatar(int skinIndex, int partnerIndex)
        {
            if (skinIndex < 0) skinIndex = 0;

            if (skinIndex > drummingAvatars.Count && partnerIndex == 0)
                skinIndex = 0;

            // If we already have a partner
            if (_currentPartnerOne != null)
                Destroy(_currentPartnerOne);

            // SET SKIN INDEX
            SaveData.Instance.avatarData.partnerOneAvatarIndex = skinIndex;

            // INSTANTIATE THE MODEL
            _currentPartnerOne = Instantiate(drummingAvatars[skinIndex], instantiationPositionPartnerOne);

            // GET PARTNER SCRIPT ON OBJECT
            //var partnerBehaviour = CurrentPartnerOne.GetComponentInChildren<Partner>
            // SET PARTNER AS ACTIVE
            _currentPartnerOne.SetActive(CurrentBehaviourPartnerOne != PartnerBehaviourType.None);

            Debug.Log($"{Prefix} Updated the avatar of agent: <color=green>{partnerIndex}</color>");
        }

        public void DestroyPartnerOne()
        {
            if (_currentPartnerOne == null) return;
            Destroy(_currentPartnerOne);
            _currentPartnerOne = null;
        }

        public void SelectNextAvatar(int partnerIndex)
        {
            var skinIndex = SaveData.Instance.avatarData.partnerOneAvatarIndex;

            SelectAvatar((skinIndex + 1) % drummingAvatars.Count, partnerIndex);
        }

        public void SelectPreviousAvatar(int partnerIndex)
        {
            var skinIndex = SaveData.Instance.avatarData.partnerOneAvatarIndex;
            SelectAvatar((drummingAvatars.Count + skinIndex - 1) % drummingAvatars.Count, partnerIndex);
        }

        public void SwitchBehaviour(PartnerBehaviourType type)
        {
            Debug.Log($"[PartnerManager] {CurrentBehaviourPartnerOne} => {type}");

            CurrentBehaviourPartnerOne = type;
            if (_currentPartnerOne != null)
            {
                _currentPartnerOne.SetActive(CurrentBehaviourPartnerOne != PartnerBehaviourType.None);
                _currentPartnerOne.GetComponentInChildren<Partner.Partner>().SwitchType(type);
            }
        }

        public void SwitchHandPreference(PartnerHandPreference preference)
        {
            partnerHandPreference = preference;
            Debug.Log($"[PartnerManager] Switched hand preference to: {preference}");

            InstantiateAvatar();
        }
    }
}