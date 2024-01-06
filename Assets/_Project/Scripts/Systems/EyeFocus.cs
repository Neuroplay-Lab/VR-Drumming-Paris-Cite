using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using ViveSR.anipal.Eye;
using System.IO;
using System;

/// <summary>
///     Handles all tasks related to the collecting and storing of eye-tracking
///     data in the game
/// </summary>
public class EyeFocus : MonoBehaviour
{

    private FocusInfo FocusInfo;
    private readonly float MaxDistance = 20;
    private readonly GazeIndex[] GazePriority = new GazeIndex[] { GazeIndex.COMBINE, GazeIndex.LEFT, GazeIndex.RIGHT };
    private static EyeData_v2 eyeData = new EyeData_v2();
    private bool eye_callback_registered = false;

    // holds info about the current eye-focus item
    private string currentFocusItem;
    private Vector3 worldCoord;

    private Queue<string> eyeFocusQueue;
    private DateTime startTime;
    private TimeSpan timeFromStart;
    private string currentScene = "Basic Room";
    private string heatmapLogDirectory;    

    void Start()
    {
        // check if eye tracking has been enabled before attempting to track eyes
        if (!SRanipal_Eye_Framework.Instance.EnableEye)
        {
            enabled = false;
            return;
        }

        // create new eye-tracking queue and start time
        eyeFocusQueue = new Queue<string>();
        startTime = DateTime.Now;

        // get current participant info to store eye-tracking heatmap in the correct location
        ParticipantData pptData = ParticipantData.GetPptData();
        heatmapLogDirectory = $@"{Application.dataPath}/Log/{pptData.date}-ppt{pptData.pptNumber}/Eye Tracking Data (Heatmaps)";
        Directory.CreateDirectory(heatmapLogDirectory);
    }

    void Update()
    {

        // preevent updates where eye tracking in not working/supported
        if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING &&
            SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.NOT_SUPPORT) return;

        if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == true && eye_callback_registered == false)
        {
            SRanipal_Eye_v2.WrapperRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
            eye_callback_registered = true;
        }
        else if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == false && eye_callback_registered == true)
        {
            SRanipal_Eye_v2.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
            eye_callback_registered = false;
        }

        foreach (GazeIndex index in GazePriority)
        {
            Ray GazeRay;
            int layer_id = LayerMask.NameToLayer("Focal Points");
            bool eye_focus;
            if (eye_callback_registered)
            {
                eye_focus = SRanipal_Eye_v2.Focus(index, out GazeRay, out FocusInfo, 0, MaxDistance, (1 << layer_id) | (1 << 0), eyeData);
            }
            else
            {
                eye_focus = SRanipal_Eye_v2.Focus(index, out GazeRay, out FocusInfo, 0, MaxDistance, (1 << layer_id) | (1 << 0));
            }

            if (eye_focus)  // if eye-focus has been correctly recorded
            {   
                if(FocusInfo.transform.gameObject.layer == 10)
                {
                    // if hitting a relevant focal point, get the name of the item
                    currentFocusItem = FocusInfo.collider.GetComponent<FocusItem>().GetItemLabel();
                }
                else // otherwise, no object found
                {
                    currentFocusItem = null;
                    return;
                }

                // get location and time data
                worldCoord = FocusInfo.point;
                timeFromStart = DateTime.Now - startTime;

                // register this eye-track event (row in eye-tracking data)
                eyeFocusQueue.Enqueue($@"{worldCoord.x},{worldCoord.y},{worldCoord.z},{currentFocusItem},{timeFromStart:mm\:ss\.fff}");
                break;
            }
        }
    }

    /// <summary>
    ///     Saves all eye-tracking data to a .csv file
    /// </summary>
    public void LogEyeData()
    {
        string savePath;
        if (File.Exists(heatmapLogDirectory + $"/{currentScene}.csv"))
        {
            /* Here when a participant is repeating a scene at a later time
             * within the same experiment so a number should be added to the
             * end of the save file to avoid overwriting previous data */
            int saveNumber = 1;

            while (File.Exists(heatmapLogDirectory + $"/{currentScene}({saveNumber}).csv"))
            {
                /* loop until high enough save number is reached as
                 * to not overwrite previous data */
                saveNumber += 1;
            }

            savePath = heatmapLogDirectory + $"/{currentScene}({saveNumber}).csv";

        }
        else 
        {
            // no save number needed (first occurance)
            savePath = heatmapLogDirectory + $"/{currentScene}.csv";
        }
        using (var writer = new StreamWriter(savePath, false))
        {
            // first, add headers to each column
            writer.WriteLine("x,y,z,focusItem,time");

            // loop each row and write to file
            while(eyeFocusQueue.Count > 0)
            {
                writer.WriteLine(eyeFocusQueue.Dequeue());
                writer.Flush();
            }
        }

    }

    /// <summary>
    ///     Should be called whenever a scene is changed to make appropriate changes
    ///     to eye-tracking recording
    /// </summary>
    /// <param name="sceneName">The name of the scene changed to</param>
    public void sceneChange(string sceneName)
    {
        LogEyeData(); // save previous eye-tracking data
        startTime = DateTime.Now; // restart time
        currentScene = sceneName; // update current scene
    }

    private static void EyeCallback(ref EyeData_v2 eye_data)
    {
        eyeData = eye_data;
    }

    /// <returns>Name of the item currently being looked at (where available)</returns>
    public string GetCurrentFocusItem()
    {
        return this.currentFocusItem;
    }

    /// <returns>World coordinates currently being looked at</returns>
    public Vector3 GetCurrentFocusCoordinates()
    {
        return worldCoord;
    }

    private void OnApplicationQuit()
    {
        LogEyeData(); // save any recorded data before quitting game
    }
}