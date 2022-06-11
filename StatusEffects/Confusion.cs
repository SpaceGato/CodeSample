using System.Collections;
using UnityEngine;

public class Confusion : MonoBehaviour
{
    private float effectTime = 1.5f;
    Player player;
    LevelManager level;
    GameObject FX;

    private void Awake()
    {
        Debug.Log(name + " is Confused");
        level = FindObjectOfType<LevelManager>();
        if (GetComponent<Player>() != null)
        {
            player = GetComponent<Player>();
            FX = Instantiate(player.confuseFX, player.transform);
            StartCoroutine(Effect(effectTime));
        }
        else
            Destroy(this);
    }

    public IEnumerator Effect(float time)
    {
        float timer = 0;
        player.isConfused = true;
        while (player.isAlive && level.levelActive && timer < time)
        {
            yield return new WaitForSeconds(0.5f);
            timer += 0.5f;
        }
        player.isConfused = false;
        Destroy(FX);
        Destroy(this);
    }

    private void OnDestroy()
    {
        Debug.Log("Confusion Removed");
    }
}
