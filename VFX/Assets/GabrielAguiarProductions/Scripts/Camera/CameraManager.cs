using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    public bool isIsometric = false;
    public Camera[] cameras;
    public PathManager pathManager;
    // [SerializeField] private Vector2 maxPosition;
    // [SerializeField] private Vector2 minPosition;
    [SerializeField] private Vector2 zoomClamp;

    public float panSpeed = 18;         //18 seems like a good middle point, try raycast for a more accurate camera movement
    private Vector3 oldPos, panOrigin;
    Vector3 pos = new Vector3(200, 200, 0);

    public float targetZoom;
    [SerializeField] private float zoomFactor = 3f;
    [SerializeField] private float zoomSmoothSpeed = 10f;
    private float velocity = 0f;

    // Start is called before the first frame update
    void Start()
    {
        pathManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PathManager>();
        
        if(isIsometric)
        {
            targetZoom = Camera.main.orthographicSize;
            cameras[1].orthographicSize = cameras[0].orthographicSize;
        }
        else
        {
            cameras[0].transform.localPosition = new Vector3(0,0,-10);
            targetZoom = -cameras[0].transform.localPosition.z;
        }

    }

    void LateUpdate()
    {
        if(PauseMenu.isPaused || MenuManager.Instance.gameEnded){ return; }
            UpdateCameraPosition();
            UpdateCameraZoom();
    }

    public void UpdateCameraPosition()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //dragOrigin = Input.mousePosition;
            oldPos = transform.position;
            panOrigin = Camera.main.ScreenToViewportPoint(Input.mousePosition);    
            return;
        }

        if (!Input.GetMouseButton(1)) return;
 

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition) - panOrigin;    //Get the difference between where the mouse clicked and where it moved
        
        if(isIsometric)
        {
            transform.position = oldPos + -pos * panSpeed;  
        }
        else
        {
            Vector3 temp = new Vector3(pos.x,0,pos.y);
            transform.position = oldPos + -temp * panSpeed; 
        }
    }

    public void UpdateCameraZoom()
    {
        float scrollData;
        scrollData = Mouse.current.scroll.ReadValue().normalized.y;

        targetZoom -= scrollData * zoomFactor;
        targetZoom = Mathf.Clamp(targetZoom, zoomClamp.x, zoomClamp.y);
        if(isIsometric)
        {
            cameras[0].orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, targetZoom, ref velocity, Time.deltaTime* zoomSmoothSpeed);
            cameras[1].orthographicSize = cameras[0].orthographicSize;
        }
        else
        {
            cameras[0].transform.localPosition = new Vector3(0,0,-targetZoom);
        }

    }

    public void OnLeftClick(InputValue value)
    {
        GetNodeAvailability();
        // Debug.Log("shooting ray..." + Mouse.current.position.ReadValue());
    }

    private void GetNodeAvailability()
    {
        // Vector3 mousePos = Mouse.current.position.ReadValue();   
        // mousePos.z=Camera.main.nearClipPlane;
        // Vector3 Worldpos=Camera.main.ScreenToWorldPoint(mousePos);  

        // Ray ray = new Ray(position, Vector3.down);
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        // Debug.DrawRay(ray.origin, ray.direction * 20, Color.green);
        // Debug.Log("shooting ray..." + Mouse.current.position.ReadValue());
        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            if(!hit.collider.isTrigger) { return; }

            if(hit.collider.gameObject.tag == "Tower")
            {
                TowerInfoDisplay towerInfoDisplay =  hit.collider.gameObject.GetComponent<TowerInfoDisplay>();
                towerInfoDisplay.DisplayTowerInfo();
            }
        }
        
    }
    
}
