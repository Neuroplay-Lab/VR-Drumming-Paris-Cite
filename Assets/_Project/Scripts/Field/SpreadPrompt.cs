using System;
using System.Collections;
using _Project.Scripts.Systems;
using DrumRhythmGame.Data;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.Field
{
    //[RequireComponent(typeof(MeshRenderer))]
    public class SpreadPrompt : MonoBehaviour
    {
        private const float KeepAlive = 0.5f;

        private const float PerfectThreshold = 0.1f;

        private const float GoodThreshold = 0.3f;

        //private static readonly int Radius = Shader.PropertyToID("_radius");
        private static readonly int Color = Shader.PropertyToID("_color");

        #region Serialized Fields

        [SerializeField] private RectTransform promptRectTransform;

        [SerializeField] private Color defaultColor;
        public InstrumentType instrumentType;

        #endregion

        private Coroutine _coroutine;
        private float _errorRate;
        private HitStatus _hitStatus;
        private bool _isCueHidden;
        private Color _offColor;

        private float _radius;

        //private MeshRenderer _renderer;
        private bool _spreading;
        public Action<InstrumentType> OnSpreadEnd;
        private TextMeshProUGUI textPro;

        #region Event Functions

        private void Awake()
        {
            //_renderer = GetComponent<MeshRenderer>();

            // _renderer.material = new Material(_renderer.material.shader);
            textPro = promptRectTransform.GetComponent<TextMeshProUGUI>();
            defaultColor.a = 1;

            _offColor = new Color(defaultColor.r, defaultColor.g, defaultColor.b, 0);

            //_color = _renderer.material.GetColor(Color);
            //_renderer.material.SetFloat(Radius, 0);

            EventManager.MusicResetEvent += CancelSpread;
            EventManager.CueStateChanged += HandleCueStateChange;
        }

        private void OnDestroy()
        {
            EventManager.MusicResetEvent -= CancelSpread;
            EventManager.CueStateChanged -= HandleCueStateChange;
        }
#if UNITY_EDITOR
        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 100, 200), $"{_errorRate:F2}");
        }
#endif

        #endregion

        public float CurrentErrorRate()
        {
            return Math.Abs(_errorRate);
        }

        private void HandleCueStateChange(bool state)
        {
            _isCueHidden = !state;
            promptRectTransform.gameObject.SetActive(!state);
        }

        public void StartSpread(float duration)
        {
            if (_isCueHidden) return;
            if (!promptRectTransform.gameObject.activeInHierarchy)
                promptRectTransform.gameObject.SetActive(true);
            _coroutine = StartCoroutine(SpreadCoroutine(duration, KeepAlive));
        }

        private void CancelSpread()
        {
            if (_spreading && _coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
                textPro.color = _offColor;
                _spreading = false;
            }

            _isCueHidden = false;
        }

        public HitStatus Hit()
        {
            return _hitStatus;
        }

        private IEnumerator SpreadCoroutine(float duration, float keepAlive)
        {
            var currentTime = 0f;
            _spreading = true;

            while (_spreading)
            {
                var progress = currentTime / duration;
                if (!_isCueHidden)
                {
                    _radius = progress >= 1 ? 1 : progress;
                    var newColor = new Color(defaultColor.r, defaultColor.g, defaultColor.b, _radius);
                    textPro.color = newColor;
                }

                yield return new WaitForFixedUpdate();
                currentTime += Time.fixedDeltaTime;

                // Perfect or Good or Bad
                _errorRate = Math.Abs(duration - currentTime);
                _hitStatus = DetectHitStatus(_errorRate);

                _spreading = currentTime < duration + keepAlive;
            }

            // 半径をゼロに戻す
            //_renderer.material.SetFloat(Radius, 0);
            //promptRectTransform.localScale = Vector3.zero;
            textPro.color = _offColor;

            // エフェクトが出てないときは、Hit()が呼ばれてもNoneを返すように
            _hitStatus = HitStatus.None;

            OnSpreadEnd(instrumentType);
        }

        private static HitStatus DetectHitStatus(float error)
        {
            if (error < PerfectThreshold) return HitStatus.Perfect;

            if (error < GoodThreshold) return HitStatus.Good;

            return HitStatus.Bad;
        }
    }
}