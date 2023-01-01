using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WinMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text wave, score;
            
    // Start is called before the first frame update
    void Start()
    {
        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateText()
    {
        wave.text = "Wave: " + WaveManager.Instance.waveIndex;
        score.text = "Score: " + PlayerManager.Instance.currentPlayerScore;
    }
}
