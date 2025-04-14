using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Assertions;

public class AvatarHandHitStateMachine : StateMachine<AvatarHandHitStateMachine.E_AvatarDrumHitState>
{
    public enum E_AvatarDrumHitState
    {
        Waiting,
        Hitting,
        Reset
    }

    private HitStateContext _context;

    [SerializeField] private TwoBoneIKConstraint _ikConstraint;
    [SerializeField] private float _hitDuration = 1f;
    [SerializeField] private float _resetDuration = 2f;

    void Awake()
    {
        ValidateContraints();
        _context = new HitStateContext(_ikConstraint);
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

    public void TriggerHit() {
        
    }
}