using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    public LayerMask iceDectectLayer, normalDetectLayer;
    public float speedOnIce = 0.5f;
    public int wood = 0;
    bool hasAxe = false;


    bool isOnIce = false, canControl = true;

    Vector3 direction;
    Tweener _tweener, _tweenerIce;

    void Start()
    {
        _tweener = this.transform.DOMove(transform.position + direction, 0.1f).SetAutoKill(false);
        _tweenerIce = this.transform.DOMove(transform.position + direction, speedOnIce).SetAutoKill(false);
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.R))
            ButtonManager.instance.ReloadLevel();

        if (Input.GetKeyDown(KeyCode.Escape))
            ButtonManager.instance.LoadLevel("LevelSelect");

        if (Input.GetKeyDown(KeyCode.F))
        {
            print("Interact");
        }


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
                    _tweener.ChangeEndValue(transform.position + direction, true).Play();
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
                    if (coll != null && coll.tag != "ice" && coll.tag != "grass" && CheckMove(direction))
                        _tweener.ChangeEndValue(transform.position + direction, true).Play();
                    else
                        StartCoroutine(SkateOnIce());
                }

                direction = Vector3.zero;

            }


        }


    }

    IEnumerator SkateOnIce()
    {

        canControl = false;

        Collider2D coll = null;
        for (int i = 1; i < 10; i++)
        {
            coll = Physics2D.OverlapCircle(transform.position + i*direction, 0.1f);
            if (coll.tag != "ice")
            {
                //print(coll.name);
                break;
            }
            else
                print("no");
        }

       // RaycastHit2D hit = Physics2D.Raycast(transform.position + direction, direction, 10f, iceDectectLayer);
        if (coll != null)
        {
            float Distance = Vector3.Distance(transform.position, coll.transform.position);
            
            if (coll.tag == "grass")
            {
                isOnIce = false;
                _tweenerIce.ChangeEndValue(coll.transform.position, true).Play();
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
        Scene scene = SceneManager.GetActiveScene();
        if (scene.buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
            ButtonManager.instance.LoadLevel(scene.buildIndex + 1);
        else
            ButtonManager.instance.LoadLevel("LevelSelect");
    }

}
