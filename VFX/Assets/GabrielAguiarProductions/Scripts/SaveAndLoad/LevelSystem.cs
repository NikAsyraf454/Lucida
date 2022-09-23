using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LevelSystem : MonoBehaviour, ISaveable
{
    [SerializeField] private int level = 1;
    [SerializeField] private int xp = 100;

    public object CaptureState()
    {
        return new SaveData
        {
            level = level,
            xp = xp
        };
    }

    public void RestoreState(object state)
    {
        var saveData = (SaveData)state;

        level = saveData.level;
        xp = saveData.xp;
    }

    // private void UpdateLoadProperties()         //if any properties needed to be updated for UI or etc
    // {}

    [Serializable]
    private struct SaveData
    {
        public int level;
        public int xp;
    }
}
