using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseBehaviour : StateMachineBehaviour
{
    NavMeshAgent agent;
    Transform player;
    float attackRange = 2f;
    float stopDistance = 2.5f; // ћ≥н≥мальна в≥дстань до гравц€

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = 4f;

        player = GameObject.FindGameObjectWithTag("Player").transform;

        agent.stoppingDistance = stopDistance; // ¬орог зупинитьс€ на ц≥й в≥дстан≥ в≥д гравц€
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector3.Distance(animator.transform.position, player.position) > stopDistance)
        {
            agent.SetDestination(player.position);
        }

        float distance = Vector3.Distance(animator.transform.position, player.position);

        if (distance < attackRange)
        {
            animator.SetBool("IsAttacking", true);
        }

        if (distance > 10)
        {
            animator.SetBool("IsChasing", false);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
        agent.speed = 2f;
    }
}
