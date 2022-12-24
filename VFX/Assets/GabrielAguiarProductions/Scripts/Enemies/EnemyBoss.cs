using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        WaveManager.Instance.bossFight = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDisable()
    {
        // MenuManager.Instance.ButtonSectionUnlocked();
    }
}
