using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;
    public int iron = 5;
    public int wood = 5;
    public int sugar = 5;
    public int crystal = 5;

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
        ResourceUI.instance.UpdateIronText(iron.ToString("00"));
        ResourceUI.instance.UpdateWoodText(wood.ToString("00"));
        ResourceUI.instance.UpdateSugarText(sugar.ToString("00"));
        ResourceUI.instance.UpdateCrystalText(crystal.ToString("00"));
    }

    public void TakeIron(int amount)
    {
        iron -= amount;
        if(iron <= 0)
        {
            iron = 0;
        }
        ResourceUI.instance.UpdateIronText(iron.ToString("00"));
    }

    public void AddIron(int amount)
    {
        iron += amount;
        ResourceUI.instance.UpdateIronText(iron.ToString("00"));
    }

    public void TakeWood(int amount)
    {
        wood -= amount;
        if (wood <= 0)
        {
            wood = 0;
        }
        ResourceUI.instance.UpdateWoodText(wood.ToString("00"));
    }

    public void AddWood(int amount)
    {
        wood += amount;
        ResourceUI.instance.UpdateWoodText(wood.ToString("00"));
    }

    public void TakeSugar(int amount)
    {
        sugar -= amount;
        if (sugar <= 0)
        {
            sugar = 0;
        }
        ResourceUI.instance.UpdateSugarText(sugar.ToString("00"));
    }

    public void AddSugar(int amount)
    {
        sugar += amount;
        ResourceUI.instance.UpdateSugarText(sugar.ToString("00"));
    }

    public void TakeCrystal(int amount)
    {
        crystal -= amount;
        if (crystal <= 0)
        {
            crystal = 0;
        }
        ResourceUI.instance.UpdateCrystalText(crystal.ToString("00"));
    }

    public void AddCrystal(int amount)
    {
        crystal += amount;
        ResourceUI.instance.UpdateCrystalText(crystal.ToString("00"));
    }
}
