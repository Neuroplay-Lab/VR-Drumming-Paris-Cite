using _Project.Scripts.Data;
using _Project.Scripts.Systems;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class SceneSelectionButton : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private string title;
        [Space] [SerializeField] private SceneSO sceneSO;

        #endregion

        public void SceneSelected()
        {
            Debug.Log("HERE");
            EventManager.InvokeSceneSelected(sceneSO);
            Debug.Log($"Selected: {title} scene");
        }
    }
}