using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettings : MonoBehaviour
{
    [SerializeField] private Button back;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider bgmVolumeSlider;

    public GameEvent onBack;
    public FloatVariable masterVolume;
    public FloatVariable sfxVolume;
    public FloatVariable bgmVolume;

    private void Awake()
    {
        InitializeSliders();
    }

    void Start()
    {
        InitializeButtons();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            OnBackButtonPressed();
    }

    private void InitializeSliders()
    {
        masterVolumeSlider.value = masterVolume.Value * 100;
        sfxVolumeSlider.value = sfxVolume.Value * 100;
        bgmVolumeSlider.value = bgmVolume.Value * 100;
    }

    public void OnBackButtonPressed()
    {
        onBack.Raise();
    }

    public void MasterVolumeValueChanged()
    {
        masterVolume.SetValue(masterVolumeSlider.value / 100f);
    }

    public void SFXVolumeValueChanged()
    {
        sfxVolume.SetValue(sfxVolumeSlider.value / 100f);
    }

    public void BGMVolumeValueChanged()
    {
        bgmVolume.SetValue(bgmVolumeSlider.value / 100f);
    }

    private void InitializeButtons()
    {
        back.onClick.AddListener(() => { OnBackButtonPressed(); });
    }
}
