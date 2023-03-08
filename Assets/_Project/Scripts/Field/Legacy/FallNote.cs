using System;
using System.Collections;
using DrumRhythmGame.Data;
using UnityEngine;
using UnityEngine.XR;

namespace DrumRhythmGame.Field
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class FallNote : MonoBehaviour
    {
        private Coroutine _coroutine;

        private float _duration;
        private float _positionToHit;
        private float _positionToDestroySelf;
        private Action<InstrumentType> _onMoveFinishHandler;

        private bool _isMoving = false;

        public void Initialize(float duration, float positionToHit, float positionToDestroySelf)
        {
            _duration = duration;
            _positionToHit = positionToHit;
            _positionToDestroySelf = positionToDestroySelf;
        }

        public void StartMoving(Action<InstrumentType> onMoveFinishHandler, InstrumentType type)
        {
            _onMoveFinishHandler = onMoveFinishHandler;
            _coroutine = StartCoroutine(MoveCoroutine(type));
            _isMoving = true;
        }
        
        private IEnumerator MoveCoroutine(InstrumentType type)
        {
            var startTime = Time.time;
            var position = transform.position;
            var initialPositionY = position.y;
            var distance = initialPositionY - _positionToHit;
            
            while (transform.position.y > _positionToDestroySelf)
            {
                var elapsedTime = Time.time - startTime;
                position.y = initialPositionY - distance * elapsedTime / _duration;
                transform.position = position;
                
                yield return new WaitForFixedUpdate();
            }
            
            // Destroy self 
            _isMoving = false;
            _onMoveFinishHandler(type);
        }

        private void OnDisable()
        {
            _isMoving = false;
            if (_coroutine == null) return;
            
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }
}