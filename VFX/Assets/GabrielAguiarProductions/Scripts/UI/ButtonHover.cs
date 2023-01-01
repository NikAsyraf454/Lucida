using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Vector3 originalPos;
    [SerializeField] private float xPosAdd;

    // Start is called before the first frame update
    void Start()
    {
        originalPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Debug.Log("enter");
        this.transform.position = new Vector3(originalPos.x + xPosAdd, originalPos.y, originalPos.z);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Debug.Log("exit");
        this.transform.position = originalPos;
    }
}
