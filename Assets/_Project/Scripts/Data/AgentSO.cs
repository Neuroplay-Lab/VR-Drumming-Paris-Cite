using UnityEngine;

namespace _Project.Scripts.Data
{
    /// <summary>
    ///     Contains all information necessary to display an Avatar in the
    ///     Avatar selection menu
    /// </summary>
    [CreateAssetMenu(fileName = "Agent ScriptableObject", menuName = "Scriptables/Agent")]
    public class AgentSO : ScriptableObject
    {
        public int index;  // where to place this avatar in the selection panel
        public new string name;
        public Sprite sprite;  // img to display in the panel
        public GameObject prefab;  // avatar model
    }
}
