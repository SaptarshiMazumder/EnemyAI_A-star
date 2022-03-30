using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDuringMeleeAttack : StateMachineBehaviour
{

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    [SerializeField]
    float timer = 0f;
    [SerializeField]
    string specificTime;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0f;
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        //int meleeIdle = Random.Range(1, 10);
        //if (Mathf.Ceil (timer) == 1f)
        //{
        //    specificTime = Mathf.Ceil(timer).ToString();

        //    animator.SetInteger("meleeidle", meleeIdle);

        //}
        
        //int meleeIdle = Random.Range(1, 10);
        //if (meleeIdle > 5)
        //{
        //    animator.SetInteger("meleeidle", meleeIdle);
        //}
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

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
