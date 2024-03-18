using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Microlight.MicroBar;

public class Enemy : MonoBehaviour
{
    public FloatVariable hp;
    public FloatVariable hpCurrent;
    public UnityEvent onHit;
    public UnityEvent onDeath;

    [SerializeField] private MicroBar healthbar;

    protected virtual void Awake()
    {
        if (healthbar != null)
            healthbar.Initialize(hp.Value);
    }

    private void ResetHP()
    {
        hpCurrent.SetValue(hp);
        onHit.Invoke(); // force refresh hp bar
    }

    public virtual void OnHit(float damage)
    {
        hpCurrent.Value -= damage;
        if (onHit != null)
            onHit.Invoke();
        if (healthbar != null)
            healthbar.UpdateHealthBar(hpCurrent.Value);

        if (hpCurrent.Value <= 0)
        {
            if (onDeath != null)
                onDeath.Invoke();
            Destroy(gameObject);
        }
    }

    protected void EnemyStart()
    {
        ResetHP();
    }
}
