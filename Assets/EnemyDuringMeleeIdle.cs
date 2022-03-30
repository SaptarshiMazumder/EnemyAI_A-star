using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDuringMeleeIdle : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    private bool canMelee = true;
    [SerializeField]
    private float timer;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
         timer = 0f;

        //int meleeAttack = Random.Range(1, 10);
        
        
        //int attackIndex = Random.Range(1, 3);
        ////animator.SetInteger("meleeattack", meleeAttack);

        //animator.SetInteger("attack", attackIndex);
        

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        
        //int meleeAttack = Random.Range(1, 10);
        //if (meleeAttack >= 7 && canMelee)
        //{
        //    canMelee = false;
        //    int attackIndex = Random.Range(1, 3);
        //    animator.SetInteger("meleeattack", meleeAttack);

        //    animator.SetInteger("attack", attackIndex);
        //}
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //int meleeAttack = Random.Range(1, 10);
        //if(meleeAttack >= 1)
        //{
        //    animator.SetInteger("meleeattack", meleeAttack);
        //}
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

    void MeleeCooldown()
    {
        canMelee = true;
    }
}
