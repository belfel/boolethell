using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LootDrop : ScriptableObject
{
    public UnlockManager.EItem item;
    public Rarity.ERarity rarity;
    public float chance;
}
