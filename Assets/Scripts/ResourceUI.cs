using UnityEngine;
using TMPro;

public class ResourceUI : MonoBehaviour
{
    public TextMeshProUGUI woodTXT;
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

    public void UpdateWoodText(string value)
    {
        woodTXT.text = value;
    }
}
