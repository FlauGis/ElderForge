using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public ItemScriptableObject item;
    public int amount;
    public bool isEmpty = true;
    public GameObject iconGO;
    public GameObject brokenIcon;
    public TMP_Text itemAmountText;
    public ClothType clothType = ClothType.None;
    public bool clothSlot = false;
    public bool isBroken = false;
    public bool isRepeirSlot = false;
    public int healthItem;

    public RepairItemManager repairItemManager;
    private void Awake()
    {
        if(clothSlot == false)
        {
            brokenIcon = transform.GetChild(1).GetChild(2).gameObject;
            iconGO = transform.GetChild(1).GetChild(0).gameObject;
            itemAmountText = transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>();
        }
        else
        {
            brokenIcon = transform.GetChild(2).GetChild(2).gameObject;
            iconGO = transform.GetChild(2).GetChild(0).gameObject;
            itemAmountText = transform.GetChild(2).GetChild(1).GetComponent<TMP_Text>();
        }

    }
    public void SetIcon(Sprite icon)
    {
        iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        iconGO.GetComponent<Image>().sprite = icon;
    }
    public void UpdateBrokenIcon()
    {
        if (brokenIcon != null)
        {
            brokenIcon.SetActive(isBroken);
        }
    }

}
