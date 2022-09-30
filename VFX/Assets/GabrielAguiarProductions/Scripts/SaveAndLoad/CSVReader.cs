using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVReader : MonoBehaviour
{
    //public TextAsset textAssetData;
    string filename = "";
    private int columnAmount= 2;

    [System.Serializable]
    public class Score
    {
        public int level;
        public int score;
    }

    [System.Serializable]
    public class ScoreList
    {
        public Score[] scores;
    }

    public ScoreList highScoreList = new ScoreList();

    // Start is called before the first frame update
    void Start()
    {
        filename = Application.dataPath + "/test.csv";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("LoadCSV")]
    void ReadCSV()
    {
        StreamReader reader = new StreamReader(filename);
        string data = reader.ReadToEnd();
        string[] data_value = data.Split(new string[] { "," , "\n"}, System.StringSplitOptions.None);
        
        int tableSize = (data_value.Length / columnAmount) - 1;
        Debug.Log(data_value);
        highScoreList.scores = new Score[tableSize];

        for(int i = 0; i < tableSize; i++)
        {
            highScoreList.scores[i] = new Score();
            highScoreList.scores[i].level = int.Parse(data_value[columnAmount * (i + 1)]);
            highScoreList.scores[i].score = int.Parse(data_value[columnAmount * (i + 1) + 1]);
        }


    }
}
