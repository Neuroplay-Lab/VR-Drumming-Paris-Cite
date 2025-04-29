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
    private AnimationCurve _hitWeightCurve;
    private float _resetDuration;
    private AnimationCurve _resetWeightCurve;

    public static AvatarHitStateMachine Create(GameObject where, FullBodyBipedIK fullBodyBipedIK, ManagedHand managedHand, float hitDuration, AnimationCurve hitWeightCurve, float resetDuration, AnimationCurve resetWeightCurve)
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
        createdMachine.SetHitWeightCurve(hitWeightCurve);
        createdMachine.SetResetWeightCurve(resetWeightCurve);

        return createdMachine;
    }

    void Start()
    {
        InitialiseStates();
    }

    private void InitialiseStates()
    {
        States.Add(E_AvatarDrumHitState.Waiting, new WaitingState(_context, E_AvatarDrumHitState.Waiting));
        States.Add(E_AvatarDrumHitState.Hitting, new HittingState(_hitDuration, _hitWeightCurve, _context, E_AvatarDrumHitState.Hitting));
        States.Add(E_AvatarDrumHitState.Reset, new ResetState(_resetDuration, _resetWeightCurve, _context, E_AvatarDrumHitState.Reset));
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

    public void SetHitWeightCurve(AnimationCurve weightCurve)
    {
        _hitWeightCurve = weightCurve;
    }
    public void SetResetWeightCurve(AnimationCurve weightCurve)
    {
        _resetWeightCurve = weightCurve;
    }
}