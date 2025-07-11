using _Project.Scripts.Data;
using UnityEngine;

public class PlaylistController : MonoBehaviour
{

    public static PlaylistController Instance;
    private Playlist currentPlaylist;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }


}
