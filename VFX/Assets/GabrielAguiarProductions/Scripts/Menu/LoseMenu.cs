using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class LoseMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text wave;

    // Start is called before the first frame update
    void Start()
    {
        wave.text = "Wave: " + WaveManager.Instance.waveIndex;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoseGame()
    {

    }

    //show score, level or wave, calculate total score in the end, (multiplier and etc))
}
