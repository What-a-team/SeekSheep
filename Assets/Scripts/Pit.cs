using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit : MonoBehaviour
{
    public Sprite pitWithRock;
    Sprite pit;

    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        pit = spriteRenderer.sprite;
    }

    public void FillPit()
    {
        RetractLastController.instance.pit = this;
        spriteRenderer.sprite = pitWithRock;
        transform.tag = "grass";
    }

    public void UnfillPit()
    {
        spriteRenderer.sprite = pit;
        transform.tag = "pit";
    }

}
