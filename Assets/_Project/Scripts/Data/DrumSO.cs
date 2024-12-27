using UnityEngine;

namespace _Project.Scripts.Data
{
    /// <summary>
    ///     Contains all information necessary to display an Drum in the
    ///     Drum selection menu
    /// </summary>
    [CreateAssetMenu(fileName = "Drum ScriptableObject", menuName = "Scriptables/Drum")]
    public class DrumSO : ScriptableObject
    {
        public int index;  // where to place this Drum in the selection panel
        public new string name;
        public Sprite sprite;  // img to display in the panel
        public GameObject prefab;  // drum model

        public int drumCount; // Number of drums in this prefab
    }
}
