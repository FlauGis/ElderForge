using System.Collections;
using UnityEngine;
using DevionGames;
using Unity.VisualScripting;

public class FireballScript : MonoBehaviour
{
    [Header("Fireball Settings")]
    public Transform spawnPoint;
    public GameObject projectilePrefab;
    public float projectileSpeed = 20f;
    public float projectileLifetime = 5f;
    public float fireRate = 1f;
    public Animator animator;
    public ManaSystem manaSystem;
    public float spellCostMana;

    [Header("Player Settings")]
    public ThirdPersonController playerController;

    public bool canShoot = true;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            if(manaSystem.currentMana >= spellCostMana)
            {
                StartCoroutine(RotateAndShoot());
            }
        }
    }

    void RotatePlayerToAim()
    {
        if (playerController != null)
        {
            Quaternion aimDirection = Quaternion.Euler(
                playerController.transform.eulerAngles.x,
                Camera.main.transform.eulerAngles.y,
                playerController.transform.eulerAngles.z
            );

            playerController.transform.rotation = Quaternion.Slerp(
                playerController.transform.rotation,
                aimDirection,
                1f 
            );
        }
    }

    IEnumerator RotateAndShoot()
    {
        canShoot = false;
        RotatePlayerToAim();
        animator.SetTrigger("Fireball");
        yield return new WaitForSeconds(0.5f);
        ShootProjectile();
        manaSystem.SpendMana(spellCostMana);
        yield return new WaitForSeconds(fireRate);
        canShoot = true; 
    }

    void ShootProjectile()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Vector3 direction;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            direction = (hit.point - spawnPoint.position).normalized;
        }
        else
        {
            direction = Camera.main.transform.forward;
        }

        GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;

            projectile.transform.rotation = Quaternion.LookRotation(rb.velocity);
        }

        Destroy(projectile, projectileLifetime);
    }
}
