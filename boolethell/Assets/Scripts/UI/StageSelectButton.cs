using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectButton : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Image borderImage;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;

    [Header("Item/boss specific")]
    [SerializeField] private Sprite iconSprite;
    [SerializeField] private string title;
    [SerializeField] private string description;


    public Sprite GetIconSprite()
    {
        return iconImage.sprite;
    }

    public Image GetIconImage()
    {
        return iconImage;
    }

    public void SetBorderAndBackgroundColor(Color color)
    {
        if (color == Color.black)
            iconImage.color = color;
        else iconImage.color = Color.white;

        if (borderImage)
            borderImage.color = color;
        if (backgroundImage)
            backgroundImage.color = new Color(color.r, color.g, color.b, 0.5f);
    }

    public Color GetBorderColor()
    {
        return borderImage.color;
    }

    public string GetTitle() { return title; }

    public string GetDescription() { return description; }

    public TMP_Text GetTitleText() { return titleText; }

    public void SetTitleText(string text) { titleText.text = text; }

    public TMP_Text GetDescriptionText() { return descriptionText; }

    public void SetDescriptionText(string text) { descriptionText.text = text; }
}
