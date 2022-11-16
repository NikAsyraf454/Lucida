using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScreen : MonoBehaviour
{
    [SerializeField] private Material fullscreenEffect;

    // Start is called before the first frame update
    void Start()
    {
        fullscreenEffect.SetFloat("_FullScreenIntensity", 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerTakeDamage()
    {
        LeanTween.cancel(gameObject);
        fullscreenEffect.SetFloat("_FullScreenIntensity", 0.1f);
        LeanTween.value( gameObject, 0.9f, 0.1f, 3f).setOnUpdate( (float val)=>{
            fullscreenEffect.SetFloat("_VignetteIntensity", val);
            Debug.Log("tweened val:"+val);
        } ).setOnComplete(ClearScreen);
    }

    private void ClearScreen()
    {
        LeanTween.value( gameObject, 0.1f, 0f, 1f).setOnUpdate( (float val)=>{
            fullscreenEffect.SetFloat("_FullScreenIntensity", val);
        } );
    }
}
