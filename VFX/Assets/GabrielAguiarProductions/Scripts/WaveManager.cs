using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public List<Waves> waves;
    private PathManager pathManager;
    private GameObject spawnPoint;
    private float timeBetweenWave = 5f;
    private float timer = 10f;
    private int waveIndex = 0;
    private PlayerManager playerManager;
    private bool canSpawnNext = false;

    void Start()
    {
        Debug.Log("here");
        pathManager = GetComponent<PathManager>();
        playerManager = FindObjectOfType<PlayerManager>();
        spawnPoint = pathManager.GetWaypoint(0);
        SpawnWave(waveIndex);
        waveIndex++;
        Debug.Log(waves.Count);
        Debug.Log(waves[0].enemyGroup.Count);
        Debug.Log(waves[0].enemyGroup[0].amount);
    }

    // Update is called once per frame
    void Update()
    {
        if(timer <= 0f && canSpawnNext)
        {
            if(waveIndex == waves.Count) { return; }
            //timer = timeBetweenWave;
            canSpawnNext = false;
            SpawnWave(waveIndex);
            waveIndex++;
        }

        timer -= Time.deltaTime;
    }

    void SpawnWave(int waveNum)
    {
        Debug.Log("Spawning the wave: " + waveNum);
        for(int i = 0; i < waves[waveNum].enemyGroup.Count; i++)
        {
            StartCoroutine(SpawnEnemy(waveNum, i));
        }
    }

    IEnumerator SpawnEnemy(int waveNum, int groupNum)
    {
        for(int j = 0; j < waves[waveNum].enemyGroup[groupNum].amount; j++)        //j is for enemy amount, i is for enemyGroup index
        {
            //SpawnEnemy(waves[waveNum].enemyGroup[i].enemyPrefab);
            GameObject enemy = Instantiate(waves[waveNum].enemyGroup[groupNum].enemyPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
            enemy.GetComponent<EnemyHealth>().playerManager = playerManager;
            yield return new WaitForSeconds(1f / waves[waveNum].enemyGroup[groupNum].rate);
        }

        if(groupNum == waves[waveNum].enemyGroup.Count-1)
        {
            Debug.Log("Last of wave " + waveNum + "group num: " + groupNum);
            timer = timeBetweenWave;
            canSpawnNext = true;
        }

        //enemyCount++;
    }
}
