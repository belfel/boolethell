using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using System;

public class Boss2 : Enemy
{
    public ActionController actionController;
    public LaserTurretAttack atkTurret;
    public SpinAttack atkSpin;

    [SerializeField] private Transform emitter;
    [SerializeField] private float globalCooldown = 2f;
    private EState currentState = EState.OnCooldown;
    private GameObject projectilesParent;
    private float globalCooldownTimer = 0f;
    private List<GameObject> laserTurrets = new List<GameObject>();

    private enum EState
    {
        Idle, OnCooldown, ExecutingAttack
    }

    private enum EAttack
    {
        Turret, Spin
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
        InitializeActionController();
    }

    private void Update()
    {
        switch (currentState)
        {
            case EState.Idle:
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

    private void InitializeActionController()
    {
        actionController.AddAttack((int)EAttack.Turret);
        actionController.AddAttack((int)EAttack.Spin);
    }

    private void OnAttackEnd()
    {
        globalCooldownTimer = 0f;
        currentState = EState.OnCooldown;
    }

    private void RandomAttack()
    {
        int r = actionController.RollAttack();
        Debug.Log(r);

        if (r == -1)
            return;
        currentState = EState.ExecutingAttack;

        EAttack attack = (EAttack)r;

        switch (attack)
        {
            case EAttack.Turret:
                StartCoroutine(PutAttackOnCooldown(attack, atkTurret.cooldown));
                SpawnLaserTurret();
                break;
            case EAttack.Spin:
                DoSpin();
                break;
        }
    }

    private IEnumerator PutAttackOnCooldown(EAttack attack, float delay)
    {
        actionController.RemoveAttack((int)attack);
        yield return new WaitForSeconds(delay);
        actionController.AddAttack((int)attack);
    }

    private void DoSpin()
    {
        emitter.DORotate(new Vector3(0, 0, 360 * atkSpin.duration / atkSpin.cycleLength), atkSpin.duration, RotateMode.FastBeyond360).SetEase(Ease.Linear);
        StartCoroutine(FireAtInterval(atkSpin.projectilePrefab, atkSpin.duration / atkSpin.projectileCount, atkSpin.projectileCount));
    }

    private IEnumerator FireAtInterval(GameObject projectilePrefab, float interval, int repeatCount)
    {
        int repeats = 0;

        while (repeats < repeatCount)
        {
            repeats++;
            Instantiate(atkSpin.projectilePrefab, transform.position, emitter.rotation);
            yield return new WaitForSeconds(interval);
        }
        OnAttackEnd();
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

            Vector2 randomDir = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
            hit = Physics2D.Raycast(gameObject.transform.position, randomDir, 100f, atkTurret.hitLayers);
            if (hit && !WillTurretsOverlap(hit.point))
                break;
        }

        GameObject turret = Instantiate(atkTurret.prefab);
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
            if (Vector3.Distance(position, go.transform.position) < atkTurret.minDistance)
            {
                overlap = true;
                break;
            }
        }
        return overlap;
    }

    [System.Serializable]
    public class ActionController
    {
        public List<int> availableAttacks = new List<int>();

        public int RollAttack()
        {
            if (availableAttacks.Count == 0)
                return -1;

            var rand = new System.Random();
            int r = rand.Next(availableAttacks.Count);

            return availableAttacks[r];
        }

        public void AddAttack(int attackId)
        {
            availableAttacks.Add(attackId);
        }

        public void RemoveAttack(int attackId)
        {
            availableAttacks.Remove(attackId);
        }
    }

    [System.Serializable]
    public class LaserTurretAttack
    {
        public GameObject prefab;
        public float cooldown = 12f;
        public float moveSpeed = 1f;
        public float minDistance = 2f;
        public LayerMask hitLayers;
    }

    [System.Serializable]
    public class SpinAttack
    {
        public GameObject projectilePrefab;
        public int projectileCount = 24;
        public float duration = 4f;
        public float cycleLength = 2f;
    }
}