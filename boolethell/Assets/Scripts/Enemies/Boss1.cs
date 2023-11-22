using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is supposed to be a tank
public class Boss1 : Enemy
{
    [Header("Prefabs")]
    [SerializeField] private GameObject scattershotPrefab;
    [SerializeField] private GameObject scattershotPhase2Prefab;
    [SerializeField] private GameObject machinegunProjectilePrefab;
    [SerializeField] private GameObject machinegunBurstProjectilePrefab;
    [SerializeField] private GameObject airstrikeProjectilePrefab;
    [SerializeField] private GameObject airstrikeExplosionPrefab;
    [SerializeField] GameObject gun;
    [SerializeField] GameObject projectileSpawn;


    [Header("General")]
    [SerializeField] private bool inPhase2 = false;
    [SerializeField] private float globalCooldown = 2f;
    [SerializeField] private float globalCooldownTimer = 0f;
    [SerializeField] private float actionSpeed = 1f;
    [SerializeField] private float gunTurnSpeed = 1f;
    [SerializeField] private float speed = 1f;

    private bool isMoving = false;
    private bool strafeBlocked = false;
    private bool lookAtPlayer = false;
    private Vector3 playerDirection;
    private EState currentState = EState.Idle;
    private GameObject projectilesParent;
    private Coroutine moveInstance;

    [Header("Scattershot variables")]
    [SerializeField] private Vector3[] scatterTargets;
    [SerializeField] private int scatterProjectileCount = 8;
    [SerializeField] private int scatterProjectileCountPhase2 = 12;

    [Header("Machinegun variables")]
    [SerializeField] private int mgBurstCount = 5;
    [SerializeField] private int mgBurstCountPhase2 = 8;
    [SerializeField] private int mgProjectilesPerBurst = 5;
    [SerializeField] private int mgProjectilesPerBurstPhase2 = 7;
    [SerializeField] private float mgBurstAngle = 40f;
    [SerializeField] private float mgBurstAnglePhase2 = 45f;
    [SerializeField] private float mgInterval = 0.4f;
    [SerializeField] private float mgIntervalPhase2 = 0.35f;
    [SerializeField] private float inaccuracyMultiplier = 1f;

    [Header("Airstrike variables")]
    [SerializeField] private GameObject airstrikeTargets;
    [SerializeField] private float minFallDuration = 2.5f;
    [SerializeField] private float maxFallDuration = 3.5f;
    [SerializeField] private float fallDelay = 1f;
    [SerializeField] private float droppingInterval = 0.05f;
    [SerializeField] private float airstrikeBlockDuration = 15f;
    private bool airstrikeBlocked = false;


    private enum EState
    {
         Idle, OnCooldown, ExecutingAttack
    }


    private void Awake()
    {
        hasHealthbar = false;
        projectilesParent = Instantiate(new GameObject("Boss1 projectiles"));
        projectilesParent.transform.position = Vector3.zero;
    }

    private void Start()
    {
        EnemyStart();
        StartCoroutine(StrafeRoutine());
    }

    private void Update()
    {
        UpdatePlayerDirection();
        if (lookAtPlayer)
            RotateBarrelTowardsPlayer();

        switch (currentState)
        {
            default:
            case EState.Idle:
                RandomAttack();
                currentState = EState.ExecutingAttack;
                break;
            case EState.ExecutingAttack:
                break;
            case EState.OnCooldown:
                globalCooldownTimer += Time.deltaTime;
                if (globalCooldownTimer >= globalCooldown)
                    currentState = EState.Idle;
                break;

        }
    }

    private void RandomAttack()
    {
        var rand = new System.Random();
        int r;
        if (airstrikeBlocked)
            r = rand.Next(3);
        else r = rand.Next(4);

        switch (r)
        {
            case 0:
                StartCoroutine(ScatterAttackRoutine());
                break;
            case 1:
                StartCoroutine(MachinegunAttackRoutine());
                break;
            case 2:
                StartCoroutine(SideMachinegunAttackRoutine());
                break;
            case 3:
                StartCoroutine(AirstrikeAttackRoutine());
                break;
        }
    }

    private void OnAttackEnd()
    {
        globalCooldownTimer = 0f;
        currentState = EState.OnCooldown;
    }

    private void OnEnterPhase2()
    {
        inPhase2 = true;
        mgBurstCount = mgBurstCountPhase2;
        mgInterval = mgIntervalPhase2;
        mgProjectilesPerBurst = mgProjectilesPerBurstPhase2;
        mgBurstAngle = mgBurstAnglePhase2;
        scattershotPrefab = scattershotPhase2Prefab;
        scatterProjectileCount = scatterProjectileCountPhase2;
    }

    private void UpdatePlayerDirection()
    {
        Vector3 playerPos = GameManager.instance.GetPlayerPosition();
        playerDirection = (playerPos - gun.transform.position).normalized;
    }

    private void RotateBarrelTowardsPlayer()
    {
        float angle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;
        gun.transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
    }

    private Vector3 GetDirectionTowardsTarget(Vector3 target)
    {
        return (target - gun.transform.position).normalized;
    }

    private IEnumerator MoveToSideRoutine(bool moveToRightSide, bool strafing = false)
    {
        var rand = new System.Random();
        float randomX = (float)rand.NextDouble() * 4f + 13.5f;   // <13.5; 17.5)
        float direction = 1;
        float moveSpeed = speed;
        if (strafing)
            moveSpeed *= 0.5f;
        if (!moveToRightSide)
        {
            randomX *= -1;
            direction = -1;
        }

        isMoving = true;
        for (;;)
        {
            if (moveToRightSide && transform.position.x >= randomX)
            {
                isMoving = false;
                break;
            }
            else if (!moveToRightSide && transform.position.x <= randomX)
            {
                isMoving = false;
                break;
            }
            transform.position += new Vector3(moveSpeed * Time.deltaTime * direction, 0f, 0f);
            yield return null;
        }
    }

    private IEnumerator StrafeRoutine()
    {
        for (;;)
        {
            if (!isMoving && !strafeBlocked)
                moveInstance = StartCoroutine(MoveToSideRoutine(!IsOnRightSide(), true));

            yield return new WaitForSeconds(0.2f);
        }
    }

    // Returns true if boss's X coordinate is >= 0 and false if X < 0
    private bool IsOnRightSide()
    {
        if (transform.position.x >= 0)
            return true;
        else return false;
    }

    // Boss goes to FARTHER side and does machinegun attack
    private IEnumerator SideMachinegunAttackRoutine()
    {
        //if (moveInstance != null)
        //    yield return moveInstance;
        strafeBlocked = true;
        if (moveInstance != null)
            StopCoroutine(moveInstance);
        yield return RotateBarrelTowardsPlayerRoutine(true);
        yield return MoveToSideRoutine(IsOnRightSide());
        yield return MachinegunAttackRoutine(true);
        strafeBlocked = false;
    }

    // Shoots a single scattershot projectile towards target
    private void ShootScatter(Vector3 target)
    {
        GameObject scattershotProjectileGO = Instantiate(scattershotPrefab, projectileSpawn.transform.position, gun.transform.rotation, projectilesParent.transform);
        ScattershotProjectile scattershotProjectile = scattershotProjectileGO.GetComponent<ScattershotProjectile>();
        scattershotProjectile.SetNumberOfScatters(scatterProjectileCount);
    }

    // Shoots a generic projectile towards the player
    private void ShootAtPlayer(bool burst = false)
    {
        RotateBarrelTowardsPlayer();

        if (burst)
        {
            float angleStep = mgBurstAngle / Mathf.Max(mgProjectilesPerBurst - 1, 1);
            
            for (int i=0; i<mgProjectilesPerBurst; i++)
            {
                float inacc = Random.Range(-1f, 1f) * inaccuracyMultiplier;
                Quaternion shotRotation = gun.transform.rotation * Quaternion.Euler(0, 0, -mgBurstAngle / 2f + angleStep * i + inacc);
                Instantiate(machinegunBurstProjectilePrefab, projectileSpawn.transform.position, shotRotation, projectilesParent.transform);
            }  
        }

        else
        {
            float inacc = Random.Range(-1f, 1f) * inaccuracyMultiplier;
            Instantiate(machinegunProjectilePrefab, projectileSpawn.transform.position, gun.transform.rotation * Quaternion.Euler(0,0, inacc), projectilesParent.transform);
        }
    }

    // Shoots a burst of generic projectiles towards the player
    private IEnumerator MachinegunAttackRoutine(bool burst = false)
    {
        if (!lookAtPlayer)
            yield return RotateBarrelTowardsPlayerRoutine(true);
        for (int i=0; i<mgBurstCount; i++)
        {
            ShootAtPlayer(burst);
            yield return new WaitForSeconds(mgInterval);
        }
        lookAtPlayer = false;
        OnAttackEnd();
    }

    // Handles airstrike move logic. Blocks airstrike move for a certain duration, after which unlocks it.
    private IEnumerator AirstrikeAttackRoutine()
    {
        airstrikeBlocked = true;
        Vector3 target = new Vector3(10f, 0f, 0f);
        bool isOnRightSide = IsOnRightSide();
        if (!isOnRightSide)
            target *= -1;

        yield return RotateBarrelTowardsTargetRoutine(target);
        ShootAirstrikeProjectile(target);
        OnAttackEnd();
        yield return new WaitForSeconds(fallDelay);
        yield return SpawnAirstrikeExplosions(isOnRightSide);
        yield return new WaitForSeconds(airstrikeBlockDuration);
        airstrikeBlocked = false;
    }

    private void ShootAirstrikeProjectile(Vector3 target)
    {
        Instantiate(airstrikeProjectilePrefab, projectileSpawn.transform.position, gun.transform.rotation, projectilesParent.transform);
    }

    private IEnumerator SpawnAirstrikeExplosions(bool isOnRightSide)
    {
        float dir = -1f;
        if (!isOnRightSide)
            dir = 1f;

        foreach (Transform t in airstrikeTargets.GetComponentInChildren<Transform>())
        {
            GameObject go = Instantiate(airstrikeExplosionPrefab, new Vector3(t.position.x * dir, t.position.y, t.position.z), Quaternion.identity, projectilesParent.transform);
            float randomFallDuration = Random.Range(minFallDuration, maxFallDuration);
            go.GetComponent<AirstrikeExplosion>().SetFallDuration(randomFallDuration);
            yield return new WaitForSeconds(droppingInterval);
        }
    }

    private IEnumerator RotateBarrelTowardsTargetRoutine(Vector3 target)
    {
        Vector3 dir = GetDirectionTowardsTarget(target);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        
        float angleEuler = angle - 90f;
        if (angleEuler < 0)
            angleEuler += 360f;
        float angleCurrent = gun.transform.rotation.eulerAngles.z;
        float increment = -1f;
        if (angleEuler - angleCurrent > 0)
            increment = 1f;
        
        for (;;)
        {
            angleCurrent = gun.transform.rotation.eulerAngles.z;
            if ((increment > 0 && angleCurrent >= angleEuler) || (increment < 0 && angleCurrent <= angleEuler))
            {
                gun.transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
                break;
            }
            gun.transform.Rotate(0f, 0f, gunTurnSpeed * increment * Time.deltaTime * actionSpeed);
            yield return null;
        }
    }

    private IEnumerator RotateBarrelTowardsPlayerRoutine(bool stayLocked = false)
    {
        Vector3 dir = playerDirection;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        float angleEuler = angle - 90f;
        if (angleEuler < 0)
            angleEuler += 360f;
        float angleCurrent = gun.transform.rotation.eulerAngles.z;
        float increment = -1f;
        if (angleEuler - angleCurrent > 0)
            increment = 1f;

        for (; ; )
        {
            angleCurrent = gun.transform.rotation.eulerAngles.z;
            if ((increment > 0 && angleCurrent >= angleEuler) || (increment < 0 && angleCurrent <= angleEuler))
            {
                gun.transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
                if (stayLocked)
                    lookAtPlayer = true;
                break;
            }
            gun.transform.Rotate(0f, 0f, gunTurnSpeed * increment * Time.deltaTime * actionSpeed);
            yield return null;
        }
    }

    private IEnumerator ScatterAttackRoutine()
    {
        foreach (Vector3 v in scatterTargets)
        {
            yield return RotateBarrelTowardsTargetRoutine(v);
            ShootScatter(v);
        }
        OnAttackEnd();
    }

    public override void OnHit(float damage)
    {
        base.OnHit(damage);
        if (hpCurrent.Value <= hp.Value / 2)
            OnEnterPhase2();
    }
}
