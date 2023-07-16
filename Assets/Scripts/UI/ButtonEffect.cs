using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float scaleSize = 1.2f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.transform.localScale *= scaleSize;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        this.transform.localScale /= scaleSize;
    }

}
