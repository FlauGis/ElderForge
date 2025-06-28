using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class RepairItemManager : MonoBehaviour
{
    public InventorySlot itemSlot;
    public InventorySlot resourcesSlot;
    public InventorySlot repairedItemSlot;

    private Sprite iconResourcesNeeeded;
    private string itemTextResourcesNeeeded;

    public GameObject iconResources;
    public GameObject textResources;

    private TMP_Text textComponent;
    void Update()
    {
        if (itemSlot.item == null)
        {
            iconResources.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            iconResources.GetComponent<Image>().sprite = null;
            textComponent = textResources.GetComponent<TMP_Text>();
            textComponent.text = "";
            return;
        }
        if (itemSlot.item != null)
        {
            FillNeedResources();
            if (itemSlot.isBroken)
            {
                if (resourcesSlot.item == itemSlot.item.resourseNeededToRapair)
                {
                    FixItem();
                }
            }
        }
    }
    public void FillNeedResources()
    {
        ItemScriptableObject resourse = itemSlot.item.resourseNeededToRapair;
        iconResourcesNeeeded = resourse.icon;
        itemTextResourcesNeeeded = resourse.itemName;
        iconResources.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        iconResources.GetComponent<Image>().sprite = iconResourcesNeeeded;
        textComponent = textResources.GetComponent<TMP_Text>();
        textComponent.text = itemTextResourcesNeeeded;

        
    }

    public void FixItem()
    {
        repairedItemSlot.item = itemSlot.item;
        repairedItemSlot.amount = itemSlot.amount;
        repairedItemSlot.isEmpty = itemSlot.isEmpty;
        repairedItemSlot.SetIcon(itemSlot.item.icon);
        repairedItemSlot.healthItem = 100;
    }
}
