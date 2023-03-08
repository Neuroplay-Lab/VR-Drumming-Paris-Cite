using UnityEngine;

namespace _Project.Scripts.Data
{
    [CreateAssetMenu(fileName = "Agent ScriptableObject", menuName = "Scriptables/Agent")]
    public class AgentSO : ScriptableObject
    {
        public int index;
        public new string name;
        public Sprite sprite;
        public GameObject prefab;
    }
}
