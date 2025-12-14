using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public static class CompleteTextWithButtonPromptSprite
{
    // capture multiple counts of {Player/Action} and {Player/Action}
    private static string ACTION_PATTERN = @"\{(.*?)\}";
    private static Regex REGEX = new Regex(ACTION_PATTERN, RegexOptions.IgnoreCase);
    public static string ReadAndReplaceBinding(string textToDisplay, string actionName, InputBinding actionNeeded, TMP_SpriteAsset spriteAsset)
    {
        string stringButtonName = actionNeeded.ToString();
        stringButtonName = RenameInput(stringButtonName, actionName);

        textToDisplay = textToDisplay.Replace(
            "BUTTONPROMPT",
            $"<sprite=\"{spriteAsset.name}\" name=\"{stringButtonName}\">");

        return textToDisplay;
    }

    public static string GetSpriteTag(string actionName, DeviceType deviceType, InputManager inputManager, ListOfTmpSpriteAssets spriteAssets)
    {
        InputBinding dynamicBinding = inputManager.GetBinding(actionName, deviceType);
        TMP_SpriteAsset spriteAsset = spriteAssets.SpriteAssets[(int)deviceType];

        //Debug.LogFormat("Retrieving sprite tag for: {0} whit path: {1}", dynamicBinding.action, dynamicBinding.effectivePath);
        string stringButtonName = dynamicBinding.effectivePath;
        stringButtonName = RenameInput(stringButtonName, actionName);

        return $"<sprite=\"{spriteAsset.name}\" name=\"{stringButtonName}\">";
    }

    public static string ReplaceActiveBindings(string textWithActions, InputManager inputManager,
            ListOfTmpSpriteAssets spriteAssets)
    {
        return ReplaceBindings(textWithActions, inputManager.GetActiveDevice(), inputManager, spriteAssets);
    }

    public static string ReplaceBindings(string textWithActions, DeviceType deviceType, InputManager inputManager,
            ListOfTmpSpriteAssets spriteAssets)
    {
        MatchCollection matches = REGEX.Matches(textWithActions);

        // original template
        var replacedText = textWithActions;

        foreach (Match match in matches)
        {
            var withBraces = match.Groups[0].Captures[0].Value;
            var innerPart = match.Groups[1].Captures[0].Value;
            //Debug.LogFormat("{0} has {1}", withBraces, innerPart);

            var tagText = GetSpriteTag(innerPart, deviceType, inputManager, spriteAssets);

            replacedText = replacedText.Replace(withBraces, tagText);
        }

        return replacedText;
    }


    private static string RenameInput(string stringButtonName, string actionName = "Interact")
    {
        stringButtonName = stringButtonName.Replace(actionName, String.Empty);

        stringButtonName = stringButtonName.Replace("<Keyboard>/", "Keyboard_");

        stringButtonName = stringButtonName.Replace("<Gamepad>/", "Gamepad_");

        return stringButtonName;
    }
}
