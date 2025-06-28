using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonActivateBook : MonoBehaviour
{
    public Book bookScript;

    [SerializeField]
    public GameObject ButtonPage1;
    [SerializeField]
    public GameObject ButtonPage2;
    [SerializeField]
    public GameObject ButtonPage3;
    [SerializeField]
    public GameObject ButtonPage4;
    [SerializeField]
    public GameObject ButtonPage5;

    public void ActivateButton()
    {
        if(bookScript.currentPage == 0 )
        {
            ButtonPage1.SetActive(false);
            ButtonPage2.SetActive(false);
            ButtonPage3.SetActive(false);
            ButtonPage4.SetActive(false);
            ButtonPage5.SetActive(false);
        }
        else if(bookScript.currentPage == 2 && bookScript.TotalPageCount == 4)
        {
            ButtonPage1.SetActive(true);
            ButtonPage2.SetActive(false);
            ButtonPage3.SetActive(false);
            ButtonPage4.SetActive(false);
            ButtonPage5.SetActive(false);
        }
        else if (bookScript.currentPage == 4 && bookScript.TotalPageCount == 6)
        {
            ButtonPage1.SetActive(false);
            ButtonPage2.SetActive(true);
            ButtonPage3.SetActive(false);
            ButtonPage4.SetActive(false);
            ButtonPage5.SetActive(false);
        }
        else if(bookScript.currentPage == 6 && bookScript.TotalPageCount == 8)
        {
            ButtonPage1.SetActive(false);
            ButtonPage2.SetActive(false);
            ButtonPage3.SetActive(true);
            ButtonPage4.SetActive(false);
            ButtonPage5.SetActive(false);
        }
        else if (bookScript.currentPage == 8 && bookScript.TotalPageCount == 10)
        {
            ButtonPage1.SetActive(false);
            ButtonPage2.SetActive(false);
            ButtonPage3.SetActive(false);
            ButtonPage4.SetActive(true);
            ButtonPage5.SetActive(false);
        }
        else if (bookScript.currentPage == 10 && bookScript.TotalPageCount == 12)
        {
            ButtonPage1.SetActive(false);
            ButtonPage2.SetActive(false);
            ButtonPage3.SetActive(false);
            ButtonPage4.SetActive(false);
            ButtonPage5.SetActive(true);
        }
        else
        {
            ButtonPage1.SetActive(false);
            ButtonPage2.SetActive(false);
            ButtonPage3.SetActive(false);
            ButtonPage4.SetActive(false);
            ButtonPage5.SetActive(false);
        }
    }
}
