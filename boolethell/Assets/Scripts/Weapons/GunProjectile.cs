using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunProjectile : MonoBehaviour
{
    private short pierce = 0;
    private short bounce = 0;
    private float damage = 10f;
    private float speed = 10f;
    private float lifetime = 10f;
    private float lifetimeCurrent = 0f;

    private void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;

        if (lifetimeCurrent >= lifetime)
            Destroy(gameObject);
        else lifetimeCurrent += Time.deltaTime;
    }

    public void SetStats(short _pierce, short _bounce, float _damage, float _projectileSpeed, float _projectileLifetime)
    {
        pierce = _pierce;
        damage = _damage;
        speed = _projectileSpeed;
        lifetime = _projectileLifetime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        
        if (enemy)
        {
            enemy.OnHit(damage);

            pierce -= 1;
            if (pierce < 0)
                Destroy(gameObject);
        }

        else
        {
            Wall wall = collision.gameObject.GetComponent<Wall>();
            if (wall && wall.blocksPlayerProjectiles)
            {
                if (bounce > 0)
                {
                    bounce -= 1;
                    //TODO bounce
                }
                else Destroy(gameObject);
            }
            else return;
        }
    }
}
