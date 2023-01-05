using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    public static EffectController Instance;
    [SerializeField] private AudioClip buttonHoverClip, buttonClickClip;

    void Awake()
    {
        Instance = this;  
    }

    public void ButtonHoverSound()
    {
        SoundManager.Instance.PlaySound(buttonHoverClip);
    }

    public void ButtonClickSound()
    {
        SoundManager.Instance.PlaySound(buttonClickClip);
    }

}
