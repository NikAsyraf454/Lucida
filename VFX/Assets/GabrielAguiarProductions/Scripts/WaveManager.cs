using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaveManager : MonoBehaviour, ISaveable
{
    public static WaveManager Instance;
    public int totalWaves = 3;
    [SerializeField] private List<GameObject> enemyPrefabList;
    [SerializeField] private List<EnemyDetails> enemyDetailsList;
    public List<EnemyMovement> enemyList;
    public List<Waves> waves;
    private PathManager pathManager;
    private GameObject spawnPoint;
    [SerializeField] private float timeBetweenWave = 5f;
    private float timer = 10f;
    public int waveIndex = 0;
    private int groupIndex = 0;
    private PlayerManager playerManager;
    private bool canSpawnNext = false;
    private bool lastOfWave = false;
    public List<EnemyHealth> enemyHealthList;
    [SerializeField] private int wavesEnded;

    void Awake()
    {
        Instance = this;
        pathManager = GetComponentInChildren<PathManager>();
        playerManager = FindObjectOfType<PlayerManager>();
        canSpawnNext = true;
        // PopulateWavesList();
        //difficultyAdjustment()
    }

    void Start()
    {
        spawnPoint = pathManager.GetWaypoint(0);
    }

    void Update()
    {
        if(MenuManager.Instance.gameEnded) { return; }

        // if(waveIndex >= waves.Count) {
        //     MenuManager.Instance.PlayerWin();
        //     Debug.Log("Win"); 
        //     return;
        // }

        if(timer <= 0f && canSpawnNext)
        {   
            canSpawnNext = false;
            lastOfWave = false;
            SpawnWave();
            TimerUI.Instance.SetTimer(0.00f);
        }

        timer -= Time.deltaTime;
        if (timer <= 0.00f) { return; }
        TimerUI.Instance.SetTimer(timer);
    }

    void SpawnWave()
    {
        // Debug.Log("Spawning the wave: " + waveIndex);
        wavesEnded = waves[waveIndex].enemyGroup.Count;
        for(groupIndex = 0; groupIndex < waves[waveIndex].enemyGroup.Count; groupIndex++)
        {
            StartCoroutine(SpawnGroup(groupIndex));
        }

        
    }

    IEnumerator SpawnGroup(int groupNum)
    {
        for(int j = 0; j < waves[waveIndex].enemyGroup[groupNum].amount; j++)        //j is for enemy amount, i is for enemyGroup index
        {
            //SpawnEnemy(waves[waveNum].enemyGroup[i].enemyPrefab);
            SpawnEnemy(waves[waveIndex].enemyGroup[groupNum].enemyId, spawnPoint.transform.position, spawnPoint.transform.rotation);
            
            // if(groupNum == waves[waveIndex].enemyGroup.Count-2)Debug.Log("Spawning " + (j+1) + "/" + waves[waveIndex].enemyGroup[groupNum].amount);
            
            if(j == waves[waveIndex].enemyGroup[groupNum].amount-1)
            {
                wavesEnded--;
            }

            if(wavesEnded <= 0 && !lastOfWave)
            {
                Debug.Log("Last of wave " + waveIndex + "group num: " + groupNum);
                lastOfWave = true;
                waveIndex++;
            }

            float temp = UnityEngine.Random.Range(waves[waveIndex].enemyGroup[groupNum].rate.x,waves[waveIndex].enemyGroup[groupNum].rate.y);
            yield return new WaitForSeconds(/* 1f / */ temp);
        }
    }

    private void SpawnEnemy(int id, Vector3 pos, Quaternion rotation)
    {
        GameObject enemy = Instantiate(enemyDetailsList[id].prefab, pos, rotation);
        enemy.GetComponent<EnemyHealth>().playerManager = playerManager;
        enemyList.Add(enemy.GetComponent<EnemyMovement>());
        enemyHealthList.Add(enemy.GetComponent<EnemyHealth>());
        enemyHealthList[enemyHealthList.Count-1].ServerOnDie += RemoveEnemy;
    }

    public void PopulateWavesList()
    {
        PlayerManager.Difficulty diff = PlayerManager.Instance.difficulty;

        int[] amount = new int[2];
        float[] spawnSpeed = new float[2];

        switch(diff)
        {
            case PlayerManager.Difficulty.Easy:
                Debug.Log("easy");
                amount[0] = 0;
                amount[1] = 3;
                spawnSpeed[0] = 1f;
                spawnSpeed[1] = 0.95f;
                break;
            case PlayerManager.Difficulty.Normal:
                Debug.Log("Normal");
                amount[0] = 3;
                amount[1] = 5;
                spawnSpeed[0] = 0.95f;
                spawnSpeed[1] = 0.9f;
                break;
            case PlayerManager.Difficulty.Hard:
                Debug.Log("Hard");
                amount[0] = 5;
                amount[1] = 7;
                spawnSpeed[0] = 0.94f;
                spawnSpeed[1] = 0.85f;
                break;
        }

        for(int i = 0; i < totalWaves; i++)
        {
                // Debug.Log("Creating wave...");
            
            Waves waveTemp = new Waves();
            waveTemp.enemyGroup = new List<EnemyGroup>();
            
            foreach(EnemyDetails enemyDetails in enemyDetailsList)
            {
                if(enemyDetails.spawnAfterLevel > i) { continue; }

                EnemyGroup enemyGroupTemp = new EnemyGroup();
                enemyGroupTemp.enemyId = enemyDetails.enemyId;
                enemyDetails.amount += UnityEngine.Random.Range(amount[0],amount[1]);
                enemyGroupTemp.amount = enemyDetails.amount;
                enemyDetails.spawnTimeRange.x *= UnityEngine.Random.Range(spawnSpeed[0],spawnSpeed[1]);
                enemyDetails.spawnTimeRange.y *= UnityEngine.Random.Range(spawnSpeed[0],spawnSpeed[1]);
                enemyGroupTemp.rate = enemyDetails.spawnTimeRange;
                if(enemyDetails.isBoss && enemyDetails.spawnAfterLevel == i) { enemyGroupTemp.amount = 1; }
                //rate changes to be faster after waves
                waveTemp.enemyGroup.Add(enemyGroupTemp);
            }
            waves.Add(waveTemp);
        }
    }

    // private void difficultyAdjustment()
    // {

    // }

    private void RemoveEnemy(GameObject other)
	{
        enemyList.Remove(other.GetComponent<EnemyMovement>());
		enemyHealthList.Remove(other.GetComponent<EnemyHealth>());

        if(enemyHealthList.Count <= 0 && lastOfWave)
        {
            if(waveIndex >= totalWaves) 
            {
                MenuManager.Instance.PlayerWin(); 
                return;
            }

            timer = timeBetweenWave;
            canSpawnNext = true;
            // TimerUI.Instance.SetTimer(timer);
            Debug.Log("Saving...");
            SaveManager.Instance.Save();
        }
    }

    public void EliminateEnemy()
    {
        Debug.Log("Eliminating all enemy");
        // int count = 0;
        foreach(EnemyHealth enemyHealth in enemyHealthList.ToArray())
        {
            // count++;
            enemyHealth.EnemyDeath();
            // Debug.Log(count);
        }
    }

    private void OnDestroy()
    {
		foreach(EnemyHealth enemyHealth in enemyHealthList.ToArray())
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
        List<float> enemyHealth = new List<float>();
        List<float> enemySpeed = new List<float>();
        int waveNum;
        float timerSave;
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

        waveNum = waveIndex;
        timerSave = timer;
        return new SaveData
        {
            enemyId = enemyId,
            enemyPos = enemyPos,
            enemypPathIndex = enemyPathIndex,
            enemyHealth = enemyHealth,
            enemySpeed = enemySpeed,
            waveNum = waveNum,
            timerSave = timerSave
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
        List<float> enemyHealth = new List<float>();
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
        
        //load wave savings
        waveIndex = saveData.waveNum;
        timer = saveData.timerSave;
    }

    [Serializable]
    private struct SaveData
    {
        public List<int> enemyId;
        public List<float> enemyPos;
        public List<int> enemypPathIndex;
        public List<float> enemyHealth;
        public List<float> enemySpeed;
        //wave saving
        public int waveNum;
        public float timerSave;
        //public List<float> waves or spawning sequence;
    }

    #endregion
}
