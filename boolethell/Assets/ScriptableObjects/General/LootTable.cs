using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LootTable : ScriptableObject
{
    public List<LootDrop> dropList;

    public List<LootDrop> GetDrops() { return dropList; }
    public void AddDrop(LootDrop newDrop) { dropList.Add(newDrop); }
    public void AddDrops(List<LootDrop> newDrops)
    {
        foreach (LootDrop drop in newDrops)
            dropList.Add(drop);
    }
    public void Clear() { dropList.Clear(); }
}
