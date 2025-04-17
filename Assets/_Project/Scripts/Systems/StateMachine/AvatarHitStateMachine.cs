using RootMotion.FinalIK;

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

    public AvatarHitStateMachine(FullBodyBipedIK fullBodyBipedIK, ManagedHand managedHand, float hitDuration, float resetDuration)
    {
        if (managedHand == ManagedHand.Right)
        {
            _context = new HitStateContext(fullBodyBipedIK.solver.rightHandEffector);
        }
        else
        {
            _context = new HitStateContext(fullBodyBipedIK.solver.leftHandEffector);
        }
        _hitDuration = hitDuration;
        _resetDuration = resetDuration;
    }

    void Awake()
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
}