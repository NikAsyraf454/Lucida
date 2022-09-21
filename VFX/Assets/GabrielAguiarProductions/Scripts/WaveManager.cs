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

    void Start()
    {
        Debug.Log("here");
        pathManager = GetComponent<PathManager>();
        playerManager = FindObjectOfType<PlayerManager>();
        spawnPoint = pathManager.GetWaypoint(0);
        StartCoroutine(SpawnWave(waveIndex));
        waveIndex++;
        Debug.Log(waves.Count);
        Debug.Log(waves[0].enemyGroup.Count);
        Debug.Log(waves[0].enemyGroup[0].amount);
    }

    // Update is called once per frame
    void Update()
    {
        if(timer <= 0f)
        {
            if(waveIndex == waves.Count) { return; }
            timer = timeBetweenWave;
            StartCoroutine(SpawnWave(waveIndex));
            waveIndex++;
        }

        timer -= Time.deltaTime;
    }

    IEnumerator SpawnWave(int waveNum)
    {
        for(int i = 0; i < waves[waveNum].enemyGroup.Count; i++)
        {
        
            for(int j = 0; j < waves[waveNum].enemyGroup[i].amount; j++)        //j is for enemy amount, i is for enemyGroup index
            {
                SpawnEnemy(waves[waveNum].enemyGroup[i].enemyPrefab);
                yield return new WaitForSeconds(1f / waves[waveNum].enemyGroup[i].rate);
            }
        }
    }

    void SpawnEnemy(GameObject prefab)
    {
        GameObject enemy = Instantiate(prefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        enemy.GetComponent<EnemyHealth>().playerManager = playerManager;
        //enemyCount++;
    }
}
