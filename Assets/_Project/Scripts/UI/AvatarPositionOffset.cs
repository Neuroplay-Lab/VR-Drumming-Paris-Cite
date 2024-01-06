using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    /// <summary>
    ///     Responsible for allowing the drumming partner's position to be altered
    ///     during the experiment through one of the settings panels
    /// </summary>
    public class AvatarPositionOffset : MonoBehaviour
    {

        [SerializeField] private GameObject partnerContainer;  // partner in scene
        [SerializeField] private Vector3 initialAvatarPosition;

        [SerializeField] private Slider leftRightSlider;
        [SerializeField] private Slider frontBackSlider;
        [SerializeField] private TextMeshProUGUI leftRightSliderLabel;
        [SerializeField] private TextMeshProUGUI frontBackSliderLabel;

        /// <summary>
        ///     Sets initial values
        /// </summary>
        private void Awake()
        {
            partnerContainer = GameObject.Find("Partner One");
            initialAvatarPosition = partnerContainer.transform.localPosition;
        }

        /// <summary>
        ///     Resets drumming partner position to the default position
        /// </summary>
        public void Reset()
        {
            leftRightSlider.value = 0;
            frontBackSlider.value = 0;
        }

        /// <summary>
        /// Used to offset the position of the drumming partner
        /// </summary>
        /// <param name="offset">The amount to offset the partner by</param>
        /// <param name="leftRightAxis">Offset in the left/right axis? If false,
        ///     will offset in the front/back axis</param>
        public void OnOffset(float offset, bool leftRightAxis)
        {
            // define values used for slider labels
            string positiveTextValue;
            string negativeTextValue;
            TextMeshProUGUI sliderText;

            // assign values based on left/right or front/back
            if (leftRightAxis)
            {
                positiveTextValue = "R+";
                negativeTextValue = "L+";
                sliderText = leftRightSliderLabel;
            }
            else
            {
                positiveTextValue = "B+";
                negativeTextValue = "F+";
                sliderText = frontBackSliderLabel;
            }

            // set slider text
            if (offset > 0)
            {
                sliderText.SetText(positiveTextValue + offset.ToString("F2"));
            }
            else if (offset < 0)
            {
                sliderText.SetText(negativeTextValue + Mathf.Abs(offset).ToString("F2"));
            }
            else
            {
                sliderText.SetText("0");
            }

            Vector3 newPosition = partnerContainer.transform.localPosition; // get current position

            // apply offset to the appropriate axis
            if (leftRightAxis)
            {
                newPosition.x = initialAvatarPosition.x + offset;

            }
            else
            {
                newPosition.z = initialAvatarPosition.z + offset;
            }

            // update partners position in game
            partnerContainer.transform.localPosition = newPosition;
        }

        /// <summary>
        ///     Used for offsetting the position of the drumming partner to the
        ///     left/right
        /// </summary>
        /// <param name = "offset" > Value by which to offset the position</param>
        public void OnLeftRightOffset(float offset)
        {
            OnOffset(offset, true);
        }

        /// <summary>
        ///     Used for offsetting the position of the drumming partner to the
        ///     front/back
        /// </summary>
        /// <param name = "offset" > Value by which to offset the position</param>
        public void OnFrontBackOffset(float offset)
        {
            OnOffset(offset, false);
        }
    }
}