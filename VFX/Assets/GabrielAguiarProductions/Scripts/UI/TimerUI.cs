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
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTimer(float time)
    {
        // StartCoroutine(UpdateTimer(time));
        timer.text = time.ToString("0.0");
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
