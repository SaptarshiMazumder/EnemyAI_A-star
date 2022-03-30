using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    [SerializeField]
    private string currentState;

    
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void ChangeAnimationState(string newState)
    {
        // stop animation from repeating itself
        if (currentState == newState)
            return;

        //play new animation
        animator.Play(newState);

        //reassign new state to current state
        currentState = newState;
    }
}
