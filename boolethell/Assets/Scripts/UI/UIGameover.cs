using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameover : MonoBehaviour
{
    [SerializeField] private Button retry;
    [SerializeField] private Button changeLoadout;

    public GameEvent onRetry;
    public GameEvent onChangeLoadout;

    private void Start()
    {
        InitializeButtons();
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
