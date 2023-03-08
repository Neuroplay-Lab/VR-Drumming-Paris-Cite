using DrumRhythmGame.Data;
using UnityEngine;
using UnityEngine.XR;

namespace DrumRhythmGame.Field
{
    /// <summary>
    /// </summary>
    public class DrumStick : MonoBehaviour
    {
        #region Serialized Fields

        public ActorType owner;
        public XRNode node;

        #endregion

        #region Event Functions

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Instrument>() != null && owner == ActorType.Player) Vibrate();
        }

        #endregion

        private void Vibrate()
        {
            var device = InputDevices.GetDeviceAtXRNode(node);
            if (device.TryGetHapticCapabilities(out var capabilities) && capabilities.supportsImpulse)
                device.SendHapticImpulse(0, 1f, 0.5f);
        }
    }
}