using DevionGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour
{
    Transform player;
    private ShieldPart shieldPart;
    public int damage = 10;

    public float firstAttackDelay = 0.5f;
    public float attackCooldown = 1.0f;
    private float lastAttackTime;
    private bool hasAttacked = false; 

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        shieldPart = FindObjectOfType<ShieldPart>();
        lastAttackTime = Time.time;
        hasAttacked = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.LookAt(player);
        float distance = Vector3.Distance(animator.transform.position, player.position);

        if (distance < 3)
        {
            if (!hasAttacked)
            {
                if (Time.time >= lastAttackTime + firstAttackDelay)
                {
                    shieldPart.TakeDamage(damage);
                    lastAttackTime = Time.time;
                    hasAttacked = true;
                }
            }
            else
            {
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    shieldPart.TakeDamage(damage);
                    lastAttackTime = Time.time;
                }
            }
        }
        else
        {
            animator.SetBool("IsAttacking", false);
        }
    }
}
