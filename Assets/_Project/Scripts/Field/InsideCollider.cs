using System.Collections;
using _Project.Scripts.Systems;
using System.Collections.Generic;
using DrumRhythmGame.Data;
using DrumRhythmGame.Systems;
using UnityEngine;


namespace DrumRhythmGame.Field
{

    [RequireComponent(typeof(Collider))]
    public class InsideCollider : MonoBehaviour 
    {
   
        private bool insideCollider = false;

        // Start is called before the first frame update
        private void Awake()
        {
            var capsuleCollider = GetComponent<CapsuleCollider>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other) //used to check if drumstick is currently inside capsule collider
        {
            if(other.gameObject.tag == "Stick")
            {
                insideCollider = true;
            }
        }

        private void OnTriggerExit(Collider other) //used to set if drumstick is not currently inside capsule collider
        {
            if (other.gameObject.tag == "Stick")
            {
                insideCollider = false;
            }
        }

        public bool currentTriggerStatus() //get current status of stick inside capsule colldier
        {
            return insideCollider;
        }
    }
}
