using _Project.Scripts.Systems;
using DrumRhythmGame.Data;
using UnityEngine;
using UnityEngine.XR;

namespace DrumRhythmGame.Field
{
    /// <summary>
    ///     Allows the keyboard to be used for playing drum hits. Useful for
    ///     development without the need for the VR headset and 2D implementations.
    /// </summary>
    public class DrumHitEmulator : MonoBehaviour
    {
        #region Serialized Fields

        // References to the drumns in the scene
        [SerializeField] private Instrument crashCymbal;
        [SerializeField] private Instrument highTom;
        [SerializeField] private Instrument middleTom;
        [SerializeField] private Instrument snareDrum;

        #endregion

        #region Event Functions

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q)) // crash cymbal key
            {
                EventManager.InvokeDrumHitEvent(ActorType.Player, InstrumentType.CrashCymbal, XRNode.LeftHand);
                PlaySound(crashCymbal);
            }

            if (Input.GetKeyDown(KeyCode.A)) // high tom key
            {
                EventManager.InvokeDrumHitEvent(ActorType.Player, InstrumentType.LeftHighTom, XRNode.LeftHand);
                PlaySound(highTom);
            }

            if (Input.GetKeyDown(KeyCode.L)) // mid tom key
            {
                EventManager.InvokeDrumHitEvent(ActorType.Player, InstrumentType.RightMiddleTom, XRNode.RightHand);
                PlaySound(middleTom);
            }

            if (Input.GetKeyDown(KeyCode.P)) // snare key
            {
                EventManager.InvokeDrumHitEvent(ActorType.Player, InstrumentType.SnareDrum, XRNode.RightHand);
                PlaySound(snareDrum);
            }
        }

        #endregion

        /// <summary>
        ///     Plays the relevant instrument sound based on the key pressed
        /// </summary>
        /// <param name="instrument">Instrument to play the sound of</param>
        private void PlaySound(Instrument instrument)
        {
            if (instrument == null || instrument.audioSource == null) return;
            instrument.audioSource.time = instrument.timeToPlayStart;
            instrument.audioSource.clip = instrument.sound;
            instrument.audioSource.volume = instrument.volume;
            instrument.audioSource.Play();
        }
    }
}