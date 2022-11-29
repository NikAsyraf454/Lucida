using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playVFX : MonoBehaviour
{
    private ParticleSystem[] ps;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponentsInChildren<ParticleSystem>();
        PlayParticleSystem();
    }

    void OnEnable()
    {
        PlayParticleSystem();
    }

    void OnDisable()
    {
        foreach(ParticleSystem p in ps)
        {
            p.Stop(true);
        }
    }

    private void PlayParticleSystem()
    {
        foreach(ParticleSystem p in ps)
        {
            p.Play();
        }
    }
}
