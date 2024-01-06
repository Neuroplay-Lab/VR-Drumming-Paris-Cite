using UnityEngine;

namespace _Project.Scripts.Data
{
    /// <summary>
    /// Holds all necessary information for the selection of new scenery.
    /// </summary>
    [CreateAssetMenu(fileName = "Scene ScriptableObject", menuName = "Scriptables/Scene")]
    public class SceneSO : ScriptableObject
    {
        public int index;  // where to place this scene in the scenery selection panel
        public new string name; // name of this scene
        public Sprite sprite;  // image of the scene for the selection panel
        public GameObject prefab; // scene prefab to load into game
    }
}
