using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using System.Threading;

public class PlayerController : MonoBehaviour
{
    public LayerMask iceDectectLayer, normalDetectLayer;
    public int step = 0;
    public float speedOnLand = 0.1f;
    public float speedOnIce = 0.5f;
    public bool overallControl = true;

    public int wood = 0;
    bool hasAxe = false;


    public bool isOnIce = false, canControl = true;

    Vector3 direction;
    Tweener _tweener, _tweenerIce;

    LevelState_ISO iso;
    public float timer = 0;

    void Start()
    {
        _tweener = this.transform.DOMove(transform.position + direction, speedOnLand).SetAutoKill(false);
        _tweenerIce = this.transform.DOMove(transform.position + direction, speedOnIce).SetAutoKill(false);
        iso = Resources.Load("LevelState_ISO") as LevelState_ISO;
    }

    // Update is called once per frame
    void Update()
    {
        CoolDown();


        if (!overallControl)
            return;

        /*
        if (Input.GetKeyDown(KeyCode.R))
            ButtonManager.instance.ReloadLevel();

        if (Input.GetKeyDown(KeyCode.Escape))
            ButtonManager.instance.LoadLevel("LevelSelect");
        */
        GetComponent<SpriteRenderer>().sortingOrder = (int)-transform.position.y;


        if (!isOnIce)
        {
            if (Input.GetKeyDown(KeyCode.W))
                direction = Vector3.up;
            else if (Input.GetKeyDown(KeyCode.S))
                direction = Vector3.down;
            else if (Input.GetKeyDown(KeyCode.A))
                direction = Vector3.left;
            else if (Input.GetKeyDown(KeyCode.D))
                direction = Vector3.right;

            if (direction != Vector3.zero)
            {
                if (CheckMove(direction))
                {
                    _tweener.ChangeEndValue(transform.position + direction, true).Play();

                    overallControl = false;
                    step++;
                }
            }

            direction = Vector3.zero;

        }
        else
        {
            if (canControl)
            {
                if (Input.GetKeyDown(KeyCode.W))
                    direction = Vector3.up;
                else if (Input.GetKeyDown(KeyCode.S))
                    direction = Vector3.down;
                else if (Input.GetKeyDown(KeyCode.A))
                    direction = Vector3.left;
                else if (Input.GetKeyDown(KeyCode.D))
                    direction = Vector3.right;

                if (direction != Vector3.zero)
                {
                    Collider2D coll = Physics2D.OverlapCircle(transform.position + direction, 0.1f);
                   // if (coll != null && coll.tag == "rock")
                   //     coll.GetComponent<Rock>().IceCheckMove(direction);
                    if (coll != null && coll.tag != "ice" && coll.tag != "grass" && CheckMove(direction))
                    {
                        _tweener.ChangeEndValue(transform.position + direction, true).Play();

                        overallControl = false;
                        step++;
                    }
                    else
                    {
                        StartCoroutine(SkateOnIce());

                        step++;
                    }

                }

                direction = Vector3.zero;

            }


        }


    }

    IEnumerator SkateOnIce()
    {

        canControl = false;

        Collider2D coll = null;
        for (int i = 1; i < 20; i++)
        {
            coll = Physics2D.OverlapCircle(transform.position + i*direction, 0.1f);
            if (coll.tag != "ice")
            {
                //print(coll.name);
                break;
            }
        }

       // RaycastHit2D hit = Physics2D.Raycast(transform.position + direction, direction, 10f, iceDectectLayer);
        if (coll != null)
        {
            
            if (coll.tag == "grass")
            {
                isOnIce = false;
                _tweenerIce.ChangeEndValue(coll.transform.position, true).Play();
            }else if (coll.tag == "rock")
            {
                 _tweenerIce.ChangeEndValue(coll.transform.position - direction, true).Play();
               //  coll.GetComponent<Rock>().IceCheckMove(direction);
                 StartCoroutine(PushRockOnIce(coll, direction));
            }
            else
                _tweenerIce.ChangeEndValue(coll.transform.position - direction, true).Play();
        } 
        else
        {
            print("null");
        }

        yield return new WaitForSeconds(speedOnIce);
        canControl = true;

    }

    IEnumerator PushRockOnIce(Collider2D coll, Vector3 direction)
    {
        overallControl = false;
        yield return new WaitForSeconds(speedOnIce);
        overallControl = true;
        coll.GetComponent<Rock>().CheckMove(direction);
    }


    bool CheckMove(Vector3 direction)
    {
        Collider2D coll = Physics2D.OverlapCircle(transform.position + direction, 0.1f);
       // RaycastHit2D coll = Physics2D.Raycast(transform.position + direction, direction, 0.1f, normalDetectLayer);

        if (!coll)
            return true;
        else
        {
            switch(coll.tag)
            {
                case "rock":  // push the rock
                    return coll.GetComponent<Rock>().CheckMove(direction);
                case "tree":
                    return false;
                case "pit":  // pit cannot pass
                    return false;
                case "water":
                    if (wood > 0)
                    {
                        wood--;
                        UIManager.instance.UpdateWoodNum(wood);
                        coll.GetComponent<Water>().BuildBridge(ToolType.Wood);
                    }
                    return false;
                case "ice":
                    isOnIce = true;
                    return true;
                case "torch":
                    GetTorch(coll);
                    return true;
                case "axe":
                    UIManager.instance.GetAxe();
                    Destroy(coll.gameObject);
                    hasAxe = true;
                    return true;
                case "stump":
                    if (hasAxe)
                    {
                        wood++;
                        UIManager.instance.UpdateWoodNum(wood);
                        Destroy(coll.gameObject);
                        return true;
                    }
                    return false;
                case "target":
                    return true;
                case "sheep":
                    FoundSheep();
                    return false;
                case "fakeTree":
                    return true;
                case "grass":
                    return true;

            }
                    
        }


        return false;
    }




    


    void GetTorch(Collider2D coll)
    {
        Light2D light = transform.GetChild(1).GetComponent<Light2D>();
        DOTween.To(() => light.pointLightOuterRadius, x => light.pointLightOuterRadius = x, 3f, 2f);
        transform.GetChild(0).gameObject.SetActive(true);
        Destroy(coll.gameObject);
    }

    void FoundSheep()
    {
        print("sheep");
        NextLevelButton.instance.LoadNextLevel();
    }

    void CoolDown()
    {
        if (!overallControl && timer <= speedOnLand)
            timer += Time.deltaTime;
        else if (timer > speedOnLand)
        {
            overallControl = true;
            timer = 0;
        }
    }

}
