using UnityEngine;

namespace _Project.Scripts.Systems
{
    /// <summary>
    /// Activates additional displays.
    /// </summary>
    public class OtherDisplayActivator : MonoBehaviour
    {
        // Note: You need to activate any additional displays, because - Unity?
        // Also the number of displays is not available in the editor.
        private void Awake()
        {
            Debug.Log("displays connected: " + Display.displays.Length);
            if (Display.displays.Length > 1)
                Display.displays[1].Activate();
            if (Display.displays.Length > 2)
                Display.displays[2].Activate();
        }
    }
}
