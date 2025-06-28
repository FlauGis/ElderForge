using DevionGames;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickslotInventory : MonoBehaviour
{
    public Transform quickslotParent;
    public InventoryManager inventoryManager;
    public int currentQuickslotID = 0;
    public Sprite selectedSprite;
    public Sprite notSelectedSprite;
    public Text healthText;
    public Transform itemContainer;
    public InventorySlot activeSlot = null;
    public Transform allWeapons;
    public Transform allShield;
    public Animator animator;
    public ItemControlHeath itemControlHeath;
    public Slider swordSliderHealth;
    [SerializeField] private ThirdPersonController thirdPersonController;
    [SerializeField] private int useStaminaCostSword = 15;
    [SerializeField] private float waitNextAttack1 = 1f;
    [SerializeField] private float waitNextAttack2 = 1f;
    [SerializeField] private float waitNextAttack3 = 1f;
    [SerializeField] private float waitAttackDamage1 = 1f;
    [SerializeField] private float waitAttackDamage2 = 1f;
    [SerializeField] private float waitAttackDamage3 = 1f;
    [SerializeField] private float waitEatAnimation = 1f;

    public GameObject useSpellFireboll;
    public GameObject magickManaSlider;

    private float lastHitTime = 0f;
    private int hitCounter = 0;
    private float timeBetweenAttacks = 2f;
    private bool isAttacking = false;
    private bool isEat = false;
    private Image currentSlotImage;
    private InventorySlot currentSlot;
    public BookActivatePanel bookActivatePanel;

    void Start()
    {

        currentSlotImage = quickslotParent.GetChild(currentQuickslotID).GetChild(0).GetComponent<Image>();
        currentSlot = quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>();
    }

    void Update()
    {
        if (inventoryManager.craftPanel.activeInHierarchy) return;

        for (int i = 0; i < quickslotParent.childCount; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                HandleQuickslotSelection(i);
            }
        }

        HandleItemUse();
    }

    private void HandleQuickslotSelection(int i)
    {
        if (currentQuickslotID == i)
        {
            ToggleQuickslotSelection();
        }
        else
        {
            SetQuickslotSelection(i);
        }
    }


    private void ToggleQuickslotSelection()
    {
        if (currentSlotImage.sprite == notSelectedSprite)
        {
            currentSlotImage.sprite = selectedSprite;
            activeSlot = currentSlot;
            ShowItemInHand();

            if (activeSlot?.item != null && activeSlot.item.itemType == ItemType.Spell)
            {
                useSpellFireboll.SetActive(true);
                useSpellFireboll.GetComponent<FireballScript>().canShoot = true;
                magickManaSlider.SetActive(true);
            }
        }
        else
        {
            currentSlotImage.sprite = notSelectedSprite;

            if (activeSlot?.item != null && activeSlot.item.itemType == ItemType.Spell)
            {
                useSpellFireboll.SetActive(false);
                magickManaSlider.SetActive(false);
            }

            activeSlot = null;
            HideItemsInHand();
        }
    }


    private void SetQuickslotSelection(int i)
    {
        if (activeSlot?.item != null && activeSlot.item.itemType == ItemType.Spell)
        {
            useSpellFireboll.SetActive(false);
            magickManaSlider.SetActive(false);
        }

        bookActivatePanel.CloseMagicBook();
        currentSlotImage.sprite = notSelectedSprite;
        currentQuickslotID = i;
        currentSlotImage = quickslotParent.GetChild(currentQuickslotID).GetChild(0).GetComponent<Image>();
        currentSlot = quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>();
        currentSlotImage.sprite = selectedSprite;
        activeSlot = currentSlot;

        if (activeSlot?.item != null && activeSlot.item.itemType == ItemType.Spell)
        {
            useSpellFireboll.SetActive(true);
            useSpellFireboll.GetComponent<FireballScript>().canShoot = true;
            magickManaSlider.SetActive(true);
        }

        ShowItemInHand();
    }

    private void HandleItemUse()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

            if(activeSlot?.item != null)
            {
                if (activeSlot.item.isConsumeable && !inventoryManager.isOpened && currentSlotImage.sprite == selectedSprite)
                {
                    if (activeSlot.item.itemType == ItemType.Food)
                    {
                        if (isEat == false)
                        {
                            isEat = true;
                            StartCoroutine(WaitEatAnimation(waitEatAnimation));
                        }
                    }
                    else if (activeSlot.item.itemType == ItemType.Weapon)
                    {
                        if (activeSlot.healthItem > 0)
                        {
                            UseWeapon();
                        }
                    }
                    else if(activeSlot.item.itemType == ItemType.SpellBook)
                    {
                        bookActivatePanel.OpenMagicBook();
                    }
                }
            }

        }
    }
 
    private void UseWeapon()
    {
        if (thirdPersonController.staminaSlider.value < useStaminaCostSword || isAttacking) return;

        if (Time.time - lastHitTime <= timeBetweenAttacks)
        {
            hitCounter++;
            if (hitCounter > 3) hitCounter = 1;
        }
        else
        {
            hitCounter = 1;
        }

        switch (hitCounter)
        {
            case 1:
                StartCoroutine(WaitNextSwordAttack1(waitNextAttack1));
                StartCoroutine(WaitAttackAnimation1(waitAttackDamage1));
                break;
            case 2:
                StartCoroutine(WaitNextSwordAttack2(waitNextAttack2));
                StartCoroutine(WaitAttackAnimation1(waitAttackDamage2));

                break;
            case 3:
                StartCoroutine(WaitNextSwordAttack3(waitNextAttack3));
                StartCoroutine(WaitAttackAnimation1(waitAttackDamage3));
                break;
        }

        lastHitTime = Time.time;
        isAttacking = true;
        thirdPersonController.UseStamina(useStaminaCostSword);
        activeSlot.healthItem -= 2;
        swordSliderHealth.value = activeSlot.healthItem;
    }

    public void SwordBreak()
    {
        if (activeSlot.healthItem <= 0)
        {
            inventoryManager.AddToBrokenItems(activeSlot.item);

            activeSlot.item = null;
            activeSlot.amount = 0;
            activeSlot.isEmpty = true;
            activeSlot.iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            activeSlot.iconGO.GetComponent<Image>().sprite = null;
            activeSlot.itemAmountText.text = "";

            itemControlHeath.SwordHP.gameObject.SetActive(false);
            HideItemsInHand();
        }
    }
    private IEnumerator WaitEatAnimation(float delay)
    {
        animator.SetTrigger("Eat");
        yield return new WaitForSeconds(delay);
        thirdPersonController.RestoreHealthAndFoood((int)activeSlot.item.changeHealth, (int)activeSlot.item.changeHunger);
        if (activeSlot.amount <= 1)
        {
            activeSlot.GetComponentInChildren<DragAndDropItem>().NullifySlotData();
        }
        else
        {
            activeSlot.amount--;
            activeSlot.itemAmountText.text = activeSlot.amount.ToString();
        }
        isEat = false;
    }
    private void ShowWeaponAttackEffect()
    {
        if (activeSlot?.item == null) return;

        for (int i = 0; i < allWeapons.childCount; i++)
        {
            Transform weapon = allWeapons.GetChild(i);
            if (activeSlot.item.inHandName == weapon.name)
            {
                DamageScript weaponScript = weapon.GetComponent<DamageScript>();
                if (weaponScript != null)
                {
                    weaponScript.Attack();  
                }
            }
        }
    }
    private IEnumerator WaitAttackAnimation1(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowWeaponAttackEffect();
    }

    private IEnumerator WaitNextSwordAttack1(float delay)
    {
        animator.SetTrigger("Sword Attack1");
        yield return new WaitForSeconds(delay);
        isAttacking = false;
        SwordBreak();
    }

    private IEnumerator WaitNextSwordAttack2(float delay)
    {
        animator.SetTrigger("Sword Attack2");
        yield return new WaitForSeconds(delay);
        isAttacking = false;
        SwordBreak();
    }

    private IEnumerator WaitNextSwordAttack3(float delay)
    {
        animator.SetTrigger("Sword Attack3");
        yield return new WaitForSeconds(delay);
        isAttacking = false;
        SwordBreak();
    }


    private void ShowItemInHand()
    {
        HideItemsInHand();
        if (activeSlot?.item == null) return;

        if (activeSlot.item.itemType == ItemType.Weapon && activeSlot.healthItem <= 0) return;

        for (int i = 0; i < allWeapons.childCount; i++)
        {
            if (activeSlot.item.inHandName == allWeapons.GetChild(i).name)
            {
                allWeapons.GetChild(i).gameObject.SetActive(true);
                itemControlHeath.SwordHP.SetActive(true);
                Transform secondChild = itemControlHeath.SwordHP.transform.GetChild(1);
                secondChild.GetComponent<Slider>().value = activeSlot.healthItem;
            }
        }
    }

    private void HideItemsInHand()
    {
        for (int i = 0; i < allWeapons.childCount; i++)
        {
            allWeapons.GetChild(i).gameObject.SetActive(false);
            itemControlHeath.SwordHP.SetActive(false);
        }
    }

    public void CheckItemInHand()
    {
        if (activeSlot != null && activeSlot.item != null)
        {
            ShowItemInHand();
        }
        else
        {
            HideItemsInHand();
        }
    }
}
