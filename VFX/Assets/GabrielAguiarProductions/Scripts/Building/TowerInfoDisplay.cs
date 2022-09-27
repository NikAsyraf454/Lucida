using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerInfoDisplay : MonoBehaviour
{
    private bool isDisplayed;
    [SerializeField] private GameObject temp;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseEnter()
    {
        Debug.Log("tower stats");
    }

    void OnMouseDown()
    {
        if(isDisplayed)
        {
            isDisplayed = false;
            temp.SetActive(false);
        }
        else
        {
            temp.SetActive(true);
            isDisplayed = true;
        }
    }
}
