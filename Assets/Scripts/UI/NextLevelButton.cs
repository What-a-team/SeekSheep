using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class NextLevelButton : MonoBehaviour
{
    public static NextLevelButton instance { get; private set; }
    public GameObject panelBetweenevel;
    public Text sheepName;
    public Button nextLevelButton;
    public float dialogStayTime = 2f;
    public LevelState_ISO iso;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);

        iso = Resources.Load("LevelState_ISO") as LevelState_ISO;
    }
    public void LoadNextLevel()
    {
        UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();

        iso.levelState[scene.buildIndex - 2] = true;  // 去掉主菜单和选关界面

        if (iso.Dialogs[scene.buildIndex - 2].findSheepDialog != "null")
            StartCoroutine(DialogAndPanel(scene.buildIndex - 2));
        else
            OnlyPanel(scene.buildIndex - 2);


        if (SceneManager.GetActiveScene().buildIndex + 1 >= SceneManager.sceneCountInBuildSettings)
        {
            nextLevelButton.interactable = false;
        }
    }

    IEnumerator DialogAndPanel(int level)
    {
        UIManager.instance.UpdateDialogAndShow(iso.Dialogs[level].findSheepDialog, dialogStayTime);
        yield return new WaitForSeconds(dialogStayTime);


        panelBetweenevel.SetActive(true);
        sheepName.text = iso.Dialogs[level].name;
    }


    public void OnlyPanel(int level)
    {
        
        panelBetweenevel.SetActive(true);
        sheepName.text = iso.Dialogs[level].name;
    }


    public void NextButton()
    {
        UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();

        if (scene.buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            

            ButtonManager.instance.LoadLevel(scene.buildIndex + 1);
        }
        else
        {
            //ButtonManager.instance.LoadLevel("LevelSelect");
            print("final!");
        }
    }

}
