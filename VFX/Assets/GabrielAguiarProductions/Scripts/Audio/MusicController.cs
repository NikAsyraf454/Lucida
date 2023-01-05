using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public static MusicController Instance;
    [SerializeField] private AudioClip bgM_MainMenu, bgM_clip1, bgM_clip2;
    void Awake()
    {
        Instance = this;  
    }

    void Start()
    {
        PlayMainMenuBgM();
    }

    public void PlayMainMenuBgM()
    {
        SoundManager.Instance.PlayMusic(bgM_MainMenu);
    }

    public void PlayLevelBgM()
    {
        int i = UnityEngine.Random.Range(1,3);
        switch(i)
        {
            case 1:
                SoundManager.Instance.PlayMusic(bgM_clip1);
                break;
            case 2:
                SoundManager.Instance.PlayMusic(bgM_clip2);
                break;
        }
    }
}
