using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEnemyProjectile : MonoBehaviour
{
    [SerializeField] private Vector3 direction = new Vector3(0f, 0f, 0f);
    [SerializeField] private float damage = 0.5f;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private float speedOverTimeDecrease = 0f;
    [SerializeField] private bool ignoreCollisions = false;
    [SerializeField] private bool ignorePlayerWhenDashing = false;
    [SerializeField] private BoolVariable isPlayerInvincible;

    void Update()
    {
        transform.position += direction.normalized * speed * Time.deltaTime;
        speed *= 1 - (speedOverTimeDecrease * Time.deltaTime);

        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ignoreCollisions)
            return;

        Player player = collision.gameObject.GetComponent<Player>();

        if (player)
        {
            if (ignorePlayerWhenDashing && isPlayerInvincible.Value)
            {
                return;
            }

            player.OnHit(damage);
            Destroy(gameObject);
        }

        else
        {
            Wall wall = collision.gameObject.GetComponent<Wall>();
            if (wall && wall.blocksEnemyProjectiles)
            {
                Destroy(gameObject);
            }
            else return;
        }
    }

    public void SetStats(Vector3 _direction, float _damage, float _speed, float _lifetime, float _speedDecrease)
    {
        direction = _direction;
        damage = _damage;
        speed = _speed;
        lifetime = _lifetime;
        speedOverTimeDecrease = _speedDecrease;
    }
}
