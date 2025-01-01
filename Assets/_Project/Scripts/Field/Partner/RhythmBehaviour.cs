using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Systems;
using DrumRhythmGame.Data;
using RootMotion.FinalIK;
using UniRx;
using UnityEngine;
using Random = System.Random;

namespace DrumRhythmGame.Field
{
    public class RhythmBehaviour : IPartnerBehaviour
    {
        public bool Enabled { get; private set; } = false;

        private readonly Random _random;
        private readonly InteractionSystem _interactionSystem;
        private readonly IReadOnlyDictionary<InstrumentType, InteractionObject> _instruments;
        private readonly Dictionary<InstrumentType, FullBodyBipedEffector> _instrumentHandMap;

        private PartnerHandPreference _partnerHandPreference = PartnerHandPreference.Both;
        private readonly Dictionary<InstrumentType, float> _animationDurations;
        private readonly IList<InstrumentType> _instrumentTypes;

        public RhythmBehaviour(InteractionSystem interactionSystem, IReadOnlyDictionary<InstrumentType, InteractionObject> instruments)
        {
            _interactionSystem = interactionSystem;
            _instruments = instruments;
            _instrumentTypes = instruments.Keys.ToList();
            _random = new Random();

            _instrumentHandMap = new Dictionary<InstrumentType, FullBodyBipedEffector>()
            {
                { InstrumentType.CrashCymbal, FullBodyBipedEffector.RightHand },
                { InstrumentType.LeftHighTom, FullBodyBipedEffector.RightHand },
                { InstrumentType.RightMiddleTom, FullBodyBipedEffector.LeftHand },
                { InstrumentType.SnareDrum, FullBodyBipedEffector.LeftHand }
            };
            _animationDurations = new Dictionary<InstrumentType, float>();

            foreach (var instrument in _instruments)
            {
                var animationSpeed = instrument.Value.GetComponentsInChildren<InteractionTarget>()
                                        .Where(target => target.effectorType == _instrumentHandMap[instrument.Key])
                                        .Select(target => target.interactionSpeedMlp)
                                        .First();

                _animationDurations.Add(instrument.Key, 1 / animationSpeed);
            }
        }

        private void OnNoteSet(InstrumentType type, float reachTime)
        {
            // Do nothing if _enabled == false
            if (!Enabled) return;

            // Make latency randomly
            float latency = 0;
            if (SaveData.Instance.partnerErrorData.Current.latencyFrequencyRate > _random.NextDouble())
            {
                latency = SaveData.Instance.partnerErrorData.Current.maxLatencyTime * (float)_random.NextDouble();
            }

            // Make miss hit randomly
            if (SaveData.Instance.partnerErrorData.Current.missHitFrequencyRate > _random.NextDouble())
            {
                type = _instrumentTypes[_random.Next(_instrumentTypes.Count)];
            }

            Observable
                .Timer(TimeSpan.FromSeconds(latency + reachTime - _animationDurations[type] / 2f))
                .Subscribe(_ =>
                    _interactionSystem.StartInteraction(_instrumentHandMap[type], _instruments[type], true));
        }

        public void Enable()
        {
            Enable(_partnerHandPreference);
        }

        public void Enable(PartnerHandPreference handPreference)
        {
            if (Enabled)
            {
                Debug.Log($"[RhythmBehaviour] Already enabled.");
                return;
            }

            _partnerHandPreference = handPreference;

            switch (_partnerHandPreference)
            {
                case PartnerHandPreference.Left:
                    _instrumentHandMap[InstrumentType.CrashCymbal] = FullBodyBipedEffector.LeftHand;
                    _instrumentHandMap[InstrumentType.LeftHighTom] = FullBodyBipedEffector.LeftHand;
                    _instrumentHandMap[InstrumentType.RightMiddleTom] = FullBodyBipedEffector.LeftHand;
                    _instrumentHandMap[InstrumentType.SnareDrum] = FullBodyBipedEffector.LeftHand;
                    break;

                case PartnerHandPreference.Right:
                    _instrumentHandMap[InstrumentType.CrashCymbal] = FullBodyBipedEffector.RightHand;
                    _instrumentHandMap[InstrumentType.LeftHighTom] = FullBodyBipedEffector.RightHand;
                    _instrumentHandMap[InstrumentType.RightMiddleTom] = FullBodyBipedEffector.RightHand;
                    _instrumentHandMap[InstrumentType.SnareDrum] = FullBodyBipedEffector.RightHand;
                    break;

                default:
                    _instrumentHandMap[InstrumentType.CrashCymbal] = FullBodyBipedEffector.RightHand;
                    _instrumentHandMap[InstrumentType.LeftHighTom] = FullBodyBipedEffector.RightHand;
                    _instrumentHandMap[InstrumentType.RightMiddleTom] = FullBodyBipedEffector.LeftHand;
                    _instrumentHandMap[InstrumentType.SnareDrum] = FullBodyBipedEffector.LeftHand;
                    break;
            }

            EventManager.MusicScoreNoteSetEvent += OnNoteSet;
            Enabled = true;
        }

        public void Disable()
        {
            if (!Enabled)
            {
                Debug.Log($"[RhythmBehaviour] Already disabled.");
                return;
            }

            EventManager.MusicScoreNoteSetEvent -= OnNoteSet;
            Enabled = false;
        }
    }
}