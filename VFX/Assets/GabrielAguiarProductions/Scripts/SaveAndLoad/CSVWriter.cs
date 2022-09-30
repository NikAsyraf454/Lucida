using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVWriter : MonoBehaviour
{
    string filename = "";

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

    void Start()
    {
        filename = Application.dataPath + "/test.csv";
    }

    void Update()
    {

    }

    [ContextMenu("SaveCSV")]
    public void WriteCSV()
    {
        if(highScoreList.scores.Length > 0)
        {
            TextWriter tw = new StreamWriter(filename, false);
            tw.WriteLine("Level, score");
            tw.Close();

            tw = new StreamWriter(filename, true);

            for(int i = 0; i < highScoreList.scores.Length; i++)
            {
                tw.WriteLine(highScoreList.scores[i].level + "," + highScoreList.scores[i].score);
            }

            tw.Close();
        }

    }

}