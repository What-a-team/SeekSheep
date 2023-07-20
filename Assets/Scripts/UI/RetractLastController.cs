using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using UnityEngine.UI;

public class RetractLastController : MonoBehaviour
{
    public static RetractLastController instance { get; private set; }

    public bool canClick;
    public PlayerController controller;
    public Button button;
    public Vector3 lastPlayerPos, currentPlayerPos;


    public Rock[] rocks = new Rock[10];
    public RockState[] rockStates = new RockState[10];


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);

    }
    private void Start()
    {

        lastPlayerPos = Vector3.zero;
        currentPlayerPos = controller.transform.position;

        rocks = FindObjectsOfType<Rock>();
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
        // player
        controller.transform.position = lastPlayerPos;
        currentPlayerPos = lastPlayerPos;

        // rock
        for (int i = 0; i < rocks.Length; i++)
        {
            if (rocks[i] != null)
            {
                rocks[i].transform.position = rockStates[i].lastPos;
                rockStates[i].curPos = rockStates[i].lastPos;
            }
        }



        canClick = false;
        button.interactable = false;

    }


    public void UpdatePlayerLast()
    {
        ES3AutoSaveMgr.Current.Save();
        canClick = true;
        button.interactable = true;
        lastPlayerPos = currentPlayerPos;
    }

    public void UpdateRockLast(int id)
    {
        rockStates[id].lastPos = rockStates[id].curPos;
    }


}



[System.Serializable]
public class RockState
{
    public Vector3 lastPos;
    public Vector3 curPos;


}
