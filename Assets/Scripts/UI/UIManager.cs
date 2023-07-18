using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    public GameObject axeImg;
    public GameObject woodImg;
    public Text woodNumText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);

    }

    
    
    public void GetAxe()
    {
        axeImg.SetActive(true);
        woodImg.SetActive(true);
        woodNumText.text = "0";
    }

    public void UpdateWoodNum(int num)
    {
        woodNumText.text = num.ToString();
    }

   


}
