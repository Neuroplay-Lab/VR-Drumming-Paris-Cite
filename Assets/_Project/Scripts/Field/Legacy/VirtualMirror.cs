using UnityEngine;

namespace DrumRhythmGame.Field
{
    public class VirtualMirror : MonoBehaviour
    {
        [SerializeField] private Transform[] drums;
        [SerializeField] private Transform rightHandIkTarget = null;
        [SerializeField] private Transform leftHandIkTarget = null;
        
        [SerializeField] private Transform rightController = null;
        [SerializeField] private Transform leftController = null;
        
        [SerializeField] private Vector3 right;
        [SerializeField] private Vector3 left;

        private void Update()
        {
            var position = (drums[0].position + drums[1].position) / 2;
            transform.position = position;
            transform.LookAt(drums[0]);

            var rp = rightController.position;
            var lp = leftController.position;
            leftHandIkTarget.position = new Vector3(rp.x, rp.y, 2 * position.z - rp.z);
            rightHandIkTarget.position = new Vector3(lp.x, lp.y, 2 * position.z - lp.z);

            //rightHandIkTarget.rotation = Quaternion.Euler(right);
            //leftHandIkTarget.rotation = Quaternion.Euler(left);
        }
    }
}
