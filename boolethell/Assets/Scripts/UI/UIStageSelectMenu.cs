using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;

public class UIStageSelectMenu : MonoBehaviour
{
    [SerializeField] private ItemManager itemManager;

    [SerializeField] private GameObject weaponSelect;
    [SerializeField] private GameObject accessorySelect;
    [SerializeField] private GameObject bossSelect;

    [SerializeField] private Button weaponSlot;
    [SerializeField] private List<Button> accessorySlots = new List<Button>();
    [SerializeField] private List<StageSelectItem> weapons = new List<StageSelectItem>();
    [SerializeField] private List<StageSelectItem> accessories = new List<StageSelectItem>();

    [SerializeField] private Button bossSlot;
    [SerializeField] private Button boss1;
    [SerializeField] private Button boss2;
    [SerializeField] private Button boss3;

    [SerializeField] private Button back;
    [SerializeField] private Button start;

    [SerializeField] private Button removeAccessory;

    [SerializeField] private List<Tuple<Button, int>> lockedAccessoryButtons = new List<Tuple<Button, int>>();

    public StageSelectData stageSelectData;

    public GameEvent onBack;
    public GameEvent onStart;

    private int accessorySlot;


    private void Awake()
    {
        SetSelectMenusActive();
    }

    void Start()
    {
        ClearStageSelectData();
        InitializeButtons();

        SetSelectMenusInactive();
    }

    private void Update()
    {
        // Close all menus on right click
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            accessorySelect.SetActive(false);
            bossSelect.SetActive(false);
            weaponSelect.SetActive(false);
        }
    }

    private void SetSelectMenusActive()
    {
        weaponSelect.SetActive(true);
        accessorySelect.SetActive(true);
        bossSelect.SetActive(true);
    }

    private void SetSelectMenusInactive()
    {
        weaponSelect.SetActive(false);
        accessorySelect.SetActive(false);
        bossSelect.SetActive(false);
    }

    private void ClearStageSelectData()
    {
        stageSelectData.weapon = null;
        stageSelectData.accessories.Clear();
        stageSelectData.boss = null;
    }

    private void OpenWeaponSelect()
    {
        // Close other selection menus
        accessorySelect.SetActive(false);
        bossSelect.SetActive(false);

        weaponSelect.SetActive(true);
    }

    private void OpenAccessorySelect(int slot)
    {
        // Close other selection menus
        bossSelect.SetActive(false);
        weaponSelect.SetActive(false);
        accessorySlot = slot;
        accessorySelect.SetActive(true);
    }

    private void OpenBossSelect()
    {
        // Close other selection menus
        accessorySelect.SetActive(false);
        weaponSelect.SetActive(false);

        bossSelect.SetActive(true);
    }

    private void OnWeaponSelect(Type type, Button button) 
    {
        if (!UnlockManager.instance.IsUnlocked(type))
        {
            Debug.Log("locked");
            return;
        }

        StageSelectButton buttonSSButton = button.GetComponent<StageSelectButton>();
        if (!buttonSSButton) { Debug.LogError("Button " + button.gameObject.name + " missing StageSelectButton script"); }

        // Set slot icon
        StageSelectButton weaponSSButton = weaponSlot.GetComponent<StageSelectButton>();
        Image weaponIconImage = weaponSSButton.GetIconImage();
        weaponIconImage.sprite = buttonSSButton.GetIconSprite();
        weaponIconImage.color = Color.white;
        weaponIconImage.preserveAspect = true;

        // Set slot border color
        weaponSSButton.SetBorderAndBackgroundColor(buttonSSButton.GetBorderColor());

        // Set slot name and description
        weaponSSButton.SetTitleText(buttonSSButton.GetTitle());
        weaponSSButton.SetDescriptionText(buttonSSButton.GetDescription());

        stageSelectData.weapon = itemManager.GetItemPrefab(UnlockManager.instance.GetEItemFromType(type), (Rarity.ERarity)UnlockManager.instance.GetRarity(type));

        weaponSelect.SetActive(false);
    }

    private void OnAccessorySelect(Type type, Button button)
    {
        if (!UnlockManager.instance.IsUnlocked(type))
        {
            Debug.Log("locked");
            return;
        }

        // If this slot had another item then unlock that item's button
        foreach (Tuple<Button, int> tuple in lockedAccessoryButtons)
            if (tuple.Item2 == accessorySlot)
            {
                tuple.Item1.interactable = true;
                lockedAccessoryButtons.Remove(tuple);
                break;
            }

        // Lock button and link it to selected accessory slot
        button.interactable = false;
        lockedAccessoryButtons.Add(new Tuple<Button, int>(button, accessorySlot));

        StageSelectButton buttonSSButton = button.GetComponent<StageSelectButton>();
        if (!buttonSSButton) { Debug.LogError("Button " + button.gameObject.name + " missing StageSelectButton script"); }

        // Get the accessory slot button that opened select menu
        Button accessory = GetLastUsedAccessoryButton();

        // Set slot icon
        StageSelectButton accessorySSButton = accessory.GetComponent<StageSelectButton>();
        Image accessoryIconImage = accessorySSButton.GetIconImage();
        accessoryIconImage.sprite = buttonSSButton.GetIconSprite();
        accessoryIconImage.color = Color.white;
        accessoryIconImage.preserveAspect = true;

        // Set slot border color
        accessorySSButton.SetBorderAndBackgroundColor(buttonSSButton.GetBorderColor());

        // Set slot name and description
        accessorySSButton.SetTitleText(buttonSSButton.GetTitle());
        accessorySSButton.SetDescriptionText(buttonSSButton.GetDescription());

        // Add accessory data to stageSelectData
        GameObject accPrefab = itemManager.GetItemPrefab(UnlockManager.instance.GetEItemFromType(type), (Rarity.ERarity)UnlockManager.instance.GetRarity(type));
        Tuple<int, GameObject> accSlot = new Tuple<int, GameObject>(accessorySlot, accPrefab);

        foreach (Tuple<int, GameObject> slot in stageSelectData.accessories)
            if (slot.Item1 == accessorySlot)
            {
                stageSelectData.accessories.Remove(slot);
                break;
            }

        stageSelectData.accessories.Add(accSlot);

        // Close selection menu
        accessorySelect.SetActive(false);
    }

    private void ClearAccessorySlot()
    {
        StageSelectButton selectedSlot = GetLastUsedAccessoryButton().gameObject.GetComponent<StageSelectButton>();
        selectedSlot.SetTitleText("Select accessory");
        selectedSlot.SetDescriptionText("");
        selectedSlot.SetBorderAndBackgroundColor(Color.white);
        selectedSlot.GetIconImage().sprite = null;
        selectedSlot.GetIconImage().color = new Color(1f, 1f, 1f, 0f);

        foreach (Tuple<Button, int> tuple in lockedAccessoryButtons)
            if (tuple.Item2 == accessorySlot)
            {
                tuple.Item1.interactable = true;
                lockedAccessoryButtons.Remove(tuple);
                break;
            }

        accessorySelect.SetActive(false);
    }

    private void OnBossSelect(Type type, Button button)
    {
        stageSelectData.boss = type;

        StageSelectButton buttonSSButton = button.GetComponent<StageSelectButton>();
        if (!buttonSSButton) { Debug.LogError("Button " + button.gameObject.name + " missing StageSelectButton script"); }

        StageSelectButton bossSSButton = bossSlot.GetComponent<StageSelectButton>();
        Image bossIconImage = bossSSButton.GetIconImage();
        bossIconImage.sprite = buttonSSButton.GetIconSprite();
        bossIconImage.color = Color.white;
        bossIconImage.preserveAspect = true;

        // Set boss name and description
        bossSSButton.SetTitleText(buttonSSButton.GetTitle());
        bossSSButton.SetDescriptionText(buttonSSButton.GetDescription());

        // Close selection menu
        bossSelect.SetActive(false);
    }

    private void OnStart()
    {
        Type bossType = stageSelectData.boss;
        if (stageSelectData.boss == null)
            return; // TODO: Visual cue

        onStart.Raise();
    }

    private void OnBack()
    {
        onBack.Raise();
    }

    private void InitializeButtons()
    {
        //TODO: lock all item and boss buttons according to UnlockManager

        AddListenersToButtons();
        SetBordersColors();
    }

    private void AddListenersToButtons()
    {
        start.onClick.AddListener(() => { OnStart(); });
        back.onClick.AddListener(() => { OnBack(); });

        weaponSlot.onClick.AddListener(() => { OpenWeaponSelect(); });
        for (int i = 0; i < accessorySlots.Count; i++)
        {
            int _i = i;
            accessorySlots[i].onClick.AddListener(() => { OpenAccessorySelect(_i); });
        }
        bossSlot.onClick.AddListener(() => { OpenBossSelect(); });

        foreach (StageSelectItem weapon in weapons)
        {
            Button button = weapon.GetButton();
            button.onClick.AddListener(() => { OnWeaponSelect(weapon.GetItemType(), button); });
        }

        foreach (StageSelectItem accessory in accessories)
        {
            Button button = accessory.GetButton();
            button.onClick.AddListener(() => { OnAccessorySelect(accessory.GetItemType(), button); });
        }
        removeAccessory.onClick.AddListener(() => { ClearAccessorySlot(); });

        boss1.onClick.AddListener(() => { OnBossSelect(typeof(Boss1), boss1); });
        boss2.onClick.AddListener(() => { OnBossSelect(typeof(Boss2), boss2); });
        boss3.onClick.AddListener(() => { OnBossSelect(typeof(Boss1), boss1); });
    }

    private void SetBordersColors()
    {
        foreach (StageSelectItem weapon in weapons)
        {
            Type type = weapon.GetItemType();
            weapon.GetComponent<StageSelectButton>().SetBorderAndBackgroundColor(UIManager.instance.GetRarityAsColor(UnlockManager.instance.GetRarity(type)));
        }

        foreach (StageSelectItem accessory in accessories)
        {
            Type type = accessory.GetItemType();
            accessory.GetComponent<StageSelectButton>().SetBorderAndBackgroundColor(UIManager.instance.GetRarityAsColor(UnlockManager.instance.GetRarity(type)));
        }
    }

    private Button GetLastUsedAccessoryButton()
    {
        return accessorySlots[accessorySlot];
    }
}
