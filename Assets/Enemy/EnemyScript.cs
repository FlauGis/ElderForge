using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    private int HP = 100;
    public Animator animator;
    public Slider healthBar;
    public bool isQuestWolf = false;
    public ScriptQuestKillWolf killWolf;

    void Start()
    {
        healthBar.gameObject.SetActive(false);
    }

    void Update()
    {
        healthBar.value = HP;
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP > 0 && !healthBar.gameObject.activeSelf)
        {
            healthBar.gameObject.SetActive(true);
        }

        if (HP <= 0)
        {
            if (isQuestWolf == true)
            {
                killWolf.questDone = true;
            }
            animator.SetTrigger("Death");

            // Disable all colliders attached to the enemy
            Collider[] colliders = GetComponents<Collider>();
            foreach (Collider col in colliders)
            {
                col.enabled = false;
            }

            healthBar.gameObject.SetActive(false);
        }
        else
        {
            animator.SetTrigger("Damage");
        }
    }
}
