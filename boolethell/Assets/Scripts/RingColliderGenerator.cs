using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingColliderGenerator : MonoBehaviour
{
    public GameObject wallPrefab;
    public int numberOfWalls;
    public float radius;
    private float angleStep;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            GenerateRing();
    }

    private void GenerateRing()
    {
        float xd =  2 * Mathf.PI;
        angleStep = 360f / numberOfWalls;

        for (int i = 0; i < numberOfWalls; i++)
        {
            Vector3 wallPos = new Vector3(radius * Mathf.Cos(xd * angleStep * i / 360f), 
                                          radius * Mathf.Sin(xd * angleStep * i / 360f), 0);

            Instantiate(wallPrefab, wallPos, Quaternion.Euler(0, 0, angleStep * i - 90f), transform);
        }
    }
}
