using System;
using System.Collections.Generic;
using DrumRhythmGame.Data;
using DrumRhythmGame.Systems;
using UnityEngine;

namespace DrumRhythmGame.Field
{
    public class SynchronousRateUseSample : MonoBehaviour
    {
        private void Awake()
        {
            /*var map = new SortedDictionary<float, float>
            {
                {0.2f, 1.0f}, // Perfect
                {0.8f, 0.5f}, // Good 
                {1.0f, 0.1f} // Bad
            };
            var sync = new HitUnitPackager(InstrumentType.CrashCymbal, 0.2f, 1.0f);
            
            var sampleData = new List<(ActorType actor, float time)>()
            {
                (ActorType.Partner, 0),
                (ActorType.Player, 0.1f), // 1 -> 0.1
                (ActorType.Player, 0.5f),
                (ActorType.Player, 0.52f),
                (ActorType.Player, 0.53f),
                (ActorType.Player, 0.54f),
                (ActorType.Partner, 0.8f),
                (ActorType.Partner, 0.9f), // 0.5 -> 0.15
                (ActorType.Partner, 1.9f),
                (ActorType.Player, 2.0f), // 1 -> 0.25
                (ActorType.Player, 2.1f) // 1 -> 0.25
            };

            foreach (var data in sampleData)
            {
                if (data.actor == ActorType.None) continue;
                
                if (data.actor == ActorType.Partner)
                {
                    sync.EnqueueGuestHit(data.time);
                }
                else
                {
                    var unit = sync.SetOwnerHitAndDetectUnit(data.time);
                    Debug.Log($"{data.time} => {unit.syncRate}");
                }
            }*/
        }
    }
}