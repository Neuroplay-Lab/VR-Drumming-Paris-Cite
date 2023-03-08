using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Systems;
using Data;
using DrumRhythmGame.Data;
using UnityEngine;
using UnityEngine.XR;

namespace DrumRhythmGame.Systems
{
    public class SyncRateController
    {
        public event Action<float, HitUnit> OnRateChange;
        
        private readonly Dictionary<InstrumentType, HitUnitPackager> _unitPackagers;
        private readonly ActorType _unitOwner;
        private readonly ActorType _unitGuest;
        
        private readonly Queue<HitUnit> _hitUnits;
        private readonly float _maxUnitQueueSize;
        
        public SyncRateController(
            IEnumerable<InstrumentType> instrumentTypes, 
            ActorType unitOwner,
            ActorType unitGuest,
            int maxUnitQueueSize,
            float perfectHitThresholdTime, 
            float missHitThresholdTime)
        {
            _unitPackagers = new Dictionary<InstrumentType, HitUnitPackager>();
            foreach (var instrumentType in instrumentTypes)
            {
                _unitPackagers.Add(instrumentType, new HitUnitPackager(
                    instrumentType, 
                    unitOwner,
                    unitGuest,
                    perfectHitThresholdTime, 
                    missHitThresholdTime));
            }

            _unitOwner = unitOwner;
            _unitGuest = unitGuest;
            
            _hitUnits = new Queue<HitUnit>();
            _maxUnitQueueSize = maxUnitQueueSize;

            EventManager.DrumHitEvent += OnDrumHit;
        }

        /// <summary>
        /// Occurs when a drum is hit.
        /// </summary>
        /// <param name="actor">Who struck a drum</param>
        /// <param name="instrumentType">Type of drum struck e.g. Tom or Hi-Hat</param>
        /// <param name="node">Left Hand / Right Hand</param>
        private void OnDrumHit(ActorType actor, InstrumentType instrumentType, XRNode node)
        {
            if (instrumentType == InstrumentType.None || MusicSequence.Instance.CurrentTime < 0) return;

            if (actor == _unitGuest)
            {
                _unitPackagers[instrumentType].EnqueueGuestHit(MusicSequence.Instance.CurrentTime);
            }
            else if (actor == _unitOwner)
            {
                var unit = _unitPackagers[instrumentType].SetOwnerHitAndDetectUnit(MusicSequence.Instance.CurrentTime);
                _hitUnits.Enqueue(unit);
                    
                if (_hitUnits.Count > _maxUnitQueueSize)
                    _hitUnits.Dequeue();
                
                OnRateChange?.Invoke(_hitUnits.Average(u => u.syncRate), unit);
            }
        }

        public void Clear()
        {
            foreach (var packager in _unitPackagers)
            {
                packager.Value.Clear();
            }
            _hitUnits.Clear();
        }
    }
}