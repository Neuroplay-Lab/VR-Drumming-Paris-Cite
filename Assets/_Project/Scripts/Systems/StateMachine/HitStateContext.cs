using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HitStateContext
{
    private TwoBoneIKConstraint _ikConstraint;

    public HitStateContext(TwoBoneIKConstraint ikConstraint)
    {
        _ikConstraint = ikConstraint;
    }

    public TwoBoneIKConstraint IKConstraint => _ikConstraint;
}