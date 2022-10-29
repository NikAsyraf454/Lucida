using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerUI : MonoBehaviour
{
    public static TimerUI Instance;
    public TMP_Text timer;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTimer(float time)
    {
        // float minutes = Mathf.FloorToInt(time / 60);  
        float seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = (int)(time * 100);
        milliseconds = milliseconds % 100;
        // timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        // timer.text = string.Format("{0}:{1}:{2}", minutes, seconds, (int)milliseconds);
        timer.text = string.Format("{0}:{1}", seconds, (int)milliseconds);
    }

    // IEnumerator UpdateTimer(float time)
    // {
    //     while(time > 0f)
    //     {
    //         timer.text = time.ToString("0.0");
    //         time -= Time.deltaTime;
    //         yield return new WaitForSeconds(0f);
    //     }
    // }
}
