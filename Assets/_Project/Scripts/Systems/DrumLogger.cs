using System;
using System.Collections.Generic;
using System.IO;
using _Project.Scripts.Systems;
using DrumRhythmGame.Data;
using UnityEngine;
using UnityEngine.XR;

public class DrumLogger : MonoBehaviour
{
    public static DrumLogger Instance { get; private set; }
    private DateTime _ogStartTime;
    private DateTime _currentFileStartTime;
    private string _hitLogDirectory;
    private int _trailCounter = 1;
    private string _currentTrail = "FreePlay";
    private string _currentAvatar = "No Avatar";
    private Queue<string> _participantHits;
    private Queue<string> _avatarHits;
    private Queue<string> _beatTimes;
    private string _drumHitHeaders = "HitCount,HitTime,Hand,Drum";
    private string _beatTimeHeaders = "BeatNumber,Time";
    private Dictionary<XRNode, string> _handMap = new Dictionary<XRNode, string> { { XRNode.LeftHand, "Left Hand" }, { XRNode.RightHand, "Right Hand" } };
    private Dictionary<InstrumentType, string> _instrumentMap = new Dictionary<InstrumentType, string> { { InstrumentType.RightMiddleTom, "Right Drum" }, { InstrumentType.LeftHighTom, "Left Drum" } };
    private Dictionary<InstrumentType, string> _avatarInstrumentMap = new Dictionary<InstrumentType, string> { { InstrumentType.LeftHighTom, "Right Drum" }, { InstrumentType.RightMiddleTom, "Left Drum" } };


    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;

            _participantHits = new Queue<string>();
            _avatarHits = new Queue<string>();
            _beatTimes = new Queue<string>();

            _ogStartTime = _currentFileStartTime = DateTime.Now;

            ParticipantData pptData = ParticipantData.GetPptData();
            _hitLogDirectory = $@"{Application.dataPath}/Log/{pptData.date}-ppt{pptData.pptNumber}/Drum Hit Data";
            Directory.CreateDirectory(_hitLogDirectory);
        }
    }

    public void RegisterHitEvent(ActorType whoHitDrum, InstrumentType whichDrum, XRNode whichHand)
    {
        if (whoHitDrum == ActorType.Player)
        {
            LogParticipantHit(_instrumentMap[whichDrum], _handMap[whichHand]);
        }
        else if (whoHitDrum == ActorType.Partner)
        {
            LogAvatarHit(_avatarInstrumentMap[whichDrum], _handMap[whichHand]);
        }
    }

    public void LogParticipantHit(string drum, string hand)
    {
        _participantHits.Enqueue($"{Math.Round((DateTime.Now - _currentFileStartTime).TotalMilliseconds)},{hand},{drum}");
    }
    public void LogAvatarHit(string drum, string hand)
    {
        _avatarHits.Enqueue($"{Math.Round((DateTime.Now - _currentFileStartTime).TotalMilliseconds)},{hand},{drum}");
    }

    public void LogBeatTime()
    {
        _beatTimes.Enqueue($"{Math.Round((DateTime.Now - _currentFileStartTime).TotalMilliseconds)}");
    }

    public void ChangedAvatar(string avatar)
    {
        LogCurrentTrial();
        _currentAvatar = avatar;
    }

    public void SetCurrentTrail(string trailName)
    {
        LogCurrentTrial();
        _currentTrail = trailName;
    }

    private void LogCurrentTrial()
    {
        string trailInfo = $@"{_currentTrail} with {_currentAvatar} started at {DateTime.Now:HH\:mm\:ss} ({_currentFileStartTime - _ogStartTime:mm\:ss} from start)";
        string saveDirectory = MakeCurrentSaveDirectory();
        LogDataToFile(_participantHits, _drumHitHeaders, saveDirectory, "ParticipantHits", trailInfo);

        if (_currentTrail != "FreePlay" && _currentTrail != "SPR")
        {
            LogDataToFile(_avatarHits, _drumHitHeaders, saveDirectory, "AvatarHits", trailInfo);
            LogDataToFile(_beatTimes, _beatTimeHeaders, saveDirectory, "BeatTimes", trailInfo);
        }
        _currentFileStartTime = DateTime.Now;
        _participantHits.Clear();
        _avatarHits.Clear();
        _beatTimes.Clear();
    }

    private void LogDataToFile(Queue<string> data, string fileHeader, string directory, string fileName, string trailInfo)
    {
        string savePath = directory + $"/{fileName}.csv";
        /* Here when a participant is repeating a scene at a later time
        * within the same experiment so a number should be added to the
        * end of the save file to avoid overwriting previous data */
        int saveNumber = 0;
        while (File.Exists(savePath))
        {
            /* loop until high enough save number is reached as
            * to not overwrite previous data */
            saveNumber += 1;
            savePath = directory + $"/{fileName}({saveNumber}).csv";
        }

        using (var writer = new StreamWriter(savePath, false))
        {
            // first, add headers to each column
            writer.WriteLine(trailInfo);
            writer.WriteLine(fileHeader);

            // loop each row and write to file
            int line = 0;
            while (data.Count > 0)
            {
                writer.WriteLine($"{++line},{data.Dequeue()}");
                writer.Flush();
            }
        }
    }

    private string MakeCurrentSaveDirectory()
    {
        string currentSaveDirectory = $@"{_hitLogDirectory}/{_trailCounter++} - {_currentTrail}";
        while (Directory.Exists(currentSaveDirectory))
        {
            currentSaveDirectory = $@"{_hitLogDirectory}/{_trailCounter++} - {_currentTrail}";
        }
        Directory.CreateDirectory(currentSaveDirectory);
        return currentSaveDirectory;
    }

    private void OnApplicationQuit()
    {
        LogCurrentTrial();
    }
}
