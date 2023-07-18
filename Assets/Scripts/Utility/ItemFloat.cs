using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFloat : MonoBehaviour
{
    public float range=0.05f, T=2f;
    Vector3 originalPos;

    private void Awake()
    {
        originalPos = transform.position;
    }
    private void Update()
    {
        transform.position = originalPos + range * new Vector3(0, Mathf.Sin(T*Time.time), 0);
    }

}
