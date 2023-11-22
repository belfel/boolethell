using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettings : MonoBehaviour
{
    [SerializeField] private Button back;

    public GameEvent onBack;
    
    void Start()
    {
        InitializeButtons();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            OnBackButtonPressed();
    }

    public void OnBackButtonPressed()
    {
        onBack.Raise();
    }

    private void InitializeButtons()
    {
        back.onClick.AddListener(() => { OnBackButtonPressed(); });
    }
}
