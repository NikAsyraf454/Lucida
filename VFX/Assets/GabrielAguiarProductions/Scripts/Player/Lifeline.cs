using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifeline : MonoBehaviour
{
    [SerializeField] private bool isTriggered = false;
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
            return temp;
        }
        else
        {
            Debug.Log("Lifeline denied");
            return 0;
        }


    }
}
