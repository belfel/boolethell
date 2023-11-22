using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject model;
    [SerializeField] GameObject healthbarPrefab;

    [SerializeField] private BoolList isInvincible;

    public FloatVariable health;
    public FloatList maxHealth;
    public Vector3Variable position;

    public UnityEvent onDamageReceived;
    public GameEvent onDeath;

    private void Start()
    {
        health.SetValue(maxHealth.GetSum());
        onDamageReceived.Invoke(); // force healthbar refresh
    }

    private void Update()
    {
        position.Value = transform.position;
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }

    public void OnHit(float damage)
    {
        if (isInvincible.IsAnyTrue())
            return;

        health.Value -= damage;
        onDamageReceived.Invoke();
        if (health.Value <= 0f)
        {
            onDamageReceived.Invoke();
            onDeath.Raise();
            Destroy(gameObject);
        }
    }

    public GameObject GetModelGO()
    {
        return model;
    }
}
