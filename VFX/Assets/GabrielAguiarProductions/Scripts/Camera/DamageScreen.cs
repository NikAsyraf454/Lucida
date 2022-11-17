using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScreen : MonoBehaviour
{
    [SerializeField] private Material fullscreenEffect;
    [SerializeField] private float defaultFullScreenIntensity = 0.08f;
    private float _fullScreenIntensity;

    // Start is called before the first frame update
    void Start()
    {
        fullscreenEffect.SetFloat("_FullScreenIntensity", 0f);
        _fullScreenIntensity = defaultFullScreenIntensity;
    }

    private void OnDisable()
    {
        fullscreenEffect.SetFloat("_FullScreenIntensity", 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerTakeDamage()
    {
        defaultFullScreenIntensity += 0.02f;
        _fullScreenIntensity = defaultFullScreenIntensity;
        LeanTween.cancel(gameObject);
        fullscreenEffect.SetFloat("_FullScreenIntensity", _fullScreenIntensity);
        LeanTween.value( gameObject, 0.9f, 0.1f, 3f).setOnUpdate( (float val)=>{
            fullscreenEffect.SetFloat("_VignetteIntensity", val);
        } ).setOnComplete(ClearScreen);

    }

    private void ClearScreen()
    {
        LeanTween.value( gameObject, _fullScreenIntensity, 0f, 1f).setOnUpdate( (float val)=>{
            fullscreenEffect.SetFloat("_FullScreenIntensity", val);
        } );
    }
}
