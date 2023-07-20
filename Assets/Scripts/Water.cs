using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public bool isOnWood = false;
    public Sprite wood;
    public Sprite rockInWater;

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
                spriteRenderer.sprite = rockInWater;
                break;
            case ToolType.Wood:
                spriteRenderer.sprite = wood;
                break;
        }

        transform.tag = "grass";
    }

    public void DestroyBridge()
    {
        spriteRenderer.sprite = null;
        transform.tag = "water";
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && spriteRenderer.sprite == wood)
        {
            isOnWood = true;

        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && spriteRenderer.sprite == wood)
        {
            RetractLastController.instance.water = this;
            RetractLastController.instance.ifDestroyWood = true;

            spriteRenderer.sprite = null;
            transform.tag = "water";
        }


       if (!RetractLastController.instance.canClick)
       {
            BuildBridge(ToolType.Wood);
       }
    }

}
