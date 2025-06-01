using UnityEngine;
using Melanchall.DryWetMidi.Multimedia;
using _Project.Scripts.Systems;
using DrumRhythmGame.Data;
using UnityEngine.InputSystem;
using InputDevice = Melanchall.DryWetMidi.Multimedia.InputDevice;

public class MIDIInputHandler : MonoBehaviour
{

    private InputDevice _midiDevice;
    [SerializeField] private AudioSource _audioSource;

    void Start()
    {
        // Debug.Log
        _midiDevice = InputDevice.GetByIndex(0);
        _midiDevice.EventReceived += OnEventRecieved;
        _midiDevice?.StartEventsListening();
    }

    private void OnEventRecieved(object sender, MidiEventReceivedEventArgs args)
    {
        Action();
    }

    private void Action()
    {
        Debug.Log("Triggered");
        _audioSource.PlayOneShot(_audioSource.clip);
        EventManager.InvokeDrumHitEvent(ActorType.Player, InstrumentType.SnareDrum, UnityEngine.XR.XRNode.LeftHand);
        Debug.Log("End");
    }

}
