using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    [SerializeField] private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        WaveManager.Instance.bossFight = true;
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDisable()
    {
        // MenuManager.Instance.ButtonSectionUnlocked();
    }

    private void TransitionAnim()
    {
        anim.SetBool("isWalking", true);
    }
}
