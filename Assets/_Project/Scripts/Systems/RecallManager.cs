using _Project.Scripts.Systems;
using UnityEngine;
using UnityEngine.UI;

public class RecallManager : MonoBehaviour
{
    private int recallCount;
    [SerializeField] private Button RestartButton;
    [SerializeField] private Button ContinueButton;
    private void OnEnable()
    {
        recallCount = 1;
        if (PlaylistController.Instance.IsRecalling())
        {
            RestartButton.gameObject.SetActive(true);
        }
        else
        {
            RestartButton.gameObject.SetActive(false);
        }
    }
    public void RestartRecall()
    {
        DrumLogger.Instance.SetCurrentTrail($"Recall(attempt {++recallCount})");
        EventManager.InvokeTimerStartEvent();
    }

    public void Continue()
    {
        if (PlaylistController.Instance.IsRecalling())
        {
            PlaylistController.Instance.EndRecall();
        }
        else
        {
            PlaylistController.Instance.ContinueToNextTrialPhase();
        }
        gameObject.SetActive(false);
    }
}
