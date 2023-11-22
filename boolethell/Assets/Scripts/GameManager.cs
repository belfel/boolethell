using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private ItemManager itemManager;
    [SerializeField] public bool bDevMode = true;
    [SerializeField] private GameObject playerPrefab;
    private Player player;

    [SerializeField] private GameObject ammoTextPrefab;

    public StageSelectData stageSelectData;

    public BoolList playerControlsDisabled;
    public BoolVariable playerControlsDisabledByPause;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        
    }

    public void SetWeaponAndAccessories()
    {
        List<Tuple<Type, Rarity.ERarity>> items; 
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Preload" || scene.name == "Menu")
            return;
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        var playerGO = Instantiate(playerPrefab, new Vector3(0f, -5f, 0f), Quaternion.identity);
        player = playerGO.GetComponent<Player>();

        GameObject weapon = Instantiate(itemManager.GetItemPrefab(UnlockManager.instance.GetEItemFromType(stageSelectData.weapon.Item1), 
                                stageSelectData.weapon.Item2), player.GetModelGO().transform);

        if (stageSelectData.accessory1 != null)
        {
            GameObject acc1 = Instantiate(itemManager.GetItemPrefab(UnlockManager.instance.GetEItemFromType(stageSelectData.accessory1.Item1),
                                stageSelectData.accessory1.Item2), player.GetModelGO().transform);

            acc1.GetComponent<Accessory>().ApplyEffect();
        }

        if (stageSelectData.accessory2 != null)
        {
            GameObject acc2 = Instantiate(itemManager.GetItemPrefab(UnlockManager.instance.GetEItemFromType(stageSelectData.accessory2.Item1),
                                stageSelectData.accessory2.Item2), player.GetModelGO().transform);

            acc2.GetComponent<Accessory>().ApplyEffect();
        }

        if (stageSelectData.accessory3 != null)
        {
            GameObject acc3 = Instantiate(itemManager.GetItemPrefab(UnlockManager.instance.GetEItemFromType(stageSelectData.accessory3.Item1),
                                stageSelectData.accessory3.Item2), player.GetModelGO().transform);

            acc3.GetComponent<Accessory>().ApplyEffect();
        }

        //Component weaponComp = player.gameObject.AddComponent(stageSelectData.weapon.Item1);
        //Weapon weapon = (Weapon)weaponComp;
        //weapon.SetRarity(stageSelectData.weapon.Item2);
    }

    public Vector3 GetPlayerPosition()
    {
        if (player)
            return player.transform.position;
        else return new Vector3();
    }

    public GameObject GetAmmoTextPrefab()
    {
        return ammoTextPrefab;
    }

    public Player GetPlayer()
    {
        return player;
    }

    public void OnGameplayPause()
    {
        playerControlsDisabledByPause.SetValue(true);
    }

    public void OnGameplayUnpause()
    {
        playerControlsDisabledByPause.SetValue(false);
    }
}
