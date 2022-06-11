using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slowed : MonoBehaviour
{
    public float effectAmount = 0.75f, effectTime = 5;
    bool player;

    private void Awake()
    {
        Debug.Log(name + " is Slowed");
        if (GetComponent<Player>() != null)
            player = true;
        else
            player = false;
        StartCoroutine(Effect(effectTime));
    }

    public IEnumerator Effect(float time)
    {
        int timer = 0;
        if (!player)
        {
            if (GetComponent<Boss>() != null)
            {
                Boss target = GetComponent<Boss>();
                BossMovement targetMovement = GetComponent<BossMovement>();
                targetMovement.moveSpeed *= effectAmount;
                while (target.isAlive && timer < time)
                {
                    yield return new WaitForSeconds(1);
                    timer++;
                }
                targetMovement.moveSpeed /= effectAmount;
            }
            else if (GetComponent<OrbitalShip>() != null)
            {
                OrbitalShip target = GetComponent<OrbitalShip>();
                OrbitalSquad squadMovement = target.squad;
                squadMovement.squadSpeed *= effectAmount;
                while (target.isAlive && timer < time)
                {
                    yield return new WaitForSeconds(1);
                    timer++;
                }
                squadMovement.squadSpeed /= effectAmount;
            }
            else
            {
                Enemy target = GetComponent<Enemy>();
                EnemyMovement targetMovement = GetComponent<EnemyMovement>();
                targetMovement.moveSpeed *= effectAmount;
                while (target.isAlive && timer < time)
                {
                    yield return new WaitForSeconds(1);
                    timer++;
                }
                targetMovement.moveSpeed /= effectAmount;
            }
        }
        else
        {
            Player target = GetComponent<Player>();
            PlayerMovement targetMovement = GetComponent<PlayerMovement>();
            targetMovement.moveSpeed *= effectAmount;
            while (target.isAlive && timer < time)
            {
                yield return new WaitForSeconds(1);
                timer++;
            }
            targetMovement.moveSpeed /= effectAmount;
        }
        Destroy(this);
    }

    private void OnDestroy()
    {
        Debug.Log("Slow Removed");
    }
}
