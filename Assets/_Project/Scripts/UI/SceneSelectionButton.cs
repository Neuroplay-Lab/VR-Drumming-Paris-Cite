using _Project.Scripts.Data;
using _Project.Scripts.Systems;
using UnityEngine;

namespace _Project.Scripts.UI
{
    /// <summary>
    /// SEEMINGLY REDUNDANT. Not actually invoked when selecting a new scene,
    /// could possibly be removed.
    /// </summary>
    public class SceneSelectionButton : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private string title;
        [Space] [SerializeField] private SceneSO sceneSO;

        #endregion

        public void SceneSelected()
        {
            EventManager.InvokeSceneSelected(sceneSO);
            Debug.Log($"Selected: {title} scene");
        }
    }
}