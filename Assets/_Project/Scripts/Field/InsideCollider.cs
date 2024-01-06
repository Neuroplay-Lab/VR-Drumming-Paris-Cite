using UnityEngine;


namespace DrumRhythmGame.Field
{
    /// <summary>
    /// Checks whether the drumstick is already inside of the drum collider.
    /// Helps to prevent double hits.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class InsideCollider : MonoBehaviour 
    {
   
        private bool insideCollider = false;

        private void Awake()
        {
            var capsuleCollider = GetComponent<CapsuleCollider>();
        }

        /// <summary>
        /// Used to check if drumstick is currently inside capsule collider
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Stick")
            {
                insideCollider = true;
            }
        }

        /// <summary>
        /// Used to set if drumstick is not currently inside capsule collider
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Stick")
            {
                insideCollider = false;
            }
        }

        /// <summary>
        /// Get current status of stick inside capsule colldier
        /// </summary>
        /// <returns></returns>
        public bool currentTriggerStatus()
        {
            return insideCollider;
        }
    }
}
