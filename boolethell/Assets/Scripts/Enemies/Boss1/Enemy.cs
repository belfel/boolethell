using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    protected bool hasHealthbar = false;

    public FloatVariable hp;
    public FloatVariable hpCurrent;
    public UnityEvent onHit;
    public UnityEvent onDeath;


    private void ResetHP()
    {
        hpCurrent.SetValue(hp);
        onHit.Invoke(); // force refresh hp bar
    }

    public virtual void OnHit(float damage)
    {
        hpCurrent.Value -= damage;
        onHit.Invoke();

        if (hpCurrent.Value <= 0)
        {
            onHit.Invoke();
            onDeath.Invoke();
            Destroy(gameObject);
        }
    }

    protected void EnemyStart()
    {
        ResetHP();
    }
}
