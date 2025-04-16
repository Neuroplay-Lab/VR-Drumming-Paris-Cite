using UnityEngine;
using RootMotion.FinalIK;
using UnityEngine.Assertions;
using UnityEngine.Animations.Rigging;

public class AvatarHitStateMachine : StateMachine<AvatarHitStateMachine.E_AvatarDrumHitState>
{
    public enum E_AvatarDrumHitState
    {
        Waiting,
        Hitting,
        Reset
    }

    private enum ManagedHand
    {
        Right,
        Left
    }

    private HitStateContext _context;

    [SerializeField] private FullBodyBipedIK _ikConstraint;
    [SerializeField] private float _hitDuration = 1f;
    [SerializeField] private float _resetDuration = 2f;

    [SerializeField] private ManagedHand managedHand;

    void Awake()
    {
        ValidateContraints();
        if (managedHand == ManagedHand.Right)
        {
            _context = new HitStateContext(_ikConstraint.solver.rightHandEffector);
        }
        else
        {
            _context = new HitStateContext(_ikConstraint.solver.leftHandEffector);
        }
        InitialiseStates();
    }

    private void ValidateContraints()
    {
        Assert.IsNotNull(_ikConstraint, "IK Contraint is not Assigned");
    }

    private void InitialiseStates()
    {
        States.Add(E_AvatarDrumHitState.Waiting, new WaitingState(_context, E_AvatarDrumHitState.Waiting));
        States.Add(E_AvatarDrumHitState.Hitting, new HittingState(_hitDuration, _context, E_AvatarDrumHitState.Hitting));
        States.Add(E_AvatarDrumHitState.Reset, new ResetState(_resetDuration, _context, E_AvatarDrumHitState.Reset));
        CurrentState = States[E_AvatarDrumHitState.Reset];
    }

    public void TriggerHit()
    {
        if (!CurrentState.Equals(E_AvatarDrumHitState.Hitting))
        {
            TransitionToState(E_AvatarDrumHitState.Hitting);
        }
        ((HittingState)States[E_AvatarDrumHitState.Hitting]).SetHitDuration(_hitDuration);
        ((ResetState)States[E_AvatarDrumHitState.Reset]).SetResetDuration(_resetDuration);
    }
}