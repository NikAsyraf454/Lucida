using UnityEngine;

[System.Serializable]
public class EnemyDetails
{
    public int enemyId;
    public GameObject prefab;
    public Vector2 spawnTimeRange;
    public int startAmount;
    public int spawnAfterLevel;
    public int health;
}
