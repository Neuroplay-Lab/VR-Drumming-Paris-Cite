using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Systems;
using DrumRhythmGame.Data;
using RootMotion.FinalIK;
using UniRx;
using UnityEngine;
using UnityEngine.XR;
using Random = System.Random;

namespace DrumRhythmGame.Field
{
    public class FollowerBehaviour : IPartnerBehaviour
    {
        public bool Enabled { get; private set; } = false;

        private readonly InteractionSystem _interactionSystem;
        private readonly Random _random;
        private readonly IReadOnlyDictionary<InstrumentType, InteractionObject> _instruments;
        private readonly IList<InstrumentType> _instrumentTypes;

        public FollowerBehaviour(InteractionSystem interactionSystem, IReadOnlyDictionary<InstrumentType, InteractionObject> instruments)
        {
            _interactionSystem = interactionSystem;
            _random = new Random();
            _instruments = instruments;
            _instrumentTypes = instruments.Keys.ToList();
        }

        public void Enable()
        {
            if (Enabled)
            {
                Debug.Log($"[FollowerBehaviour] Already enabled.");
                return;
            }
            
            EventManager.DrumHitEvent += OnDrumHit;
            Enabled = true;
        }

        public void Disable()
        {
            if (!Enabled)
            {
                Debug.Log($"[FollowerBehaviour] Already disabled.");
                return;
            }
            
            EventManager.DrumHitEvent -= OnDrumHit;
            Enabled = false;
        }

        private void OnDrumHit(ActorType actor, InstrumentType type, XRNode node)
        {
            // Do nothing if _enabled == false
            if (!Enabled || actor == ActorType.Partner) return;
            
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
            
            // Call Partner's hit function
            Observable.Timer(TimeSpan.FromSeconds(latency)).Subscribe(_ => Hit(type, node));
        }
        
        private void Hit(InstrumentType type, XRNode node)
        {
            // Effector
            FullBodyBipedEffector effector;
            switch (node)
            {
                case XRNode.RightHand:
                    effector = FullBodyBipedEffector.LeftHand;
                    break;
                case XRNode.LeftHand:
                    effector = FullBodyBipedEffector.RightHand;
                    break;
                default:
                    Debug.LogWarning($"{node} is not supported.");
                    return;
            }
            
            _interactionSystem.StartInteraction(effector, _instruments[type], true);
        }
    }
}