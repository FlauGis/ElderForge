using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;  // Include this to use Slider

public class PatrollBehaivior : StateMachineBehaviour
{
    float timer;
    List<Transform> points = new List<Transform>();
    NavMeshAgent agent;

    Transform player;
    float chaseRange = 10f;
    Slider healthBar;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        Transform pointsObject = GameObject.FindGameObjectWithTag("Points").transform;
        foreach (Transform t in pointsObject)
        {
            points.Add(t);
        }
        agent = animator.GetComponent<NavMeshAgent>();
        agent.SetDestination(points[0].position);

        player = GameObject.FindGameObjectWithTag("Player").transform;

        Transform thirdChild = animator.gameObject.transform.GetChild(3);

        Transform firstChildInThirdChild = thirdChild.GetChild(0);

        healthBar = firstChildInThirdChild.GetComponent<Slider>();

        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(false);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(points[Random.Range(0, points.Count)].position);
        }
        timer += Time.deltaTime;
        if (timer > 10)
        {
            animator.SetBool("IsPatrolling", false);
        }

        float distance = Vector3.Distance(animator.transform.position, player.position);

        if (distance < chaseRange)
        {
            animator.SetBool("IsChasing", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
    }
}
