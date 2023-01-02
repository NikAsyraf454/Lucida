using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifeline : MonoBehaviour
{
    [SerializeField] private bool isTriggered = false;
    [SerializeField] private GameObject lifelineMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GaveLifeline()
    {
        // UnityEngine.Random.InitState(randomSeed);
        // randomSeed++;
        int i = UnityEngine.Random.Range(1,2);
        // int i = 1;
        if(i == 1)
        {
            int temp = UnityEngine.Random.Range(2,5);
            Debug.Log("Lifeline intiatied: " + temp);
            WaveManager.Instance.EliminateEnemy();
            PauseMenu.Instance.HaltGame();
            LifeLineMenu();
            return temp;
        }
        else
        {
            Debug.Log("Lifeline denied");
            return 0;
        }
    }

    public void LifeLineMenu()
    {
        lifelineMenu.SetActive(true);
    }

    public void ContinueAfterLifeline()
    {
        WaveManager.Instance.SetTimer(10f);
        PauseMenu.Instance.ResumeGame();
        lifelineMenu.SetActive(false);

    }
}
