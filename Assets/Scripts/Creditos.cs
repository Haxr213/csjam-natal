using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Creditos : MonoBehaviour
{
    [SerializeField] private int idCenaMenu;
    public float velocidadeRolagem = 40f;
    public float alturaMaxima = 111f; // Defina aqui a altura onde o texto deve parar
    public float tempoEspera = 5f; // Tempo de espera em segundos antes de carregar o menu
    
    private RectTransform rectTransform;
    private bool chegouNoFinal = false;
    private float tempoDecorrido = 0f;
    
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Só move o texto se ainda não chegou na altura máxima
        if (rectTransform.anchoredPosition.y < alturaMaxima)
        {
            rectTransform.anchoredPosition += new Vector2(0, velocidadeRolagem * Time.deltaTime);
            
            // Garante que não ultrapasse a altura máxima
            if (rectTransform.anchoredPosition.y >= alturaMaxima)
            {
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, alturaMaxima);
                chegouNoFinal = true;
            }
        }
        
        // Inicia contagem após chegar no final
        if (chegouNoFinal)
        {
            tempoDecorrido += Time.deltaTime;
            
            // Carrega o menu após 5 segundos
            if (tempoDecorrido >= tempoEspera)
            {
                SceneManager.LoadScene(idCenaMenu);
            }
        }
    }
}