using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private WaveManagerGifts waveManagerGifts;
    [SerializeField] private List<WaveManager> waveManagers;
    private void Start()
    {
        SetWaveManagerGiftsActive(false);
        foreach (var waveManager in waveManagers)
        {
            SetWaveManagerActive(waveManagers.IndexOf(waveManager), false);
        }
    }
    private void Update()
    {
        
    }

    private void SetWaveManagerGiftsActive(bool isActive)
    {
        waveManagerGifts.enabled = isActive;
    }

    public void SetWaveManagerActive(int index, bool isActive)
    {
        if (index >= 0 && index < waveManagers.Count)
        {
            waveManagers[index].enabled = isActive;
        }
        else
        {
            Debug.LogWarning("Invalid WaveManager index: " + index);
        }
    }
}
