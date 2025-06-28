using DevionGames;
using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject UIBG;
    public Transform inventoryPanel;
    public Transform mainInventoryPanel;
    public GameObject craftPanel;
    public GameObject magicBookPanel;
    public BookActivatePanel magicBookScript;
    public ItemControlHeath itemControlHeath;
    public Transform quickslotPanel;
    public List<InventorySlot> slots = new List<InventorySlot>();
    public bool isOpened;
    public float reachDistance = 3f;
    private Camera mainCamera;
    private CraftManager craftManager;
    public GameObject crossHair;
    [SerializeField] private Transform player;
    public Transform point;
    public Animator animator;
    public Transform attackPoint;
    public Behaviour playerMovement;
    public bool isItemHeld = false;
    public Book bookScript;
    public ArmorPart armorPart;
    public QuickslotInventory quickslotInventory;
    private CraftScriptableObject newCraftItem;

    public ThirdPersonCamera cameraController;
    private bool canTake = true;
    private bool isRepairTableOpen = false;

    [SerializeField]
    public ItemScriptableObject spel1;
    [SerializeField]
    public Sprite FirebollPage;
    public Sprite DefoultPage;
    public BookActivatePanel bookActivatePanel;
    public Rigidbody playerRigidbody;

    public ConversationManager conversationManager;
    public GameObject activeDialoge;
    public ScriptQuestKillWolf scriptQuestKillWolf;
    public NPCConversation firstDialogWithOldWilfred;
    public NPCConversation secondDialogWithOldWilfred;
    public NPCConversation DoneQuestDialogWithOldWilfred;
    public NPCConversation FinithDialogWithOldWilfred;

    private void Awake()
    {
        UIBG.SetActive(true);
    }

    void Start()
    {
        playerRigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        craftManager = FindObjectOfType<CraftManager>();
        cameraController = mainCamera.GetComponent<ThirdPersonCamera>();

        for (int i = 0; i < inventoryPanel.childCount; i++)
        {
            if (inventoryPanel.GetChild(i).GetComponent<InventorySlot>() != null)
            {
                slots.Add(inventoryPanel.GetChild(i).GetComponent<InventorySlot>());
            }
        }
        for (int i = 0; i < quickslotPanel.childCount; i++)
        {
            if (quickslotPanel.GetChild(i).GetComponent<InventorySlot>() != null)
            {
                slots.Add(quickslotPanel.GetChild(i).GetComponent<InventorySlot>());
            }
        }

        UIBG.SetActive(false);
        mainInventoryPanel.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            OpenAndCloseInventoryPanel();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(attackPoint.position, attackPoint.forward);
            RaycastHit hit;

            int layerMask = LayerMask.GetMask("Default", "Interactable", "RepeirTable", "CraftingTable", "Old Wilfred");

            Debug.DrawRay(attackPoint.position, attackPoint.forward * reachDistance, Color.green, 1f);

            if (Physics.Raycast(ray, out hit, reachDistance, layerMask))
            {
                Item itemComponent = hit.collider.GetComponent<Item>();
                ScrollCraft craftComponent = hit.collider.GetComponent<ScrollCraft>();

                if (itemComponent != null && canTake)
                {
                    if (itemComponent.item.isSpell)
                    {
                        StartCoroutine(PickupSpellPageWithDelay(itemComponent, hit.collider.gameObject));
                    }
                    else
                    {
                        StartCoroutine(PickupItemWithDelay(itemComponent, hit.collider.gameObject));
                    }
                    canTake = false;
                }
                else if (craftComponent != null && canTake)
                {
                    StartCoroutine(PickupSpellPageWithDelay(craftComponent, hit.collider.gameObject));
                    newCraftItem = craftComponent.scroll;
                }
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("RepeirTable"))
                {

                    ToggleRepairTableUI();
                }
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("CraftingTable"))
                {
                    craftManager.OpenAndCloseCraftPanel();
                }
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Old Wilfred"))
                {
                    if(scriptQuestKillWolf.questTake == false && scriptQuestKillWolf.questDone == false && scriptQuestKillWolf.questFineshed == false)
                    {
                        conversationManager.StartConversation(firstDialogWithOldWilfred);
                    }
                    else if (scriptQuestKillWolf.questTake == true && scriptQuestKillWolf.questDone == false && scriptQuestKillWolf.questFineshed == false)
                    {
                        conversationManager.StartConversation(secondDialogWithOldWilfred);
                    }
                    else if (scriptQuestKillWolf.questTake == true && scriptQuestKillWolf.questDone == true && scriptQuestKillWolf.questFineshed == false)
                    {
                        conversationManager.StartConversation(DoneQuestDialogWithOldWilfred);
                    }
                    else
                    {
                        conversationManager.StartConversation(FinithDialogWithOldWilfred);
                    }
                }
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Mage Eldar"))
                {

                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseAllUI();
        }
    }

    private void OpenAndCloseInventoryPanel()
    {
        if (!isItemHeld)
        {
            if (!Input.GetKey(KeyCode.Mouse0))
            {
                if (magicBookScript.isOpened && Input.GetKey(KeyCode.Mouse0))
                {
                    return;
                }
                if (activeDialoge.activeSelf)
                {
                    return;
                }
                playerRigidbody.constraints = RigidbodyConstraints.None;
                playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                isOpened = !isOpened;
                craftPanel.gameObject.SetActive(false);
                craftManager.isOpened = false;

                bookActivatePanel.CloseMagicBook();

                if (!magicBookScript.isOpened)
                {
                    if (isOpened)
                    {
                        OpenInventory();
                    }
                    else
                    {
                        CloseInventory();
                    }
                }
            }
        }
    }
    private void CloseAllUI()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        CloseInventory();
        bookActivatePanel.CloseMagicBook();
        if (isOpened)
        {
            isOpened = false;
        }

        if (craftPanel.gameObject.activeSelf)
        {
            craftPanel.gameObject.SetActive(false);
            craftManager.isOpened = false;
        }
    }
    private void ToggleRepairTableUI()
    {
        if (!isRepairTableOpen)
        {
            // Відкриваємо панель
            cameraController.canRotateCamera = false;
            UIBG.SetActive(true);
            crossHair.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            mainInventoryPanel.gameObject.SetActive(true);
            mainInventoryPanel.GetChild(0).gameObject.SetActive(true);
            mainInventoryPanel.GetChild(1).gameObject.SetActive(true);
            mainInventoryPanel.GetChild(2).gameObject.SetActive(false);

            if (playerMovement != null)
            {
                playerMovement.enabled = false;
            }

            // Заморожуємо Rigidbody, щоб гравець не міг рухатись
            if (playerRigidbody != null)
            {
                playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
            }
        }
        else
        {
            // Закриваємо панель
            cameraController.canRotateCamera = true;
            UIBG.SetActive(false);
            crossHair.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            mainInventoryPanel.gameObject.SetActive(false);

            if (playerMovement != null)
            {
                playerMovement.enabled = true;
            }

            // Розморожуємо Rigidbody, щоб гравець міг рухатись
            if (playerRigidbody != null)
            {
                playerRigidbody.constraints = RigidbodyConstraints.None;
                playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            }
        }

        // Перемикаємо стан
        isRepairTableOpen = !isRepairTableOpen;
    }

    private IEnumerator PickupSpellPageWithDelay(ScrollCraft itemComponent, GameObject itemObject)
    {
        animator.SetTrigger("Pickup");

        yield return new WaitForSeconds(0.6f);

        bookScript.AddPageWithDefault(FirebollPage, DefoultPage);

        Destroy(itemObject);
        AddNewItemToCraft();
        canTake = true;
    }
    public void AddNewItemToCraft()
    {
        if (craftManager != null && newCraftItem != null)
        {
            bool exists = craftManager.allCrafts.Exists(craft => craft == newCraftItem);

            if (!exists)
            {
                craftManager.allCrafts.Add(newCraftItem);
                
            }

        }
    }

    private IEnumerator PickupSpellPageWithDelay(Item itemComponent, GameObject itemObject)
    {
        animator.SetTrigger("Pickup");

        yield return new WaitForSeconds(0.6f);

        bookScript.AddPageWithDefault(FirebollPage, DefoultPage);

        Destroy(itemObject);
        canTake = true;
    }
    private IEnumerator PickupItemWithDelay(Item itemComponent, GameObject itemObject)
    {
        animator.SetTrigger("Pickup");

        yield return new WaitForSeconds(0.6f);

        AddItem(itemComponent.item, itemComponent.amount, itemComponent.healthItem, itemComponent.isBroken);
        craftManager.currentCraftItem.FillItemDetails();
        Destroy(itemObject);
        canTake = true;
    }
    private void OpenInventory()
    {
        cameraController.canRotateCamera = false;
        UIBG.SetActive(true);
        mainInventoryPanel.gameObject.SetActive(true);
        mainInventoryPanel.GetChild(0).gameObject.SetActive(false);
        mainInventoryPanel.GetChild(1).gameObject.SetActive(true);
        mainInventoryPanel.GetChild(2).gameObject.SetActive(true);
        crossHair.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
    }

    private void CloseInventory()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        UIBG.SetActive(false);
        mainInventoryPanel.gameObject.SetActive(false);
        crossHair.SetActive(true);
        cameraController.canRotateCamera = true;

        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
    }

    public void AddItem(ItemScriptableObject _item, int _amount, int _itemHealth, bool _isBroken)
    {
        int amount = _amount;

        foreach (InventorySlot slot in slots)
        {
            if (!slot.isEmpty && slot.item == _item && slot.amount < _item.maximumAmount)
            {
                int spaceLeft = _item.maximumAmount - slot.amount;

                if (amount <= spaceLeft)
                {
                    slot.amount += amount;
                    if (slot.item.maximumAmount != 1)
                    {
                        slot.itemAmountText.text = slot.amount.ToString();
                    }
                    return;
                }
                else
                {
                    slot.amount = _item.maximumAmount;
                    amount -= spaceLeft;
                    if (slot.item.maximumAmount != 1)
                    {
                        slot.itemAmountText.text = slot.amount.ToString();
                    }
                }
            }
        }

        foreach (InventorySlot slot in slots)
        {
            if (slot.isEmpty)
            {
                slot.item = _item;
                slot.isEmpty = false;
                slot.SetIcon(_item.icon);
                slot.healthItem = _itemHealth;

                if (_isBroken == true)
                {
                    slot.isBroken = true;
                    slot.brokenIcon .SetActive(true);
                }
                if (amount <= _item.maximumAmount)
                {
                    slot.amount = amount;
                    if (slot.item.maximumAmount != 1)
                    {
                        slot.itemAmountText.text = slot.amount.ToString();
                    }
                    return;
                }
                else
                {
                    slot.amount = _item.maximumAmount;
                    amount -= _item.maximumAmount;
                    if (slot.item.maximumAmount != 1)
                    {
                        slot.itemAmountText.text = slot.amount.ToString();
                    }
                }
            }
        }

        if (amount > 0)
        {
            GameObject itemObject = Instantiate(_item.itemDropPrefab, player.position + Vector3.up + player.forward, Quaternion.identity);
            itemObject.GetComponent<Item>().amount = amount;
        }
    }
    public void AddItemFromButton()
    {
        if (IsSpellInInventory(spel1))
        {
            return;
        }

        AddItem(spel1, 1, 0, false);
        craftManager.currentCraftItem.FillItemDetails();
    }

    private bool IsSpellInInventory(ItemScriptableObject item)
    {
        if (item.itemType == ItemType.Spell)
        {
            foreach (InventorySlot slot in slots)
            {
                if (!slot.isEmpty && slot.item == item)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public void AddToBrokenItems(ItemScriptableObject brokenItem)
    {
        int amount = 1;

        foreach (InventorySlot slot in slots)
        {
            if (!slot.isEmpty && slot.item == brokenItem && slot.amount < brokenItem.maximumAmount)
            {
                int spaceLeft = brokenItem.maximumAmount - slot.amount;

                if (amount <= spaceLeft)
                {
                    slot.amount += amount;
                    if (slot.item.maximumAmount != 1)
                    {
                        slot.itemAmountText.text = slot.amount.ToString();
                    }
                    return;
                }
                else
                {
                    slot.amount = brokenItem.maximumAmount;
                    amount -= spaceLeft;
                    if (slot.item.maximumAmount != 1)
                    {
                        slot.itemAmountText.text = slot.amount.ToString();
                    }
                }
            }
        }

        foreach (InventorySlot slot in slots)
        {
            if (slot.isEmpty)
            {
                slot.item = brokenItem;
                slot.amount = amount;
                slot.isEmpty = false;
                slot.isBroken = true;
                slot.brokenIcon.SetActive(true);
                slot.SetIcon(brokenItem.icon);
                return;
            }
        }

        if (amount > 0)
        {
            GameObject brokenItemObject = Instantiate(brokenItem.itemDropPrefab, player.position + Vector3.up + player.forward, Quaternion.identity);
            brokenItemObject.GetComponent<Item>().amount = amount;
        }
    }

}
