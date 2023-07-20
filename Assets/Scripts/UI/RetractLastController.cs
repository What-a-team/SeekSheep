using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using UnityEngine.UI;

public class RetractLastController : MonoBehaviour
{
    public static RetractLastController instance { get; private set; }

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
    // Update is called once per frame
    void Update()
    {
        
    }
}


public class StateChangeBools
{
    public bool rock;
    public bool pit;
    public bool water;
    public bool stump;
    public bool torch;
    public bool axe;
    public bool target;
    public bool door;
}
