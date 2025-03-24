using UnityEngine;
using UnityEngine.Animations;

public class RandomIdleAnimation : StateMachineBehaviour
{

    [SerializeField] private float timeUnitlNextAnimation = 5f;

    [SerializeField] private int animationVariants;

    private bool readyForNextAnimation;
    private float timeSinceLastAnimation;
    private int currentAnimationIndex;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ResetIdle();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!readyForNextAnimation)  // check if the animation is ready to be changed
        {
            timeSinceLastAnimation += Time.deltaTime;
            if (timeSinceLastAnimation >= timeUnitlNextAnimation && stateInfo.normalizedTime % 1 < 0.02f)
            { // animation is ready to be changed and current animation is just begun
                readyForNextAnimation = true;
                currentAnimationIndex = Random.Range(1, animationVariants + 1) * 2 - 1; // choose random animation
                animator.SetFloat("Idle Animation", currentAnimationIndex - 1); // jump to closest basic idle
            }
        }
        else if (stateInfo.normalizedTime % 1 >= 0.98f) // animation running - check to go back to basic idle when complete
        {
            ResetIdle();
        }

        animator.SetFloat("Idle Animation", currentAnimationIndex, 0.5f, Time.deltaTime); // smoothly transition to the next animation
    }

    private void ResetIdle()
    {
        if (readyForNextAnimation)
        {
            currentAnimationIndex -= 1;
        }
        timeSinceLastAnimation = 0;
        readyForNextAnimation = false;
    }

}
