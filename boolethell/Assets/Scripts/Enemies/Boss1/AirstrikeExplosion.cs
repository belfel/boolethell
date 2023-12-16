using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirstrikeExplosion : MonoBehaviour
{
   
    private Collider2D hitbox;
    private GameObject warningZoneGO;

    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private GameObject warningZone; 

    [SerializeField] private float fallDuration = 5f;
    [SerializeField] private float startScale = 6f;
    [SerializeField] private float endScale = 1f;
    [SerializeField] private float damage = 1f;


    private void Awake()
    {
        hitbox = gameObject.GetComponent<Collider2D>();
    }

    void Start()
    {
        warningZoneGO = Instantiate(warningZone, transform.position, Quaternion.identity);
        warningZoneGO.transform.localScale = new Vector3(startScale, startScale, 1f);
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
        float scale = startScale;
        float timePassed = 0f;

        for (; ;)
        {
            timePassed += Time.deltaTime;

            gameObject.transform.localScale = new Vector3(scale, scale, 1f);
            scale = (1 - (timePassed / fallDuration)) * (startScale - endScale) + 1f;
            if (timePassed > fallDuration)
                break;

            yield return null;
        }

        gameObject.transform.localScale = new Vector3(startScale, startScale, 1f);
        sprite.transform.localScale = new Vector3(1f, 1f, 1f);
        sprite.color = new Color(1, 0, 0, 1);
        yield return new WaitForFixedUpdate();
        DealDamage();

        yield return new WaitForSeconds(0.2f);

        Destroy(gameObject);
    }

    private void OnDisable()
    {
        Destroy(warningZoneGO);
    }
}
