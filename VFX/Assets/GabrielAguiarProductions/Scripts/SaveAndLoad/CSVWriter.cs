using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVWriter : MonoBehaviour
{
    string filename => $"{Application.persistentDataPath}/performance.csv";

    // public ScoreList highScoreList = new ScoreList();

    void Start()
    {
        // filename = Application.persistentDataPath + "/performance.csv";
    }

    void Update()
    {

    }

    [ContextMenu("SaveCSV")]
    public void WriteCSV(ScoreList highScoreListParam)
    {
        ScoreList highScoreList = new ScoreList();
        highScoreList.scores = new Score[highScoreListParam.scores.Length + 1];
        
        for(int i = 0; i<highScoreListParam.scores.Length; i++)
        {
            highScoreList.scores[i] = highScoreListParam.scores[i];
        }

        Score temp = new Score();
        temp.health = PlayerManager.Instance.currentPlayerHealth;
        temp.level = WaveManager.Instance.waveIndex;
        temp.score = PlayerManager.Instance.GetFinalScore();
        highScoreList.scores[(highScoreList.scores.Length-1)] = temp;
        // highScoreList.scores[(highScoreList.scores.Length-1)].health = PlayerManager.Instance.currentPlayerHealth;
        // highScoreList.scores[highScoreList.scores.Length-1].level = WaveManager.Instance.waveIndex;
        // highScoreList.scores[highScoreList.scores.Length-1].score = PlayerManager.Instance.GetFinalScore();

        if(highScoreList.scores.Length > 0)
        {
            TextWriter tw = new StreamWriter(filename, false);
            tw.WriteLine("Health, Level, Score");
            tw.Close();

            tw = new StreamWriter(filename, true);

            for(int i = 0; i < highScoreList.scores.Length; i++)
            {
                tw.WriteLine(highScoreList.scores[i].health + "," 
                + highScoreList.scores[i].level + "," 
                + highScoreList.scores[i].score);
            }

            tw.Close();
        }

    }

    public void WriteEmptyCSV()
    {
        TextWriter tw = new StreamWriter(filename, false);
        tw.WriteLine("Health, Level, Score");
        tw.Close();

        tw = new StreamWriter(filename, true);

        int i = 0;

        tw.WriteLine(i + "," 
        + i + "," 
        + i);

        tw.Close();
    }

}