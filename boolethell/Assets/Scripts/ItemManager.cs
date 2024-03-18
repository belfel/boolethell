using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] List<GameObject> pistolPrefabs;
    [SerializeField] List<GameObject> smgPrefabs;
    [SerializeField] List<GameObject> shotgunPrefabs;
    [SerializeField] List<GameObject> revolverPrefabs;
    [SerializeField] List<GameObject> flamethrowerPrefabs;
    [SerializeField] List<GameObject> lifestonePrefabs;


    public GameObject GetItemPrefab(UnlockManager.EItem item, Rarity.ERarity rarity)
    {
        GameObject prefab = null;

        switch (item) 
        {
            case UnlockManager.EItem.pistol:
                prefab = pistolPrefabs[(int)rarity - 1];
                break;
            case UnlockManager.EItem.smg:
                prefab = smgPrefabs[(int)rarity - 1];
                break;
            case UnlockManager.EItem.shotgun:
                prefab = shotgunPrefabs[(int)rarity - 1];
                break;
            case UnlockManager.EItem.revolver:
                prefab = revolverPrefabs[(int)rarity - 1];
                break;
            case UnlockManager.EItem.flamethrower:
                prefab = flamethrowerPrefabs[(int)rarity - 1];
                break;
            case UnlockManager.EItem.lifestone:
                prefab = lifestonePrefabs[(int)rarity - 1];
                break;
        }

        return prefab;
    }
}
