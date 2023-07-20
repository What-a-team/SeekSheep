using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public LayerMask iceDectectLayer, normalDetectLayer;
    public float speedOnIce = 0.3f;
    public float speed = 0.1f;
    public Sprite rockOnTarget;

    Sprite rockNotTarget;
    Tweener _tweener, _tweenerIce;

    SpriteRenderer spriteRenderer;
    GameObject rock_light;

    [Header("DuiYin-targetsmanager")]
    public TargetsManager targetsManager;

    void Start()
    {
        _tweener = this.transform.DOMove(transform.position, speed).SetAutoKill(false);
        _tweenerIce = this.transform.DOMove(transform.position, speedOnIce).SetAutoKill(false);

        spriteRenderer = this.GetComponent<SpriteRenderer>();
        rockNotTarget = spriteRenderer.sprite;
        rock_light = this.transform.GetChild(0).gameObject;


        targetsManager = this.GetComponentInParent<TargetsManager>();

    }


    private void Update()
    {
        spriteRenderer.sortingOrder = (int)-transform.position.y;
    }


    public bool CheckMove(Vector3 direction)
    {
        Collider2D coll = Physics2D.OverlapCircle(transform.position + direction, 0.1f);
        //RaycastHit2D hit = Physics2D.Raycast(transform.position + direction, direction, 0.1f);
        if (!coll)
        {
            _tweener.ChangeEndValue(transform.position + direction, true).Play();
            return true;
        }
        else
        {
            switch(coll.tag)
            {
                case "tree":
                    return false;
                case "rock":
                    return false;
                case "pit":
                    coll.GetComponent<Pit>().FillPit();
                    MoveAndDestroy(direction);
                    return true;
                case "water":
                    coll.GetComponent<Water>().BuildBridge(ToolType.Stone);
                    MoveAndDestroy(direction);
                    return true;
                case "target":
                    _tweener.ChangeEndValue(transform.position + direction, true).Play();
                    return true;
                case "ice":  // 石头可以在冰上推一步
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.2f, iceDectectLayer);
                    if (hit.transform.tag == "grass")
                        return false;
                    else
                        _tweener.ChangeEndValue(transform.position + direction, true).Play();
                    return true;
                case "axe":
                    return false;
                case "fakeTree":
                    _tweener.ChangeEndValue(transform.position + direction, true).Play();
                    return true;
                case "sheep":
                    return false;
                case "grass":
                    _tweener.ChangeEndValue(transform.position + direction, true).Play();
                    return true;
            }
        }
        return false;
    }


    public void IceCheckMove(Vector3 direction)
    {
        Collider2D coll = null;
        int i;
        for (i = 1; i < 20; i++)
        {
            coll = Physics2D.OverlapCircle(transform.position + i * direction, 0.1f);
            if (coll != null && coll.tag != "ice")
            {
                print("[rock]:"+coll.name);
                break;
            }
                
        }

        //RaycastHit2D hit = Physics2D.Raycast(transform.position + direction, direction, 0.1f);
        if (!coll)
        {
            _tweener.ChangeEndValue(transform.position + direction, true).Play();
            print("rock-no");
            return;
        }
        else
        {
            switch (coll.tag)
            {
                case "ice-obstacle":
                    _tweenerIce.ChangeEndValue(transform.position + (i - 1) * direction, true).Play();
                    return;
                case "rock":
                    StartCoroutine(SkateOnIce(direction, (i - 1) * direction, coll));
                    //_tweenerIce.ChangeEndValue(transform.position + (i - 1) * direction, true).Play();
                    //coll.GetComponent<Rock>().IceCheckMove(direction);
                    return;
                case "tree":
                    _tweenerIce.ChangeEndValue(transform.position + (i - 1) * direction, true).Play();
                    return;
                case "ice":
                    return;
                case "pit":
                    coll.GetComponent<Pit>().FillPit();
                    MoveAndDestroy(i * direction);
                    return;
                case "water":
                    coll.GetComponent<Water>().BuildBridge(ToolType.Stone);
                    MoveAndDestroy(i * direction);
                    return;
                case "target":
                    _tweenerIce.ChangeEndValue(transform.position + i*direction, true).Play();
                    return;
                case "sheep":
                    return;
                case "grass":
                    _tweenerIce.ChangeEndValue(transform.position + (i-1)*direction, true).Play();
                    return;
            }
        }
        return;
    }



    IEnumerator SkateOnIce(Vector3 direction, Vector3 distance, Collider2D coll)
    {
        _tweenerIce.ChangeEndValue(transform.position + distance, true).Play();
        yield return new WaitForSeconds(speedOnIce);

        coll.GetComponent<Rock>().IceCheckMove(direction);

    }



    void MoveAndDestroy(Vector3 direction)
    {
        _tweener.ChangeEndValue(transform.position + direction, true).Play();
        _tweener.Kill();
        Destroy(this.gameObject);
    }

    public void RockOnTartget()
    {

        targetsManager.completeNum++;
        targetsManager.OpenDoor();
    }
    public void RockStayOnTarget()
    {
        spriteRenderer.sprite = rockOnTarget;
        rock_light.SetActive(true);
    }

    public void RockLeaveTarget()
    {
        spriteRenderer.sprite = rockNotTarget;
        rock_light.SetActive(false);
        targetsManager.completeNum--;

    }


}
