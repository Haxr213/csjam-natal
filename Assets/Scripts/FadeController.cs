using UnityEngine;
using UnityEngine.UI; 
using System.Collections;
using UnityEngine.SceneManagement; 

public class FadeController : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 1.0f;
    
    [Tooltip("O ID da cena (index no Build Settings) a ser carregada após o fade out.")]
    public int targetSceneIndex; 

    // O método Start é chamado assim que o script é ativado.
    private void Start()
    {
        // 1. Garante que o Canvas Group foi atribuído
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        // 2. Garante que o fade começa completamente visível (Alpha = 1)
        if (canvasGroup != null)
        {
             canvasGroup.alpha = 1f;
        }

        // 3. Inicia o fade out automaticamente
        StartDefaultSceneChange(); 
    }

    // Método principal para iniciar o fade out e carregar a cena
    public void StartFadeOutAndLoadScene(int sceneIndex)
    {
        if (canvasGroup != null)
        {
            targetSceneIndex = sceneIndex;
            StartCoroutine(FadeOutCoroutine(canvasGroup, fadeDuration));
        }
    }

    // Corrotina para realizar o esmaecimento e a mudança de cena
    private IEnumerator FadeOutCoroutine(CanvasGroup group, float duration)
    {
        float startAlpha = group.alpha;
        float targetAlpha = 0f; 
        float time = 0f;

        // 1. Fase de FADE OUT
        while (time < duration)
        {
            time += Time.deltaTime;
            group.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null; 
        }

        // Garante que o Alpha termine exatamente em 0.0
        group.alpha = targetAlpha;

        // 2. Fase de MUDANÇA DE CENA
        if (targetSceneIndex >= 0 && targetSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(targetSceneIndex, LoadSceneMode.Single);
        }
        else
        {
            Debug.LogError("O ID da cena alvo (" + targetSceneIndex + ") é inválido ou não existe no Build Settings!");
        }
    }

    // Método de atalho que usa o ID do Inspector
    public void StartDefaultSceneChange()
    {
        StartFadeOutAndLoadScene(targetSceneIndex);
    }
}