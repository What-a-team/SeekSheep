using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Axe : MonoBehaviour
{
    public GameObject axeImg;
    public GameObject woodImg;
    public Text woodNumText;
    
    public void GetAxe()
    {
        axeImg.SetActive(true);
        woodImg.SetActive(true);
        woodNumText.text = "1";
        Destroy(this.gameObject);
    }

    public void UpdateWoodNum(int num)
    {
        woodNumText.text = num.ToString();
    }
}
