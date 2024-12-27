using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Data;
using _Project.Scripts.Field;
using _Project.Scripts.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts
{
    public class AgentModalManager : SingletonMonoBehaviour<AgentModalManager>
    {
        private static readonly string AgentPath = "Agents/ScriptableObjects"; // where all agents are stored

        #region Serialized Fields

        [SerializeField] private GameObject avatarPrefab;
        [SerializeField] private List<AgentSO> agents = new List<AgentSO>();

        [Space]
        [Header("Colors")]
        [SerializeField]
        private Color passiveColor;

        [SerializeField] private Color activeColor;

        #endregion

        private readonly List<Button> agentButtons = new List<Button>();

        private Transform avatarGrid;
        private AgentStruct selectedAvatar;

        #region Event Functions

        protected override void Awake()
        {
            base.Awake();
            // TODO: Shouldn't be fetching by name.
            avatarGrid = transform.Find("AvatarGrid_ScrollRect").GetComponentInChildren<GridLayoutGroup>().transform;
            agents = Resources.LoadAll<AgentSO>(AgentPath).ToList();
            PopulateAgentModal();
        }

        #endregion

        /// <summary>
        /// After loading agents from the Resources folder, we populate the agent modal with the agents.
        /// </summary>
        private void PopulateAgentModal()
        {
            SortAgentsByIndex();
            gameObject.SetActive(true);
            foreach (var agent in agents)
            {
                var button = Instantiate(avatarPrefab, avatarGrid).transform;
                SetInitialButtonAppearance(button, agent);

                button.GetComponent<Button>().onClick.AddListener(() =>
                {
                    // TODO: This could probably be extracted to a method.
                    button.GetComponent<Image>().color = activeColor;
                    ResetButtonAppearance();
                    selectedAvatar = new AgentStruct(button, agent);
                    SelectAgent(agent);
                });
                agentButtons.Add(button.GetComponent<Button>());
            }

            SetFirstButtonAsClear();
        }

        /// <summary>
        ///     Ensures the first button in the Agent selection panel is used for
        ///     clearing any selected partner
        /// </summary>
        private void SetFirstButtonAsClear()
        {
            avatarGrid.GetChild(0).GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                ResetButtonAppearance();
                selectedAvatar = new AgentStruct();
                PartnerManager.Instance.DestroyPartnerOne();
            });
        }

        private static void SetInitialButtonAppearance(Transform avatar, AgentSO agent)
        {
            // TODO: Probably shouldn't be fetching by name.
            avatar.GetChild(0).Find("AvatarImage_Image").GetComponent<Image>().sprite = agent.sprite;
            avatar.GetChild(1).Find("AvatarIndex_Text").GetComponent<TextMeshProUGUI>().text =
                agent.index.ToString();
        }

        /// <summary>
        ///     Called to select an Agent
        /// </summary>
        /// <param name="agent">The agent selected to show in the simulation</param>
        private static void SelectAgent(AgentSO agent)
        {
            Debug.Log($"Selected {agent.name} ({agent.index})");
            EventManager.InvokeAgentSelected(agent);
        }

        private void ResetButtonAppearance()
        {
            if (selectedAvatar.Button == null) return;
            selectedAvatar.Button.GetComponent<Image>().color = passiveColor;
        }

        /// <summary>
        ///    Sorts the agents by their index.
        /// </summary>
        private void SortAgentsByIndex()
        {
            agents.Sort((x, y) => x.index.CompareTo(y.index));
        }

        #region Nested type: ${0}

        /// <summary>
        ///   Struct to hold the agent button and the agent scriptable object.
        /// </summary>
        private struct AgentStruct
        {
            public Transform Button { get; }
            public AgentSO AgentSo { get; }

            public AgentStruct(Transform button, AgentSO agentSo)
            {
                Button = button;
                AgentSo = agentSo;
            }
        }

        public AgentSO GetSelectedAgent()
        {
            return selectedAvatar.AgentSo;
        }

        #endregion
    }
}