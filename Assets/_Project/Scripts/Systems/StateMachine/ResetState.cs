using UnityEngine;

public class ResetState : HitState
{
    private float _elapsedTime = 0f;
    private float _resetTime;
    public ResetState(float resetDuration, HitStateContext context, AvatarHandHitStateMachine.E_AvatarDrumHitState stateKey)
    : base(context, stateKey)
    {
        _resetTime = resetDuration;
    }

    public override void EnterState()
    {
        _elapsedTime = 0f;
    }
    public override void ExitState() { }
    public override void UpdateState()
    {
        _elapsedTime += Time.deltaTime;
        _context.IKConstraint.weight = Mathf.Lerp(_context.IKConstraint.weight, 0f, _elapsedTime / _resetTime);
    }
    public override AvatarHandHitStateMachine.E_AvatarDrumHitState GetNextState()
    {
        if (_context.IKConstraint.weight == 0f)
        {
            return AvatarHandHitStateMachine.E_AvatarDrumHitState.Waiting;
        }
        return StateKey;
    }
}