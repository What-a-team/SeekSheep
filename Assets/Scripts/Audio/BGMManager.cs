using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGMManager : MonoBehaviour
{
    public static BGMManager instance { get; private set; }

    [Header("Music")]
    public List<BGM> bgms = new List<BGM>();


    AudioSource audioSrc;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);

        audioSrc = GetComponent<AudioSource>();
        audioSrc.clip = FindBGMByName(BGMtype.main);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            audioSrc.mute = !audioSrc.mute;
        }

    }



   
  


    public AudioClip FindBGMByName(BGMtype name)
    {
        for (int i = 0; i < bgms.Count; i++)
        {
            if (bgms[i].name == name)
            {
                print("[bgm]find " + name);
                return bgms[i].clip;
            }
        }
        print("not found!");
        return null;
    }

}


[System.Serializable]
public class BGM
{
    public BGMtype name;
    public AudioClip clip;

}
