using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StageSelectData : ScriptableObject
{
    public Tuple<Type, Rarity.ERarity> weapon;
    public Tuple<Type, Rarity.ERarity> accessory1;
    public Tuple<Type, Rarity.ERarity> accessory2;
    public Tuple<Type, Rarity.ERarity> accessory3;
    public Type boss;
}
