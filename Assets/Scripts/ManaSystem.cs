using UnityEngine;
using UnityEngine.UI;

public class ManaSystem : MonoBehaviour
{
    [Header("Mana Settings")]
    public Slider manaSlider;
    public float maxMana = 100f;
    public float regenRate = 5f;

    public float currentMana;

    void Start()
    {
        currentMana = maxMana;
        UpdateManaUI();
    }

    void Update()
    {
        RegenerateMana();
        UpdateManaUI();
    }

    private void RegenerateMana()
    {
        if (currentMana < maxMana)
        {
            currentMana += regenRate * Time.deltaTime;
            currentMana = Mathf.Clamp(currentMana, 0, maxMana);
        }
    }

    // Функція для витрати мани
    public bool SpendMana(float amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            currentMana = Mathf.Clamp(currentMana, 0, maxMana);
            UpdateManaUI();
            return true; 
        }
        else
        {
            return false; 
        }
    }

    // Функція для додавання мани
    public void AddMana(float amount)
    {
        currentMana += amount;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
        UpdateManaUI();
    }

    private void UpdateManaUI()
    {
        if (manaSlider != null)
        {
            manaSlider.value = currentMana / maxMana;
        }
    }
}
