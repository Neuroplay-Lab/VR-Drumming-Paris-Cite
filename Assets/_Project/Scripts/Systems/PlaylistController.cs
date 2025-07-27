using System.Collections;
using _Project.Scripts.Data;
using _Project.Scripts.Systems;
using UnityEngine;

public class PlaylistController : MonoBehaviour
{

    public static PlaylistController Instance;
    private Playlist currentPlaylist;
    // Start is called before the first frame update

    private Coroutine coroutine;
    void Start()
    {
        Instance = this;
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
        }
    }

    private IEnumerator IteratePlaylist()
    {
        foreach (PlaylistItem item in currentPlaylist.playlistItems)
        {
            EventManager.InvokeMusicSettingChangeEvent(item.track);
            MusicSequence.Instance.Play();
            yield return new WaitForSeconds(item.duration);
            MusicSequence.Instance.Reset();
        }
    }

    public void SetCurrentPlaylist(Playlist playlist)
    {
        currentPlaylist = playlist;
    }

}
