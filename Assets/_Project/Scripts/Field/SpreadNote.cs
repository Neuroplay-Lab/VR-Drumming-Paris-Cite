using System;
using System.Collections;
using _Project.Scripts.Systems;
using DrumRhythmGame.Data;
using UnityEngine;

namespace _Project.Scripts.Field
{
    [RequireComponent(typeof(MeshRenderer))]
    public class SpreadNote : MonoBehaviour
    {
        private const float KeepAlive = 0.5f;
        
        private const float PerfectThreshold = 0.1f;
        private const float GoodThreshold = 0.3f;
        private static readonly int Radius = Shader.PropertyToID("_radius");
        private static readonly int Color = Shader.PropertyToID("_color");

        private MeshRenderer _renderer;
        private bool _spreading = false;
        private HitStatus _hitStatus = default;
        private Coroutine _coroutine;

        [SerializeField] private Color _color = default;
        public InstrumentType instrumentType;
        public Action<InstrumentType> onSpreadEnd;

        private void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();

            _renderer.material = new Material(_renderer.material.shader);
            _renderer.material.SetColor(Color, _color);
            _renderer.material.SetFloat(Radius, 0);

            EventManager.MusicResetEvent += CancelSpread;
        }

        private void OnDestroy()
        {
            EventManager.MusicResetEvent -= CancelSpread;
        }

        public void StartSpread(float duration)
        {
            _coroutine = StartCoroutine(SpreadCoroutine(duration, KeepAlive));
        }

        public void CancelSpread()
        {
            if (_spreading && _coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
                _renderer.material.SetFloat(Radius, 0);
                _spreading = false;
            }
        }

        public HitStatus Hit() => _hitStatus;

        private IEnumerator SpreadCoroutine(float duration, float keepAlive)
        {
            var currentTime = 0f;
            _spreading = true;
            
            while (_spreading)
            {
                var progress = currentTime / duration;
                
                // keepAliveによってradius > 1となる場合は1に丸める
                var radius = progress >= 1 ? 1 : progress;
                
                _renderer.material.SetFloat(Radius, radius);
                
                yield return new WaitForFixedUpdate();

                currentTime += Time.fixedDeltaTime;
                
                // Perfect or Good or Bad
                _hitStatus = DetectHitStatus(Mathf.Abs(duration - currentTime));
                
                _spreading = currentTime < duration + keepAlive;
            }
            
            // 半径をゼロに戻す
            _renderer.material.SetFloat(Radius, 0);
            
            // エフェクトが出てないときは、Hit()が呼ばれてもNoneを返すように
            _hitStatus = HitStatus.None;

            onSpreadEnd(instrumentType);
        }

        private HitStatus DetectHitStatus(float error)
        {
            if (error < PerfectThreshold) return HitStatus.Perfect;
            
            if (error < GoodThreshold) return HitStatus.Good;
            
            return HitStatus.Bad;
        }
    }
}
