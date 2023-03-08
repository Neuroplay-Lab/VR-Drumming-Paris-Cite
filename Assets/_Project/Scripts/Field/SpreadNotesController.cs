using System.Collections.Generic;
using _Project.Scripts.Data;
using _Project.Scripts.Field;
using _Project.Scripts.Systems;
using DrumRhythmGame.Data;
using DrumRhythmGame.Systems;
using UnityEngine;
using UnityEngine.XR;

namespace DrumRhythmGame.Field
{
    public class SpreadNotesController : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private SpreadNote[] notes;
        [SerializeField] private float duration;
        [SerializeField] private MusicSequence sequence;

        #endregion

        private readonly InstrumentType[] _instrumentTypes =
        {
            InstrumentType.CrashCymbal,
            InstrumentType.LeftHighTom,
            InstrumentType.RightMiddleTom,
            InstrumentType.SnareDrum
        };

        private readonly Dictionary<HitStatus, int> _points = new Dictionary<HitStatus, int>
        {
            {HitStatus.None, 0},
            {HitStatus.Bad, 1},
            {HitStatus.Good, 5},
            {HitStatus.Perfect, 10}
        };

        private int _beatCounter;
        private bool _loopScore;

        private MusicScore _musicScore;
        private float _noteMarginTime;
        private Dictionary<InstrumentType, SpreadNote> _notes;

        #region Event Functions

        private void OnEnable()
        {
            EventManager.DrumHitEvent += OnDrumHit;
            EventManager.MusicSettingChangeEvent += UpdateScore;
        }

        private void OnDisable()
        {
            EventManager.DrumHitEvent -= OnDrumHit;
            EventManager.MusicSettingChangeEvent -= UpdateScore;
        }

        #endregion

        private void UpdateScore(MusicSetting setting)
        {
            Debug.Log("UpdateScore");
            _musicScore = MusicScoreLoader.Load(setting.scoreAsset, setting.instrumentCount);
            _noteMarginTime = setting.noteMarginTime;
            _loopScore = setting.loopScore;

            _notes = new Dictionary<InstrumentType, SpreadNote>();
            foreach (var note in notes)
            {
                _notes.Add(note.instrumentType, note);
                note.onSpreadEnd += OnNoteEnd;
            }

            MusicSequence.Instance.SetTrigger(-duration, OnTime);
        }

        private void OnDrumHit(ActorType actor, InstrumentType type, XRNode node)
        {
            if (actor == ActorType.Player)
            {
                var status = _notes[type].Hit();
                GameData.Instance.AddScorePoint(_points[status]);

                //_notes[type].CancelSpread();
            }
        }

        private void OnNoteEnd(InstrumentType type)
        {
            GameData.Instance.AddScorePoint(0);
        }

        /// <summary>
        ///     Called {reachTime} seconds before the beat
        /// </summary>
        /// <param name="delay"></param>
        private void OnTime(float delay)
        {
            if (_beatCounter >= _musicScore.rowCount)
            {
                // Do nothing if the music score has been ended and loop flag is false
                if (_loopScore == false) return;

                // Reset counter
                _beatCounter %= _musicScore.rowCount;
            }

            for (var i = 0; i < _musicScore.columnCount; i++)
                if (_musicScore.score[_beatCounter, i])
                {
                    _notes[_instrumentTypes[i]].StartSpread(duration);

                    EventManager.InvokeMusicScoreNoteSetEvent(_instrumentTypes[i], _noteMarginTime);
                }

            _beatCounter++;
        }
    }
}