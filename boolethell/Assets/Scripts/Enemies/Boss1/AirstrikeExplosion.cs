using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirstrikeExplosion : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Collider2D hitbox;

    [SerializeField] private float fallDuration = 5f;
    [SerializeField] private float startScale = 6f;
    [SerializeField] private float endScale = 1f;
    [SerializeField] private float damage = 1f;


    private void Awake()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        hitbox = gameObject.GetComponent<Collider2D>();
    }

    void Start()
    {
        sprite.color = new Color(0, 0, 0, 0);
        StartCoroutine(FallRoutine());
    }

    void Update()
    {

    }

    public void SetFallDuration(float seconds)
    {
        fallDuration = seconds;
    }

    private void DealDamage()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        ContactFilter2D cf = new ContactFilter2D().NoFilter();
        int colNum = hitbox.Overlap(cf, colliders);
        
        foreach(Collider2D c in colliders)
        {
            Player player = c.gameObject.GetComponent<Player>();

            if (player)
            {
                player.OnHit(damage);
            }
        }
    }

    private IEnumerator FallRoutine()
    {
        float alpha = 0;
        float scale = startScale;
        float timePassed = 0f;

        for (; ;)
        {
            gameObject.transform.localScale = new Vector3(scale, scale, 1f);
            sprite.color = new Color(0, 0, 0, alpha);
            scale = (1 - (timePassed / fallDuration)) * (startScale - endScale) + 1f;
            alpha = timePassed / fallDuration;
            if (alpha > 1f)
                break;
            else timePassed += Time.deltaTime;

            yield return null;
        }

        gameObject.transform.localScale = new Vector3(startScale, startScale, 1f);
        sprite.color = new Color(1, 0, 0, alpha);
        yield return new WaitForFixedUpdate();
        DealDamage();

        yield return new WaitForSeconds(0.2f);

        Destroy(gameObject);
    }
}
