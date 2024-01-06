using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Used to show/hide the gaze ray from the researcher's point of view (never
///     visible to the participant).
/// </summary>
public class ToggleGazeRay : MonoBehaviour
{
    [SerializeField] private LineRenderer gazeRay;
    [SerializeField] private bool gazeRayShown;
    [SerializeField] private GameObject showText;
    [SerializeField] private GameObject hideText;

    private void Reset()
    {
        gazeRay = GameObject.FindObjectOfType<EyeFocus>().GetComponent<LineRenderer>();
        gazeRayShown = gazeRay.enabled;
        showText = transform.Find("Show Text").gameObject;
        hideText = transform.Find("Hide Text").gameObject;
        UpdateText();
    }

    /// <summary>
    ///     Handles UI updates to the menu butttons
    /// </summary>
    private void UpdateText()
    {
        if (gazeRayShown)
        {
            showText.SetActive(false);
            hideText.SetActive(true);
        }
        else
        {
            showText.SetActive(true);
            hideText.SetActive(false);
        }
    }

    /// <summary>
    ///     Handles the hiding/showing of the gaze ray
    /// </summary>
    public void ToggleGazeRayVisibility()
    {
        if (gazeRayShown)
        {
            gazeRay.enabled = false;
            gazeRayShown = false;
        }
        else
        {
            gazeRay.enabled = true;
            gazeRayShown = true;
        }

        UpdateText(); // update the UI text to reflect the state of the gaze ray
    }
}
