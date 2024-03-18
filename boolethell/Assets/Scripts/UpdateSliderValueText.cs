using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateSliderValueText : MonoBehaviour
{
    [SerializeField] private bool refreshOnStart = true;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Slider slider;

    private void Start()
    {
        if (refreshOnStart)
            Refresh();
    }

    public void Refresh()
    {
        text.text = slider.value.ToString();
    }
}
