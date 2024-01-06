using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    /// <summary>
    /// Allows for the repositioning of the prompt panel during run time.
    /// </summary>
    public class PromptPositionSettingPanel : MonoBehaviour
    {
        #region Serialized Fields

        // position of prompt panel
        [SerializeField] private Transform promptPanelTransform;

        // UI Sliders
        [Space] [SerializeField] private Slider yPositionSlider;
        [SerializeField] private TextMeshProUGUI yPositionText;

        #endregion

        private float _startingYPosition;

        #region Event Functions

        protected void Awake()
        {
            // get initial y position of prompt panel
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