using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManagerGifts : MonoBehaviour
{
    public List<WaveGifts> gifts = new List<WaveGifts>();
    private List<Gift> activeGifts = new List<Gift>();
    public bool isWaitingForNextWave;
    public bool wavesFinish;
    public int currentWave;
    public Transform initPosition;
    private LevelManager levelManager;
    public int goodGiftsArrived;

    [Header("Gift Prefabs")]
    [SerializeField] private Gift[] goodGiftPrefabs;
    [SerializeField] private Gift[] badGiftPrefabs;

    [SerializeField]
    private Transform highlight;

    void Start()
    {
        levelManager = GetComponent<LevelManager>();
        StartCoroutine(ProcessWave());
    }

    private void Update()
    {
        CheckCounterForNextWave();

        // Se o jogador quer interagir com o presente, pode usar um método aqui para verificar a posição do highlight
        if (highlight.gameObject.activeSelf)
        {
            TryRemoveGiftAtHighlightPosition();
        }
    }

    private void CheckCounterForNextWave()
    {

        if (isWaitingForNextWave && !wavesFinish)
        {
            gifts[currentWave].counterToNextWave -= 1 * Time.deltaTime;
            if (gifts[currentWave].counterToNextWave <= 0)
            {
                ChangeWave();
                Debug.Log("Set Next Wave");
            }
        }
    }

    public void ChangeWave()
    {
        if (this.enabled == true)
        {
            if (wavesFinish) return;
            currentWave++;

            StartCoroutine(ProcessWave());
        }
    }

    private IEnumerator ProcessWave()
    {
        if (wavesFinish)
            yield break;

        isWaitingForNextWave = false;

        gifts[currentWave].counterToNextWave = gifts[currentWave].timeForNextWave;

        // Inicia a wave randômica
        yield return StartCoroutine(SpawnGiftWave());

        // Espera todos os presentes sumirem (chegar ao fim ou serem destruídos)
        yield return StartCoroutine(WaitForActiveGiftsToEnd());

        if (currentWave >= gifts.Count - 1)
        {
            wavesFinish = true;
            Debug.Log("Wave de presentes finalizada!");
        }
        else
        {
            isWaitingForNextWave = true;
        }
    }

    private IEnumerator WaitForActiveGiftsToEnd()
    {
        while (activeGifts.Count > 0)
        {
            activeGifts.RemoveAll(g => g == null);
            yield return null;
        }
    }

    private void TryRemoveGiftAtHighlightPosition()
    {
        // Verifica se o player está interagindo com o slot do presente
        Vector2 highlightPos = highlight.position;
        for (int i = activeGifts.Count - 1; i >= 0; i--)
        {
            Gift gift = activeGifts[i];

            if (gift == null) continue;

            if (Vector2.Distance(highlightPos, gift.transform.position) < 0.5f)
            {
                Destroy(gift.gameObject);
                activeGifts.RemoveAt(i);
                Debug.Log("Present removed!");
                break;
            }
        }
    }

    public void StartWaveOfGifts()
    {
        StartCoroutine(ProcessWave()); // Inicia a onda de presentes
    }

    public void StopWaveOfGifts()
    {
        foreach (var gift in activeGifts)
        {
            if (gift != null)
                gift.PauseMovement();
        }
    }

    public void ResumeWaveOfGifts()
    {
        foreach (var gift in activeGifts)
        {
            if (gift != null)
                gift.ResumeMovement();
        }
    }

    private IEnumerator SpawnGiftWave()
    {
        activeGifts.Clear();
        goodGiftsArrived = 0;

        for (int i = 0; i < gifts[currentWave].totalGifts; i++)
        {
            bool isBadGift = Random.value <= gifts[currentWave].badGiftChance;

            Gift prefabToSpawn;

            if (isBadGift)
            {
                prefabToSpawn = badGiftPrefabs[
                    Random.Range(0, badGiftPrefabs.Length)
                ];
            }
            else
            {
                prefabToSpawn = goodGiftPrefabs[
                    Random.Range(0, goodGiftPrefabs.Length)
                ];
            }

            Gift gift = Instantiate(
                prefabToSpawn,
                initPosition.position,
                Quaternion.identity
            );

            gift.Init(
                levelManager,
                isBadGift ? GiftQuality.Bad : GiftQuality.Good
            );

            gift.OnGiftReachedEnd += OnGiftReachedEnd;
            activeGifts.Add(gift);

            yield return new WaitForSeconds(gifts[currentWave].spawnInterval);
        }
    }

    private void OnGiftReachedEnd(Gift gift)
    {
        if (gift.quality == GiftQuality.Good)
            goodGiftsArrived++;
        else
            goodGiftsArrived--;

        activeGifts.Remove(gift);
    }
}

[System.Serializable]
public class WaveGifts
{
    public int totalGifts = 10;
    public float badGiftChance = 0.3f;
    public float spawnInterval = 0.5f;
    public float timeForNextWave = 10;
    [HideInInspector] public float counterToNextWave = 0;
}
