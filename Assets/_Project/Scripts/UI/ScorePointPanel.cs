using System;
using _Project.Scripts.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    /// <summary>
    /// Responsible for displaying synchrony score and error rate in
    /// researchers view.
    /// </summary>
    public class ScorePointPanel : MonoBehaviour
    {
        #region Serialized Fields

        // Synchrony GUI elements
        [SerializeField] private TextMeshProUGUI scorePointText;
        [SerializeField] private TextMeshProUGUI synchronousRateText;
        [SerializeField] private Slider synchronousRateSlider;

        // Error GUI elements
        [Space] [SerializeField] private TextMeshProUGUI errorRateText;
        [SerializeField] private Slider errorRateSlider;

        #endregion

        private float _averageErrorRate;

        private ErrorRateController _errorRateController;
        private float _synchronousRate;

        #region Event Functions

        private void Awake()
        {
            _errorRateController = ErrorRateController.Instance;
            // We don't visualise the score points but just in case we want to use it in the future
            if (scorePointText != null) 
                EventManager.PlayerScoreUpdateEvent += OnPlayerPointUpdate;
            
            EventManager.SyncRateChangeEvent += OnSyncRateUpdate;
            EventManager.ErrorRateChanged += OnErrorRateUpdate;

            EventManager.MusicResetEvent += SetSliderValuesToDefault;
        }

        /// <summary>
        ///  Handles animation of the sliders
        ///  <!--Could probably add a tweening library to make this more readable, like DOTween-->
        /// </summary>
        private void Update()
        {
            // TODO: Extract magic numbers to constants or a settings file
            synchronousRateSlider.value =
                Mathf.Lerp(synchronousRateSlider.value, _synchronousRate, Time.deltaTime * 10);

            errorRateSlider.value =
                Mathf.Lerp(errorRateSlider.value, 100 - _averageErrorRate / 0.5f * 100, Time.deltaTime * 15);
        }

        private void OnDestroy()
        {
            if (scorePointText != null)
                EventManager.PlayerScoreUpdateEvent += OnPlayerPointUpdate;
            EventManager.SyncRateChangeEvent -= OnSyncRateUpdate;
        }

        #endregion

        private void SetSliderValuesToDefault()
        {
            _synchronousRate = 0;
            _averageErrorRate = 0;

            synchronousRateText.text = "0%";
            errorRateText.text = "100%";
        }

        private void OnErrorRateUpdate(float errorRate)
        {
            _averageErrorRate = _errorRateController.GetAverageErrorRate();
            var rate = Math.Abs(100 - _averageErrorRate / 0.5f * 100);
            errorRateText.text = $"{rate:F0}%";
        }

        private void OnPlayerPointUpdate(int addedPoint)
        {
            scorePointText.text = $"{GameData.Instance.ScorePoint}";
        }

        private void OnSyncRateUpdate(float rate)
        {
            synchronousRateText.text = $"{rate * 100:F0}%";
            _synchronousRate = rate;
        }
    }
}