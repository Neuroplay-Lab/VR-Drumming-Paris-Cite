using System.Collections;
using _Project.Scripts.Data;
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
        { StopCoroutine(coroutine); }
    }

    private IEnumerator IteratePlaylist()
    {
        foreach (PlaylistItem item in currentPlaylist.playlistItems)
        {
            Debug.Log($"Starting {item.duration}");
            yield return new WaitForSeconds(item.duration);
            Debug.Log($"Ending {item.duration}");
        }
    }

    public void SetCurrentPlaylist(Playlist playlist)
    {
        currentPlaylist = playlist;
    }

}
