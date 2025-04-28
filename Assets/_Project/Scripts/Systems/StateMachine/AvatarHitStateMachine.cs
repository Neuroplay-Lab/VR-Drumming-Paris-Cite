using RootMotion.FinalIK;
using UnityEngine;

public class AvatarHitStateMachine : StateMachine<AvatarHitStateMachine.E_AvatarDrumHitState>
{
    public enum E_AvatarDrumHitState
    {
        Waiting,
        Hitting,
        Reset
    }

    public enum ManagedHand
    {
        Right,
        Left
    }

    private HitStateContext _context;
    private float _hitDuration;
    private float _resetDuration;

    public static AvatarHitStateMachine Create(GameObject where, FullBodyBipedIK fullBodyBipedIK, ManagedHand managedHand, float hitDuration, float resetDuration)
    {
        AvatarHitStateMachine createdMachine = where.AddComponent<AvatarHitStateMachine>();
        if (managedHand == ManagedHand.Right)
        {
            createdMachine.SetContext(new HitStateContext(fullBodyBipedIK.solver.rightHandEffector));
        }
        else
        {
            createdMachine.SetContext(new HitStateContext(fullBodyBipedIK.solver.leftHandEffector));
        }
        createdMachine.SetHitDuration(hitDuration);
        createdMachine.SetResetDuration(resetDuration);

        return createdMachine;
    }

    void Start()
    {
        InitialiseStates();
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
        TransitionToState(E_AvatarDrumHitState.Hitting);
    }

    public void SetContext(HitStateContext context)
    {
        _context = context;
    }

    public void SetHitDuration(float hitDuration)
    {
        _hitDuration = hitDuration;
    }

    public void SetResetDuration(float resetDuration)
    {
        _resetDuration = resetDuration;
    }
}