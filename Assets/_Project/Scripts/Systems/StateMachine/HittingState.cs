using RootMotion.FinalIK;
using UnityEngine;

public class HittingState : HitState
{
    private float _elapsedTime = 0f;
    private float _initialWeight;
    private float _hitDuration;
    private AnimationCurve _weightCurve;
    public HittingState(float hitDuration, AnimationCurve weightCurve, HitStateContext context, AvatarHitStateMachine.E_AvatarDrumHitState stateKey)
    : base(context, stateKey)
    {
        _hitDuration = hitDuration;
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
        _context.handEffector.positionWeight = Mathf.Lerp(_initialWeight, 1f, _weightCurve.Evaluate(_elapsedTime / _hitDuration));
        _context.handEffector.rotationWeight = _context.handEffector.positionWeight;
    }
    public override AvatarHitStateMachine.E_AvatarDrumHitState GetNextState()
    {
        if (_context.handEffector.positionWeight == 1f)
        {
            return AvatarHitStateMachine.E_AvatarDrumHitState.Reset;
        }

        return StateKey;
    }

    public void SetHitDuration(float hitDuration)
    {
        _hitDuration = hitDuration;
    }
}