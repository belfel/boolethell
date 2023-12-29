using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Camera : MonoBehaviour
{
    [SerializeField] private Vector3Variable playerPosition;

    void Update()
    {
        float playerPosY = playerPosition.Value.y;
        if (playerPosY < 0)
            gameObject.transform.position = new Vector3(0f, Mathf.Max(-4f, playerPosY), -20f);
        else gameObject.transform.position = new Vector3(0f, Mathf.Min(5f, playerPosY), -20f);
    }
}
