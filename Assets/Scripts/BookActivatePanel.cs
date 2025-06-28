using DevionGames;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro.Examples;
using UnityEngine;

public class BookActivatePanel : MonoBehaviour
{
    public GameObject magickBook;
    public GameObject inventoryPanel;
    public GameObject craftPanel;
    public GameObject UIBG;
    public GameObject crossHair;
    public CraftManager craftManager;
    public InventoryManager inventoryManager;
    public bool isOpened;

    public Behaviour playerMovement;
    private ThirdPersonCamera cameraController;
    private Camera mainCamera;


    private void Start()
    {
        mainCamera = Camera.main;
        cameraController = mainCamera.GetComponent<ThirdPersonCamera>();

        magickBook.SetActive(false);
    }
    public void OpenMagicBook()
    {
        if (inventoryManager.activeDialoge.activeSelf)
        {
            return;
        }
        magickBook.SetActive(true);
        cameraController.canRotateCamera = false;
        UIBG.SetActive(true);
        inventoryPanel.SetActive(false);
        craftPanel.SetActive(false);
        crossHair.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
    }
    public void CloseMagicBook()
    {
        if (!Input.GetKey(KeyCode.Mouse0))
        {
            magickBook.SetActive(false);
            UIBG.SetActive(false);
            crossHair.SetActive(true);
            cameraController.canRotateCamera = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (playerMovement != null)
            {
                playerMovement.enabled = true;
            }
        }
    }
}
