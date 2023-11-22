using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIRewards : MonoBehaviour
{
    [SerializeField] private Button repeat;
    [SerializeField] private Button changeLoadout;
    [SerializeField] private Vector2 firstSlotPos;
    [SerializeField] private float slotDistHorizontal;
    [SerializeField] private float slotDistVertical;
    [SerializeField] private int evenSlotsPerRow;
    [SerializeField] private int oddSlotsPerRow;
    

    public LootTable rolledRewards;
    public GameEvent onRepeat;
    public GameEvent onChangeLoadout;

    private List<RewardIcon> rewardIcons = new List<RewardIcon>();

    [SerializeField] private GameObject pistolRewardIconPrefab;

    private void Start()
    {
        InitializeButtons();
        ShowRewards();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            foreach (RewardIcon rewardIcon in rewardIcons)
                Destroy(rewardIcon.gameObject);
            rewardIcons.Clear();
            ShowRewards();
        }
    }

    // This function instantiates item reward icons starting from the middle outwards.
    // Handles both even and odd cases. 
    public void ShowRewards()
    {
        int row = 0;
        int count = 0;
        int halfCount = 0;
        int symmetry = -1;
        int slotsPerRow = oddSlotsPerRow;
        int odd = 1;
        float xInit = 0f;
        Vector3 pos;
        if (rolledRewards.GetDrops().Count % 2 == 0)
        {
            symmetry = 1;
            slotsPerRow = evenSlotsPerRow;
            xInit = slotDistHorizontal / 2;
            odd = 0;
        }

        foreach (LootDrop drop in rolledRewards.GetDrops())
        {
            pos = new Vector3(xInit * symmetry + (slotDistHorizontal * halfCount * symmetry), firstSlotPos.y - (slotDistVertical * row), 0f);
            GameObject icon = Instantiate(pistolRewardIconPrefab, transform);
            icon.transform.localPosition = pos;
            RewardIcon rewardIcon = icon.GetComponent<RewardIcon>();
            rewardIcon.SetItemAndRarity(drop.item, drop.rarity);
            rewardIcons.Add(rewardIcon);

            symmetry *= -1;
            count++;
            if (count % 2 == odd)
            {
                halfCount++;
            }

            if (count == slotsPerRow)
            {
                count = 0;
                halfCount = 0;
                row++;
            }
        }
    }

    private void onRepeatButtonClicked()
    {
        onRepeat.Raise();
    }

    private void onChangeLoadoutButtonClicked()
    {
        onChangeLoadout.Raise();
    }

    private void InitializeButtons()
    {
        repeat.onClick.AddListener(() => { onRepeatButtonClicked();});
        changeLoadout.onClick.AddListener(() => { onChangeLoadoutButtonClicked();});
    }

}
