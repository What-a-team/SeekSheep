using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaoKe : MonoBehaviour
{
    public Sprite stoneinwater;
    public float moveY = 50f;
    public RectTransform UI;
    Tweener _tweener;
    bool hasTriggered = false;

    private void Awake()
    {
        _tweener = UI.DOLocalMove(UI.localPosition, 0.5f).SetAutoKill(false);
    }

    private void Update()
    {
        if (!hasTriggered && GetComponent<SpriteRenderer>().sprite == stoneinwater)
        {
            hasTriggered = true;
            StartCoroutine(UIShowUp());
        }
    }

    IEnumerator UIShowUp()
    {
        _tweener.ChangeEndValue(UI.localPosition+new Vector3(0, -moveY, 0), true).Play();
        yield return new WaitForSeconds(1f);
        _tweener.ChangeEndValue(UI.localPosition+new Vector3(0, moveY, 0), true).Play();
    }
}
