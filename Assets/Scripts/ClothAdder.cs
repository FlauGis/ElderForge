using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ваш скрипт должен иметь название "ClothAdder" 
public class ClothAdder : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer playerSkin;
    [SerializeField] private List<GameObject> _equipedClothes;

    private void Start()
    {
        _equipedClothes = new List<GameObject>();
    }

    public void addClothes(GameObject clothPrefab)
    {
        GameObject clothObj = Instantiate(clothPrefab, playerSkin.transform.parent);
        SkinnedMeshRenderer[] renderers = clothObj.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer renderer in renderers)
        {
            renderer.bones = playerSkin.bones;
            renderer.rootBone = playerSkin.rootBone;
        }
        _equipedClothes.Add(clothObj);
    }
    public void removeClothes(GameObject searchedClothObject)
    {
        foreach (GameObject clothObj in _equipedClothes)
        {
            if(clothObj.name.Contains(searchedClothObject.name))
            {
                _equipedClothes.Remove(clothObj);
                Destroy(clothObj);
                return;
            }    
        }

    }
}