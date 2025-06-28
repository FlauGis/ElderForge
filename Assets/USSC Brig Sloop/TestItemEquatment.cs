using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItemEquatment : MonoBehaviour
{
    [SerializeField] private GameObject topPrefab;
    [SerializeField] private GameObject pantsPrefab;
    [SerializeField] private GameObject shoesPrefab;
    [SerializeField] private GameObject chestPlatePrefab;
    [SerializeField] private GameObject startpants;
    [SerializeField] private SkinnedMeshRenderer playerSkin1;
    [SerializeField] private SkinnedMeshRenderer playerSkin2;
    [SerializeField] private SkinnedMeshRenderer playerSkin3;
    [SerializeField] private SkinnedMeshRenderer playerSkin4;
    void Start()
    {
        addClothesplayerSkin1(startpants);
        addClothesplayerSkin2(startpants);
        addClothesplayerSkin3(startpants);
        addClothesplayerSkin4(topPrefab);
        addClothesplayerSkin4(pantsPrefab);
        addClothesplayerSkin4(shoesPrefab);
        addClothesplayerSkin4(chestPlatePrefab);
    }

    void addClothesplayerSkin1(GameObject clothPrefab)
    {
        GameObject clothObj = Instantiate(clothPrefab, playerSkin1.transform.parent);
        SkinnedMeshRenderer[] renderers = clothObj.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer renderer in renderers)
        {
            renderer.bones = playerSkin1.bones;
            renderer.rootBone = playerSkin1.rootBone;
        }
    }
    void addClothesplayerSkin2(GameObject clothPrefab)
    {
        GameObject clothObj = Instantiate(clothPrefab, playerSkin2.transform.parent);
        SkinnedMeshRenderer[] renderers = clothObj.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer renderer in renderers)
        {
            renderer.bones = playerSkin2.bones;
            renderer.rootBone = playerSkin2.rootBone;
        }
    }
    void addClothesplayerSkin3(GameObject clothPrefab)
    {
        GameObject clothObj = Instantiate(clothPrefab, playerSkin3.transform.parent);
        SkinnedMeshRenderer[] renderers = clothObj.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer renderer in renderers)
        {
            renderer.bones = playerSkin3.bones;
            renderer.rootBone = playerSkin3.rootBone;
        }
    }
    void addClothesplayerSkin4(GameObject clothPrefab)
    {
        GameObject clothObj = Instantiate(clothPrefab, playerSkin4.transform.parent);
        SkinnedMeshRenderer[] renderers = clothObj.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer renderer in renderers)
        {
            renderer.bones = playerSkin4.bones;
            renderer.rootBone = playerSkin4.rootBone;
        }
    }
}
