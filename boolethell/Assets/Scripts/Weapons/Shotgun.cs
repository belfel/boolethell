using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    private int projectileCount = 6;
    private float spreadAngle = 30f;

    public override void Shoot()
    {
        currentAmmo -= 1;

        float angle = -spreadAngle/2;
        float angleIncrement = spreadAngle / (float)projectileCount;
        float variance = 8f;

        for (int i=0; i<projectileCount; i++) 
        {
            GameObject newProjectile = Instantiate(projectilePrefab, gameObject.transform.position, gameObject.transform.rotation);
            GunProjectile proj = newProjectile.GetComponent<GunProjectile>();
            proj.transform.Rotate(Vector3.forward, angle + Random.Range(-variance, variance));
            angle += angleIncrement;
            float randSpeedMult = Random.Range(0.95f, 1.05f);
            proj.SetStats(pierce, bounce, damage, projectileSpeed * randSpeedMult, projectileLifetime);
        }
        onShoot.Raise();
    }
}
