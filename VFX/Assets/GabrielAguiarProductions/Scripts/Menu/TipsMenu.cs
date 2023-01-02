using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TipsMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] tipPanels;
    [SerializeField] private int panelIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        tipPanels[0].SetActive(true);
        UpdateTextPanelIndex();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextTip()
    {
        tipPanels[panelIndex].SetActive(false);
        panelIndex++;
        tipPanels[panelIndex].SetActive(true);
        UpdateTextPanelIndex();
    }

    public void DoneTips()
    {
        MenuManager.Instance.DoneTips();
        this.gameObject.SetActive(false);
    }

    private void UpdateTextPanelIndex()
    {
        TMP_Text[] text = tipPanels[panelIndex].GetComponentsInChildren<TMP_Text>();
        // foreach(TMP_Text t in text)
        // {
        //     Debug.Log(t.text);
        // }
        text[1].text = (panelIndex+1) + "/" + tipPanels.Length;
    }
}
