using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Field;
using _Project.Scripts.Systems;
using DrumRhythmGame.Data;
using DrumRhythmGame.Field;
using DrumRhythmGame.Systems;
using UnityEngine;
using UnityEngine.XR;

namespace DrumRhythmGame.Field
{
    [RequireComponent(typeof(MeshRenderer))]
    public class SpreadNoteHitEffect : MonoBehaviour
    {
        [SerializeField] private SpreadNote spreadNote;
        [SerializeField] private Color effectColor;
        [SerializeField] private float duration;
        [SerializeField] private InstrumentType instrumentType;
        
        private static readonly int Radius = Shader.PropertyToID("_radius");
        private static readonly int Color = Shader.PropertyToID("_color");

        private MeshRenderer _renderer;
        private float _noteHoleRadius;
        private Coroutine _coroutine;
        
        private void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();
            _noteHoleRadius = spreadNote.transform.localScale.x / transform.localScale.x;

            //effectColor = UnityEngine.Color.white;
            effectColor.a = 0;

            _renderer.material = new Material(_renderer.material.shader);
            _renderer.material.SetColor(Color, effectColor);
            _renderer.material.SetFloat(Radius, _noteHoleRadius);

            //EventManager.DrumHitEvent += OnHit;
        }

        private void OnHit(ActorType actor, InstrumentType type, XRNode node)
        {
            if (actor != ActorType.Player || type != instrumentType) return;
            
            StartEffect();
        }

        private void StartEffect()
        {
            CancelEffect();

            _coroutine = StartCoroutine(HitEffectCoroutine());
        }

        private void CancelEffect()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }

        private IEnumerator HitEffectCoroutine()
        {
            var material = _renderer.material;
            var color = effectColor;
            color.a = 0.8f;

            var startTime = Time.time;
            float progress;

            while ((progress = (Time.time - startTime) / duration) < 1)
            {
                color.a = (1f - progress) * 0.8f;
                var radius = _noteHoleRadius + (1f - _noteHoleRadius) * progress;
                
                material.SetColor(Color, color);
                material.SetFloat(Radius, radius);
                _renderer.material = material;
                
                yield return new WaitForEndOfFrame();
            }

            color.a = 0;
            material.SetColor(Color, color);
            material.SetFloat(Radius, _noteHoleRadius);
        }

        private void OnDestroy()
        {
            CancelEffect();
            
            EventManager.DrumHitEvent -= OnHit;
        }
    }
}