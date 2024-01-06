using System;
using System.IO;
using UnityEngine;

/// <summary>
/// Used for saving data on the participant, used for appropriately naming
/// save files with the current date and participant number.
/// </summary>
[Serializable] public class ParticipantData
{

    public string date;
    public int pptNumber;

    private static string saveDir = Application.dataPath + "/SaveData";
    private static string pptDataFilePath = saveDir + "/ParticipantData.json";

    public ParticipantData(DateTime date, int ParticipantNumber)
    {
        this.date = $"{date.Date:yyyy-MM-dd}";
        this.pptNumber = ParticipantNumber;
    }

    public ParticipantData(string date, int ParticipantNumber)
    {
        this.date = date;
        this.pptNumber = ParticipantNumber;
    }

    /// <returns>Relevant data on the currently saved participant</returns>
    public static ParticipantData GetPptData()
    {

        if (File.Exists(pptDataFilePath))
        {
            string jsonStr = File.ReadAllText(pptDataFilePath);
            return JsonUtility.FromJson<ParticipantData>(jsonStr);
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Generates the relevant information for the next participant, either with
    /// an updated participant number or with a new date and participant number of 0.
    /// </summary>
    /// <returns>Data relating to the next participant</returns>
    public static ParticipantData GetNextParticipantID()
    {
        ParticipantData previousPptData = GetPptData();

        if (previousPptData == null)
        {
            return new ParticipantData(DateTime.Now.Date, 0);
        }
        else
        {
            if (previousPptData.date == $"{DateTime.Now:yyyy-MM-dd}")
            {
                return new ParticipantData(previousPptData.date, previousPptData.pptNumber + 1);
            }
            else
            {
                return new ParticipantData(DateTime.Now.Date, 0);
            }
        }
    }

    /// <summary>
    /// Saves participant data for reference in future experiments.
    /// </summary>
    public void Save()
    {

        if (!Directory.Exists(saveDir))
        {
            Directory.CreateDirectory(saveDir);
        }

        File.WriteAllText(pptDataFilePath, JsonUtility.ToJson(this));
    }
}
