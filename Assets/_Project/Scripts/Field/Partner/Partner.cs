using System.Collections.Generic;
using System.Linq;
using DrumRhythmGame.Data;
using DrumRhythmGame.Field;
using RootMotion.FinalIK;
using UnityEngine;

namespace _Project.Scripts.Field.Partner
{
    [RequireComponent(typeof(InteractionSystem))]
    [DefaultExecutionOrder(1)]
    public class Partner : MonoBehaviour
    {
        [Header("Animation setting")]
        [SerializeField] private InteractionObject crashCymbal;
        [SerializeField] private InteractionObject highTom;
        [SerializeField] private InteractionObject middleTom;
        [SerializeField] private InteractionObject snareDrum;

        [SerializeField] private PartnerHandPreference partnerHandPreference;

        private Dictionary<InstrumentType, InteractionObject> _instruments;
        private Dictionary<PartnerBehaviourType, IPartnerBehaviour> _behaviours;

        private void Awake()
        {
            var interactionSystem = GetComponent<InteractionSystem>();
            _instruments = new Dictionary<InstrumentType, InteractionObject>()
            {
                { InstrumentType.CrashCymbal, crashCymbal },
                { InstrumentType.LeftHighTom, highTom },
                { InstrumentType.RightMiddleTom, middleTom },
                { InstrumentType.SnareDrum, snareDrum }
            };

            _behaviours = new Dictionary<PartnerBehaviourType, IPartnerBehaviour>()
            {
                { PartnerBehaviourType.None, new NoneBehaviour() },
                { PartnerBehaviourType.Follow, new FollowerBehaviour(interactionSystem, _instruments) },
                { PartnerBehaviourType.Rhythm, new RhythmBehaviour(interactionSystem, _instruments) }
            };
        }

        private void OnEnable()
        {
            if (PartnerManager.Instance.CurrentBehaviourPartnerOne == PartnerBehaviourType.Rhythm)
            {
                ((RhythmBehaviour)_behaviours[PartnerManager.Instance.CurrentBehaviourPartnerOne]).Enable(partnerHandPreference);
            }
            else
            {
                _behaviours[PartnerManager.Instance.CurrentBehaviourPartnerOne].Enable();
            }

            Debug.Log($"[Partner: {name}] {PartnerManager.Instance.CurrentBehaviourPartnerOne} enabled.");
        }

        private void OnDisable()
        {
            _behaviours[PartnerManager.Instance.CurrentBehaviourPartnerOne].Disable();

            Debug.Log($"[Partner: {name}] {PartnerManager.Instance.CurrentBehaviourPartnerOne} disabled.");
        }

        public void SwitchType(PartnerBehaviourType type)
        {
            foreach (var behaviour in _behaviours.Values.Where(b => b.Enabled))
            {
                behaviour.Disable();
            }

            _behaviours[type].Enable();
        }
    }
}
