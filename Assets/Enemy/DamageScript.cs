using UnityEngine;

public class DamageScript : MonoBehaviour
{
    public Transform attackPoint; 
    public float attackRange = 1f; 
    public int damageAmount = 10;
    public LayerMask enemyLayer; 

    public void Attack()
    {
        Debug.DrawRay(attackPoint.position, attackPoint.forward * attackRange, Color.red, 1f);

        RaycastHit hit;
        if (Physics.Raycast(attackPoint.position, attackPoint.forward, out hit, attackRange, enemyLayer))
        {
            EnemyScript enemy = hit.collider.GetComponent<EnemyScript>();
            if (enemy != null)
            {
                enemy.TakeDamage(damageAmount);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(attackPoint.position, attackPoint.position + attackPoint.forward * attackRange);
        }
    }
}
