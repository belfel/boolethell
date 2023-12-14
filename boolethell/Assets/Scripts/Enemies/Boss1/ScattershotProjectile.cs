using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScattershotProjectile : Projectile
{
    [SerializeField] private GameObject scatterPrefab;
    [SerializeField] private int numberOfScatters = 6;
    [SerializeField] private float inaccuracyMultiplier = 1f;

    protected override void OnLifetimeOver()
    {
        Scatter();
        Destroy(gameObject);
    }

    public void SetNumberOfScatters(int num)
    {
        numberOfScatters = num;
    }

    private void Scatter()
    {
        float step = 360f / numberOfScatters;
        
        for (int i=0; i<numberOfScatters; i++)
        {
            float inacc = Random.Range(-1f, 1f) * inaccuracyMultiplier;
            Instantiate(scatterPrefab, transform.position, transform.rotation * Quaternion.Euler(0, 0, step * i + inacc));
        }
    }
}
