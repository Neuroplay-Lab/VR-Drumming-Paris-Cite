using System;
using DrumRhythmGame.Data;
using UnityEngine;

namespace Data
{
    [Serializable]
    public struct HitUnit
    {
        [SerializeField] public InstrumentType instrumentType;
        [SerializeField] public ActorType owner;
        [SerializeField] public ActorType guest;
        [SerializeField] public float startTime;
        [SerializeField] public float endTime;
        [SerializeField] public float ownerHitTime;
        [SerializeField] public float[] guestHitTimes;
        [SerializeField] public float syncRate;

        public HitUnit(
            InstrumentType instrumentType, 
            ActorType owner,
            ActorType guest,
            float ownerHitTime, 
            float[] guestHitTimes,
            float perfectHitThresholdTime, 
            float missHitThresholdTime)
        {
            this.instrumentType = instrumentType;
            this.owner = owner;
            this.guest = guest;
            this.ownerHitTime = ownerHitTime;
            this.guestHitTimes = guestHitTimes;

            if (guestHitTimes.Length == 0)
            {
                // If there is no guest hit in the unit, the unit sync rate is zero.
                startTime = ownerHitTime;
                endTime = ownerHitTime;
                syncRate = 0;
            }
            else
            {
                startTime = ownerHitTime < guestHitTimes[0] ? ownerHitTime : guestHitTimes[0];
                endTime = ownerHitTime > guestHitTimes[guestHitTimes.Length-1] ? ownerHitTime : guestHitTimes[guestHitTimes.Length-1];

                // Init sync rate as negative value
                var syncRateSum = 0f;
                foreach (var guestHitTime in guestHitTimes)
                {
                    var hitSyncRate = 0f;
    
                    var error = Mathf.Abs(ownerHitTime - guestHitTime);
                    if (error <= perfectHitThresholdTime)
                    {
                        hitSyncRate = 1f;
                    }
                    else if (error <= missHitThresholdTime)
                    {
                        hitSyncRate = (error - missHitThresholdTime) / (perfectHitThresholdTime - missHitThresholdTime);
                    }
    
                    syncRateSum += hitSyncRate;
                }
    
                // The unit sync rate is the average of the sync rates for each hit in the unit.
                syncRate = syncRateSum / guestHitTimes.Length;
            }
        }
    }
}