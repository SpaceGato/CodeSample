using System;
using System.Collections.Generic;
using UnityEngine;

public class Projectile: MonoBehaviour
{
    public GameObject impactFX;
    public bool playerProjectile, bossProjectile;

    public float speed;
    public bool targeting, stun, confuse;
    [NonSerialized] public float damage;
    [NonSerialized] public bool 
        piercing, dot, slow, weaponsOfficer, terminalBallistics;

    [NonSerialized] public List<Collider2D> closeRangeUnits = new List<Collider2D>();
    Vector2 screenPos;
    Rigidbody2D rb;
    LevelManager level;
    Player player;

    private void Awake()
    {
        level = GameObject.Find("Level").GetComponent<LevelManager>();
        player = GameObject.Find("Player").GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        if (playerProjectile)
        {
            speed = player.proSpeed;
            damage = player.damage;

            piercing = player.piercing;
            dot = player.dot;
            slow = player.slow;
            weaponsOfficer = player.weaponsOfficer;
            terminalBallistics = player.terminalBallistics;
        }
    }
    private void Start()
    {
        if (targeting)
        {
            Vector2 targetPos = player.transform.position;
            transform.up = targetPos - rb.position;
        }
        rb.velocity = transform.up * speed;
    }

    private void FixedUpdate()
    {
        screenPos = Camera.main.WorldToViewportPoint(transform.position);
        if (screenPos.x < 0.25 || screenPos.x > 0.75 || screenPos.y < -0.05 || screenPos.y > 1.05)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<HazardObject>())
        {
            if (playerProjectile || bossProjectile)
            {
                HazardObject hazardObject = collider.GetComponent<HazardObject>();
                if (hazardObject.hazardType == HazardObject.HazardType.Asteroid)
                {
                    hazardObject.Death();
                    Instantiate(impactFX, transform.position, transform.rotation);
                    if (!piercing)
                        Destroy(gameObject);
                }
            }
        }

        if (level.levelActive)
        {
            if (playerProjectile)
            {
                if (closeRangeUnits.Contains(collider))
                    damage *= 1.4f;

                if (collider.GetComponent<Boss>() && collider.GetComponent<Boss>().isAlive)
                {
                    Boss boss = collider.GetComponent<Boss>();
                    boss.TakeDamage(damage);
                    Debug.Log(collider.name + " took " + damage + " damage");
                    ApplyEffect(boss.gameObject);
                    Instantiate(impactFX, transform.position, transform.rotation);
                    if (!piercing)
                        Destroy(gameObject);
                }
                else if (collider.GetComponent<CentipedeSegment>() && collider.GetComponent<CentipedeSegment>().isAlive)
                {
                    CentipedeSegment segment = collider.GetComponent<CentipedeSegment>();
                    segment.TakeDamage(damage);
                    Debug.Log(collider.name + " took " + damage + " damage");
                    ApplyEffect(segment.boss.gameObject);
                    Instantiate(impactFX, transform.position, transform.rotation);
                    if (!piercing)
                        Destroy(gameObject);
                }
                else if (collider.GetComponent<OrbitalShip>() && collider.GetComponent<OrbitalShip>().isAlive)
                {
                    OrbitalShip ship = collider.GetComponent<OrbitalShip>();
                    ship.TakeDamage(damage);
                    Debug.Log(collider.name + " took " + damage + " damage");
                    ApplyEffect(ship.gameObject);
                    Instantiate(impactFX, transform.position, transform.rotation);
                    if (!piercing)
                        Destroy(gameObject);
                }
                else if (collider.GetComponent<Enemy>() && collider.GetComponent<Enemy>().isAlive)
                {
                    Enemy enemy = collider.GetComponent<Enemy>();
                    if (enemy.isAlive)
                    {
                        enemy.TakeDamage(damage);
                        Debug.Log(collider.name + " took " + damage + " damage");
                        ApplyEffect(enemy.gameObject);
                        Instantiate(impactFX, transform.position, transform.rotation);
                        if (!piercing)
                            Destroy(gameObject);
                    }
                }
            }

            else if (collider.GetComponent<Player>() && collider.GetComponent<Player>().isAlive)
            {
                Player player = collider.GetComponent<Player>();
                player.TakeDamage(damage);
                ApplyEffect(player.gameObject);
                Instantiate(impactFX, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }

    private void ApplyEffect(GameObject target)
    {
        if (terminalBallistics && target.GetComponent<TerminalEffect>() == null)
        {            
            target.AddComponent<TerminalEffect>();
        }
        if (weaponsOfficer && target.GetComponent<ExplosiveEffect>() == null)
        {
            if (target.GetComponent<Boss>() == null)
                target.AddComponent<ExplosiveEffect>();
        }
        if (dot)
            target.AddComponent<DamageOverTime>().effectAmount = damage/2;
        if (slow && target.GetComponent<Slowed>() == null)
            target.AddComponent<Slowed>();
        if(confuse)
            target.AddComponent<Confusion>();
        if(stun)
            target.AddComponent<Stunned>();   
    }
}