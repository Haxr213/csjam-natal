using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject towersUI;
    [SerializeField] private GameObject buttonUI;
    [SerializeField] private GameObject removeTurretUI;
    [Header("Values")]
    [SerializeField] private float positionYTowersActive = 100f;
    [SerializeField] private float positionYTowersInactive = -500f;
    [SerializeField] private float positionYButtonActive = 500f;
    [SerializeField] private float positionYButtonInactive = 50f;
    [SerializeField] private float positionYRemoveTowersActive = 100f;
    [SerializeField] private float positionYRemoveTowersInactive = -500f;
    [SerializeField] private string actionButton = "{Player/Action}";
    [SerializeField] private string cancelButton = "{Tower/Cancel}";


    private void Start()
    {
        setInactiveTowersUI();
        SetInactiveRemoveTurretUI();
        SetInactiveButtonUI();
    }


    public void setInactiveTowersUI()
    {
        Debug.Log("Set Inactive Towers UI");
        RectTransform rectTransform = towersUI.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, positionYTowersInactive);
        towersUI.SetActive(false);
    }

    public void SetActiveTowersUI()
    {
        Debug.Log("Set Active Towers UI");
        towersUI.SetActive(true);
        RectTransform rectTransform = towersUI.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, positionYTowersActive);
    }

    public void SetInactiveRemoveTurretUI()
    {
        Debug.Log("Set Inactive Remove Turret UI");
        RectTransform rectTransform = removeTurretUI.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, positionYRemoveTowersInactive);
        removeTurretUI.SetActive(false);
    }

    public void SetActiveRemoveTurretUI()
    {
        Debug.Log("Set Active Remove Turret UI");
        removeTurretUI.SetActive(true);
        RectTransform rectTransform = removeTurretUI.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, positionYRemoveTowersActive);
    }

    public void SetActiveButtonUI()
    {
        RectTransform rectTransformButton = buttonUI.GetComponent<RectTransform>();
        rectTransformButton.anchoredPosition = new Vector2(rectTransformButton.anchoredPosition.x, positionYButtonActive);

        GameObject textInput = buttonUI.transform.GetChild(0).gameObject;
        SetTextToTextBox textMesh = textInput.GetComponent<SetTextToTextBox>();
        textMesh.message = cancelButton;
        textMesh.SetText();
    }

    public void SetInactiveButtonUI()
    {
        RectTransform rectTransformButton = buttonUI.GetComponent<RectTransform>();
        rectTransformButton.anchoredPosition = new Vector2(rectTransformButton.anchoredPosition.x, positionYButtonInactive);

        GameObject textInput = buttonUI.transform.GetChild(0).gameObject;
        SetTextToTextBox textMesh = textInput.GetComponent<SetTextToTextBox>();
        textMesh.message = actionButton;
        textMesh.SetText();
    }

}
