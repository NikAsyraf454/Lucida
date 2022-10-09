using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVReader : MonoBehaviour
{
    //public TextAsset textAssetData;
    string filename => $"{Application.persistentDataPath}/performance.csv";
    private int columnAmount= 3;

    public ScoreList highScoreList = new ScoreList();

    // Start is called before the first frame update
    void Start()
    {
        // filename = Application.persistentDataPath + "/performance.csv";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("LoadCSV")]
    public ScoreList ReadCSV()
    {
        StreamReader reader = new StreamReader(filename);
        string data = reader.ReadToEnd();
        string[] data_value = data.Split(new string[] { "," , "\n"}, System.StringSplitOptions.None);
        
        int tableSize = (data_value.Length / columnAmount) - 1;
        // Debug.Log(data_value);
        highScoreList.scores = new Score[tableSize];

        for(int i = 0; i < tableSize; i++)
        {
            highScoreList.scores[i] = new Score();
            highScoreList.scores[i].health = int.Parse(data_value[columnAmount * (i + 1)]);
            highScoreList.scores[i].level = int.Parse(data_value[columnAmount * (i + 1) + 1]);
            highScoreList.scores[i].score = int.Parse(data_value[columnAmount * (i + 1) + 2]);
        }
        reader.Close();

        return highScoreList;
    }
}
