using System.Collections;
using UnityEngine;

public class Stunned : MonoBehaviour
{
    private float effectTime = 1.5f;
    Player player;
    PlayerMovement movement;
    LevelManager level;
    GameObject FX;

    private void Awake()
    {
        Debug.Log(name + " is Stunned");
        level = FindObjectOfType<LevelManager>();
        if (GetComponent<Player>() != null)
        {
            player = GetComponent<Player>();
            movement = GetComponent<PlayerMovement>();
            FX = Instantiate(player.stunFX, player.transform);
            StartCoroutine(Effect(effectTime));
        }
        else
            Destroy(this);
    }

    private IEnumerator Effect(float time)
    {        
        float timer = 0;
        movement.moveAllowed = false;
        player.isStunned = true;
        while (player.isAlive && level.levelActive && timer < time)
        {
            yield return new WaitForSeconds(0.5f);
            timer += 0.5f;
        }
        player.isStunned = false;
        movement.moveAllowed = true;
        Destroy(FX);
        Destroy(this);
    }

    private void OnDestroy()
    {
        Debug.Log("Stun Removed");
    }
}
