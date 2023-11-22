using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeStone : Accessory
{
    [SerializeField] private FloatList modifierList;
    [SerializeField] private FloatVariable lifestoneHPVariable;
    [SerializeField] private float maxHPAdded;

    public override void ApplyEffect()
    {
        lifestoneHPVariable.SetValue(maxHPAdded);
        modifierList.AddVariable(lifestoneHPVariable);
    }

    private void OnDisable()
    {
        modifierList.RemoveVariable(lifestoneHPVariable);
    }
}
