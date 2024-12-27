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
    public class DrumModalManager : SingletonMonoBehaviour<DrumModalManager>
    {
        private static readonly string DrumPath = "Drums/ScriptableObjects"; // where all drums are stored

        #region Serialized Fields

        [SerializeField] private GameObject drumPrefab;
        [SerializeField] private List<DrumSO> drums = new List<DrumSO>();

        [Space]
        [Header("Colors")]
        [SerializeField]
        private Color passiveColor;

        [SerializeField] private Color activeColor;

        #endregion

        private readonly List<Button> drumButtons = new List<Button>();

        private Transform drumGrid;
        private DrumStruct selectedDrum;

        #region Event Functions

        protected override void Awake()
        {
            base.Awake();
            // TODO: Shouldn't be fetching by name.
            drumGrid = transform.Find("AvatarGrid_ScrollRect").GetComponentInChildren<GridLayoutGroup>().transform;
            drums = Resources.LoadAll<DrumSO>(DrumPath).ToList();
            PopulateDrumModal();
        }

        #endregion

        /// <summary>
        /// After loading drums from the Resources folder, we populate the drum modal with the drums.
        /// </summary>
        private void PopulateDrumModal()
        {
            SortDrumsByIndex();
            gameObject.SetActive(true);
            foreach (var drum in drums)
            {
                var button = Instantiate(drumPrefab, drumGrid).transform;
                SetInitialButtonAppearance(button, drum);

                button.GetComponent<Button>().onClick.AddListener(() =>
                {
                    // TODO: This could probably be extracted to a method.
                    button.GetComponent<Image>().color = activeColor;
                    ResetButtonAppearance();
                    selectedDrum = new DrumStruct(button, drum);
                    SelectDrum(drum);
                });
                drumButtons.Add(button.GetComponent<Button>());
            }

            SetFirstButtonAsClear();
        }

        /// <summary>
        ///     Ensures the first button in the Drum selection panel is used for
        ///     clearing any selected partner
        /// </summary>
        private void SetFirstButtonAsClear()
        {
            drumGrid.GetChild(0).GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                ResetButtonAppearance();
                selectedDrum = new DrumStruct();
                DrumManager.Instance.DestroyDrum();
            });
        }

        private static void SetInitialButtonAppearance(Transform drumAvatar, DrumSO drum)
        {
            // TODO: Probably shouldn't be fetching by name.
            drumAvatar.GetChild(0).Find("AvatarImage_Image").GetComponent<Image>().sprite = drum.sprite;
            drumAvatar.GetChild(0).Find("AvatarImage_Image").transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            drumAvatar.GetChild(0).Find("AvatarImage_Image").transform.SetLocalPositionAndRotation(Vector3.zero,
                Quaternion.identity);
            drumAvatar.GetChild(1).Find("AvatarIndex_Text").GetComponent<TextMeshProUGUI>().text =
                drum.drumCount.ToString();
        }

        /// <summary>
        ///     Called to select an Drum
        /// </summary>
        /// <param name="drum">The drum selected to show in the simulation</param>
        private static void SelectDrum(DrumSO drum)
        {
            Debug.Log($"Selected {drum.name} ({drum.index})");
            EventManager.InvokeDrumSelected(drum);
        }

        private void ResetButtonAppearance()
        {
            if (selectedDrum.Button == null) return;
            selectedDrum.Button.GetComponent<Image>().color = passiveColor;
        }

        /// <summary>
        ///    Sorts the drums by their index.
        /// </summary>
        private void SortDrumsByIndex()
        {
            drums.Sort((x, y) => x.index.CompareTo(y.index));
        }

        #region Nested type: ${0}

        /// <summary>
        ///   Struct to hold the drum button and the drum scriptable object.
        /// </summary>
        private struct DrumStruct
        {
            public Transform Button { get; }
            private DrumSO DrumSo { get; }

            public DrumStruct(Transform button, DrumSO drumSo)
            {
                Button = button;
                DrumSo = drumSo;
            }
        }

        #endregion
    }
}