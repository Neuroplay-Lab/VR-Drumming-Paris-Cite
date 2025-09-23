using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Data;
using _Project.Scripts.Systems;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlaylistController : MonoBehaviour
{

    public static PlaylistController Instance;
    private Playlist currentPlaylist;
    private RandomisedTrial currentTrial;
    private AgentSO _currentPartner = null;
    public bool PartnerIsActive = false;
    [SerializeField] private RecallManager recallButtons;

    private Coroutine coroutine;
    private Coroutine subCoroutine;
    private bool recalling = false;
    void Start()
    {
        Instance = this;
        EventManager.AgentSelected += UpdateCurrentPartnerStored;
    }

    public void Play()
    {
        if (GameData.Instance.currentPlayType is PlayType.Playlist)
        {
            coroutine = StartCoroutine(IteratePlaylist());
        }
        else if (GameData.Instance.currentPlayType is PlayType.RandomisedTrial)
        {
            coroutine = StartCoroutine(IterateTrial());
        }
    }

    public void Reset()
    {
        if (coroutine != null)
        {
            if (subCoroutine != null)
            {
                StopCoroutine(subCoroutine);
                EventManager.InvokeRemoveAgent();
            }
            StopCoroutine(coroutine);
            if (MusicSequence.Instance.IsPlaying)
            {
                MusicSequence.Instance.Reset();
            }
            DrumLogger.Instance.SetCurrentTrail("FreePlay");
        }
    }

    private IEnumerator IteratePlaylist()
    {
        foreach (PlaylistItem item in currentPlaylist.playlistItems)
        {
            EventManager.InvokeMusicSettingChangeEvent(item.track);
            if (item.hidePartner && PartnerIsActive)
            {
                EventManager.InvokeRemoveAgent();
            }
            else if (!item.hidePartner && !PartnerIsActive)
            {
                EventManager.InvokeAgentSelected(_currentPartner);
            }
            MusicSequence.Instance.Play();
            if (item.track.name.Trim().ToLower() == "recall")
            {
                break;
            }
            if (item.track.name.Trim().ToLower() == "break")
            {
                yield return new WaitForSeconds(item.duration - 1);
                EventManager.InvokeAgentPrepareEvent();
                yield return new WaitForSeconds(1);
            }
            else
            {
                yield return new WaitForSeconds(item.duration);
            }
            MusicSequence.Instance.Reset();
        }
        if (currentPlaylist.playlistItems[^1].track.name.Trim().ToLower() != "recall")
        {
            Reset();
        }
        else
        {
            recallButtons.gameObject.SetActive(true);
            recalling = true;
            while (recalling)
            {
                yield return null;
            }
            if (subCoroutine is not null)
            {
                StopCoroutine(subCoroutine);
            }
        }
    }

    private IEnumerator IterateTrial()
    {
        TrialPhase[] trailPhases = currentTrial.GetTrailPhases();

        PlaylistItem shownBreakItem = new PlaylistItem(currentTrial.breakObject, currentTrial.breakTimeSecs, false);
        PlaylistItem hiddenBreakItem = new PlaylistItem(currentTrial.breakObject, currentTrial.breakTimeSecs, true);
        PlaylistItem inteferenceItem = new PlaylistItem(currentTrial.interferenceObject, currentTrial.interferenceTimeSecs, true);
        PlaylistItem recallItem = new PlaylistItem(currentTrial.recallObject, 0, true);

        foreach (TrialPhase phase in trailPhases)
        {
            Queue<int> agentQueue;
            if (phase.availableAgents.Length == 1)
            {
                agentQueue = RandomisedEvenBinaryQueue(currentTrial.tracksPerBlock);
            }
            else
            {
                agentQueue = new Queue<int>(new int[currentTrial.tracksPerBlock]);
            }
            Queue<int> strongTrackQueue = RandomTrackOrder(phase.availableStrongSequences.Length);
            Queue<int> weakTrackQueue = RandomTrackOrder(phase.availableWeakSequences.Length);
            Queue<int> strongOrWeakQueue = RandomisedEvenBinaryQueue(currentTrial.tracksPerBlock);

            for (int i = 0; i < currentTrial.tracksPerBlock; i++)
            {
                PlaylistItem currentTrack;
                if (strongOrWeakQueue.Dequeue() == 0)
                {
                    currentTrack = new PlaylistItem(
                        phase.availableStrongSequences[strongTrackQueue.Dequeue()],
                        currentTrial.trackTimeSecs,
                        false
                    );
                }
                else
                {
                    currentTrack = new PlaylistItem(
                        phase.availableStrongSequences[weakTrackQueue.Dequeue()],
                        currentTrial.trackTimeSecs,
                        false
                    );
                }

                UpdateCurrentPartnerStored(phase.availableAgents[agentQueue.Dequeue()]);

                currentPlaylist = Playlist.CreatePlaylist(
                    new PlaylistItem[] {
                    shownBreakItem,
                    currentTrack,
                    hiddenBreakItem,
                    inteferenceItem,
                    hiddenBreakItem,
                    recallItem
                    }
                );

                yield return subCoroutine = StartCoroutine(IteratePlaylist());
            }
        }
    }

    private Queue<int> RandomisedEvenBinaryQueue(int length)
    {
        Queue<int> binaryQueue = new Queue<int>(length);
        int[] binaryDigitUsageCount = new int[2];
        for (int i = 0; i < length; i++)
        {
            int randomBinaryDigit = UnityEngine.Random.Range(0, 2);
            if (binaryDigitUsageCount[randomBinaryDigit] >= length / 2)
            {
                binaryQueue.Enqueue(1 - randomBinaryDigit);
                Debug.Log(1 - randomBinaryDigit);
            }
            else
            {
                binaryQueue.Enqueue(randomBinaryDigit);
                binaryDigitUsageCount[randomBinaryDigit]++;
                Debug.Log(randomBinaryDigit);
            }
        }
        return binaryQueue;
    }

    private Queue<int> RandomTrackOrder(int availableTrackCount)
    {
        Queue<int> trackIndexQueue = new Queue<int>(currentTrial.tracksPerBlock / 2);
        List<int> availableTracks = new List<int>(availableTrackCount);
        availableTracks.AddRange(Enumerable.Range(0, availableTrackCount));
        for (int i = 0; i < currentTrial.tracksPerBlock / 2; i++)
        {
            int randomTrackIndex = UnityEngine.Random.Range(0, availableTracks.Count - 1);
            trackIndexQueue.Enqueue(availableTracks[randomTrackIndex]);
        }
        return trackIndexQueue;
    }

    public void SetCurrentPlaylist(Playlist playlist)
    {
        currentPlaylist = playlist;
    }

    public void SetRandomisedTrial(RandomisedTrial trial)
    {
        currentTrial = trial;
    }

    private void UpdateCurrentPartnerStored(AgentSO agent)
    {
        _currentPartner = agent;
    }

    public void EndRecall()
    {
        recalling = false;
    }

}
