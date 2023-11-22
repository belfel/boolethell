using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardsManager : MonoBehaviour
{
    public LootTable table;
    public LootTable rolledRewards;
    private List<LootDrop> drops = new List<LootDrop>();

    public GameEvent rewardsRolled;

    public void RollRewards()
    {
        drops = RollRewardsFromTable();
        rolledRewards.Clear();
        rolledRewards.AddDrops(drops);
        rewardsRolled.Raise();
        UnlockRolledItems(drops);
    }

    private void UnlockRolledItems(List<LootDrop> items)
    {
        foreach (LootDrop drop in drops)
        {
            UnlockManager.instance.UnlockItem(drop.item, drop.rarity);
        }
    }

    private List<LootDrop> RollRewardsFromTable()
    {
        int numOfRarities = Enum.GetNames(typeof(Rarity.ERarity)).Length;

        List<LootDrop> drops = table.GetDrops();
        List<LootDrop> rolledDrops = new List<LootDrop>();
        List<UnlockManager.EItem> items = new List<UnlockManager.EItem>();

        float[] rarityChances = new float[numOfRarities];

        foreach (LootDrop drop in drops)
        {
            if (!items.Contains(drop.item))
                items.Add(drop.item);
        }

        foreach (UnlockManager.EItem item in items)
        {
            LootDrop drop;
            for (int i = 1; i < numOfRarities + 1; i++)
            {
                drop = drops.Find(x => x.item == item && x.rarity == (Rarity.ERarity)i);
                if (drop) rarityChances[i-1] = drop.chance;
            }

            float roll = UnityEngine.Random.Range(1f, 100f);
            float leftBracket = 0f;
            float rightBracket = rarityChances[0];
            int unlockedRarity = -1;

            for (int i = 1; i < numOfRarities + 1; i++)
            {
                if (roll > leftBracket && roll <= rightBracket)
                    unlockedRarity = i;

                leftBracket += rarityChances[i-1];
                if (!(i == numOfRarities))
                    rightBracket += rarityChances[i];
            }
            leftBracket += rarityChances[numOfRarities - 1];

            Array.Clear(rarityChances, 0, numOfRarities);

            if (roll >= leftBracket)
                continue;
            if (unlockedRarity == -1)
                Debug.LogError("Error in calculation for item: " + item + ". Rarity: " + (Rarity.ERarity)unlockedRarity + ". Left bracket: " + leftBracket + ". Roll: " + roll);

            rolledDrops.Add(drops.Find(x => x.item == item && x.rarity == (Rarity.ERarity)unlockedRarity));
        }

        return rolledDrops;
    }
}
