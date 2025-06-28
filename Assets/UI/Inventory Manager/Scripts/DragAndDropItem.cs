using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class DragAndDropItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public InventorySlot oldSlot;
    private Transform player;
    private QuickslotInventory quickslotInventory;
    private CraftManager craftManager;
    public List<ClothAdder> clothAdders;
    public InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();

        quickslotInventory = FindObjectOfType<QuickslotInventory>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        oldSlot = transform.GetComponentInParent<InventorySlot>();

        if (oldSlot.clothType != ClothType.None)
        {
            clothAdders = new List<ClothAdder>();
            clothAdders.AddRange(FindObjectsOfType<ClothAdder>());
        }

        craftManager = FindObjectOfType<CraftManager>();

    }

    public void OnDrag(PointerEventData eventData)
    {

        if (oldSlot.isEmpty)
            return;
        GetComponent<RectTransform>().position += new Vector3(eventData.delta.x, eventData.delta.y);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        inventoryManager.isItemHeld = true;

        if (oldSlot.isEmpty)
            return;

        GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.75f);

        GetComponentInChildren<Image>().raycastTarget = false;

        transform.SetParent(transform.parent.parent.parent);
    }
    public void ReturnBackToSlot()
    {
        if (oldSlot.isEmpty)
            return;

        GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1f);

        GetComponentInChildren<Image>().raycastTarget = true;


        transform.SetParent(oldSlot.transform);
        transform.position = oldSlot.transform.position;
        if (oldSlot.item != null && oldSlot.clothType != ClothType.None)
        {
            if (oldSlot.item.clothType == ClothType.Legs || oldSlot.item.clothType == ClothType.Body || oldSlot.item.clothType == ClothType.Head || oldSlot.item.clothType == ClothType.Feet)
            {
                foreach (ClothAdder clothAdder in clothAdders)
                {
                    clothAdder.addClothes(oldSlot.item.itemPrefabArmorInPlayer);
                    oldSlot.transform.GetChild(0).gameObject.SetActive(false);
                    if (oldSlot.item.clothType == ClothType.Head)
                    {
                        inventoryManager.itemControlHeath.HeatHP.SetActive(true);
                    }
                    else if (oldSlot.item.clothType == ClothType.Body)
                    {
                        inventoryManager.itemControlHeath.BodyHP.SetActive(true);
                    }
                    else if (oldSlot.item.clothType == ClothType.Legs)
                    {
                        inventoryManager.itemControlHeath.LegsHP.SetActive(true);
                    }
                    else if (oldSlot.item.clothType == ClothType.Feet)
                    {
                        inventoryManager.itemControlHeath.FeetHP.SetActive(true);
                    }

                    inventoryManager.armorPart.EquipArmor(oldSlot);
                }
            }
            else if (oldSlot.item.clothType == ClothType.Shield)
            {
                quickslotInventory.animator.SetBool("WeaponMove", true);
                for (int i = 0; i < quickslotInventory.allShield.childCount; i++)
                {
                    if (oldSlot.item.inHandName == quickslotInventory.allShield.GetChild(i).name)
                    {
                        quickslotInventory.allShield.GetChild(i).gameObject.SetActive(true);
                        oldSlot.transform.GetChild(0).gameObject.SetActive(false);
                        inventoryManager.itemControlHeath.ShieldHP.SetActive(true);
                    }
                }
            }
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        inventoryManager.isItemHeld = false;
        if (oldSlot.isEmpty)
            return;

        GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1f);

        GetComponentInChildren<Image>().raycastTarget = true;

        transform.SetParent(oldSlot.transform);
        transform.position = oldSlot.transform.position;

        if (eventData.pointerCurrentRaycast.gameObject.name == "UIBG")
        {
           
            if (oldSlot.item.itemType == ItemType.Spell)
            {
                NullifySlotData();
                quickslotInventory.useSpellFireboll.SetActive(false);
                quickslotInventory.magickManaSlider.SetActive(false);
                return;
            }

            if (oldSlot.clothType != ClothType.None && oldSlot.item != null)
            {
                
                if (oldSlot.item.clothType == ClothType.Legs || oldSlot.item.clothType == ClothType.Body || oldSlot.item.clothType == ClothType.Head || oldSlot.item.clothType == ClothType.Feet)
                {
                    foreach (ClothAdder clothAdder in clothAdders)
                    {
                        clothAdder.removeClothes(oldSlot.item.itemPrefabArmorInPlayer);
                        oldSlot.transform.GetChild(0).gameObject.SetActive(true);
                        if (oldSlot.item.clothType == ClothType.Head)
                        {
                            inventoryManager.itemControlHeath.HeatHP.SetActive(false);
                        }
                        else if (oldSlot.item.clothType == ClothType.Body)
                        {
                            inventoryManager.itemControlHeath.BodyHP.SetActive(false);
                        }
                        else if (oldSlot.item.clothType == ClothType.Legs)
                        {
                            inventoryManager.itemControlHeath.LegsHP.SetActive(false);
                        }
                        else if (oldSlot.item.clothType == ClothType.Feet)
                        {
                            inventoryManager.itemControlHeath.FeetHP.SetActive(false);
                        }
                    }
                }
                else if (oldSlot.item.clothType == ClothType.Shield)
                {
                    quickslotInventory.animator.SetBool("WeaponMove", false);
                    for (int i = 0; i < quickslotInventory.allShield.childCount; i++)
                    {
                        if (oldSlot.item.inHandName == quickslotInventory.allShield.GetChild(i).name)
                        {
                            quickslotInventory.allShield.GetChild(i).gameObject.SetActive(false);
                            oldSlot.transform.GetChild(0).gameObject.SetActive(true);
                            inventoryManager.itemControlHeath.ShieldHP.SetActive(false);
                        }
                    }
                }
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                GameObject itemObject = Instantiate(oldSlot.item.itemDropPrefab, player.position + Vector3.up + player.forward, Quaternion.identity);

                itemObject.GetComponent<Item>().amount = Mathf.CeilToInt((float)oldSlot.amount / 2);
                itemObject.GetComponent<Item>().healthItem = oldSlot.healthItem;
                itemObject.GetComponent<Item>().isBroken = oldSlot.isBroken;
                oldSlot.amount -= Mathf.CeilToInt((float)oldSlot.amount / 2);
                oldSlot.itemAmountText.text = oldSlot.amount.ToString();
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                GameObject itemObject = Instantiate(oldSlot.item.itemDropPrefab, player.position + Vector3.up + player.forward, Quaternion.identity);

                itemObject.GetComponent<Item>().amount = 1;
                itemObject.GetComponent<Item>().healthItem = oldSlot.healthItem;
                itemObject.GetComponent <Item>().isBroken = oldSlot.isBroken;
                oldSlot.amount--;
                oldSlot.itemAmountText.text = oldSlot.amount.ToString();
            }
            else {

                GameObject itemObject = Instantiate(oldSlot.item.itemDropPrefab, player.position + Vector3.up + player.forward, Quaternion.identity);

                itemObject.GetComponent<Item>().amount = oldSlot.amount;
                itemObject.GetComponent<Item>().healthItem = oldSlot.healthItem;
                itemObject.GetComponent<Item>().isBroken = oldSlot.isBroken;

                NullifySlotData();

                craftManager.currentCraftItem.FillItemDetails();

            }
            quickslotInventory.CheckItemInHand();
        }
        else if (eventData.pointerCurrentRaycast.gameObject.transform.parent.parent == null)
        {
            return;
        }
        else if(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<InventorySlot>() != null)
        {
            if (oldSlot.clothType != ClothType.None && oldSlot.item != null)
            {
                if(oldSlot.item.clothType == ClothType.Legs || oldSlot.item.clothType == ClothType.Body || oldSlot.item.clothType == ClothType.Head || oldSlot.item.clothType == ClothType.Feet)
                {
                    foreach (ClothAdder clothAdder in clothAdders)
                    {
                        clothAdder.removeClothes(oldSlot.item.itemPrefabArmorInPlayer);
                        oldSlot.transform.GetChild(0).gameObject.SetActive(true);
                        if (oldSlot.item.clothType == ClothType.Head)
                        {
                            inventoryManager.itemControlHeath.HeatHP.SetActive(false);
                        }
                        else if (oldSlot.item.clothType == ClothType.Body)
                        {
                            inventoryManager.itemControlHeath.BodyHP.SetActive(false);
                        }
                        else if (oldSlot.item.clothType == ClothType.Legs)
                        {
                            inventoryManager.itemControlHeath.LegsHP.SetActive(false);
                        }
                        else if (oldSlot.item.clothType == ClothType.Feet)
                        {
                            inventoryManager.itemControlHeath.FeetHP.SetActive(false);
                        }
                    }
                }
                else if (oldSlot.item.clothType == ClothType.Shield)
                {
                    quickslotInventory.animator.SetBool("WeaponMove", false);
                    for (int i = 0; i < quickslotInventory.allShield.childCount; i++)
                    {
                        if (oldSlot.item.inHandName == quickslotInventory.allShield.GetChild(i).name)
                        {
                            quickslotInventory.allShield.GetChild(i).gameObject.SetActive(false);
                            oldSlot.transform.GetChild(0).gameObject.SetActive(true);
                            inventoryManager.itemControlHeath.ShieldHP.SetActive(false);
                        }
                    }
                }
            }
            InventorySlot inventorySlot = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<InventorySlot>();
            if (inventorySlot.clothType != ClothType.None)
            {
                if(inventorySlot.clothType == oldSlot.item.clothType) 
                {
                    if (oldSlot.healthItem == 0 && oldSlot.item.itemType != ItemType.Resource)
                    {
                        ReturnBackToSlot();
                        return;
                    }
                    ExchangeSlotData(inventorySlot);
                    if (inventorySlot.item.clothType == ClothType.Legs || inventorySlot.item.clothType == ClothType.Body || inventorySlot.item.clothType == ClothType.Head || inventorySlot.item.clothType == ClothType.Feet)
                    {
                        foreach (ClothAdder clothAdder in inventorySlot.GetComponentInChildren<DragAndDropItem>().clothAdders)
                        {
                            clothAdder.addClothes(inventorySlot.item.itemPrefabArmorInPlayer);
                            ExchangeSlotDataCloth(inventorySlot);

                            if (inventorySlot.item.clothType == ClothType.Head)
                            {
                                inventoryManager.itemControlHeath.HeatHP.SetActive(true);
                            }
                            else if (inventorySlot.item.clothType == ClothType.Body)
                            {
                                inventoryManager.itemControlHeath.BodyHP.SetActive(true);
                            }
                            else if (inventorySlot.item.clothType == ClothType.Legs)
                            {
                                inventoryManager.itemControlHeath.LegsHP.SetActive(true);
                            }
                            else if (inventorySlot.item.clothType == ClothType.Feet)
                            {
                                inventoryManager.itemControlHeath.FeetHP.SetActive(true);
                            }

                            inventoryManager.armorPart.EquipArmor(inventorySlot);
                        }
                    }
                    else if (inventorySlot.item.clothType == ClothType.Shield)
                    {
                        quickslotInventory.animator.SetBool("WeaponMove", true);
                        for (int i = 0; i < quickslotInventory.allShield.childCount; i++)
                        {
                            if (inventorySlot.item.inHandName == quickslotInventory.allShield.GetChild(i).name)
                            {
                                quickslotInventory.allShield.GetChild(i).gameObject.SetActive(true);
                                ExchangeSlotDataCloth(inventorySlot);
                                inventoryManager.itemControlHeath.ShieldHP.SetActive(true);

                            }
                        }
                    }
                }
                else if (oldSlot.clothType == ClothType.None && inventorySlot.clothType != ClothType.None)
                {
                    return;
                }
                else
                {
                    if (oldSlot.item.clothType == ClothType.Legs || oldSlot.item.clothType == ClothType.Body || oldSlot.item.clothType == ClothType.Head || oldSlot.item.clothType == ClothType.Feet)
                    {
                        ReturnBackToSlot();
                        oldSlot.transform.GetChild(0).gameObject.SetActive(false);
                    }
                    else if (oldSlot.item.clothType == ClothType.Shield)
                    {
                        quickslotInventory.animator.SetBool("WeaponMove", true);
                        for (int i = 0; i < quickslotInventory.allShield.childCount; i++)
                        {
                            if (oldSlot.item.inHandName == quickslotInventory.allShield.GetChild(i).name)
                            {
                                quickslotInventory.allShield.GetChild(i).gameObject.SetActive(true);
                                oldSlot.transform.GetChild(0).gameObject.SetActive(false);
                            }
                        }
                    }
                }

            }
            else
            {
                if(inventorySlot.isEmpty == false)
                {
                    if (oldSlot.clothType == ClothType.Head || oldSlot.clothType == ClothType.Body || oldSlot.clothType == ClothType.Legs || oldSlot.clothType == ClothType.Feet || oldSlot.clothType == ClothType.Shield && inventorySlot.clothType == ClothType.None)
                    {
                        ReturnBackToSlot();
                        return;
                    }
                    ExchangeSlotData(inventorySlot);
                }
                else //333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333
                {
                    if(oldSlot.isRepeirSlot == true) 
                    {
                        if (oldSlot.isRepeirSlot == true)
                        {
                            if (oldSlot.repairItemManager.itemSlot.item != null &&
                                oldSlot.repairItemManager.resourcesSlot.item != null &&
                                oldSlot.repairItemManager.resourcesSlot.amount > 0)
                            {
                                oldSlot.repairItemManager.itemSlot.item = null;
                                oldSlot.repairItemManager.itemSlot.amount = 0;
                                oldSlot.repairItemManager.itemSlot.isEmpty = true;
                                oldSlot.repairItemManager.itemSlot.isBroken = false;
                                oldSlot.repairItemManager.itemSlot.healthItem = 0;
                                oldSlot.repairItemManager.itemSlot.brokenIcon.SetActive(false);
                                oldSlot.repairItemManager.itemSlot.iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                                oldSlot.repairItemManager.itemSlot.iconGO.GetComponent<Image>().sprite = null;
                                oldSlot.repairItemManager.itemSlot.itemAmountText.text = "";
 
                                oldSlot.repairItemManager.resourcesSlot.amount--;
                                oldSlot.repairItemManager.resourcesSlot.itemAmountText.text = oldSlot.repairItemManager.resourcesSlot.amount.ToString();

                                if (oldSlot.repairItemManager.resourcesSlot.amount <= 0)
                                {
                                    oldSlot.repairItemManager.resourcesSlot.item = null;
                                    oldSlot.repairItemManager.resourcesSlot.amount = 0;
                                    oldSlot.repairItemManager.resourcesSlot.isEmpty = true;
                                    oldSlot.repairItemManager.resourcesSlot.isBroken = false;
                                    oldSlot.repairItemManager.resourcesSlot.healthItem = 0;
                                    oldSlot.repairItemManager.resourcesSlot.brokenIcon.SetActive(false);
                                    oldSlot.repairItemManager.resourcesSlot.iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                                    oldSlot.repairItemManager.resourcesSlot.iconGO.GetComponent<Image>().sprite = null;
                                    oldSlot.repairItemManager.resourcesSlot.itemAmountText.text = "";
                                }
                            }
                        }
                    }
                    ExchangeSlotData(inventorySlot);
                }
                quickslotInventory.CheckItemInHand();
            }
        }
        if (oldSlot.amount <= 0)
        {
            NullifySlotData();
        }
       
    }
    public void NullifySlotData()
    {

        oldSlot.item = null;
        oldSlot.amount = 0;
        oldSlot.isEmpty = true;
        oldSlot.isBroken = false;
        oldSlot.healthItem = 0;
        oldSlot.brokenIcon.SetActive(false);
        oldSlot.iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        oldSlot.iconGO.GetComponent<Image>().sprite = null;
        oldSlot.itemAmountText.text = "";
    }
    void ExchangeSlotDataCloth(InventorySlot newSlot)
    {
        oldSlot.transform.GetChild(0).gameObject.SetActive(true);
        newSlot.transform.GetChild(0).gameObject.SetActive(false);
    }

    void ExchangeSlotData(InventorySlot newSlot)
    {
        ItemScriptableObject item = newSlot.item;
        int amount = newSlot.amount;
        bool isEmpty = newSlot.isEmpty;
        bool isBroken = newSlot.isBroken;
        GameObject iconGO = newSlot.iconGO;
        TMP_Text itemAmountText = newSlot.itemAmountText;
        int newSlotHealth = newSlot.healthItem;

        if (item == null)
        {
            if (oldSlot.item.maximumAmount > 1 && oldSlot.amount > 1)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    newSlot.item = oldSlot.item;
                    newSlot.amount = Mathf.CeilToInt((float)oldSlot.amount / 2);
                    newSlot.isEmpty = false;
                    newSlot.isBroken = oldSlot.isBroken;
                    newSlot.SetIcon(oldSlot.iconGO.GetComponent<Image>().sprite);
                    newSlot.UpdateBrokenIcon();
                    newSlot.itemAmountText.text = newSlot.amount.ToString();
                    newSlot.healthItem = oldSlot.healthItem;

                    oldSlot.amount = Mathf.FloorToInt((float)oldSlot.amount / 2);
                    oldSlot.itemAmountText.text = oldSlot.amount.ToString();
                    return;
                }
                else if (Input.GetKey(KeyCode.LeftControl))
                {
                    newSlot.item = oldSlot.item;
                    newSlot.amount = 1;
                    newSlot.isEmpty = false;
                    newSlot.isBroken = oldSlot.isBroken;
                    newSlot.SetIcon(oldSlot.iconGO.GetComponent<Image>().sprite);
                    newSlot.UpdateBrokenIcon();
                    newSlot.itemAmountText.text = newSlot.amount.ToString();
                    newSlot.healthItem = oldSlot.healthItem;

                    oldSlot.amount--;
                    oldSlot.itemAmountText.text = oldSlot.amount.ToString();
                    return;
                }
            }
        }

        if (newSlot.item != null)
        {
            if (oldSlot.item.name.Equals(newSlot.item.name))
            {
                if (Input.GetKey(KeyCode.LeftShift) && oldSlot.amount > 1)
                {
                    if (Mathf.CeilToInt((float)oldSlot.amount / 2) < newSlot.item.maximumAmount - newSlot.amount)
                    {
                        newSlot.amount += Mathf.CeilToInt((float)oldSlot.amount / 2);
                        newSlot.itemAmountText.text = newSlot.amount.ToString();

                        oldSlot.amount -= Mathf.CeilToInt((float)oldSlot.amount / 2);
                        oldSlot.itemAmountText.text = oldSlot.amount.ToString();
                    }
                    else
                    {
                        int difference = newSlot.item.maximumAmount - newSlot.amount;
                        newSlot.amount = newSlot.item.maximumAmount;
                        newSlot.itemAmountText.text = newSlot.amount.ToString();

                        oldSlot.amount -= difference;
                        oldSlot.itemAmountText.text = oldSlot.amount.ToString();
                    }
                    return;
                }
                else if (Input.GetKey(KeyCode.LeftControl) && oldSlot.amount > 1)
                {
                    if (newSlot.item.maximumAmount != newSlot.amount)
                    {
                        newSlot.amount++;
                        newSlot.itemAmountText.text = newSlot.amount.ToString();

                        oldSlot.amount--;
                        oldSlot.itemAmountText.text = oldSlot.amount.ToString();
                    }
                    return;
                }
                else
                {
                    if (newSlot.amount + oldSlot.amount >= newSlot.item.maximumAmount)
                    {
                        if (oldSlot.item.itemType == ItemType.Resource)
                        {
                            int difference = newSlot.item.maximumAmount - newSlot.amount;
                            newSlot.amount = newSlot.item.maximumAmount;
                            newSlot.itemAmountText.text = newSlot.amount.ToString();

                            oldSlot.amount -= difference;
                            oldSlot.itemAmountText.text = oldSlot.amount.ToString();
                        }
                        else
                        {
                            oldSlot.itemAmountText.text = amount > 1 ? amount.ToString() : "";
                            newSlot.itemAmountText.text = amount > 1 ? amount.ToString() : "";
                        }
                    }
                    else
                    {
                        newSlot.amount += oldSlot.amount;
                        newSlot.itemAmountText.text = newSlot.amount.ToString();
                        NullifySlotData();
                    }
                    return;
                }
            }
        }

        if (oldSlot.item != null && (oldSlot.item.itemType == ItemType.Armor || oldSlot.item.itemType == ItemType.Shield || oldSlot.item.itemType == ItemType.Weapon))
        {
            if (newSlot.item != null && (newSlot.item.itemType == ItemType.Armor || newSlot.item.itemType == ItemType.Shield || oldSlot.item.itemType == ItemType.Weapon))
            {
                int tempHealth = oldSlot.healthItem;
                oldSlot.healthItem = newSlot.healthItem;
                newSlot.healthItem = tempHealth;
            }
            else
            {
                newSlot.healthItem = oldSlot.healthItem;
            }
        }

        newSlot.item = oldSlot.item;
        newSlot.amount = oldSlot.amount;
        newSlot.isEmpty = false;
        newSlot.isBroken = oldSlot.isBroken;

        if (!oldSlot.isEmpty)
        {
            newSlot.SetIcon(oldSlot.iconGO.GetComponent<Image>().sprite);
            newSlot.UpdateBrokenIcon();

            if (oldSlot.item.itemType == ItemType.Resource)
            {
                newSlot.itemAmountText.text = newSlot.amount.ToString();
            }
            else
            {
                newSlot.itemAmountText.text = oldSlot.amount > 1 ? oldSlot.amount.ToString() : "";
            }
        }
        else
        {
            newSlot.iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            newSlot.iconGO.GetComponent<Image>().sprite = null;
            newSlot.brokenIcon.SetActive(false);
            newSlot.itemAmountText.text = "";
        }

        oldSlot.item = item;
        oldSlot.amount = amount;
        oldSlot.healthItem = newSlotHealth;
        oldSlot.isEmpty = isEmpty;
        oldSlot.isBroken = isBroken;

        if (!isEmpty)
        {
            oldSlot.SetIcon(item.icon);
            oldSlot.UpdateBrokenIcon();
            oldSlot.itemAmountText.text = amount > 1 ? amount.ToString() : "";
        }
        else
        {
            oldSlot.iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            oldSlot.iconGO.GetComponent<Image>().sprite = null;
            oldSlot.brokenIcon.SetActive(false);
            oldSlot.itemAmountText.text = "";
        }
    }
}
