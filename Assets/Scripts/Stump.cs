using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stump : MonoBehaviour
{
    void Awake()
    {
        GetComponent<SpriteRenderer>().sortingOrder = (int)-transform.position.y;
    }

}
