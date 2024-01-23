using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPause : MonoBehaviour
{
    public GameEvent onResume;
    public GameEvent onSettings;
    public GameEvent onQuit;

    [SerializeField] private Button resume;
    [SerializeField] private Button settings;
    [SerializeField] private Button quit;
    [SerializeField] private Button confirmYes;
    [SerializeField] private Button confirmNo;

    [SerializeField] private GameObject confirmation;

    private void Awake()
    {
        confirmation.SetActive(false);
    }

    private void Start()
    {
        InitializeButtons();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            OnResumeButtonPressed();
    }

    public void OnResumeButtonPressed()
    {
        onResume.Raise();
    }

    public void OnSettingsButtonPressed()
    {
        onSettings.Raise();
    }

    public void OnQuitButtonPressed()
    {
        confirmation.SetActive(true);
        
    }

    public void OnConfirmationYesPressed()
    {
        onQuit.Raise();
    }

    public void OnConfirmationNoPressed()
    {
        confirmation.SetActive(false);
    }

    private void InitializeButtons()
    {
        resume.onClick.AddListener(() => { OnResumeButtonPressed(); });
        settings.onClick.AddListener(() => { OnSettingsButtonPressed(); });
        quit.onClick.AddListener(() => { OnQuitButtonPressed(); });
        confirmYes.onClick.AddListener(() => { OnConfirmationYesPressed(); });
        confirmNo.onClick.AddListener(() => { OnConfirmationNoPressed(); });
    }
}
