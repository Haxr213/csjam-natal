using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TowerRequestManager : MonoBehaviour
{
    public List<Tower> towers = new List<Tower>();    
    private static Animator anim;
    public bool isTowerPriced;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public static void OpenRequestPanel()
    {
        anim.SetBool("IsOpen", true);
    }
    public static void CloseRequestPanel()
    {
        anim.SetBool("IsOpen", false);
    }

    public void RequestTowerBuy(string towerName)
    {
        var tower = towers.Find(x => x.towerName == towerName);

        if(tower.currentData.ironPrice <= PlayerData.instance.iron &&
            tower.currentData.woodPrice <= PlayerData.instance.wood &&
            tower.currentData.sugarPrice <= PlayerData.instance.sugar &&
            tower.currentData.crystalPrice <= PlayerData.instance.crystal)
        {
            PlayerData.instance.TakeIron(tower.currentData.ironPrice);
            PlayerData.instance.TakeWood(tower.currentData.woodPrice);
            PlayerData.instance.TakeSugar(tower.currentData.sugarPrice);
            PlayerData.instance.TakeCrystal(tower.currentData.crystalPrice);
            isTowerPriced = true;
        }
        else
        {
            isTowerPriced = false;
            Debug.Log("Not money for tower : " + towerName);
            return;
        }
    }
}