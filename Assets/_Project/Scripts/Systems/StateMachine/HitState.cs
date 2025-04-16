using UnityEngine;

public abstract class HitState : BaseState<AvatarHitStateMachine.E_AvatarDrumHitState>
{
    protected HitStateContext _context;

    public HitState(HitStateContext context, AvatarHitStateMachine.E_AvatarDrumHitState stateKey)
    : base(stateKey)
    {
        _context = context;
    }
}