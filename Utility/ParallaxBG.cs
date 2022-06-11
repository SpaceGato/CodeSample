using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBG : MonoBehaviour
{
    public float layerSpeed;
    private float height, newYPos;

    Vector2 startPos;

    private void Start()
    {
        height = GetComponent<SpriteRenderer>().bounds.size.y;
        startPos = transform.position;
    }

    private void FixedUpdate()
    {
        newYPos = Mathf.Repeat(Time.time * -layerSpeed, height);
        transform.position = startPos + Vector2.up * newYPos;
    }
}
