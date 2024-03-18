using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private enum EState
    {
        mainMenu = 0, settingsMainMenu, stageSelect, gameplay, pauseScreen, settingsPauseScreen, gameover, rewardsScreen
    }

    [SerializeField] private EState initialState;
    private EState currentState;

    public StageSelectData stageSelectData;

    public GameEvent gameplayPaused;
    public GameEvent gameplayUnpaused;

    [Header("UI Prefabs")]
    [SerializeField] private GameObject mainMenuPrefab;
    private GameObject mainMenu;
    
    [SerializeField] private GameObject stageSelectPrefab;
    private GameObject stageSelect;
    
    [SerializeField] private GameObject mainMenuSettingsPrefab;
    private GameObject mainMenuSettings;
    
    [SerializeField] private GameObject pauseMenuSettingsPrefab;
    private GameObject pauseMenuSettings;
    
    [SerializeField] private GameObject pauseScreenPrefab;
    private GameObject pauseScreen;
    
    [SerializeField] private GameObject gameoverPrefab;
    private GameObject gameoverScreen;
    
    [SerializeField] private GameObject rewardsScreenPrefab;
    private GameObject rewardsScreen;

    [Header("Listener groups")]
    [SerializeField] private GameObject mainMenuListeners;
    [SerializeField] private GameObject stageSelectListeners;
    [SerializeField] private GameObject mainMenuSettingsListeners;
    [SerializeField] private GameObject pauseMenuSettingsListeners;
    [SerializeField] private GameObject pauseScreenListeners;
    [SerializeField] private GameObject gameoverListeners;
    [SerializeField] private GameObject rewardsScreenListeners;
    [SerializeField] private GameObject gameplayListeners;

    private Dictionary<Type, Sprite> itemTypeIconPairs = new Dictionary<Type, Sprite>();
    [Header("Sprites")]
    [SerializeField] private Sprite placeholderSprite;
    [SerializeField] private Sprite pistolSprite;
    [SerializeField] private Sprite revolverSprite;
    [SerializeField] private Sprite smgSprite;
    [SerializeField] private Sprite shotgunSprite;
    [SerializeField] private Sprite flamethrowerSprite;
    [SerializeField] private Sprite lifestoneSprite;

    private void Awake()
    {
        // singleton
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        InitializeIcons();
    }

    private void Start()
    {
        ToggleListenersAndUIFromState(initialState);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && currentState == EState.gameplay)
        {
            gameplayPaused.Raise();
        }
    }

    #region listener delegates
    public void OnMainMenuSettingsButtonPressed()
    {
        ToggleListenersAndUIFromState(EState.settingsMainMenu);
    }

    public void OnMainMenuStartButtonPressed()
    {
        ToggleListenersAndUIFromState(EState.stageSelect);
    }

    public void OnMainMenuQuitButtonPressed()
    {
        Application.Quit();
    }

    public void OnMainMenuSettingsBackButtonPressed()
    {
        ToggleListenersAndUIFromState(EState.mainMenu);
    }

    public void OnStageSelectBackButtonPressed()
    {
        ToggleListenersAndUIFromState(EState.mainMenu);
    }

    public void OnStageSelectStartButtonPressed()
    {
        ToggleListenersAndUIFromState(EState.gameplay);
    }

    public void OnGameplayVictory()
    {
        ToggleListenersAndUIFromState(EState.rewardsScreen);
    }

    public void OnGameplayPause()
    {
        ToggleListenersAndUIFromState(EState.pauseScreen);
    }

    public void OnGameplayGameover()
    {
        ToggleListenersAndUIFromState(EState.gameover);
    }

    public void OnPauseMenuResumeButtonPressed()
    {
        Time.timeScale = 1f;
        ToggleListenersAndUIFromState(EState.gameplay);
    }

    public void OnPauseMenuSettingsButtonPressed()
    {
        ToggleListenersAndUIFromState(EState.settingsPauseScreen);
    }

    public void OnPauseMenuSettingsBackButtonPressed()
    {
        ToggleListenersAndUIFromState(EState.pauseScreen);
    }

    public void OnPauseMenuQuitButtonPressed()
    {
        ToggleListenersAndUIFromState(EState.gameover);
    }

    public void OnGameoverRetryButtonPressed()
    {
        ToggleListenersAndUIFromState(EState.gameplay);
    }

    public void OnRewardsRepeatButtonPressed()
    {
        ToggleListenersAndUIFromState(EState.gameplay);
    }

    public void OnRewardsChangeLoadoutButtonPressed()
    {
        SceneManager.LoadSceneAsync("Menu").completed += handle => ToggleListenersAndUIFromState(EState.stageSelect);
    }

    public void OnGameoverChangeLoadoutButtonPressed()
    {
        SceneManager.LoadSceneAsync("Menu").completed += handle => ToggleListenersAndUIFromState(EState.stageSelect);
    }
    #endregion

    private void ToggleListenersAndUIFromState(EState newState)
    {
        DisableAllListeners();
        DestroyMenu(currentState);

        switch (newState)
        {
            case EState.mainMenu:
                mainMenu = Instantiate(mainMenuPrefab, transform);
                mainMenuListeners.SetActive(true);
                break;
            case EState.stageSelect:
                stageSelect = Instantiate(stageSelectPrefab, transform);
                stageSelectListeners.SetActive(true);
                break;
            case EState.settingsMainMenu:
                mainMenuSettings = Instantiate(mainMenuSettingsPrefab, transform);
                mainMenuSettingsListeners.SetActive(true);
                break;
            case EState.pauseScreen:
                Time.timeScale = 0f;
                pauseScreen = Instantiate(pauseScreenPrefab, transform);
                pauseScreenListeners.SetActive(true);
                break;
            case EState.gameplay:
                gameplayListeners.SetActive(true);
                if (!(currentState == EState.pauseScreen))
                    LoadStageFromStageSelectData();
                else gameplayUnpaused.Raise();
                break;
            case EState.settingsPauseScreen:
                pauseMenuSettings = Instantiate(pauseMenuSettingsPrefab, transform);
                pauseMenuSettingsListeners.SetActive(true);
                break;
            case EState.gameover:
                Time.timeScale = 0f;
                gameoverScreen = Instantiate(gameoverPrefab, transform);
                gameoverListeners.SetActive(true);
                break;
            case EState.rewardsScreen:
                Time.timeScale = 0f;
                rewardsScreen = Instantiate(rewardsScreenPrefab, transform);
                rewardsScreenListeners.SetActive(true);
                break;
        }

        currentState = newState;
    }

    private void DestroyMenu(EState state)
    {
        switch (state)
        {
            case EState.mainMenu:
                if (mainMenu) Destroy(mainMenu);
                break;
            case EState.stageSelect:
                if (stageSelect) Destroy(stageSelect);
                break;
            case EState.settingsMainMenu:
                if (mainMenuSettings) Destroy(mainMenuSettings);
                break;
            case EState.pauseScreen:
                if (pauseScreen) { Destroy(pauseScreen); }
                    break;
            case EState.gameplay:
                break;
            case EState.settingsPauseScreen:
                if (pauseMenuSettings) Destroy(pauseMenuSettings);
                break;
            case EState.gameover:
                if (gameoverScreen) { Destroy(gameoverScreen); Time.timeScale = 1f; }
                break;
            case EState.rewardsScreen:
                if (rewardsScreen) { Destroy(rewardsScreen); Time.timeScale = 1f; }
                break;
        }
    }

    private void LoadStageFromStageSelectData()
    {
        Type bossType = stageSelectData.boss;

        if (bossType == typeof(Boss1))
        {
            SceneManager.LoadScene("Boss1");
        }

        if (bossType == typeof(Boss2))
        {
            SceneManager.LoadScene("Boss2");
        }
    }

    private void DisableAllListeners()
    {
        mainMenuListeners.SetActive(false);
        stageSelectListeners.SetActive(false);
        mainMenuSettingsListeners.SetActive(false);
        pauseMenuSettingsListeners.SetActive(false);
        pauseScreenListeners.SetActive(false);
        gameoverListeners.SetActive(false);
        gameplayListeners.SetActive(false);
        rewardsScreenListeners.SetActive(false);
    }

    public string GetRarityAsString(short rarity)
    {
        switch (rarity)
        {
            default:
            case 0:
                return "Locked";
            case 1:
                return "Common";
            case 2:
                return "Uncommon";
            case 3:
                return "Rare";
            case 4:
                return "Legendary";
        }
    }

    public Color GetRarityAsColor(short rarity) 
    {
        switch (rarity) 
        {
            default:
            case 0:
                return Color.black;
            case 1:
                return Color.gray;
            case 2:
                return Color.green;
            case 3:
                return new Color(0f, 0.39f, 1f);
            case 4:
                return Color.yellow;
        }
    }

    private void InitializeIcons()
    {
        if (!placeholderSprite)
            Debug.LogError("No placeholder sprite assigned!");

        if (pistolSprite) itemTypeIconPairs.Add(typeof(Pistol), pistolSprite);
        else itemTypeIconPairs.Add(typeof(Pistol), placeholderSprite);

        if (pistolSprite) itemTypeIconPairs.Add(typeof(SMG), smgSprite);
        else itemTypeIconPairs.Add(typeof(SMG), placeholderSprite);

        if (pistolSprite) itemTypeIconPairs.Add(typeof(Shotgun), shotgunSprite);
        else itemTypeIconPairs.Add(typeof(Shotgun), placeholderSprite);

        if (pistolSprite) itemTypeIconPairs.Add(typeof(Revolver), revolverSprite);
        else itemTypeIconPairs.Add(typeof(Revolver), placeholderSprite);

        if (pistolSprite) itemTypeIconPairs.Add(typeof(Flamethrower), flamethrowerSprite);
        else itemTypeIconPairs.Add(typeof(Flamethrower), placeholderSprite);


        if (pistolSprite) itemTypeIconPairs.Add(typeof(LifeStone), lifestoneSprite);
        else itemTypeIconPairs.Add(typeof(LifeStone), placeholderSprite);
    }

    public Sprite GetItemSprite(Type type)
    {
        Sprite val;
        if (!itemTypeIconPairs.TryGetValue(type, out val))
            Debug.LogError("Requested type not in dictionary");
        return val;
    }

    public Sprite GetItemSprite(UnlockManager.EItem item)
    {
        return GetItemSprite(UnlockManager.instance.GetItemType(item));
    }

    public string GetItemName(UnlockManager.EItem item)
    {
        switch (item)
        {
            case UnlockManager.EItem.pistol:
                return "Pistol";
            case UnlockManager.EItem.smg:
                return "SMG";
            case UnlockManager.EItem.shotgun:
                return "Shotgun";
            case UnlockManager.EItem.revolver:
                return "Revolver";
            case UnlockManager.EItem.flamethrower:
                return "Flamethrower";
            case UnlockManager.EItem.lifestone:
                return "Lifestone";
            default:
                return "";
        }
    }
}
