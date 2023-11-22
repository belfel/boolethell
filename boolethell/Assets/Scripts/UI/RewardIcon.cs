using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardIcon : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Image border;
    [SerializeField] Image background;
    [SerializeField] TMP_Text text;

    private UnlockManager.EItem item;
    private Rarity.ERarity rarity;
    private Color rarityColor;

    public void Reveal()
    {
        icon.color = Color.white;
        border.color = rarityColor;
        text.color = rarityColor;
        background.color = new Color(rarityColor.r, rarityColor.g, rarityColor.b, 0.2f);
        text.text = UIManager.instance.GetItemName(item) + " (" + UIManager.instance.GetRarityAsString((short)rarity) + ")";
    }

    public void SetItemAndRarity(UnlockManager.EItem _item, Rarity.ERarity _rarity)
    {
        item = _item;
        rarity = _rarity;
        rarityColor = SetColorByRarity(rarity);
        icon.sprite = UIManager.instance.GetItemSprite(item);
    }

    private Color SetColorByRarity(Rarity.ERarity rarity)
    {
        return UIManager.instance.GetRarityAsColor((short)rarity);
    }
}
