using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace _Project.Scripts.Data
{
    [Serializable]
    [CreateAssetMenu(fileName = "RandomisedTrial", menuName = "Randomised Trial", order = 2)]
    public class RandomisedTrial : ScriptableObject
    {
        [SerializeField] private bool RandomiseOrderOfPhases = true;

        [SerializeField] private TrailPhase[] trailPhases;

        public TrailPhase[] GetTrailPhases()
        {
            if (RandomiseOrderOfPhases)
            {
                TrailPhase[] returnedArray = new TrailPhase[trailPhases.Length];
                Array.Copy(trailPhases, returnedArray, trailPhases.Length);
                for (int i = 0; i < returnedArray.Length; i++)
                {
                    TrailPhase tmp = returnedArray[i];
                    int r = UnityEngine.Random.Range(i, returnedArray.Length);
                    returnedArray[i] = returnedArray[r];
                    returnedArray[r] = tmp;
                }
                return returnedArray;
            }
            else
            {
                return trailPhases;
            }
        }
    }

    [Serializable]
    public struct TrailPhase
    {
        [SerializeField] private AgentSO[] availableAgents;
        [SerializeField] private MusicSetting[] availableStrongSequences;
        [SerializeField] private MusicSetting[] availableWeakSequences;
    }
}