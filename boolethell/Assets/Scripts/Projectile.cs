using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected float damage = 0f;
    [SerializeField] protected float speedMultiplier = 1f;
    [SerializeField] protected float lifetime = 10f;
    [SerializeField] protected float sizeOverTimeMultiplier = 1f;

    [SerializeField] protected bool ignoreCollisions = false;
    [SerializeField] protected bool ignorePlayerWhenDashing = false;
    [SerializeField] protected bool bounceOffWalls = false;
    [SerializeField] protected BoolVariable isPlayerDashing;

    // Start is called before the first frame update
    public virtual void Start()
    {
        AddForce(transform.up);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
            OnLifetimeOver();

        if (sizeOverTimeMultiplier != 1f)
            transform.localScale += new Vector3(-1f + sizeOverTimeMultiplier, -1f + sizeOverTimeMultiplier, 0) * Time.deltaTime;
    }

    protected virtual void OnLifetimeOver()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ignoreCollisions)
            return;

        Player player = collision.gameObject.GetComponent<Player>();

        if (player)
        {
            if (ignorePlayerWhenDashing && isPlayerDashing.Value)
            {
                return;
            }

            player.OnHit(damage);
            Destroy(gameObject);
        }

        else
        {
            Wall wall = collision.gameObject.GetComponent<Wall>();
            if (wall && wall.blocksEnemyProjectiles && !bounceOffWalls)
            {
                Destroy(gameObject);
            }
            else return;
        }
    }

    public void AddForce(Vector2 force)
    {
        rb.AddForce(force * speedMultiplier / Time.fixedDeltaTime);
    }
}
