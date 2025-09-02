using System;
using System.Collections;
using System.Linq;
using _Project.Scripts.Data;
using _Project.Scripts.Systems;
using UnityEngine;

public class PlaylistController : MonoBehaviour
{

    public static PlaylistController Instance;
    private Playlist currentPlaylist;
    // Start is called before the first frame update

    private AgentSO _currentPartner = null;
    public bool PartnerIsActive = false;

    private Coroutine coroutine;
    void Start()
    {
        Instance = this;
        EventManager.AgentSelected += UpdateCurrentPartnerStored;
    }

    public void Play()
    {
        coroutine = StartCoroutine(IteratePlaylist());
    }

    public void Reset()
    {
        if (coroutine != null)
        {
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
            yield return new WaitForSeconds(item.duration);
            MusicSequence.Instance.Reset();
        }
        if (currentPlaylist.playlistItems[currentPlaylist.playlistItems.Length - 1].track.name.Trim().ToLower() != "recall")
        {
            Reset();
        }
    }

    public void SetCurrentPlaylist(Playlist playlist)
    {
        currentPlaylist = playlist;
    }

    private void UpdateCurrentPartnerStored(AgentSO agent)
    {
        _currentPartner = agent;
    }

}
