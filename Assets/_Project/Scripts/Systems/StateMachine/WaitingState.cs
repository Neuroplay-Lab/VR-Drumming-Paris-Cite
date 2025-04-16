using UnityEngine;

public class WaitingState : HitState
{
    public WaitingState(HitStateContext context, AvatarHitStateMachine.E_AvatarDrumHitState stateKey)
    : base(context, stateKey) { }

    public override void EnterState() { }
    public override void ExitState() { }
    public override void UpdateState() { }
    public override AvatarHitStateMachine.E_AvatarDrumHitState GetNextState()
    {
        return StateKey;
    }
}