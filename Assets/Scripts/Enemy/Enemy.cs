using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("OnDead")]
    public int sugarOnDead;
    public int woodOnDead;
    public int ironOnDead;
    public int crystalOnDead;
     
    [SerializeField]
    private AudioClip audioDeath;

    [Header("Life")]
    public bool isDead;
    public float maxLife = 1;
    public float currentLife = 0;

    void Start()
    {
        currentLife = maxLife;
    }

    public void TakeDamage(float dmg)
    {
        if (isDead) return;

        currentLife -= dmg;
        var fillValue = currentLife / maxLife;

        if (currentLife <= 0) OnDead();
    }

    private void OnDead()
    {
        isDead = true;
        currentLife = 0;

        //AudioManager.Instance.PlaySoundAtLocation(audioDeath, transform.position, 0.2f);
        Destroy(gameObject);
        PlayerData.instance.AddSugar(sugarOnDead);
        PlayerData.instance.AddWood(woodOnDead);
        PlayerData.instance.AddIron(ironOnDead);
        PlayerData.instance.AddCrystal(crystalOnDead);
    }
}
