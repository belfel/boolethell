using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private Button start;
    [SerializeField] private Button saveslot1;
    [SerializeField] private Button saveslot2;
    [SerializeField] private Button saveslot3;
    [SerializeField] private Button wipeData;
    [SerializeField] private Button settings;
    [SerializeField] private Button quit;

    [SerializeField] private short selectedSaveslot = 1;

    public GameEvent onMainMenuStart;
    public GameEvent onMainMenuSettings;
    public GameEvent onMainMenuQuit;

    private void Start()
    {
        InitializeButtons();

        //TODO: replace with a global variable set from save file
        OnSlotButtonPressed(1);
    }

    private void OnStartButtonPressed()
    {
        onMainMenuStart.Raise();
    }

    private void OnSlotButtonPressed(short slot)
    {
        selectedSaveslot = slot;
        SetSelectedSaveslotButtonUnclickable();
    }

    private void SetSelectedSaveslotButtonUnclickable()
    {
        switch (selectedSaveslot)
        {
            default:
            case 1:
                saveslot1.interactable = false;
                saveslot2.interactable = true;
                saveslot3.interactable = true;
                break;
            case 2:
                saveslot1.interactable = true;
                saveslot2.interactable = false;
                saveslot3.interactable = true;
                break;
            case 3:
                saveslot1.interactable = true;
                saveslot2.interactable = true;
                saveslot3.interactable = false;
                break;
        }
    }

    private void OnSettingsButtonPressed()
    {
        onMainMenuSettings.Raise();
    }

    private void OnQuitButtonPressed() 
    {
        Application.Quit();
    }

    private void InitializeButtons()
    {
        start.onClick.AddListener(delegate { OnStartButtonPressed(); });
        saveslot1.onClick.AddListener(delegate { OnSlotButtonPressed(1); });
        saveslot2.onClick.AddListener(delegate { OnSlotButtonPressed(2); });
        saveslot3.onClick.AddListener(delegate { OnSlotButtonPressed(3); });
        settings.onClick.AddListener(delegate { OnSettingsButtonPressed(); });
        quit.onClick.AddListener(delegate { OnQuitButtonPressed(); });
    }
}
