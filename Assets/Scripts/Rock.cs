using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public float speed = 0.1f;
    Tweener _tweener;

    void Start()
    {
        _tweener = this.transform.DOMove(transform.position, speed).SetAutoKill(false);

    }



    public bool CheckMove(Vector3 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + direction, direction, 0.1f);
        if (!hit)
        {
            _tweener.ChangeEndValue(transform.position + direction, true).Play();
            return true;
        }
        else
        {
            switch(hit.collider.tag)
            {
                case "tree":
                    return false;
                case "rock":
                    return false;
                case "pit":
                    hit.collider.GetComponent<Pit>().FillPit();
                    MoveAndDestroy(direction);
                    return true;
                case "water":
                    hit.collider.GetComponent<Water>().BuildBridge(ToolType.Stone);
                    MoveAndDestroy(direction);
                    return true;
                case "ice":
                    return false;
                case "axe":
                    return false;
                case "sheep":
                    return false;
                case "grass":
                    _tweener.ChangeEndValue(transform.position + direction, true).Play();
                    return true;
            }
        }
        return false;
    }


    void MoveAndDestroy(Vector3 direction)
    {
        _tweener.ChangeEndValue(transform.position + direction, true).Play();
        _tweener.Kill();
        Destroy(this.gameObject);
    }

}
