using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Scripts.Data
{
    [Serializable]
    [CreateAssetMenu(fileName = "RandomisedPlaylist", menuName = "Randomised Playlist", order = 2)]
    public class RandomisedPlaylist : ScriptableObject
    {
        [SerializeField] private bool RandomiseOrderOfPhases = true;

        [SerializeField] private TrailPhase[] trailPhases;
    }

    [Serializable]
    public struct TrailPhase
    {
        [SerializeField] private AgentSO[] availableAgents;
        [SerializeField] private MusicSetting[] availableStrongSequences;
        [SerializeField] private MusicSetting[] availableWeakSequences;
    }
}