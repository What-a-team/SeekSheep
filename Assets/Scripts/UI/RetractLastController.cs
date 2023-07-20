using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class RetractLastController : MonoBehaviour
{
    public static RetractLastController instance { get; private set; }

    public bool canClick;
    public bool ifMoveRock, ifGetAxe, ifGetWood, ifWoodIntoWater, ifDoorOpen, ifGetTorch, ifMovePlayer, ifDestroyWood, ifRockIntoWater;
    public bool isRockIntoPit, ifMoveSheep;
    public PlayerController controller;
    public SheepController sheepController;
    public Button button;


    Vector3 lastPlayerPos, currentPlayerPos;
    Vector3 lastRockPos, currentRocPos;
    Vector3 lastSheepPos, currentSheepPos;
    int rockId;


    public Rock[] rocks = new Rock[20];
    public RockState[] rockStates = new RockState[20];
    public Vector3 axePos, stumpPos, rockPos, torchPos;
    public Water water;
    public Pit pit;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);

        controller = FindObjectOfType<PlayerController>();
        sheepController = FindObjectOfType<SheepController>();

        rocks = FindObjectsOfType<Rock>();

        if (FindObjectOfType<Axe>() != null)
            axePos = FindObjectOfType<Axe>().transform.position;
    }
    private void Start()
    {

        lastPlayerPos = Vector3.zero;
        currentPlayerPos = controller.transform.position;
        lastSheepPos = Vector3.zero;
        currentSheepPos = sheepController.transform.position;



        for (int i = 0; i < rocks.Length; i++)
        {
            rocks[i].id = i;
            rockStates[i].curPos = rocks[i].transform.position;
            rockStates[i].lastPos = rockStates[i].curPos;
        }

    }
    void Update()
    {
   
    }

    public void RetractLastStep()
    {
       
        if (!canClick)
            return;

        if (ifMovePlayer)
        {
            controller.transform.position = lastPlayerPos;
            currentPlayerPos = lastPlayerPos;

            controller.isOnIce = controller.isLastOnIce;
            
        }

        if (ifMoveSheep)
        {
            sheepController.transform.position = lastSheepPos;
            currentSheepPos = lastSheepPos;

        }
        

        // rock
        if (ifMoveRock)
        {
            if (rocks[rockId] != null)
            {
                rocks[rockId].transform.position = rockStates[rockId].lastPos;
                rockStates[rockId].curPos = rockStates[rockId].lastPos;
            }

            ifMoveRock = false;
        }

        if (ifGetAxe)
        {
            UIManager.instance.ReturnAxe();
            controller.hasAxe = false;
            GameObject obj = Instantiate(Resources.Load("Prefabs/axe") as GameObject);
            obj.transform.position = axePos;

        }else if (ifGetWood)
        {
            controller.wood--;
            UIManager.instance.UpdateWoodNum(controller.wood);
            GameObject obj = Instantiate(Resources.Load("Prefabs/stump") as GameObject);
            obj.transform.position = stumpPos;

        }else if (ifWoodIntoWater)
        {
            controller.wood++;
            UIManager.instance.UpdateWoodNum(controller.wood);
            water.DestroyBridge();

        }else if (ifRockIntoWater)
        {
            water.DestroyBridge();
            GameObject obj = Instantiate(Resources.Load("Prefabs/rock") as GameObject);
            obj.transform.position = rockPos;
        }else if (ifGetTorch)
        {
            controller.RemoveTorch(torchPos);
            GameObject obj = Instantiate(Resources.Load("Prefabs/torch") as GameObject);
            obj.transform.position = torchPos;
        }else if (isRockIntoPit)
        {
            pit.UnfillPit();
            GameObject obj = Instantiate(Resources.Load("Prefabs/rock") as GameObject);
            obj.transform.position = rockPos;
        }

        
        if (ifDestroyWood)
        {
            water.BuildBridge(ToolType.Wood);
        }




        canClick = false;
        button.interactable = false;

    }


    public void UpdatePlayerLast()
    {
        canClick = true;
        button.interactable = true;
        lastPlayerPos = currentPlayerPos;

    }



    public void UpdatePlayerCurrent(Vector3 pos, bool isOnIce)
    {
        currentPlayerPos = pos;

    }

    public void UpdateSheepLast()
    {
        lastSheepPos = currentSheepPos;

    }

    public void UpdateSheepCurrent(Vector3 pos)
    {
        currentSheepPos = pos;

    }

    public void UpdateRockLast(int id)
    {
        ifMoveRock = true;
        rockId = id;
        rockStates[id].lastPos = rockStates[id].curPos;
        lastRockPos = rockStates[id].lastPos;
    }

    public void UpdateRockCurrent(int id, Vector3 pos)
    {
        rockStates[id].curPos = pos;
        currentRocPos = pos;
    }



    public void BackToOriginal()
    {
        ifMovePlayer = true;
        ifMoveRock = false;
        ifDoorOpen = false;
        ifGetAxe = false;
        ifGetTorch = false;
        ifGetWood = false;
        ifWoodIntoWater = false;
        ifRockIntoWater = false;
        ifDestroyWood = false;
        ifGetTorch = false;
        isRockIntoPit = false;

        if (sheepController.canCooperation)
            ifMoveSheep = true;
    }

}



[System.Serializable]
public class RockState
{
    public Vector3 lastPos;
    public Vector3 curPos;


}
