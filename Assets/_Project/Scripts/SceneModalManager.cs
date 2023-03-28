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
    public class SceneModalManager : SingletonMonoBehaviour<SceneModalManager>
    {
        private static readonly string ScenePath = "Scenes/ScriptableObjects";

        #region Serialized Fields

        [SerializeField] private GameObject scenePrefab;
        [SerializeField] private List<SceneSO> scenes = new List<SceneSO>();

        [Space] [Header("Colors")] [SerializeField]
        private Color passiveColor;

        [SerializeField] private Color activeColor;

        #endregion

        private readonly List<Button> sceneButtons = new List<Button>();
        
        private Transform sceneGrid;
        private SceneStruct selectedScene;

        #region Event Functions

        protected override void Awake()
        {
            base.Awake();
            // TODO: Shouldn't be fetching by name.
            sceneGrid = transform.Find("SceneGrid_ScrollRect").GetComponentInChildren<GridLayoutGroup>().transform;
            scenes = Resources.LoadAll<SceneSO>(ScenePath).ToList();
            PopulateSceneModal();
        }

        #endregion

        /// <summary>
        /// After loading scenes from the Resources folder, we populate the scene modal with the scenes.
        /// </summary>
        private void PopulateSceneModal()
        {
            SortScenesByIndex();
            gameObject.SetActive(true);
            foreach (var scene in scenes)
            {
                var button = Instantiate(scenePrefab, sceneGrid).transform;
                SetInitialButtonAppearance(button, scene);

                button.GetComponent<Button>().onClick.AddListener(() =>
                {
                    // TODO: This could probably be extracted to a method.
                    button.GetComponent<Image>().color = activeColor;
                    ResetButtonAppearance();
                    selectedScene = new SceneStruct(button, scene);
                    SelectScene(scene);
                });
                sceneButtons.Add(button.GetComponent<Button>());
            }

            SetFirstButtonAsClear();
        }

        private void SetFirstButtonAsClear()
        {
            sceneGrid.GetChild(0).GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                ResetButtonAppearance();
                selectedScene = new SceneStruct();
                SceneryManager.Instance.DestroyScene();
                PartnerManager.Instance.DestroyPartnerOne();
            });
        }

        private static void SetInitialButtonAppearance(Transform avatar, SceneSO agent)
        {
            // TODO: Probably shouldn't be fetching by name.
            avatar.GetChild(0).Find("AvatarImage_Image").GetComponent<Image>().sprite = agent.sprite;
            avatar.GetChild(1).Find("AvatarIndex_Text").GetComponent<TextMeshProUGUI>().text =
                agent.index.ToString();
        }

        private static void SelectScene(SceneSO agent)
        {
            Debug.Log($"Selected {agent.name} ({agent.index})");
            EventManager.InvokeSceneSelected(agent);
        }

        private void ResetButtonAppearance()
        {
            if (selectedScene.Button == null) return;
            selectedScene.Button.GetComponent<Image>().color = passiveColor;
        }

        /// <summary>
        ///    Sorts the agents by their index.
        /// </summary>
        private void SortScenesByIndex()
        {
            scenes.Sort((x, y) => x.index.CompareTo(y.index));
        }

        #region Nested type: ${0}

        /// <summary>
        ///   Struct to hold the agent button and the agent scriptable object.
        /// </summary>
        private struct SceneStruct
        {
            public Transform Button { get; }
            private SceneSO SceneSo { get; }

            public SceneStruct(Transform button, SceneSO sceneSo)
            {
                Button = button;
                SceneSo = sceneSo;
            }
        }

        #endregion
    }
}