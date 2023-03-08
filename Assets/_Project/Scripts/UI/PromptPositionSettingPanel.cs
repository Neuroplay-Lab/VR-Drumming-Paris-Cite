using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class PromptPositionSettingPanel : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private Transform promptPanelTransform;

        [Space] [SerializeField] private Slider yPositionSlider;

        [SerializeField] private TextMeshProUGUI yPositionText;

        #endregion

        private bool _isSliderBeingDragged;
        private float _startingYPosition;

        #region Event Functions

        protected void Awake()
        {
            _startingYPosition = promptPanelTransform.position.y;
        }

        /// <summary>
        ///     Resets the slider value and the position of the prompt to their default values.
        /// </summary>
        public void Reset()
        {
            // TODO: make this a static class and a static function so we can call it as Instance.Reset()
            yPositionSlider.value = 0;
            var position = promptPanelTransform.position;
            position = new Vector3(position.x, _startingYPosition,
                position.z);
            promptPanelTransform.position = position;
        }

        private void OnEnable()
        {
            yPositionSlider.onValueChanged.AddListener(OnYPositionSliderValueChanged);
        }

        private void OnDisable()
        {
            yPositionSlider.onValueChanged.RemoveListener(OnYPositionSliderValueChanged);
        }

        #endregion

        /// <summary>
        /// Updates the position of the prompt panel located at the back-wall of the stage.
        /// </summary>
        /// <param name="value">A float to offset the starting position by</param>
        private void OnYPositionSliderValueChanged(float value)
        {
            yPositionText.text = value.ToString("F2");
            var position = promptPanelTransform.position;
            var newPosition = new Vector3(position.x, _startingYPosition + value, position.z);
            promptPanelTransform.position = newPosition;
        }
    }
}