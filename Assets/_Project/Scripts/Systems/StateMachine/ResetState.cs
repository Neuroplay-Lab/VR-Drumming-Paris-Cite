using UnityEngine;

public class ResetState : HitState
{
    private float _elapsedTime = 0f;
    private float _resetTime;
    public ResetState(float resetDuration, HitStateContext context, AvatarHitStateMachine.E_AvatarDrumHitState stateKey)
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
        _context.handEffector.positionWeight = Mathf.Lerp(_context.handEffector.positionWeight, 0f, _elapsedTime / _resetTime);
        _context.handEffector.rotationWeight = _context.handEffector.positionWeight;
    }
    public override AvatarHitStateMachine.E_AvatarDrumHitState GetNextState()
    {
        if (_context.handEffector.positionWeight == 0f)
        {
            return AvatarHitStateMachine.E_AvatarDrumHitState.Waiting;
        }
        return StateKey;
    }

    public void SetResetDuration(float resetDuration)
    {
        _resetTime = resetDuration;
    }
}