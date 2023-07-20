using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelManager : MonoBehaviour
{
    public static LevelManager instance { get; private set; }


    public List<Button> buttons;

    public LevelState_ISO iso;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
           // DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        iso = Resources.Load("LevelState_ISO") as LevelState_ISO;
        for (int i = 1; i < iso.levelState.Count; i++)
        {
            if (iso.levelState[i-1] == true)
                buttons[i].interactable = true;

        }
    }
}
