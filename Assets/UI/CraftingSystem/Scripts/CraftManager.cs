using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using TMPro;
using TMPro.Examples;
using DevionGames;
using System.Drawing;

public class CraftManager : MonoBehaviour
{
    public bool isOpened;
    public GameObject craftingPanel;
    public GameObject mainInventoryPanel;
    public GameObject magicBookPanel;
    public BookActivatePanel magicBookScript;
    public GameObject crosshair;

    public Transform craftItemsPanel;
    public GameObject craftItemButtonPrefab;

    public GameObject UIBG;
    public Button craftBtn;
    public FillCraftItemDetails currentCraftItem;

    public KeyCode openCloseCraftButton;

    public List<CraftScriptableObject> allCrafts;

    private ThirdPersonCamera cameraController;
    [Header("Craft Item Details")]
    public TMP_Text craftItemName;
    public TMP_Text craftItemDescription;
    public Image craftItemImage;
    public TMP_Text craftItemDuration;
    public TMP_Text craftItemAmount;
    private Camera mainCamera;
    public Transform point;
    private Rigidbody playerRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        cameraController = mainCamera.GetComponent<ThirdPersonCamera>();
        playerRigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();

        GameObject craftItemButton = Instantiate(craftItemButtonPrefab, craftItemsPanel);
        craftItemButton.GetComponent<Image>().sprite = allCrafts[0].finalCraft.icon;
        craftItemButton.GetComponent<FillCraftItemDetails>().currentCraftItem = allCrafts[0];
        craftItemButton.GetComponent<FillCraftItemDetails>().FillItemDetails();
        Destroy(craftItemButton);

        craftingPanel.gameObject.SetActive(false);
    }
    public void FillItemDetailsHelper()
    {
       currentCraftItem.FillItemDetails();
    }
    // Update is called once per fram
    public void OpenAndCloseCraftPanel()
    {
        if (magicBookScript.isOpened && Input.GetKey(KeyCode.Mouse0))
        {
            return;
        }
        isOpened = !isOpened;

        mainInventoryPanel.gameObject.SetActive(false);

        if (magicBookScript.isOpened)
        {
            magicBookPanel.SetActive(false);
            magicBookScript.isOpened = false;
        }

        if (magicBookScript.isOpened == false)
        {
            if (isOpened)
            {
                OpenCraftPanel();
                FreezePlayerMovement(); // Заморожуємо рух гравця при відкритті панелі крафту
            }
            else
            {
                CloseCraftPanel();
                UnfreezePlayerMovement(); // Розморожуємо рух гравця при закритті панелі крафту
            }
        }
    }

    // Заморожуємо рух гравця
    private void FreezePlayerMovement()
    {
        if (playerRigidbody != null)
        {
            playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    // Розморожуємо рух гравця
    private void UnfreezePlayerMovement()
    {
        if (playerRigidbody != null)
        {
            playerRigidbody.constraints = RigidbodyConstraints.None;
            playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    private void OpenCraftPanel()
    {
        magicBookScript.isOpened = false;
        craftingPanel.SetActive(true);
        UIBG.SetActive(true);
        crosshair.SetActive(false);
        cameraController.canRotateCamera = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    private void CloseCraftPanel()
    {
        craftingPanel.SetActive(false);
        UIBG.SetActive(false);
        crosshair.SetActive(true);
        cameraController.canRotateCamera = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void LoadCraftItems(string craftType)
    {
        for (int i = 0; i < craftItemsPanel.childCount; i++)
        {
            Destroy(craftItemsPanel.GetChild(i).gameObject);
        }
        foreach (CraftScriptableObject cso in allCrafts)
        {
            if (cso.craftType.ToString().ToLower() == craftType.ToLower())
            {
                GameObject craftItemButton = Instantiate(craftItemButtonPrefab, craftItemsPanel);
                craftItemButton.transform.GetChild(0).GetComponent<Image>().sprite = cso.finalCraft.icon;
                craftItemButton.transform.GetComponent<FillCraftItemDetails>().currentCraftItem = cso;
            }
        }
    }
}
