using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using ViveSR.anipal.Eye;
using System.IO;

public class EyeFocus : MonoBehaviour
{

    private FocusInfo FocusInfo;
    private readonly float MaxDistance = 20;
    private readonly GazeIndex[] GazePriority = new GazeIndex[] { GazeIndex.COMBINE, GazeIndex.LEFT, GazeIndex.RIGHT };
    private static EyeData_v2 eyeData = new EyeData_v2();
    private bool eye_callback_registered = false;

    private string currentFocusItem;
    private Vector3 worldCoord;

    private Queue<string> eyeFocusQueue;
    private System.DateTime startTime;
    private string currentScene = "Basic Room";
    private string logDirectory;
    private int participantNo = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        // check if eye tracking has been enabled before attempting to track eyes
        if (!SRanipal_Eye_Framework.Instance.EnableEye)
        {
            enabled = false;
            return;
        }

        eyeFocusQueue = new Queue<string>();
        startTime = System.DateTime.Now;
        if (!Directory.Exists($@"{Application.dataPath}/Log"))
        {
            Directory.CreateDirectory($@"{Application.dataPath}/Log");
        }
        while (Directory.Exists($@"{Application.dataPath}/Log/{System.DateTime.Now:yyyy-MM-dd}-ppt{participantNo}"))
        {
            participantNo += 1;
        }

        logDirectory = $@"{Application.dataPath}/Log/{System.DateTime.Now:yyyy-MM-dd}-ppt{participantNo}";
        Directory.CreateDirectory(logDirectory);
    }

    // Update is called once per frame
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

            if (eye_focus)
            {   
                if(FocusInfo.transform.gameObject.layer == 10) // if hitting a relevant focal point
                {
                    currentFocusItem = FocusInfo.transform.name;
                } else
                {
                    currentFocusItem = null;
                }

                worldCoord = FocusInfo.point;

                eyeFocusQueue.Enqueue($@"{worldCoord.x},{worldCoord.y},{worldCoord.z},{currentFocusItem},{System.DateTime.Now - startTime:mm:ss}");
                break;
            }
        }
    }

    public void LogEyeData()
    {
        string savePath;
        if (File.Exists(logDirectory + $"/{currentScene}.csv"))
        {
            int saveNumber = 1;

            while (File.Exists(logDirectory + $"/{currentScene}({saveNumber}).csv"))
            {
                saveNumber += 0;
            }

            savePath = logDirectory + $"/{currentScene}({saveNumber}).csv";

        } else
        {
            savePath = logDirectory + $"/{currentScene}.csv";
        }
        using (var writer = new StreamWriter(savePath, false))
        {
            writer.WriteLine("x,y,z,focusItem,time");

            while(eyeFocusQueue.Count > 0)
            {
                writer.WriteLine(eyeFocusQueue.Dequeue());
                writer.Flush();
            }
        }

    }

    public void sceneChange(string sceneName)
    {
        LogEyeData();
        currentScene = sceneName;
    }

    private static void EyeCallback(ref EyeData_v2 eye_data)
    {
        eyeData = eye_data;
    }

    public string GetCurrentFocusItem()
    {
        return this.currentFocusItem;
    }

    public Vector3 GetCurrentFocusCoordinates()
    {
        return worldCoord;
    }

    private void OnApplicationQuit()
    {
        LogEyeData();
    }
}