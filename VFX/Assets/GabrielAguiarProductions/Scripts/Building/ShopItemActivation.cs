using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemActivation : MonoBehaviour
{
    [SerializeField] private GameObject[] defaultElement;
    [SerializeField] private GameObject[] cancelElement;

    public void ButtonActivated()
    {
        foreach(GameObject g in defaultElement)
        {
            g.SetActive(false);
        }
        foreach(GameObject g in cancelElement)
        {
            g.SetActive(true);
        }
    }

    public void ButtonDeactivated()
    {
        foreach(GameObject g in cancelElement)
        {
            g.SetActive(false);
        }
        foreach(GameObject g in defaultElement)
        {
            g.SetActive(true);
        }
    }
}
