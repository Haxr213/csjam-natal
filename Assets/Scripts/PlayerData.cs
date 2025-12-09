using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;
    public int money = 10;

    public void Awake()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion

    }

    private void Start()
    {
        ResourceUI.instance.UpdateWoodText(money.ToString("00"));
    }

    public void TakeMoney(int amount)
    {
        money -= amount;
        if(money <= 0)
        {
            money = 0;
        }
        ResourceUI.instance.UpdateWoodText(money.ToString("00"));
    }

    public void AddMoney(int amount)
    {
        money += amount;
        ResourceUI.instance.UpdateWoodText(money.ToString("00"));
    }
}
