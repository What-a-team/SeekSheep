using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public Sprite wood;
    public Sprite stone;

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    public void BuildBridge(ToolType type)
    {
        switch(type)
        {
            case ToolType.Stone:
                spriteRenderer.sprite = stone;
                break;
            case ToolType.Wood:
                spriteRenderer.sprite = wood;
                break;
        }

        transform.tag = "grass";
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (spriteRenderer.sprite == wood)
        {
            spriteRenderer.sprite = null;
            transform.tag = "water";
        }
        
    }

}
