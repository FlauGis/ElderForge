using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemControlHeath : MonoBehaviour
{
    public GameObject HeatHP;
    public GameObject BodyHP;
    public GameObject LegsHP;
    public GameObject FeetHP;
    public GameObject ShieldHP;
    public GameObject SwordHP;

    public void Start()
    {
        HeatHP.SetActive(false);
        BodyHP.SetActive(false);
        LegsHP.SetActive(false);
        FeetHP.SetActive(false);
        ShieldHP.SetActive(false);
        SwordHP.SetActive(false);
    }
}
