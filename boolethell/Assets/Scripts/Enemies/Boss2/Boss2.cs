using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : Enemy
{
    [SerializeField] GameObject laserTurretPrefab;
    [SerializeField] private float globalCooldown = 2f;
    [SerializeField] private float laserTurretThrowSpeed = 1f;
    [SerializeField] private float laserTurretMinDistance = 2f;
    [SerializeField] private LayerMask laserTurretHitLayers;

    private EState currentState = EState.OnCooldown;
    private GameObject projectilesParent;
    private float globalCooldownTimer = 0f;
    private List<GameObject> laserTurrets = new List<GameObject>();

    private enum EState
    {
        Idle, OnCooldown, ExecutingAttack
    }

    protected override void Awake()
    {
        base.Awake();
        projectilesParent = new GameObject("Boss2 projectiles");
        projectilesParent.transform.position = Vector3.zero;
    }

    private void Start()
    {
        EnemyStart();
    }

    private void Update()
    {
        switch (currentState)
        {
            case EState.Idle:
                currentState = EState.ExecutingAttack;
                RandomAttack();
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

    private void OnAttackEnd()
    {
        globalCooldownTimer = 0f;
        currentState = EState.OnCooldown;
    }

    private void RandomAttack()
    {
        var rand = new System.Random();
        int r = rand.Next(1);

        switch (r)
        {
            case 0:
                SpawnLaserTurret();
                break;
        }
    }

    private void SpawnLaserTurret()
    {
        RaycastHit2D hit;
        int numOfTries = 0;
        while (true)
        {
            if (numOfTries > 50)
            {
                OnAttackEnd();
                return;
            }
            numOfTries++;

            Vector2 randomDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            hit = Physics2D.Raycast(gameObject.transform.position, randomDir, 100f, laserTurretHitLayers);
            if (hit && !WillTurretsOverlap(hit.point))
                break;
        }

        GameObject turret = Instantiate(laserTurretPrefab);
        laserTurrets.Add(turret);
        Rigidbody2D laserTurret = turret.GetComponent<Rigidbody2D>();
        laserTurret.MovePosition(hit.point);

        OnAttackEnd();
    }

    private bool WillTurretsOverlap(Vector3 position)
    {
        bool overlap = false;
        foreach (GameObject go in laserTurrets)
        {
            if (Vector3.Distance(position, go.transform.position) < laserTurretMinDistance)
            {
                overlap = true;
                break;
            }
        }
        return overlap;
    }
}
