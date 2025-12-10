using UnityEngine;
using TMPro;

public class ResourceUI : MonoBehaviour
{
    public TextMeshProUGUI ironTXT;
    public TextMeshProUGUI woodTXT;
    public TextMeshProUGUI sugarTXT;
    public TextMeshProUGUI crystalTXT;

    public static ResourceUI instance;

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

    public void UpdateIronText(string value)
    {
        ironTXT.text = value;
    }

    public void UpdateWoodText(string value)
    {
        woodTXT.text = value;
    }
    public void UpdateSugarText(string value)
    {
        sugarTXT.text = value;
    }

    public void UpdateCrystalText(string value)
    {
        crystalTXT.text = value;
    }

}
