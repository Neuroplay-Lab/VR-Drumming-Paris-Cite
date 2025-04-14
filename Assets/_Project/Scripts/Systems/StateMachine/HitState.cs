using UnityEngine;

public abstract class HitState : BaseState<AvatarHandHitStateMachine.E_AvatarDrumHitState>
{
    protected HitStateContext _context;

    public HitState(HitStateContext context, AvatarHandHitStateMachine.E_AvatarDrumHitState stateKey)
    : base(stateKey) {
        _context = context;
    }
}