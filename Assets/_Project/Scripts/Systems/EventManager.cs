using System;
using _Project.Scripts.Data;
using DrumRhythmGame.Data;
using UnityEngine;
using UnityEngine.XR;

namespace _Project.Scripts.Systems
{
    /// <summary>
    /// A static class that handles all events in the game.
    /// </summary>
    [HelpURL("https://en.wikipedia.org/wiki/Observer_pattern")]
    public static class EventManager
    {
        public static event Action<ActorType, InstrumentType, XRNode> DrumHitEvent;
        public static event Action<int> PlayerScoreUpdateEvent;
        public static event Action<float> SyncRateChangeEvent;
        public static event Action<InstrumentType, float> MusicScoreNoteSetEvent;

        public static event Action<float> ErrorRateChanged;

        public static event Action<MusicSetting> MusicSettingChangeEvent;

        public static event Action MusicStartEvent;
        public static event Action MusicResetEvent;

        public static event Action<bool> CueStateChanged;

        public static event Action<bool> LoggingStateChanged;

        public static event Action<AgentSO> AgentSelected;
        public static event Action<SceneSO> SceneSelected;


        public static void InvokeDrumHitEvent(ActorType actor, InstrumentType type, XRNode node)
        {
            DrumHitEvent?.Invoke(actor, type, node);
        }

        public static void InvokePlayerScoreUpdateEvent(int score)
        {
            PlayerScoreUpdateEvent?.Invoke(score);
        }

        public static void InvokeSyncRateChangeEvent(float rate)
        {
            SyncRateChangeEvent?.Invoke(rate);
        }

        public static void InvokeMusicScoreNoteSetEvent(InstrumentType type, float reachTime)
        {
            MusicScoreNoteSetEvent?.Invoke(type, reachTime);
        }

        public static void InvokeMusicStartEvent()
        {
            MusicStartEvent?.Invoke();
        }

        public static void InvokeMusicResetEvent()
        {
            MusicResetEvent?.Invoke();
        }

        public static void InvokeLoggingStateChanged(bool obj)
        {
            LoggingStateChanged?.Invoke(obj);
        }


        public static void InvokeCueStateChanged(bool state)
        {
            CueStateChanged?.Invoke(state);
        }

        public static void InvokeAgentSelected(AgentSO obj)
        {
            AgentSelected?.Invoke(obj);
        }

        public static void InvokeSceneSelected(SceneSO obj)
        {
            SceneSelected?.Invoke(obj);
        }

        public static void InvokeMusicSettingChangeEvent(MusicSetting obj)
        {
            MusicSettingChangeEvent?.Invoke(obj);
        }

        public static void InvokeErrorRateChanged(float newErrorRate)
        {
            ErrorRateChanged?.Invoke(newErrorRate);
        }
    }
}