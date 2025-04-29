using RootMotion.FinalIK;
using UnityEngine;

public class ResetState : HitState
{
    private float _elapsedTime = 0f;
    private float _initialWeight;
    private float _resetTime;
    private AnimationCurve _weightCurve;
    public ResetState(float resetDuration, AnimationCurve weightCurve, HitStateContext context, AvatarHitStateMachine.E_AvatarDrumHitState stateKey)
    : base(context, stateKey)
    {
        _resetTime = resetDuration;
        _weightCurve = weightCurve;
    }

    public override void EnterState()
    {
        _elapsedTime = 0f;
        _initialWeight = _context.handEffector.positionWeight;
    }
    public override void ExitState() { }
    public override void UpdateState()
    {
        _elapsedTime += Time.deltaTime;
        _context.handEffector.positionWeight = Mathf.Lerp(_initialWeight, 0f, _weightCurve.Evaluate(_elapsedTime / _resetTime));
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