using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInitializer : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private StageSelectData stageSelectData;

    private Player player;

    void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        var playerGO = Instantiate(playerPrefab, transform.position, transform.rotation);
        player = playerGO.GetComponent<Player>();

        Instantiate(stageSelectData.weapon, player.GetModelGO().transform);

        foreach (Tuple<int, GameObject> slot in stageSelectData.accessories)
        {
            Instantiate(slot.Item2, player.GetModelGO().transform);
        }
    }
}
