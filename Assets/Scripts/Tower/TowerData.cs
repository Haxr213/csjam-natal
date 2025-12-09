using UnityEngine;

[System.Serializable]
public class TowerData
{
    [Header("Tower Level")]
    public int towerLevel;

    [Header("Tower Price")]
    public int upgradePrice;
    public int buyPrice;
    public int sellPrice;

    [Header("Tower Settings")]
    public float range;
    public float attackRange;
    public float dmg;
    public float timeToShot;
    public float attackSpeedMultiplier;
    public float moveSpeed;
}
