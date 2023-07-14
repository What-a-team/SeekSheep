using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit : MonoBehaviour
{
    public Sprite pitWithRock;

    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void FillPit()
    {
        spriteRenderer.sprite = pitWithRock;
        transform.tag = "grass";
    }

}
