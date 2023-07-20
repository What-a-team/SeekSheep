using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFloat : MonoBehaviour
{
    public float range=0.05f, T=2f;
    Vector3 originalPos;

    private void Start()
    {
        originalPos = transform.position;
    }
    private void Update()
    {
        transform.position = originalPos + 0.5F * range * new Vector3(0, Mathf.Sin(T*Time.time) + Mathf.Cos(2*T * Time.time), 0);
    }

}
