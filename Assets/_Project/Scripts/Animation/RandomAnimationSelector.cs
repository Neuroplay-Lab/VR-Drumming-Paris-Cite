using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class RandomAnimationSelector : StateMachineBehaviour
{
    public int range = 2;
    public string randomString;

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash, AnimatorControllerPlayable controller)
    {
        base.OnStateMachineEnter(animator, stateMachinePathHash, controller);
        var random = Random.Range(0, range);
        animator.SetInteger(randomString, random);
    }
}
