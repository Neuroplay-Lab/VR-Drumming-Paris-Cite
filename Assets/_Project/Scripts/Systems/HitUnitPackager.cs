using System.Collections.Generic;
using System.Linq;
using Data;
using DrumRhythmGame.Data;

namespace DrumRhythmGame.Systems
{
    public class HitUnitPackager
    {
        private readonly InstrumentType _instrumentType;
        private readonly ActorType _unitOwner;
        private readonly ActorType _unitGuest;
        private readonly float _perfectHitThresholdTime;
        private readonly float _missHitThresholdTime;

        private readonly Queue<float> _guestHitTimes;
        private float _previousPlayerHitTime;

        public HitUnitPackager(
            InstrumentType instrumentType, 
            ActorType unitOwner,
            ActorType unitGuest,
            float perfectHitThresholdTime, 
            float missHitThresholdTime)
        {
            _instrumentType = instrumentType;
            _unitOwner = unitOwner;
            _unitGuest = unitGuest;
            _perfectHitThresholdTime = perfectHitThresholdTime;
            _missHitThresholdTime = missHitThresholdTime;
            _guestHitTimes = new Queue<float>();
        }

        /// <summary>
        /// Enqueue guest's hit time
        /// </summary>
        /// <param name="time"></param>
        public void EnqueueGuestHit(float time) => _guestHitTimes.Enqueue(time);

        /// <summary>
        /// Set owner's hit time and calculate the newest unit sync rate
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public HitUnit SetOwnerHitAndDetectUnit(float time)
        {
            // Calculate border value to separate each hit
            var unitBorderTime = (_previousPlayerHitTime + time) / 2f;
            // Extract hit times
            var unitGuestHitCount = _guestHitTimes.Count(t => t < unitBorderTime);
            var unitGuestHitTimes = new List<float>();
            for (var i = 0; i < unitGuestHitCount; i++)
                unitGuestHitTimes.Add(_guestHitTimes.Dequeue());
            
            var hitUnit = new HitUnit(
                _instrumentType, 
                _unitOwner,
                _unitGuest,
                _previousPlayerHitTime, 
                unitGuestHitTimes.ToArray(),
                _perfectHitThresholdTime,
                _missHitThresholdTime);
            
            _previousPlayerHitTime = time;

            return hitUnit;
        }

        public void Clear()
        {
            _guestHitTimes.Clear();
            _previousPlayerHitTime = 0;
        }
    }
}