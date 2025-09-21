using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Systems;
using UnityEngine;

public class RecallManager : MonoBehaviour
{
    private int recallCount;
    private void OnEnable()
    {
        recallCount = 1;
    }
    public void ResartRecall()
    {
        DrumLogger.Instance.SetCurrentTrail($"Recall(attempt {++recallCount})");
        EventManager.InvokeTimerStartEvent();
    }

    public void EndRecall()
    {
        PlaylistController.Instance.EndRecall();
        gameObject.SetActive(false);
    }
}
