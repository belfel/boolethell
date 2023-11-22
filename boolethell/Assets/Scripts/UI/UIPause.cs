using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPause : MonoBehaviour
{
    [SerializeField] private Button resume;
    [SerializeField] private Button settings;
    [SerializeField] private Button quit;

    public GameEvent onResume;
    public GameEvent onSettings;
    public GameEvent onQuit;

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
        onQuit.Raise();
    }

    private void InitializeButtons()
    {
        resume.onClick.AddListener(() => { OnResumeButtonPressed(); });
        settings.onClick.AddListener(() => { OnSettingsButtonPressed(); });
        quit.onClick.AddListener(() => { OnQuitButtonPressed(); });
    }
}
