using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalk : StateMachineBehaviour
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
        if(enemyAI.enemyIsWalking)
        {
            animator.SetBool("Walk", true);
        }

        if (enemyAI.isAttacking1)
        {
            animator.SetBool("Attack1", true);
        }

        if(enemyAI.isJumping)
        {
            animator.SetBool("Jump", true);
        }

        //if (!enemyAI.enemyIsWalking)
        //{
        //    animator.SetInteger("AttackIndex", Random.Range(1, 4));
        //}
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
