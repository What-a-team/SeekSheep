using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.Universal;

public class TargetsManager : MonoBehaviour
{
    //public static TargetsManager instance { get; private set; }

    public GameObject door;
    public float doorDisappearTime = 1f;
    public int completeNum;  // 推到了目标位置上的箱子数量

    public int totalNum;
    bool hasOpen = false;

    Light2D doorlight;
    float originIntensity;

    private void Awake()
    {
        totalNum = GetComponentsInChildren<Target>().Length;
       // totalNum = transform.childCount;
        doorlight = door.transform.GetChild(0).GetComponent<Light2D>();
        originIntensity = doorlight.intensity;
    }

    public void ChangeDoorLight()
    {
        doorlight.intensity = originIntensity * (1.5f + 2f*completeNum / totalNum);
    }

   
    public void OpenDoor()
    {
        if (!hasOpen && totalNum == completeNum)
            StartCoroutine(IOpenDoor());
           
    }

    IEnumerator IOpenDoor()
    {
        doorlight.intensity *= 1.2f;
        DOTween.To(() => doorlight.pointLightOuterRadius, x => doorlight.pointLightOuterRadius = x, 3f, 0.3f);
        //Tweener t = door.GetComponent<SpriteRenderer>().DOColor(Color.clear, doorDisappearTime);
        //t.Kill();
        yield return new WaitForSeconds(doorDisappearTime);
        Destroy(door);
        hasOpen = true;
;
    }

}
