using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGraphManager :  MonoBehaviour
{
    public static UIGraphManager Instance;
    public UIGridRenderer uIGridRenderer;
    public UILineRenderer healthLineRenderer;
    public UILineRenderer levelLineRenderer;
    public UILineRenderer scoreLineRenderer;
    private Vector2Int gridSize;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        HealthGraphMenu();
    }

    // Update is called once per frame
    void Update()
    {
        // if(uIGridRenderer != null)
        // {
        //     if(gridSize != uIGridRenderer.gridSize)
        //     {
        //         gridSize = uIGridRenderer.gridSize;
        //         SetVerticesDirty();
        //     }
        // }
    }

    public void HealthGraphMenu()
    {
        CSVToPoints(healthLineRenderer, 1);
        levelLineRenderer.gameObject.SetActive(false);
        scoreLineRenderer.gameObject.SetActive(false);
    }

    public void LevelGraphMenu()
    {
        healthLineRenderer.gameObject.SetActive(false);
        CSVToPoints(levelLineRenderer, 2);
        scoreLineRenderer.gameObject.SetActive(false);
    }

    public void ScoreGraphMenu()
    {
        healthLineRenderer.gameObject.SetActive(false);
        levelLineRenderer.gameObject.SetActive(false);
        CSVToPoints(scoreLineRenderer, 3);
    }

    [ContextMenu("CSVtoPoints")]
    public void CSVToPoints(UILineRenderer uILineRenderer, int j)
    {
        uILineRenderer.points.Clear();
        uILineRenderer.gameObject.SetActive(false);

        int i, max = 0;
        int[] arr = MenuManager.Instance.GetCSVColumn(j);

        for(i = 0; i<arr.Length; i++)
        {
            if(arr[i] > 40) { arr[i] = arr[i]/10; }
            uILineRenderer.points.Add(new Vector2(i, arr[i])); // divide 10 beause its too big, for health dont divide
            if(max < arr[i]) { max = arr[i]; }
            // Debug.Log(arr[i]);
        }

        if(max > 40) { max = max/10;}
        

        uILineRenderer.gameObject.SetActive(true);
        uIGridRenderer.gameObject.SetActive(false);
        gridSize = new Vector2Int(i+1, max+1);
        uIGridRenderer.UpdateGridSize(gridSize);
        uILineRenderer.gridSize = gridSize;
        uIGridRenderer.gameObject.SetActive(true);
    }
}
