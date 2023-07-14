using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Axe : MonoBehaviour
{
    public GameObject axeImg;
    public GameObject woodImg;
    
    public void GetAxe()
    {
        axeImg.SetActive(true);
        Destroy(this.gameObject);
    }
}
