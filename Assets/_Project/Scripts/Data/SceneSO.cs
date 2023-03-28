using UnityEngine;

namespace _Project.Scripts.Data
{
    [CreateAssetMenu(fileName = "Scene ScriptableObject", menuName = "Scriptables/Scene")]
    public class SceneSO : ScriptableObject
    {
        public int index;
        public new string name;
        public Sprite sprite;
        public GameObject prefab;
    }
}
