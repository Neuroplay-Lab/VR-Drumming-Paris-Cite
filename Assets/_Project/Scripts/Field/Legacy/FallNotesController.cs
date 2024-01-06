// TODO: Potentially now redundant - check and remove

using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Systems;
using DrumRhythmGame.Data;
using DrumRhythmGame.Field;
using UnityEngine;
using UnityEngine.XR;

namespace _Project.Scripts.Field.Legacy
{
    public class FallNotesController : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private GameObject notePrefab;

        [SerializeField] private MusicSequence sequence;

        #endregion

        private readonly InstrumentType[] _instrumentTypes =
        {
            InstrumentType.CrashCymbal,
            InstrumentType.LeftHighTom,
            InstrumentType.RightMiddleTom,
            InstrumentType.SnareDrum
        };

        private int _beatCounter;
        private bool _loopScore;
        private Dictionary<InstrumentType, SpriteRenderer> _markers;

        private MusicScore _musicScore;
        private Vector3 _noteDestroyPosition;
        private Vector3 _noteGenerationPosition;
        private Vector3 _noteHitPosition;
        private float _noteMarginTime;
        private Dictionary<InstrumentType, List<FallNote>> _notes;

        #region Event Functions

        private void Start()
        {
            var setting = sequence.Setting;

            // Load given music score
            _musicScore = MusicScoreLoader.Load(setting.scoreAsset, setting.instrumentCount);
            _noteMarginTime = setting.noteMarginTime;
            _loopScore = setting.loopScore;

            // Calculate static positions
            var center = transform.position;
            var halfHeight = transform.localScale.y * 0.5f;
            _noteGenerationPosition = center + new Vector3(0, halfHeight * 0.9f, 0);
            _noteHitPosition = center + new Vector3(0, -halfHeight * 0.8f, 0);
            _noteDestroyPosition = center + new Vector3(0, -halfHeight * 0.9f, 0);

            // Create markers to hit
            _markers = new Dictionary<InstrumentType, SpriteRenderer>();
            for (var i = 0; i < _musicScore.columnCount; i++)
            {
                var pos = _noteHitPosition + new Vector3(-HorizontalOffset(i), 0, 0);
                var marker = Instantiate(notePrefab, pos, Quaternion.identity);
                marker.transform.localScale *= 2f;

                var spriteRenderer = marker.GetComponent<SpriteRenderer>();
                spriteRenderer.sortingOrder = 1;
                _markers.Add(_instrumentTypes[i], spriteRenderer);
            }

            _notes = new Dictionary<InstrumentType, List<FallNote>>();
            foreach (var type in _instrumentTypes) _notes[type] = new List<FallNote>();

            // Register handler for beat
            MusicSequence.Instance.SetTrigger(-_noteMarginTime, OnTime);

            // Register handler for hit
            EventManager.DrumHitEvent += OnDrumHit;
            StartCoroutine(ResetMarkerColorCoroutine());
        }

        #endregion

        private void OnDrumHit(ActorType actor, InstrumentType type, XRNode node)
        {
            if (actor != ActorType.Player) return;

            _markers[type].color = Color.yellow;

            if (_notes[type].Count == 0) return;

            var targetNote = _notes[type][0];

            var error = Mathf.Abs(_noteHitPosition.y - targetNote.transform.position.y);
            Debug.Log($"Hit error of {type} : {error}");

            if (error < 0.1f)
            {
                // Excellent hit
                GameData.Instance.AddScorePoint(10);
                Destroy(targetNote.gameObject);
                _notes[type].RemoveAt(0);
            }
            else if (error < 0.2f)
            {
                // Good hit
                GameData.Instance.AddScorePoint(5);
                Destroy(targetNote.gameObject);
                _notes[type].RemoveAt(0);
            }
            else if (error < 0.3f)
            {
                // Bad hit
                GameData.Instance.AddScorePoint(1);
                Destroy(targetNote.gameObject);
                _notes[type].RemoveAt(0);
            }
            else
            {
                GameData.Instance.AddScorePoint(0);
            }
        }

        private void OnNoteReachEnd(InstrumentType type)
        {
            GameData.Instance.AddScorePoint(0);
            Destroy(_notes[type][0].gameObject);
            _notes[type].RemoveAt(0);
        }

        private IEnumerator ResetMarkerColorCoroutine()
        {
            float Whiten(float primaryColor)
            {
                return primaryColor + 0.05f <= 1f ? primaryColor + 0.05f : 1f;
            }

            while (gameObject.activeSelf)
            {
                foreach (var spriteRenderer in _markers.Values)
                {
                    var color = spriteRenderer.color;
                    color.r = Whiten(color.r);
                    color.g = Whiten(color.g);
                    color.b = Whiten(color.b);

                    spriteRenderer.color = color;
                }

                yield return null;
            }
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
                    var genPos = _noteGenerationPosition + new Vector3(-HorizontalOffset(i), 0, 0);
                    var noteObj = Instantiate(notePrefab, genPos, Quaternion.identity);
                    noteObj.transform.localScale *= 1.5f;

                    var renderer = noteObj.GetComponent<SpriteRenderer>();
                    renderer.color = Color.red;
                    renderer.sortingOrder = 2;

                    var note = noteObj.GetComponent<FallNote>();
                    note.Initialize(_noteMarginTime, _noteHitPosition.y, _noteDestroyPosition.y);
                    note.StartMoving(OnNoteReachEnd, _instrumentTypes[i]);
                    _notes[_instrumentTypes[i]].Add(note);

                    EventManager.InvokeMusicScoreNoteSetEvent(_instrumentTypes[i], _noteMarginTime);
                }

            _beatCounter++;
        }

        private float HorizontalOffset(int order)
        {
            return 0.4f * transform.localScale.x * (1f - 2f * order / (_musicScore.columnCount - 1));
        }
    }
}