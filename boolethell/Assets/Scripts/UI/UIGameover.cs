using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameover : MonoBehaviour
{
    [SerializeField] private Button retry;
    [SerializeField] private Button changeLoadout;
    [SerializeField] private TMP_Text hpLeft;
    public FloatVariable bossHP;
    public FloatVariable bossMaxHP;

    public GameEvent onRetry;
    public GameEvent onChangeLoadout;

    private void Awake()
    {
        InitializeButtons();
    }

    private void Start()
    {
        float hpPercent = bossHP.Value / bossMaxHP.Value * 100f;
        hpLeft.text = "HP left: " + hpPercent.ToString("F0") + "%";
    }

    private void OnRetryButtonPressed() 
    {
        onRetry.Raise();
    }

    private void OnChangeLoadoutButtonPressed()
    {
        onChangeLoadout.Raise();
    }

    private void InitializeButtons()
    {
        retry.onClick.AddListener(delegate { OnRetryButtonPressed(); });
        changeLoadout.onClick.AddListener(delegate { OnChangeLoadoutButtonPressed(); });
    }
}
