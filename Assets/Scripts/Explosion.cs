using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float explosionRadius = 5f;
    public int explosionDamage = 30;
    public LayerMask enemyLayer;

    void Start()
    {

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayer);

        foreach (Collider hit in hitColliders)
        {
            EnemyScript enemy = hit.GetComponent<EnemyScript>();
            if (enemy != null)
            {
                enemy.TakeDamage(explosionDamage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
