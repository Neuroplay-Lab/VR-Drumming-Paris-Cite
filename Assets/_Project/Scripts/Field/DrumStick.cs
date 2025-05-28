using DrumRhythmGame.Data;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

namespace DrumRhythmGame.Field
{
    /// <summary>
    ///     Used to provide haptic feedback to the hand controllers when a drum
    ///     hit is made
    /// </summary>
    public class DrumStick : MonoBehaviour
    {

        private enum Hand
        {
            Left,
            Right
        }
        #region Serialized Fields

        public ActorType owner;
        public XRNode node;

        [SerializeField] private Hand hand;

        public SteamVR_Action_Vibration hapticAction;

        #endregion

        #region Event Functions

        /// <summary>
        ///     Handles collisions that should cause haptic feedback for the VR controller.
        ///     Vibrations will only occur when the object collided with is of type
        ///     <c>Instrument</c>
        /// </summary>
        /// <param name="other">The other object involved in the collision</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Instrument>() != null && owner == ActorType.Player) Vibrate();
        }

        #endregion

        /// <summary>
        ///     Sends a haptic vibration to the relevant VR controller
        /// </summary>
        private void Vibrate()
        {
            if (hand == Hand.Left)
            {
                hapticAction.Execute(0, 0.25f, 150, 75, SteamVR_Input_Sources.LeftHand);
            }
            else
            {
                hapticAction.Execute(0, 0.25f, 150, 75, SteamVR_Input_Sources.RightHand);
            }

            // below uses xr method which may not be working

            // var device = InputDevices.GetDeviceAtXRNode(node);
            // if (device.TryGetHapticCapabilities(out var capabilities) && capabilities.supportsImpulse)
            //     device.SendHapticImpulse(0, 1f, 0.5f);
        }
    }
}