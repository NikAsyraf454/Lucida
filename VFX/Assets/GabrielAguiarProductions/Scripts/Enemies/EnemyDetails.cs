using UnityEngine;

[System.Serializable]
public class EnemyDetails
{
    public int enemyId;
    public GameObject prefab;
    public Vector2 spawnTimeRange;
    public int amount;
    public int spawnAfterLevel;
    public int health;
    public bool isBoss;
    // public bool spawned;
}
