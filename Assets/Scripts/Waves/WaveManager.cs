using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public List<WaveObject> waves = new List<WaveObject>();
    public bool isWaitingForNextWave;
    public bool wavesFinish;
    public int currentWave;
    public Transform initPosition;
    private Enemy enemy;
    private LevelManager levelManager;

    private void Start()
    {
        levelManager = GetComponent<LevelManager>();
        StartCoroutine(ProcessWave());
    }

    private void Update()
    {
        CheckCounterForNextWave();
    }

    private void CheckCounterForNextWave()
    {
        
        if (isWaitingForNextWave && !wavesFinish)
        {
            waves[currentWave].counterToNextWave -= 1 * Time.deltaTime;
            if (waves[currentWave].counterToNextWave <= 0)
            {
                ChangeWave();
                Debug.Log("Set Next Wave");
            }
        }
    }

    public void ChangeWave()
    {
        if(this.enabled == true)
        {
            if (wavesFinish) return;
            currentWave++;

            StartCoroutine(ProcessWave());
        }
    }

    private IEnumerator ProcessWave()
    {
        if(wavesFinish)
            yield break;
        isWaitingForNextWave = false;
        waves[currentWave].counterToNextWave = waves[currentWave].timeForNextWave;

        List<Enemy> currentWaveEnemies = new List<Enemy>();
        for (int i = 0; i < waves[currentWave].enemys.Count; i++)
        {
            enemy = Instantiate(waves[currentWave].enemys[i], initPosition.position, initPosition.rotation);
            enemy.GetComponent<EnemyMoviment>().levelManager = levelManager;

            if (currentWave >= waves.Count - 1)
                currentWaveEnemies.Add(enemy);
            yield return new WaitForSeconds(waves[currentWave].timerPerCreation);
        }

        if (currentWave >= waves.Count - 1)
        {
            yield return StartCoroutine(WaitForEnemiesToBeDestroyed(currentWaveEnemies));

            Debug.Log("Nível Terminado!");
            wavesFinish = true;
        }
        else
        {
            isWaitingForNextWave = true;
        }
    }

    private IEnumerator WaitForEnemiesToBeDestroyed(List<Enemy> enemies)
    {
        while (true)
        {
            // Remove os inimigos que já foram destruídos
            enemies.RemoveAll(enemy => enemy == null);

            // Se não houver mais inimigos, saia do loop
            if (enemies.Count == 0)
                yield break;

            yield return null;
        }
    }
}

[System.Serializable]
public class WaveObject
{
    public float timerPerCreation = 1;
    public float timeForNextWave = 10;
    [HideInInspector] public float counterToNextWave = 0;
    public List<Enemy> enemys = new List<Enemy>();
}