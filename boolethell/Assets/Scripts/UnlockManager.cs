using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class UnlockManager : MonoBehaviour
{
    public static UnlockManager instance;

    public enum EItem
    {
        pistol, smg, shotgun, lifestone
    }

    // 0=not unlocked, 1=common, 2=uncommon, 3=rare, 4=legendary
    private Dictionary<Type, short> unlocks = new Dictionary<Type, short>();
    private Dictionary<EItem, Type> itemTypes = new Dictionary<EItem, Type>();

    private void Start()
    {
        unlocks.Add(typeof(Pistol), 1);
        unlocks.Add(typeof(Shotgun), 0);
        unlocks.Add(typeof(SMG), 0);
        unlocks.Add(typeof(LifeStone), 1);
    }

    public short GetRarity(Type type)
    {
        short value;
        if (unlocks.TryGetValue(type, out value))
        {
            return value;
        }
        else return 0;
    }

    public bool IsUnlocked(Type type)
    {
        short value;
        if (unlocks.TryGetValue(type, out value))
        {
            if (value != 0)
                return true;
        }
        return false;
    }

    public Type GetItemType(EItem item)
    {
        Type type;
        if (!itemTypes.TryGetValue(item, out type))
            Debug.LogError("Requested item not in dictionary.");
        return type;
    }

    public EItem GetEItemFromType(Type type)
    {
        return itemTypes.FirstOrDefault(x => x.Value == type).Key;
    }

    public void UnlockItem(EItem item, Rarity.ERarity rarity)
    {
        short val = 99;
        unlocks.TryGetValue(GetItemType(item), out val);
        if (val < (short)rarity)
            unlocks[GetItemType(item)] = (short)rarity;
    }

    private void LinkItemsToTypes()
    {
        itemTypes.Add(EItem.pistol, typeof(Pistol));
        itemTypes.Add(EItem.smg, typeof(SMG));
        itemTypes.Add(EItem.shotgun, typeof(Shotgun));
        itemTypes.Add(EItem.lifestone, typeof(LifeStone));
    }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        LinkItemsToTypes();
    }
}
