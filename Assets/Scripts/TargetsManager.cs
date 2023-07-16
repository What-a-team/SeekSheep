using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TargetsManager : MonoBehaviour
{
    public static TargetsManager instance { get; private set; }

    public GameObject door;
    public float doorDisappearTime = 1f;
    public int totalNum;
    public int completeNum;  // �Ƶ���Ŀ��λ���ϵ���������

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

    }

    public void OpenDoor()
    {
        if (totalNum == completeNum)
            StartCoroutine(IOpenDoor());
           
    }

    IEnumerator IOpenDoor()
    {
        Tweener t = door.GetComponent<SpriteRenderer>().DOColor(Color.clear, doorDisappearTime);
        t.Kill();
        yield return new WaitForSeconds(doorDisappearTime);
        Destroy(door);
;
    }

}
