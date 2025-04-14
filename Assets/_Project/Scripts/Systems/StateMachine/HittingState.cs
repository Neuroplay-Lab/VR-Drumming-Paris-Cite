using UnityEngine;

public class HittingState : HitState
{
    private float _elapsedTime = 0f;
    private float _hitDuration;
    public HittingState(float hitDuration, HitStateContext context, AvatarHandHitStateMachine.E_AvatarDrumHitState stateKey)
    : base(context, stateKey)
    {
        _hitDuration = hitDuration;
    }

    public override void EnterState()
    {
        _elapsedTime = 0f;
    }

    public override void ExitState() { }

    public override void UpdateState()
    {
        _elapsedTime += Time.deltaTime;
        _context.IKConstraint.weight = Mathf.Lerp(_context.IKConstraint.weight, 1f, _elapsedTime / _hitDuration);
    }
    public override AvatarHandHitStateMachine.E_AvatarDrumHitState GetNextState()
    {
        if (_context.IKConstraint.weight == 1f)
        {
            return AvatarHandHitStateMachine.E_AvatarDrumHitState.Reset;
        }

        return StateKey;
    }
}