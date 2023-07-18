using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    void Awake()
    {
        GetComponent<SpriteRenderer>().sortingOrder = (int)-transform.position.y;
    }

   
}
