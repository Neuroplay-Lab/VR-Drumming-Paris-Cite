using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Used for eye-tracking debugging purposes. Allows a ball to move around a
///     scene based on where the eye focus is.
/// </summary>
public class ballMover : MonoBehaviour
{

    [SerializeField] private EyeFocus eyeTracker;

    void Update()
    {
        Vector3 position = eyeTracker.GetCurrentFocusCoordinates();

        if(position != null)
        {
            this.transform.position = position;
        }
    }
}
