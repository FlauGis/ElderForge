using DevionGames;
using UnityEngine;
using UnityEngine.UI;

public class ShieldPart : MonoBehaviour
{
    [Header("Shield Settings")]
    public InventorySlot shieldSlot;
    public Slider shieldSlider;
    public ThirdPersonController thirdPersonController;
    public InventoryManager inventoryManager;
    public ItemControlHeath itemControlHeath;
    public ArmorPart armorPart;
    public QuickslotInventory quickslotInventory;
    public Animator animator;

    private void Start()
    {
        UpdateShieldSlider();
        UpdateShieldHealth();
    }
    private void Update()
    {
        UpdateShieldSlider();
        UpdateShieldHealth();
    }
    public void EquipShield(InventorySlot shieldSlot)
    {
        if (shieldSlot.item == null)
        {
            return;
        }

        UpdateShieldSlider();
        UpdateShieldHealth();
    }

    public void TakeDamage(int damage)
    {
        if (!Input.GetKey(KeyCode.Mouse1))
        {
            armorPart.TakeDamage(damage);
            return;
        }
        if (!shieldSlot.isEmpty && shieldSlot.item != null)
        {
            shieldSlot.healthItem -= damage;

            if (shieldSlot.healthItem <= 0)
            {
                shieldSlot.healthItem = 0;
                UpdateShieldSlider();
                BreakShield();
                return;
            }

           
            UpdateShieldSlider();
            return;
        }

        if (armorPart != null)
        {
            armorPart.TakeDamage(damage);
            return;
        }
    }

    private void UpdateShieldSlider()
    {
        if (shieldSlot.item == null)
        {
            shieldSlider.maxValue = 0;
            shieldSlider.value = 0;
            return;
        }

        shieldSlider.maxValue = shieldSlot.item.maximumDurability;
        shieldSlider.value = shieldSlot.healthItem;
    }

    private void UpdateShieldHealth()
    {
        if (shieldSlot.item == null)
            return;

        UpdateShieldSlider();
    }

    public void BreakShield()
    {
        if (shieldSlot.isEmpty || shieldSlot.item == null)
            return;

        if (shieldSlot.healthItem <= 0)
        {
            for (int i = 0; i < quickslotInventory.allShield.childCount; i++)
            {
               if (shieldSlot.item.inHandName == quickslotInventory.allShield.GetChild(i).name)
                {
                    quickslotInventory.allShield.GetChild(i).gameObject.SetActive(false);
                    animator.SetBool("WeaponMove", false);
                    animator.SetBool("BlockShield", false);
               }
            }
            shieldSlot.item.isBroken = true;
            inventoryManager.AddToBrokenItems(shieldSlot.item);

            shieldSlot.item = null;
            shieldSlot.amount = 0;
            shieldSlot.isEmpty = true;
            shieldSlot.iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            shieldSlot.iconGO.GetComponent<Image>().sprite = null;
            shieldSlot.itemAmountText.text = "";

            Transform child = shieldSlot.transform.GetChild(0);
            child.gameObject.SetActive(true);

            itemControlHeath.ShieldHP.gameObject.SetActive(false);
        }
    }
}
