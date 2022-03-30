using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationTransitions : MonoBehaviour
{
    public Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    void TransitionToMeleeIdle()
    {
        int meleeIdle = Random.Range(1, 10);
        int attackIndex = Random.Range(1, 3);
        anim.SetInteger("meleeidle", meleeIdle);
        anim.SetInteger("attack", attackIndex);
        //if (meleeIdle > 5)
        //{
        //    anim.SetInteger("meleeidle", meleeIdle);
        //}
    }

    void TransitionToMeleeAttack()
    {
        int attackIndex = Random.Range(1, 3);
        int meleeAttack = Random.Range(1, 10);
       
        anim.SetInteger("attack", attackIndex);
        anim.SetInteger("meleeattack", meleeAttack);
    }
}
