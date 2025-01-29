﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Data;
using UnityEngine;

namespace _Project.Scripts.Systems
{
    /// <summary>
    /// Handles the selection, playing and prompt display for each track.
    /// </summary>
    [DefaultExecutionOrder(1)]
    [RequireComponent(typeof(AudioSource))]
    public class MusicSequence : SingletonMonoBehaviour<MusicSequence>
    {
        private static readonly int Playing = Animator.StringToHash("isPlaying");

        #region Serialized Fields

        [SerializeField] private Animator promptAnimator;

        [SerializeField] private MusicSetting setting;

        #endregion

        private readonly Dictionary<float, Action<float>> _triggerEvents = new Dictionary<float, Action<float>>();
        private int bpm;
        private Coroutine coroutine;

        private AudioSource source;

        public bool IsPlaying { get; private set; }

        public MusicSetting Setting
        {
            get => setting;
            private set
            {
                setting = value;
                bpm = setting.customBpm > 0 ? setting.customBpm : UniBpmAnalyzer.AnalyzeBpm(setting.bgm);
                source.clip = setting.bgm;
                Debug.Log($"BPM: {bpm}");
            }
        }

        public float CurrentTime { get; private set; } = -1f;

        #region Event Functions

        protected override void Awake()
        {
            base.Awake();

            source = GetComponent<AudioSource>();
            source.playOnAwake = false;
            bpm = setting.customBpm > 0 ? setting.customBpm : UniBpmAnalyzer.AnalyzeBpm(setting.bgm);
            source.clip = setting.bgm;
        }

        public void Reset()
        {
            if (!source.isPlaying) return;

            IsPlaying = false;

            promptAnimator.SetBool(Playing, false);
            promptAnimator.gameObject.SetActive(false);
            source.Stop();
            StopCoroutine(coroutine);
            coroutine = null;
            CurrentTime = -1f;

            EventManager.InvokeMusicResetEvent();
        }

        private void OnEnable()
        {
            EventManager.CueStateChanged += CueStateChanged;
            EventManager.MusicSettingChangeEvent += MusicSettingChanged;
        }

        private void OnDisable()
        {
            EventManager.CueStateChanged -= CueStateChanged;
            EventManager.MusicSettingChangeEvent -= MusicSettingChanged;
        }

        #endregion

        /// <summary>
        ///   Called when the music setting is changed
        /// </summary>
        /// <param name="newSetting"></param>
        private void MusicSettingChanged(MusicSetting newSetting)
        {
            Debug.Log("Music Setting Changed");
            if (_triggerEvents.Count > 0)
                _triggerEvents.Clear();
            Reset();
            Setting = newSetting;
        }

        private void CueStateChanged(bool state)
        {
            promptAnimator.gameObject.SetActive(state);
        }

        /// <summary>
        ///  Called to begin playing music
        /// </summary>
        public void Play()
        {
            if (source.isPlaying) return;

            IsPlaying = true;

            promptAnimator.gameObject.SetActive(true);
            source.Play();
            coroutine = StartCoroutine(BeatCoroutine(bpm));
            StartCoroutine(PlayPromptLoop());
            EventManager.InvokeMusicStartEvent();
        }

        /// <summary>
        /// Triggers denote a point in time at which a note should be played
        /// </summary>
        /// <param name="delayKey"></param>
        /// <param name="handler"></param>
        public void SetTrigger(float delayKey, Action<float> handler)
        {
            if (_triggerEvents.ContainsKey(delayKey))
                _triggerEvents[delayKey] += handler;
            else
                _triggerEvents.Add(delayKey, handler);
        }

        /// <summary>
        /// Creates a steady interval of time at which a note should be played
        /// </summary>
        /// <param name="bpm"> Beats per minute</param>
        /// <returns></returns>
        private IEnumerator BeatCoroutine(int bpm)
        {
            var startTime = Time.fixedTime + setting.initialDelayTime;

            var counters = _triggerEvents.Keys.ToDictionary(delayKey => delayKey, delayKey => 1);

            var interval = setting.speedMagnification * (setting.beatNumber / 16f) * 60f / bpm;

            while (true)
            {
                foreach (var trigger in _triggerEvents)
                {
                    CurrentTime = Time.fixedTime - startTime;
                    float sampleTime = source.timeSamples / (source.clip.frequency * interval);
                    if (Mathf.FloorToInt(sampleTime) >= Mathf.FloorToInt(source.clip.samples / (source.clip.frequency * interval)))
                    {
                        source.timeSamples = 0;
                    }
                    int adjustedTime = Mathf.FloorToInt(sampleTime - (setting.initialDelayTime / interval) - (trigger.Key / interval) + 1) % setting.beatNumber;
                    if (adjustedTime < 1 && counters[trigger.Key] > 1)
                    {
                        adjustedTime = setting.beatNumber + adjustedTime;
                    }

                    if (adjustedTime >= counters[trigger.Key])
                    {
                        // source.Pause();
                        // Debug.Break();

                        trigger.Value.Invoke(trigger.Key);
                        counters[trigger.Key]++;
                        if (counters[trigger.Key] > setting.beatNumber)
                        {
                            counters[trigger.Key] = 1;
                        }
                    }
                }

                // if (!source.isPlaying)
                //     source.Play();

                yield return null;
            }
        }

        /// <summary>
        /// Plays the prompt animation located in the background
        /// </summary>
        /// <returns></returns>
        private IEnumerator PlayPromptLoop()
        {
            Debug.Log("Prompt Loop");
            yield return new WaitForSeconds(setting.initialDelayTime);
            promptAnimator.SetBool(Playing, true);
        }
    }
}