using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    public static EffectController Instance;
    [SerializeField] private AudioClip buttonHoverClip, buttonClickClip, placeTowerClip, winClip, loseClip;

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

    public void PlaceTowerSound()
    {
        SoundManager.Instance.PlaySound(placeTowerClip);
    }

    public void WinMenuSound()
    {
        SoundManager.Instance.PlaySound(winClip);
    }

    public void LoseMenuSound()
    {
        SoundManager.Instance.PlaySound(loseClip);
    }
}
