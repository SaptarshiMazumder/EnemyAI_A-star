using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDuringWalk : StateMachineBehaviour
{
    [SerializeField]
    EnemyAI enemyAI;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //enemyAI = animator.GetComponentInParent<EnemyAI>();
        enemyAI = GameObject.FindGameObjectWithTag("EnemyParent").GetComponent<EnemyAI>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!enemyAI.enemyIsWalking)
        {
            animator.SetBool("Walk", false);
        }

        //if (enemyAI.isAttacking1)
        //{
        //    animator.SetInteger("AttackIndex", Random.Range(1, 4));
        //}

        if(enemyAI.isAttacking1)
        {
            animator.SetBool("Attack1", true);
        }
        else if(enemyAI.isIdling)
        {
            animator.SetBool("Idle", true);
        }
        else if(enemyAI.isJumping)
        {
            animator.SetBool("Jump", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
