using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeTree : MonoBehaviour
{
    SpriteRenderer spriteRenderer;


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = (int)-transform.position.y;
    }
   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Color origin = spriteRenderer.color;
            origin.a = 0.5f;
            spriteRenderer.color = origin;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Color origin = spriteRenderer.color;
            origin.a = 1f;
            spriteRenderer.color = origin;
        }
    }


}
