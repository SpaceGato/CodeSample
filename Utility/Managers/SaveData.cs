using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveData", menuName = "SaveData")]
public class SaveData : ScriptableObject
{
    [NonSerialized] public float
        pickupDropChance = 20, 
        enemySpeedMultiplier = 1f;
    [NonSerialized] public int
        scoreMultiplier = 1,
        priceMultiplier0 = 0, 
        priceMultiplier1 = 0, 
        priceMultiplier2 = 0;
    public int
        score = 0, 
        credits = 0;

    public Dictionary<string, Upgrade> upgrades = new Dictionary<string, Upgrade>();

    public void ResetData()
    {
        pickupDropChance = 20;
        enemySpeedMultiplier = 1f;
        scoreMultiplier = 1;

        priceMultiplier0 = 0;
        priceMultiplier1 = 0;
        priceMultiplier2 = 0;

        score = 0;
        credits = 0;
        upgrades.Clear();
    }
}
