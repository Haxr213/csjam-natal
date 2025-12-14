using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "ListOfTmpSpriteAssets", menuName = "Scriptable Objects/ListOfTmpSpriteAssets")]
public class ListOfTmpSpriteAssets : ScriptableObject
{
    public List<TMP_SpriteAsset> SpriteAssets;
}
