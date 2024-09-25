using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField] private LineRenderer aimLine;
    [SerializeField] private LineRenderer damageLine;

    [SerializeField] private float range = 9999f;
    [SerializeField] private float aimDelay = 0f;
    [SerializeField] private float repeatAimDelay = 0f;
    [SerializeField] private float aimDuration = 2f;
    [SerializeField] private float repeatAimDuration = 2f;
    [SerializeField] private float fireDuration = 9999f;
    [SerializeField] private float continuousDamageMultiplier = 1f;
    [SerializeField] private float singleInstanceDamage = 1f;
    [SerializeField] private int repeatCount = 0;
    [SerializeField] private bool singleDamageInstance = false;

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask laserBlockingLayers;

    private void Start()
    {
        Invoke(nameof(Aim), aimDelay);
    }

    private void Aim()
    {
        aimLine.enabled = true;
        Invoke(nameof(Fire), aimDuration);
    }

    private void RepeatAim()
    {
        aimLine.enabled = true;
        Invoke(nameof(Fire), repeatAimDuration);
    }

    private void Fire()
    {
        aimLine.enabled = false;
        damageLine.enabled = true;

        Invoke(nameof(StopFiring), fireDuration);

        if (singleDamageInstance)
            DealDamageOnce();
    }

    private void StopFiring()
    {
        if (repeatCount > 0)
        {
            repeatCount--;
            Invoke(nameof(RepeatAim), repeatAimDelay);
        }
    }

    private void DealDamageOnce()
    {

    }
}
