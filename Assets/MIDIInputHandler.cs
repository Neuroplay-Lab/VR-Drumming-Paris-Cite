using UnityEngine;
using Melanchall.DryWetMidi.Multimedia;
using _Project.Scripts.Systems;
using DrumRhythmGame.Data;

public class MIDIInputHandler : MonoBehaviour
{

    private InputDevice _midiDevice;
    [SerializeField] private AudioSource _audioSource;

    void Start()
    {
        _midiDevice = InputDevice.GetByIndex(0);
        _midiDevice.EventReceived += OnEventRecieved;
        _midiDevice?.StartEventsListening();
    }

    private void OnEventRecieved(object sender, MidiEventReceivedEventArgs args)
    {
        _audioSource.PlayOneShot(_audioSource.clip);
        EventManager.InvokeDrumHitEvent(ActorType.Player, InstrumentType.SnareDrum, UnityEngine.XR.XRNode.LeftHand);
    }

}
