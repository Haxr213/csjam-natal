using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource audioDeathPrefab;
    public AudioSource audioCoinPrefab;
    public AudioSource audioSellPrefab;
    public AudioSource audioUpgradePrefab;
    public AudioSource audioNegativePrefab;
    public AudioSource audioNegative2Prefab;
    public AudioClip clipNegative2;
    public TowerRequestManager requestTower;
    
    void Awake()
    {
        Instance = this;
    }

    public void PlaySoundMonsterPrice(AudioSource source)
    {
        if (requestTower.isTowerPriced == true)
        {
            AudioSource audioBuySource = Instantiate(source);
            source.Play();
        }
        else if(requestTower.isTowerPriced == false)
        {
            AudioSource audioBuySource = Instantiate(audioNegative2Prefab);
            audioBuySource.PlayOneShot(clipNegative2);
            Destroy(audioBuySource.gameObject, clipNegative2.length);
        }  
    }

    public void PlaySound(AudioClip clip, AudioSource source)
    {
        AudioSource audioSellSource = Instantiate(source);
        audioSellSource.PlayOneShot(clip);
        Destroy(audioSellSource.gameObject, clip.length);
    }

    public void PlaySoundAtLocation(AudioClip clip, Vector3 position, float volume)
    {
        AudioSource audioDeathSource = Instantiate(audioDeathPrefab, position, Quaternion.identity);
        AudioSource audioCoinSource = Instantiate(audioCoinPrefab, position, Quaternion.identity);
        audioDeathSource.clip = clip;
        audioCoinSource.clip = clip;
        audioDeathSource.volume = volume;
        audioCoinSource.volume = volume;
        audioDeathSource.Play();
        audioCoinSource.Play();
        Destroy(audioDeathSource.gameObject, clip.length);
        Destroy(audioCoinSource.gameObject, clip.length);
    }
}
