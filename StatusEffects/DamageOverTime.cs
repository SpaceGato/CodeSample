using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTime : MonoBehaviour
{
    public float effectAmount, effectTime = 3;
    bool playerBool;
    Player player;
    GameObject FX;

    private void Awake()
    {
        Debug.Log(name + " has Damage Over Time");
        player = FindObjectOfType<Player>();
        FX = Instantiate(player.dotFX, gameObject.transform);
        if (GetComponent<Player>() != null)
            playerBool = true;
        else
            playerBool = false;
        StartCoroutine(Effect(effectTime));
    }

    public IEnumerator Effect(float time)
    {
        int timer = 0;
        if (!playerBool)
        {
            if (GetComponent<Boss>() != null)
            {
                Boss target = GetComponent<Boss>();
                while (target.isAlive && timer < time)
                {
                    yield return new WaitForSeconds(1);
                    FindObjectOfType<AudioManager>().Play("CollisionFX");
                    target.TakeDamage(effectAmount / time);
                    timer++;
                }
            }
            else if (GetComponent<OrbitalShip>() != null)
            {
                OrbitalShip target = GetComponent<OrbitalShip>();
                while (target.isAlive && timer < time)
                {
                    yield return new WaitForSeconds(1);
                    FindObjectOfType<AudioManager>().Play("CollisionFX");
                    target.TakeDamage(effectAmount / time);
                    timer++;
                }
            }
            else
            {
                Enemy target = GetComponent<Enemy>();
                while (target.isAlive && timer < time)
                {
                    yield return new WaitForSeconds(1);
                    FindObjectOfType<AudioManager>().Play("CollisionFX");
                    target.TakeDamage(effectAmount / time);
                    timer++;
                }
            }
        }
        else
        {
            Player target = GetComponent<Player>();
            while (target.isAlive && timer < time)
            {
                yield return new WaitForSeconds(1);
                FindObjectOfType<AudioManager>().Play("CollisionFX");
                target.TakeDamage(effectAmount / time);
                timer++;
            }
        }
        Destroy(this);
    }

    private void OnDestroy()
    {
        Debug.Log("Damage Over Time Removed");
    }
}
