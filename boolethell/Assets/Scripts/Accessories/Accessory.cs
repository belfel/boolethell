using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Accessory : MonoBehaviour
{
    [SerializeField] protected Rarity.ERarity rarity = Rarity.ERarity.common;

    public abstract void ApplyEffect();
}
