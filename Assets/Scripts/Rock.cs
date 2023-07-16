using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public float speed = 0.1f;
    public Sprite rockOnTarget;
    Sprite rockNotTarget;
    Tweener _tweener;
    SpriteRenderer spriteRenderer;
    GameObject rock_light;
    void Start()
    {
        _tweener = this.transform.DOMove(transform.position, speed).SetAutoKill(false);
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        rockNotTarget = spriteRenderer.sprite;
        rock_light = this.transform.GetChild(0).gameObject;
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
                case "target":
                    _tweener.ChangeEndValue(transform.position + direction, true).Play();
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

    public void RockOnTartget()
    {
        spriteRenderer.sprite = rockOnTarget;
        rock_light.SetActive(true);
        TargetsManager.instance.completeNum++;
        TargetsManager.instance.OpenDoor();
    }

    public void RockLeaveTarget()
    {
        spriteRenderer.sprite = rockNotTarget;
        rock_light.SetActive(false);
        TargetsManager.instance.completeNum--;

    }


}
