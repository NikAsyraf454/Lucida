using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaveManager : MonoBehaviour, ISaveable
{
    [SerializeField] private List<GameObject> enemyPrefabList;
    public List<EnemyMovement> enemyList;
    public List<Waves> waves;
    private PathManager pathManager;
    private GameObject spawnPoint;
    private float timeBetweenWave = 5f;
    private float timer = 10f;
    private int waveIndex = 0;
    private int groupIndex = 0;
    private PlayerManager playerManager;
    private bool canSpawnNext = false;
    public List<EnemyHealth> enemyHealthList;

    void Start()
    {
        Debug.Log("here");
        pathManager = GetComponent<PathManager>();
        playerManager = FindObjectOfType<PlayerManager>();
        spawnPoint = pathManager.GetWaypoint(0);
        canSpawnNext = true;
        //SpawnWave();
        //waveIndex++;
        // Debug.Log(waves.Count);
        // Debug.Log(waves[0].enemyGroup.Count);
        // Debug.Log(waves[0].enemyGroup[0].amount);
    }

    // Update is called once per frame
    void Update()
    {
        if(timer <= 0f && canSpawnNext)
        {
            if(waveIndex == waves.Count) { return; }
            //timer = timeBetweenWave;
            canSpawnNext = false;
            SpawnWave();
            //waveIndex++;
        }

        timer -= Time.deltaTime;
    }

    void SpawnWave()
    {
        Debug.Log("Spawning the wave: " + waveIndex);
        for(groupIndex = 0; groupIndex < waves[waveIndex].enemyGroup.Count; groupIndex++)
        {
            StartCoroutine(SpawnGroup(groupIndex, 0));
        }

        waveIndex++;
    }

    IEnumerator SpawnGroup(int groupNum, int j)
    {
        for(; j < waves[waveIndex].enemyGroup[groupNum].amount; j++)        //j is for enemy amount, i is for enemyGroup index
        {
            //SpawnEnemy(waves[waveNum].enemyGroup[i].enemyPrefab);
            SpawnEnemy(waves[waveIndex].enemyGroup[groupNum].enemyId, spawnPoint.transform.position, spawnPoint.transform.rotation);
            yield return new WaitForSeconds(1f / waves[waveIndex].enemyGroup[groupNum].rate);
        }

        if(groupNum == waves[waveIndex].enemyGroup.Count-1)
        {
            Debug.Log("Last of wave " + waveIndex + "group num: " + groupNum);
            timer = timeBetweenWave;
            canSpawnNext = true;
        }

        //enemyCount++;
    }

    private void SpawnEnemy(int id, Vector3 pos, Quaternion rotation)
    {
        GameObject enemy = Instantiate(enemyPrefabList[id], pos, rotation);
        enemy.GetComponent<EnemyHealth>().playerManager = playerManager;
        enemyList.Add(enemy.GetComponent<EnemyMovement>());
        enemyHealthList.Add(enemy.GetComponent<EnemyHealth>());
        enemyHealthList[enemyHealthList.Count-1].ServerOnDie += RemoveEnemy;
    }

    private void RemoveEnemy(GameObject other)
	{
        enemyList.Remove(other.GetComponent<EnemyMovement>());
		enemyHealthList.Remove(other.GetComponent<EnemyHealth>());
    }

    private void OnDestroy()
    {
		foreach(EnemyHealth enemyHealth in enemyHealthList)
		{
			enemyHealth.ServerOnDie -= RemoveEnemy;
			enemyHealthList.Remove(enemyHealth);
		}
	}

    #region Save and Load

    public object CaptureState()
    {
        List<int> enemyId = new List<int>();
        List<float> enemyPos = new List<float>();
        List<int> enemyPathIndex = new List<int>();
        List<int> enemyHealth = new List<int>();
        List<float> enemySpeed = new List<float>();
        foreach(EnemyMovement enemyMovement in enemyList)
        {
            enemyId.Add(enemyMovement.enemyId);
            enemyPos.Add(enemyMovement.gameObject.transform.position.x);
            enemyPos.Add(enemyMovement.gameObject.transform.position.y);
            enemyPos.Add(enemyMovement.gameObject.transform.position.z);

            enemyPathIndex.Add(enemyMovement.pathIndex);
            enemyHealth.Add(enemyMovement.enemyHealth.CurrentHealth);
            Debug.Log("saving:" + enemyMovement.enemyHealth.CurrentHealth);
            enemySpeed.Add(enemyMovement.enemySpeed);
        }
        return new SaveData
        {
            enemyId = enemyId,
            enemyPos = enemyPos,
            enemypPathIndex = enemyPathIndex,
            enemyHealth = enemyHealth,
            enemySpeed = enemySpeed
        };
    }

    public void RestoreState(object state)
    {
        var saveData = (SaveData)state;

        UpdateLoadProperties(saveData);
    }

    private void UpdateLoadProperties(SaveData saveData)         //if any properties needed to be updated for UI or etc
    {
        List<int> enemyId = new List<int>();
        List<float> enemyPos = new List<float>();
        List<int> enemyPathIndex = new List<int>();
        List<int> enemyHealth = new List<int>();
        List<float> enemySpeed = new List<float>();

        enemyId = saveData.enemyId;
        enemyPos = saveData.enemyPos;
        enemyPathIndex = saveData.enemypPathIndex;
        enemyHealth = saveData.enemyHealth;
        enemySpeed = saveData.enemySpeed;

        int j = 0;
        foreach(int i in enemyId)
        {
            SpawnEnemy(i, new Vector3(enemyPos[j*3], enemyPos[j*3+1], enemyPos[j*3+2]), Quaternion.identity);
            enemyList[j].pathIndex = enemyPathIndex[j];
            enemyHealthList[j].CurrentHealth = enemyHealth[j];
            Debug.Log("Load:" + enemyHealth[j]);
            enemyList[j].enemySpeed = enemySpeed[j];
            j++;
        }

    }

    [Serializable]
    private struct SaveData
    {
        public List<int> enemyId;
        public List<float> enemyPos;
        public List<int> enemypPathIndex;
        public List<int> enemyHealth;
        public List<float> enemySpeed;
        //public List<float> waves or spawning sequence;
    }

    #endregion
}
