using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.Systems
{
    /// <summary>
    ///     A system that drives the assigned BlendShape weights of a skinned mesh renderer to reflect blinking
    /// </summary>
    [HelpURL("https://docs.unity3d.com/Manual/BlendShapes.html")]
    public class BlinkController : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] [Tooltip("The mesh renderer to drive the blend shapes of")]
        private SkinnedMeshRenderer meshRenderer;

        [Header("Controls")] [SerializeField] [Range(0, 1)] [Tooltip("How fast does the blink happen?")]
        private float blinkSpeed = 0.3f;

        [SerializeField] [Range(0, 60)] [Tooltip("How spaced apart are the blinks?")]
        private float blinkInterval = 5f;

        [SerializeField] [Range(0, 60)] [Tooltip("How much variability should we add to the blink interval?")]
        private float blinkRandomness = 2f;

        [Header("BlendShapeIndexes")] [SerializeField]
        private List<int> blinkBlendShapeIndexes = new List<int> {22, 24};

        #endregion

        private readonly Vector3 _gizmosYOffset = new Vector3(0, 1.5F, 0);

        private bool _isBlinking;

        private float _lastTimeBlinked;
        private float _variance;

        #region Event Functions

        private void OnEnable()
        {
            _isBlinking = true;
            StartCoroutine(BlinkInterval());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            _isBlinking = false;
        }
#if UNITY_EDITOR
        /// <summary>
        ///     Displays a countdown timer until the next blink
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying) return;
            var nextBlink = blinkInterval + _variance;
            var timeSinceLastBlink = Time.time - _lastTimeBlinked;
            var timeUntilNextBlink = nextBlink - timeSinceLastBlink;
            Handles.Label(transform.position + _gizmosYOffset, timeUntilNextBlink.ToString("F2"));
        }
#endif

        #endregion

        private IEnumerator BlinkInterval()
        {
            while (_isBlinking)
            {
                _variance = Random.Range(0, blinkRandomness);
                yield return new WaitForSeconds(blinkInterval + _variance);
                _lastTimeBlinked = Time.time;
                StartCoroutine(Blink());
            }
        }

        /// <summary>
        ///     Linearly interpolates the assigned blend shapes to 100% and back to 0% over blinkTime
        ///     @note - Honestly, this would be two lines with a DOTween tween, but I wanted to keep this project dependency-free
        /// </summary>
        /// <returns></returns>
        private IEnumerator Blink()
        {
            var blinkTime = blinkSpeed;
            var blinkTimeHalf = blinkTime / 2;

            var t = 0f;
            while (t < blinkTimeHalf)
            {
                t += Time.deltaTime;
                var blendShapeWeight = Mathf.Lerp(0, 100, t / blinkTimeHalf);
                foreach (var blendShapeIndex in blinkBlendShapeIndexes)
                    meshRenderer.SetBlendShapeWeight(blendShapeIndex, blendShapeWeight);
                yield return null;
            }

            t = 0f;
            while (t < blinkTimeHalf)
            {
                t += Time.deltaTime;
                var blendShapeWeight = Mathf.Lerp(100, 0, t / blinkTimeHalf);
                foreach (var blendShapeIndex in blinkBlendShapeIndexes)
                    meshRenderer.SetBlendShapeWeight(blendShapeIndex, blendShapeWeight);
                yield return null;
            }
        }
    }
}