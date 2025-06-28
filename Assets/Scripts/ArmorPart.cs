using DevionGames;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmorPart : MonoBehaviour
{
    [Header("Armor Settings")]
    public InventorySlot helmetSlot;
    public InventorySlot chestplateSlot;
    public InventorySlot leggingsSlot;
    public InventorySlot bootsSlot;

    public Slider helmetSlider;
    public Slider chestplateSlider;
    public Slider leggingsSlider;
    public Slider bootsSlider;

    public ThirdPersonController thirdPersonController;
    public InventoryManager inventoryManager;
    public ItemControlHeath itemControlHeath;
    public ClothAdder clothAdder;

    private void Start()
    {
        UpdateAllArmorSliders();
        UpdateAllArmorHealth();
    }

    public void EquipArmor(InventorySlot armorSlot)
    {
        UpdateArmorSliderForSlot(armorSlot, GetSliderForSlot(armorSlot));
        UpdateArmorHealth(armorSlot);
    }

    public void TakeDamage(int damage)
    {
        int totalWeight = 0;
        if (!helmetSlot.isEmpty) totalWeight += 20;
        if (!chestplateSlot.isEmpty) totalWeight += 45;
        if (!leggingsSlot.isEmpty) totalWeight += 25;
        if (!bootsSlot.isEmpty) totalWeight += 10;

        if (totalWeight == 0)
        {
            thirdPersonController.DecreaseHealth(damage);
            return;
        }

        float damagePerWeight = (float)damage / totalWeight;

        ApplyDistributedDamage(helmetSlot, helmetSlider, Mathf.RoundToInt(damagePerWeight * 20));
        ApplyDistributedDamage(chestplateSlot, chestplateSlider, Mathf.RoundToInt(damagePerWeight * 45));
        ApplyDistributedDamage(leggingsSlot, leggingsSlider, Mathf.RoundToInt(damagePerWeight * 25));
        ApplyDistributedDamage(bootsSlot, bootsSlider, Mathf.RoundToInt(damagePerWeight * 10));
    }


    private void ApplyDistributedDamage(InventorySlot armorSlot, Slider armorSlider, int damage)
    {
        if (armorSlot.item == null)
        {
            return;
        }

        armorSlot.healthItem -= damage;

        if (armorSlot.healthItem <= 0)
        {
            armorSlot.healthItem = 0;
            UpdateArmorSliderForSlot(armorSlot, armorSlider);
            BreakArmor(armorSlot);
            return;
        }

        UpdateArmorSliderForSlot(armorSlot, armorSlider);
    }

    public void UpdateAllArmorSliders()
    {
        UpdateArmorSliderForSlot(helmetSlot, helmetSlider);
        UpdateArmorSliderForSlot(chestplateSlot, chestplateSlider);
        UpdateArmorSliderForSlot(leggingsSlot, leggingsSlider);
        UpdateArmorSliderForSlot(bootsSlot, bootsSlider);
    }

    private void UpdateArmorSliderForSlot(InventorySlot armorSlot, Slider armorSlider)
    {
        if (armorSlot.item == null)
        {
            armorSlider.maxValue = 0;
            armorSlider.value = 0;
            return;
        }

        armorSlider.maxValue = armorSlot.item.maximumDurability;
        armorSlider.value = armorSlot.healthItem;
    }

    public void UpdateAllArmorHealth()
    {
        UpdateArmorHealth(helmetSlot);
        UpdateArmorHealth(chestplateSlot);
        UpdateArmorHealth(leggingsSlot);
        UpdateArmorHealth(bootsSlot);
    }

    private void UpdateArmorHealth(InventorySlot armorSlot)
    {
        if (armorSlot.isEmpty || armorSlot.item.itemType != ItemType.Armor)
            return;

        ItemScriptableObject armor = armorSlot.item;
        UpdateArmorSliderForSlot(armorSlot, GetSliderForSlot(armorSlot));
    }

    private Slider GetSliderForSlot(InventorySlot armorSlot)
    {
        if (armorSlot == helmetSlot) return helmetSlider;
        if (armorSlot == chestplateSlot) return chestplateSlider;
        if (armorSlot == leggingsSlot) return leggingsSlider;
        if (armorSlot == bootsSlot) return bootsSlider;

        return null;
    }

    public void BreakArmor(InventorySlot armorSlot)
    {
        if (armorSlot.item == null)
            return;

        if (armorSlot.healthItem <= 0)
        {

            clothAdder.removeClothes(armorSlot.item.itemPrefabArmorInPlayer);

            armorSlot.item.isBroken = true;

            inventoryManager.AddToBrokenItems(armorSlot.item);

            armorSlot.item = null;
            armorSlot.amount = 0;
            armorSlot.isEmpty = true;
            armorSlot.iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            armorSlot.iconGO.GetComponent<Image>().sprite = null;
            armorSlot.itemAmountText.text = "";

            Transform child = armorSlot.transform.GetChild(0);
            child.gameObject.SetActive(true);

            if (armorSlot.clothType == ClothType.Head)
            {
                itemControlHeath.HeatHP.gameObject.SetActive(false);
            }
            if (armorSlot.clothType == ClothType.Body)
            {
                itemControlHeath.BodyHP.gameObject.SetActive(false);
            }
            if (armorSlot.clothType == ClothType.Legs)
            {
                itemControlHeath.LegsHP.gameObject.SetActive(false);
            }
            if (armorSlot.clothType == ClothType.Feet)
            {
                itemControlHeath.FeetHP.gameObject.SetActive(false);
            }
        }
    }
}
