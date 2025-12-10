using UnityEngine;

[System.Serializable]
public class TowerData
{
    [Header("Tower Level")]
    public int towerLevel;

    [Header("Tower Price")]
    public int upgradePrice;
    public int ironPrice;
    public int woodPrice;
    public int sugarPrice;
    public int crystalPrice;

    [Header("Tower Settings")]
    public float range;
    public float dmg;
    public float timeToShot;
    public float attackSpeedMultiplier;
}
