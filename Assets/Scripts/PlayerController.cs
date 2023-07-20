using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    public LayerMask iceDectectLayer, normalDetectLayer;
    public int step = 0;
    public float speedOnLand = 0.1f;
    public float speedOnIce = 0.5f;

    public bool overallControl = true;
    public int wood = 0;
    public bool hasAxe = false;
    bool hasCoolDown = true;

    public bool isOnIce = false, canControl = true;
    public bool isLastOnIce = false;


    public SheepController sheepController;

    Vector3 direction;
    Tweener _tweener, _tweenerIce;

    float timer = 0;

    public bool hasFindSheep = false;

    void Start()
    {
        _tweener = this.transform.DOMove(transform.position + direction, speedOnLand).SetAutoKill(false);
        _tweenerIce = this.transform.DOMove(transform.position + direction, speedOnIce).SetAutoKill(false);


        sheepController = FindObjectOfType<SheepController>();

        isLastOnIce = isOnIce;
    }

    // Update is called once per frame
    void Update()
    {
        CoolDown();

        

        if (!overallControl || !hasCoolDown)
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
                RetractLastController.instance.BackToOriginal();

                if (CheckMove(direction, this.transform))
                {
                    RetractLastController.instance.UpdatePlayerLast();
                    _tweener.ChangeEndValue(transform.position + direction, true).Play();
                    RetractLastController.instance.UpdatePlayerCurrent(transform.position+direction, isOnIce);


                    if (SoundManager.instance != null)
                        SoundManager.instance.SoundPlayOneShot(Soundtype.step);



                    hasCoolDown = false;
                    step++;
                }
                if (CheckMove(-direction, sheepController.transform))
                {
                    sheepController.CooperateWithPlayer(-direction);  // opposite direction
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

                    RetractLastController.instance.BackToOriginal();


                    Collider2D coll = Physics2D.OverlapCircle(transform.position + direction, 0.1f);


                    if (coll != null && coll.tag == "water")
                    {
                        Debug.Log("water!");
                        if (wood > 0)
                        {
                            RetractLastController.instance.ifMovePlayer = false;
                            RetractLastController.instance.ifMoveSheep = false;
                            RetractLastController.instance.ifWoodIntoWater = true;
                            RetractLastController.instance.water = coll.GetComponent<Water>();
                            RetractLastController.instance.canClick = true;
                            RetractLastController.instance.button.interactable = true;

                            wood--;
                            UIManager.instance.UpdateWoodNum(wood);
                            coll.GetComponent<Water>().BuildBridge(ToolType.Wood);
                        }
                        direction = Vector3.zero;
                        return;
                    }
                    else if (coll != null && coll.tag != "ice" && coll.tag != "grass" && CheckMove(direction, this.transform))
                    {
                        RetractLastController.instance.UpdatePlayerLast();
                        _tweenerIce.ChangeEndValue(transform.position + direction, true).Play();
                        RetractLastController.instance.UpdatePlayerCurrent(transform.position + direction, isOnIce);

                        hasCoolDown = false;
                        step++;
                    }
                    else
                    {
                        Debug.Log("why!");
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
            coll = Physics2D.OverlapCircle(transform.position + i * direction, 0.1f);
            if (coll.tag != "ice")
            {
                //print(coll.name);
                break;
            }
        }

        // RaycastHit2D hit = Physics2D.Raycast(transform.position + direction, direction, 10f, iceDectectLayer);
        if (coll != null)
        {
            isLastOnIce = isOnIce;

            if (coll.tag == "grass" || coll.tag == "target")
            {
                isOnIce = false;

                RetractLastController.instance.UpdatePlayerLast();
                _tweenerIce.ChangeEndValue(coll.transform.position, true).Play();

                RetractLastController.instance.UpdatePlayerCurrent(coll.transform.position, isOnIce);


                //_tweenerIce.ChangeEndValue(coll.transform.position, true).Play();
            }
            else if (coll.tag == "rock")
            {
                RetractLastController.instance.UpdatePlayerLast();
                _tweenerIce.ChangeEndValue(coll.transform.position - direction, true).Play();
                RetractLastController.instance.UpdatePlayerCurrent(coll.transform.position - direction, isOnIce);

                //_tweenerIce.ChangeEndValue(coll.transform.position - direction, true).Play();
                //  coll.GetComponent<Rock>().IceCheckMove(direction);
                StartCoroutine(PushRockOnIce(coll, direction));
            }
            else
            {
                RetractLastController.instance.UpdatePlayerLast();
                _tweenerIce.ChangeEndValue(coll.transform.position - direction, true).Play();
                RetractLastController.instance.UpdatePlayerCurrent(coll.transform.position - direction, isOnIce);
                //_tweenerIce.ChangeEndValue(coll.transform.position - direction, true).Play();
            }
                
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
        hasCoolDown = false;
        yield return new WaitForSeconds(speedOnIce);
        hasCoolDown = true;
        coll.GetComponent<Rock>().CheckMove(direction);

    }


    bool CheckMove(Vector3 direction, Transform transform)
    {
        Collider2D coll = Physics2D.OverlapCircle(transform.position + direction, 0.1f);
        // RaycastHit2D coll = Physics2D.Raycast(transform.position + direction, direction, 0.1f, normalDetectLayer);

        if (!coll)
            return true;
        else
        {
            switch (coll.tag)
            {
                case "rock":  // push the rock
                    return coll.GetComponent<Rock>().CheckMove(direction);
                case "tree":
                    return false;
                case "pit":  // pit cannot pass
                    return false;
                case "water":
                    if (wood > 0 && transform == this.transform)
                    {
                        RetractLastController.instance.ifMovePlayer = false;
                        RetractLastController.instance.ifMoveSheep = false;
                        RetractLastController.instance.ifWoodIntoWater = true;
                        RetractLastController.instance.water = coll.GetComponent<Water>();
                        RetractLastController.instance.canClick = true;
                        RetractLastController.instance.button.interactable = true;

                        wood--;
                        UIManager.instance.UpdateWoodNum(wood);
                        coll.GetComponent<Water>().BuildBridge(ToolType.Wood);
                    }
                    return false;
                case "ice":
                    isLastOnIce = isOnIce;
                    isOnIce = true;


                    return true;
                case "torch":
                    RetractLastController.instance.ifGetTorch = true;
                    RetractLastController.instance.torchPos = transform.position + direction;

                    GetTorch(coll);
                    return true;
                case "axe":
                    RetractLastController.instance.ifGetAxe = true;

                    UIManager.instance.GetAxe();
                    Destroy(coll.gameObject);
                    hasAxe = true;
                    return true;
                case "stump":
                    if (hasAxe && transform == this.transform)
                    {
                        RetractLastController.instance.ifGetWood = true;
                        RetractLastController.instance.stumpPos = transform.position+direction;

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

    public void RemoveTorch(Vector3 pos)
    {
        Light2D light = transform.GetChild(1).GetComponent<Light2D>();
        DOTween.To(() => light.pointLightOuterRadius, x => light.pointLightOuterRadius = x, 1.8f, 2f);
        transform.GetChild(0).gameObject.SetActive(false);

    }


    void FoundSheep()
    {
        hasFindSheep = true;
        print("sheep");
        NextLevelButton.instance.LoadNextLevel();
        overallControl = false;
    }

    void CoolDown()
    {
        if (!hasCoolDown && timer <= speedOnLand)
            timer += Time.deltaTime;
        else if (timer > speedOnLand)
        {
            hasCoolDown = true;
            timer = 0;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "sheep" && !hasFindSheep && sheepController.canCooperation)
            FoundSheep();

        
    }
}




