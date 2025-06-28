using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum ItemType { Default, Food, Weapon, Instrument, Spell, Armor, Shield, Resource, SpellBook}
public enum ClothType { None, Head, Body, Legs, Feet, Shield, Nofing }

public class ItemScriptableObject : ScriptableObject
{
    public string itemName;
    public int maximumAmount;
    public GameObject itemDropPrefab;
    public GameObject itemPrefabArmorInPlayer;
    public Sprite icon;
    public ItemType itemType;
    public ClothType clothType = ClothType.None;
    public string itemDescription;
    public bool isConsumeable;
    public bool isSpell = false;
    public string inHandName;
    public int itemHealth;
    public bool isBroken = false;

    [Header("Armor Characteristics")]
    public int maximumDurability;

    [Header("Consumable Characteristics")]
    public float changeHealth;
    public float changeHunger;

    [Header("RepairItem")]
    public ItemScriptableObject resourseNeededToRapair;

}
