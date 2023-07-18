using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class NextLevelButton : MonoBehaviour
{
    public static NextLevelButton instance { get; private set; }
    public GameObject panelBetweenevel;
    public Button nextLevelButton;
    LevelState_ISO iso;
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
        UIManager.instance.UpdateDialogAndShow("complete", 2f);
        panelBetweenevel.SetActive(true);



        if (SceneManager.GetActiveScene().buildIndex + 1 >= SceneManager.sceneCountInBuildSettings)
        {
            nextLevelButton.interactable = false;
        }
    }

    public void NextButton()
    {
        UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();

        if (scene.buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            iso.levelState[scene.buildIndex - 2] = true;  // 去掉主菜单和选关界面

            ButtonManager.instance.LoadLevel(scene.buildIndex + 1);
        }
        else
        {
            //ButtonManager.instance.LoadLevel("LevelSelect");
            print("final!");
        }
    }

}
