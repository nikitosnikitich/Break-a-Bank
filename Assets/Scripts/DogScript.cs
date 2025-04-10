using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class DogScript : MonoBehaviour
{
    public AIPath path;
    public SpriteRenderer spriteRenderer;

    [SerializeField] private AIDestinationSetter ds;

    private bool isMoving;
    private bool startRunningAnimationTriggered = false;
    private bool startRunningAnimationPlayed = false;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        ds.enabled = false;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(path.desiredVelocity.x >= 0.01f)
        {
            spriteRenderer.flipX = false;
            isMoving = true;

        }
        else if (path.desiredVelocity.x <= -0.01f)
        {
            spriteRenderer.flipX = true;
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        AnimationController();
    }

    void AnimationController()
    {
        if (isMoving && !startRunningAnimationTriggered && !startRunningAnimationPlayed)
        {
            animator.Play("StartRunning");
            startRunningAnimationTriggered = true;
        }
        else if (isMoving && !startRunningAnimationPlayed && animator.GetCurrentAnimatorStateInfo(0).IsName("StartRunning") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            startRunningAnimationPlayed = true;
        }
        else if (isMoving && startRunningAnimationPlayed)
        {
            animator.Play("Running");
            startRunningAnimationTriggered = false;
        }
        else if (!isMoving && startRunningAnimationPlayed)
        {
            animator.Play("StopRunning");
            startRunningAnimationPlayed = false;
        }
        else if (!isMoving && !startRunningAnimationPlayed)
        {
            animator.Play("Idle");
        }
    }

    public void DogActivation()
    {
        gameObject.GetComponent<Renderer>().sortingOrder = 7;
        ds.enabled = true;
    }
}
