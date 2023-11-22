using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public GameEvent onShoot;
    [SerializeField] protected Rarity.ERarity rarity = Rarity.ERarity.common;
    [SerializeField] protected int maxAmmo;
    
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float reloadTime;
    [SerializeField] protected short pierce;
    [SerializeField] protected short bounce;
    [SerializeField] protected float damage;
    [SerializeField] protected float projectileSpeed = 10f;
    [SerializeField] protected float projectileLifetime = 10f;

    [SerializeField] protected GameObject projectilePrefab;

    public BoolList controlsDisabled;
    public StringVariable ammoText;
    public GameEvent ammoTextUpdated;

    protected float attackCooldown = 0f;
    protected float reloadTimeCurrent = 0f;
    protected bool isReadyToShoot = true;
    protected bool isReloading = false;
    protected int currentAmmo;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoText();
    }

    protected virtual void Update()
    {
        TryShoot();

        if (Input.GetKeyDown(KeyCode.R))
            TryReload();
    }

    public virtual void Shoot()
    {
        currentAmmo -= 1;
        GameObject newProjectile = Instantiate(projectilePrefab, gameObject.transform.position, gameObject.transform.rotation);
        GunProjectile proj = newProjectile.GetComponent<GunProjectile>();
        proj.SetStats(pierce, bounce, damage, projectileSpeed, projectileLifetime);
        onShoot.Raise();
    }

    public virtual void TryShoot()
    {
        if (isReloading)
        {
            if (reloadTimeCurrent >= reloadTime)
            {
                isReloading = false;
                isReadyToShoot = true;
                currentAmmo = maxAmmo;
                reloadTimeCurrent = 0f;
                UpdateAmmoText();
            }
            else reloadTimeCurrent += Time.deltaTime;
        }
        else if (!isReadyToShoot)
        {
            if (attackCooldown >= attackSpeed)
            {
                isReadyToShoot = true;
                attackCooldown = 0f;
            }
            else attackCooldown += Time.deltaTime;
        }

        if (Input.GetMouseButton(0) && isReadyToShoot && !isReloading && !controlsDisabled.IsAnyTrue())
        {
            Shoot();
            isReadyToShoot = false;
            if (currentAmmo <= 0)
            {
                currentAmmo = 0;
                isReloading = true;
            }
            UpdateAmmoText();
        }
    }

    private void TryReload()
    {
        if (!isReloading && currentAmmo < maxAmmo)
        {
            isReloading = true;
        }
    }

    private void UpdateAmmoText()
    {
        ammoText.SetValue(currentAmmo.ToString() + "/" + maxAmmo.ToString());

        if (isReloading)
            ammoText.SetValue("Reloading...");

        ammoTextUpdated.Raise();
    }

    public virtual void SetStats(int _ammo, float _atkSpd, float _reloadTime, float _damage, short _pierce, short _bounce, float _projSpd, float _projLifetime)
    {
        maxAmmo = _ammo;
        currentAmmo = maxAmmo;
        attackSpeed = _atkSpd;
        reloadTime = _reloadTime;
        damage = _damage;
        pierce = _pierce;
        bounce = _bounce;
        projectileSpeed = _projSpd;
        projectileLifetime = _projLifetime;
    }
}
