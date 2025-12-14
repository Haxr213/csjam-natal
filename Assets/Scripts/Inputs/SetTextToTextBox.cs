using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SetTextToTextBox : MonoBehaviour
{
    [TextArea(2, 3)]
    [SerializeField] public string message = "Press {Player/Action} to interact which is also {Player/Action}.";

    [Header("Setup for sprites")]
    [SerializeField] private ListOfTmpSpriteAssets listOfTmpSpriteAssets;
    [SerializeField] private DeviceType deviceType;
    [SerializeField] private InputManager inputManager;

    private PlayerController _playerInput;
    private TMP_Text _textBox;

    private void Awake()
    {
        _playerInput = inputManager.GetPlayerInput();
        _textBox = GetComponent<TMP_Text>();

        inputManager.ActiveDeviceChangeEvent += SetText;
    }

    private void OnDestroy()
    {
        inputManager.ActiveDeviceChangeEvent -= SetText;
    }
    void Start()
    {
        SetText();
    }

    [ContextMenu("Set Text")]
    public void SetText()
    {
        if ((int)deviceType > listOfTmpSpriteAssets.SpriteAssets.Count - 1)
        {
            Debug.LogError($"Missing sprite Asset for {deviceType}");
            return;
        }

        InputBinding oldBinding = _playerInput.Player.Action.bindings[(int)deviceType];

        InputBinding dynamicBinding = inputManager.GetBinding("Action", deviceType);

        _textBox.text = CompleteTextWithButtonPromptSprite.ReplaceActiveBindings(message,
                inputManager, listOfTmpSpriteAssets);

        // _textBox.text = CompleteTextWithButtonPromptSprite.ReadAndReplaceBinding(
        //     message,
        //     "Action:",
        //     dynamicBinding,
        //     listOfTmpSpriteAssets.SpriteAssets[(int)deviceType]);

        // _textBox.text += CompleteTextWithButtonPromptSprite.GetSpriteTag(
        //     "Action",
        //     deviceType,
        //     inputManager,
        //     listOfTmpSpriteAssets);
    }
}
