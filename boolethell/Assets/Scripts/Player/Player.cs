using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Microlight.MicroBar;

public class Player : MonoBehaviour
{
    public FloatVariable health;
    public FloatList maxHealth;
    public Vector3Variable position;

    public UnityEvent onDamageReceived;
    public GameEvent onDeath;
    public GameEvent onGameover;

    [SerializeField] private GameObject model;
    [SerializeField] private MicroBar healthbar;
    [SerializeField] private BoolList isInvincible;
    [SerializeField] private BoolList controlsDisabled;
    [SerializeField] private BoolVariable controlsDisabledByDeath;

    private bool isDead = false;

    private void Awake()
    {
        controlsDisabledByDeath.SetValue(false);
        controlsDisabled.AddVariable(controlsDisabledByDeath);
        health.SetValue(maxHealth.GetSum());
        healthbar.Initialize(health.Value);
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
        if (isInvincible.IsAnyTrue() || isDead)
            return;

        health.Value -= damage;
        onDamageReceived.Invoke();
        healthbar.UpdateHealthBar(health.Value);

        if (health.Value <= 0f)
            StartCoroutine(OnDeathSequenceRoutine()); 
    }

    public GameObject GetModelGO()
    {
        return model;
    }

    private IEnumerator OnDeathSequenceRoutine()
    {
        onDeath.Raise();
        controlsDisabledByDeath.SetValue(true);
        isDead = true;

        float timer = 0f;
        while (timer <= 3f)
        {
            Time.timeScale = Mathf.Max(0f, Mathf.Pow((3f - timer) / 3f, 2f));

            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        onGameover.Raise();
        Destroy(gameObject);
    }
}
