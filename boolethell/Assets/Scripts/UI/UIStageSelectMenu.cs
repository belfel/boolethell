using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStageSelectMenu : MonoBehaviour
{
    [SerializeField] private ItemManager itemManager;

    [SerializeField] private GameObject weaponSelect;
    [SerializeField] private GameObject accessorySelect;
    [SerializeField] private GameObject bossSelect;

    [SerializeField] private Button weapon;
    [SerializeField] private Button accessory1;
    [SerializeField] private Button accessory2;
    [SerializeField] private Button accessory3;

    [SerializeField] private Button pistol;
    [SerializeField] private Button smg;
    [SerializeField] private Button shotgun;

    [SerializeField] private Button lifestone;

    [SerializeField] private Button boss;
    [SerializeField] private Button boss1;
    [SerializeField] private Button boss2;
    [SerializeField] private Button boss3;

    [SerializeField] private Button back;
    [SerializeField] private Button start;

    [SerializeField] private Sprite pistolSprite;

    [SerializeField] private List<Tuple<Button, short>> lockedAccessoryButtons;

    public StageSelectData stageSelectData;

    public GameEvent onBack;
    public GameEvent onStart;

    private short accessorySlot;

    void Start()
    {
        lockedAccessoryButtons = new List<Tuple<Button, short>>();
        ClearStageSelectData();
        InitializeButtons();
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

    private void OpenAccessorySelect(short slot)
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
        StageSelectButton weaponSSButton = weapon.GetComponent<StageSelectButton>();
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

        // If this slot had another item unlock that item's button
        foreach (Tuple<Button, short> tuple in lockedAccessoryButtons)
            if (tuple.Item2 == accessorySlot)
            {
                tuple.Item1.interactable = true;
                lockedAccessoryButtons.Remove(tuple);
                break;
            }

        // Lock button and link it to selected accessory slot
        button.interactable = false;
        lockedAccessoryButtons.Add(new Tuple<Button, short>(button, accessorySlot));

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
                stageSelectData.accessories.Remove(slot);
        
        stageSelectData.accessories.Add(accSlot);

        // Close selection menu
        accessorySelect.SetActive(false);
    }

    private void OnBossSelect(Type type, Button button)
    {
        stageSelectData.boss = type;

        StageSelectButton buttonSSButton = button.GetComponent<StageSelectButton>();
        if (!buttonSSButton) { Debug.LogError("Button " + button.gameObject.name + " missing StageSelectButton script"); }

        StageSelectButton bossSSButton = boss.GetComponent<StageSelectButton>();
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
        start.onClick.AddListener(delegate { OnStart(); });
        back.onClick.AddListener(() => { OnBack(); });

        weapon.onClick.AddListener(delegate { OpenWeaponSelect(); });
        accessory1.onClick.AddListener(delegate { OpenAccessorySelect(1); });
        accessory2.onClick.AddListener(delegate { OpenAccessorySelect(2); });
        accessory3.onClick.AddListener(delegate { OpenAccessorySelect(3); });
        boss.onClick.AddListener(delegate { OpenBossSelect(); });

        pistol.onClick.AddListener(delegate { OnWeaponSelect(typeof(Pistol), pistol); });
        smg.onClick.AddListener(delegate { OnWeaponSelect(typeof(SMG), smg); });
        shotgun.onClick.AddListener(delegate { OnWeaponSelect(typeof(Shotgun), shotgun); });

        lifestone.onClick.AddListener(delegate { OnAccessorySelect(typeof(LifeStone), lifestone); });

        boss1.onClick.AddListener(delegate { OnBossSelect(typeof(Boss1), boss1); });
        boss2.onClick.AddListener(delegate { OnBossSelect(typeof(Boss1), boss1); });
        boss3.onClick.AddListener(delegate { OnBossSelect(typeof(Boss1), boss1); });
    }

    private void SetBordersColors()
    {
        pistol.GetComponent<StageSelectButton>().SetBorderAndBackgroundColor(UIManager.instance.GetRarityAsColor(UnlockManager.instance.GetRarity(typeof(Pistol))));
        smg.GetComponent<StageSelectButton>().SetBorderAndBackgroundColor(UIManager.instance.GetRarityAsColor(UnlockManager.instance.GetRarity(typeof(SMG))));
        shotgun.GetComponent<StageSelectButton>().SetBorderAndBackgroundColor(UIManager.instance.GetRarityAsColor(UnlockManager.instance.GetRarity(typeof(Shotgun))));

        lifestone.GetComponent<StageSelectButton>().SetBorderAndBackgroundColor(UIManager.instance.GetRarityAsColor(UnlockManager.instance.GetRarity(typeof(LifeStone))));
    }

    private Button GetLastUsedAccessoryButton()
    {
        Button accessory;

        if (accessorySlot == 1) accessory = accessory1;
        else if (accessorySlot == 2) accessory = accessory2;
        else accessory = accessory3;

        return accessory;
    }
}
