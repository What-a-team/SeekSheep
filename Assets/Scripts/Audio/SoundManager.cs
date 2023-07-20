using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }

    [Header("Sound")]
    public List<SoundEffect> sounds = new List<SoundEffect>();

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

    }

    public void SoundPlayOneShot(Soundtype name)
    {
        AudioClip clip = FindSoundEffectByName(name);
        audioSrc.PlayOneShot(clip);
    }


    public AudioClip FindSoundEffectByName(Soundtype name)
    {
        for (int i = 0; i < sounds.Count; i++)
        {
            if (sounds[i].name == name)
            {
                print("[sound]find " + name);
                return sounds[i].clip;
            }
        }
        print("not found!");
        return null;
    }


}


[System.Serializable]
public class SoundEffect
{
    public Soundtype name;
    public AudioClip clip;

}