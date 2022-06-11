using System.Collections;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public GameObject pickupFX;

    public PickupType pickupType;
    public enum PickupType
    {
        Health, Money
    }

    public int amount;
    [System.NonSerialized] public float speed = 15;
    [System.NonSerialized] public bool levelEndCash;

    Vector2 screenPos;
    GameManager gameManager;
    LevelManager level;
    Player player;
    Rigidbody2D rb;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        level = FindObjectOfType<LevelManager>();
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody2D>();

        if (levelEndCash)
        {
            if (level.bossLevel)
                amount = 500;
            else
                amount = 200;
            transform.localScale = Vector2.zero;
            StartCoroutine(ScaleUp(new Vector2(2,2)));
        }
        else
            rb.velocity = -transform.up * speed;
    }

    private void Update()
    {
        screenPos = Camera.main.WorldToViewportPoint(transform.position);
        if (screenPos.y < -0.1 || screenPos.y > 1.1)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            player = collision.GetComponent<Player>();
            switch (pickupType)
            {
                case PickupType.Health:
                    player.HP.CurrentValue += player.HP.MaxValue * ((float)amount/100);
                    break;

                case PickupType.Money:
                    gameManager.UpdateCredits(amount);
                    break;
            }
            Instantiate(pickupFX, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    private IEnumerator ScaleUp(Vector2 targetScale)
    {
        Vector2 startScale = transform.localScale;
        float lerpTime = 0;
        float lerpSpeed = 1;
        while(transform.localScale.x < targetScale.x)
        {
            transform.localScale = Vector2.Lerp(startScale, targetScale, lerpTime);
            lerpTime += Time.deltaTime * lerpSpeed;
            yield return null;
        }
    }
}
