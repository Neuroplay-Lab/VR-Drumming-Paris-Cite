using _Project.Scripts.Systems;
using DrumRhythmGame.Data;
using UnityEngine;
using UnityEngine.XR;

namespace DrumRhythmGame.Field
{
    public class DrumHitEmulator : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private Instrument crashCymbal;
        [SerializeField] private Instrument highTom;
        [SerializeField] private Instrument middleTom;
        [SerializeField] private Instrument snareDrum;

        #endregion

        #region Event Functions

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                EventManager.InvokeDrumHitEvent(ActorType.Player, InstrumentType.CrashCymbal, XRNode.LeftHand);
                PlaySound(crashCymbal);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                EventManager.InvokeDrumHitEvent(ActorType.Player, InstrumentType.LeftHighTom, XRNode.LeftHand);
                PlaySound(highTom);
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                EventManager.InvokeDrumHitEvent(ActorType.Player, InstrumentType.RightMiddleTom, XRNode.RightHand);
                PlaySound(middleTom);
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                EventManager.InvokeDrumHitEvent(ActorType.Player, InstrumentType.SnareDrum, XRNode.RightHand);
                PlaySound(snareDrum);
            }
        }

        #endregion

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