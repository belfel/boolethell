using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : MonoBehaviour
{
    [SerializeField] private Transform laserStart;
    [SerializeField] private Transform rotatingPart;
    [SerializeField] private LineRenderer damagingLaser;
    [SerializeField] private LineRenderer trackingLaser;
    [SerializeField] private Vector3Variable targetPos;
    [SerializeField] private LayerMask hitLayers;
    [SerializeField] private LayerMask trackingHitLayers;
    [SerializeField] private LayerMask playerLayer;
    
    [SerializeField] private float damageMultiplier = 1f;
    [SerializeField] private float activationTime = 2f;
    [SerializeField] private float trackingTime = 4f;
    [SerializeField] private float firingDelayTime = 2f;
    [SerializeField] private float damagingBeamWidth = 0.5f;
    [SerializeField] private float trackingBeamWidth = 0.1f;
    [SerializeField] private float trackingBeamWarningAlpha = 0.3f;

    private float rayDist = 50f;
    private bool trackTarget = false;
    private bool shootLaser = false;

    private void Awake()
    {
        damagingLaser.widthMultiplier = damagingBeamWidth;
        trackingLaser.widthMultiplier = trackingBeamWidth;
    }

    private void Start()
    {
        StartCoroutine(LaserRoutine());
    }

    private void Update()
    {
        if (trackTarget)
        {
            RotateGunTowardsTarget();
            ShootTrackingLaser();
        }

        if (shootLaser)
        {
            ShootDamagingLaser();
        }
    }

    private void RotateGunTowardsTarget()
    {
        Vector3 targetDir = (targetPos.Value - rotatingPart.position).normalized;
        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
        rotatingPart.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
    }

    private void ShootDamagingLaser()
    {
        RaycastHit2D hit = Physics2D.Raycast(laserStart.position, laserStart.up, rayDist, hitLayers.value);
        if (!hit)
            return;

        DrawRay(laserStart.position, hit.point);
        GameObject hitGO = hit.rigidbody.gameObject;
        if (hitGO.layer == playerLayer)
        {
            Player playerComp = hitGO.GetComponent<Player>();
            if (playerComp)
                playerComp.OnHit(Time.deltaTime * damageMultiplier);
        }
    }

    private void ShootTrackingLaser()
    {
        RaycastHit2D hit = Physics2D.Raycast(laserStart.position, laserStart.up, rayDist, trackingHitLayers.value);
        if (!hit)
            return;

        DrawRay(laserStart.position, hit.point);
    }

    private void DrawRay(Vector2 startPos, Vector2 endPos)
    {
        if (trackTarget)
        {
            trackingLaser.SetPosition(0, startPos);
            trackingLaser.SetPosition(1, endPos);
        }
        else
        {
            damagingLaser.SetPosition(0, startPos);
            damagingLaser.SetPosition(1, endPos);
        }
    }

    private IEnumerator LaserRoutine()
    {
        yield return new WaitForSeconds(activationTime);
        trackTarget = true;
        yield return new WaitForSeconds(trackingTime); 
        trackTarget = false;
        trackingLaser.widthMultiplier = damagingBeamWidth;
        trackingLaser.material.SetColor("_Color", new Color(1f, 1f, 1f, trackingBeamWarningAlpha));
        yield return new WaitForSeconds(firingDelayTime);
        trackingLaser.gameObject.SetActive(false);
        shootLaser = true;
    }
}
