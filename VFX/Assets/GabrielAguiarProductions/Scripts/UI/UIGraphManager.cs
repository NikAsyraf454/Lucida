using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGraphManager : MonoBehaviour
{
    public static UIGraphManager Instance;
    public UIGridRenderer uIGridRenderer;
    public UILineRenderer uILineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        CSVToPoints();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("CSVtoPoints")]
    public void CSVToPoints()
    {
        uILineRenderer.points.Clear();
        uILineRenderer.gameObject.SetActive(false);

        int i, max = 0;
        int[] arr = MenuManager.Instance.GetCSVColumn(3);

        for(i = 0; i<arr.Length; i++)
        {
            uILineRenderer.points.Add(new Vector2(i, arr[i]/10)); // divide 10 beause its too big, for health dont divide
            if(max < arr[i]) { max = arr[i]; }
            Debug.Log(arr[i]);
        }

        if(max > 40) { max = max/10;}
        uIGridRenderer.gridSize = new Vector2Int(i+1, max+1);

        uILineRenderer.gameObject.SetActive(true);
    }
}
