using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StageSelectData : ScriptableObject
{
    public GameObject weapon;
    public List<Tuple<int, GameObject>> accessories = new List<Tuple<int, GameObject>>();
    public Type boss;
}
