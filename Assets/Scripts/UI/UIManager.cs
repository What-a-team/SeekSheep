using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    [Header("axe")]
    public GameObject axeImg;
    public GameObject woodImg;
    public Text woodNumText;

    [Header("dialog")]
    public RectTransform dialogUI;
    public float moveY = 120f, showUpTime = 0.5f;
    public Text DialogText;


    Tweener _tweener;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);


        _tweener = dialogUI.DOLocalMove(dialogUI.localPosition, showUpTime).SetAutoKill(false);

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

   public void UpdateDialogAndShow(string text, float stayTime)
   {
        DialogText.text = text;

        StartCoroutine(UIShowUp(stayTime));
   }


    IEnumerator UIShowUp(float stayTime)
    {
        _tweener.ChangeEndValue(dialogUI.localPosition + new Vector3(0, -moveY, 0), true).Play();
        yield return new WaitForSeconds(stayTime);
        _tweener.ChangeEndValue(dialogUI.localPosition + new Vector3(0, moveY, 0), true).Play();
    }

}
