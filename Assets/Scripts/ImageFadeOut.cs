using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class ImageFadeOut : MonoBehaviour
{
    // A referência ao componente Image que será manipulado.
    private Image targetImage;

    // A duração total do fade out em segundos.
    public float fadeOutDuration = 1.5f;

    // A cor de base da imagem.
    [HideInInspector] public Color baseColor;

    private void Awake()
    {
        // Garante que a referência ao componente Image seja obtida.
        targetImage = GetComponent<Image>();
    }

    private void Start()
    {
        if (targetImage != null)
        {
            // 1. Pega a cor original da imagem (RGB).
            baseColor = targetImage.color;

            // 2. IMPORTANTE: Garante que a imagem comece totalmente opaca (Alpha = 1).
            // Isso é crucial para o fade out.
            targetImage.color = new Color(baseColor.r, baseColor.g, baseColor.b, 1f);

            // 3. Inicia a Corrotina de Fade Out.
            StartCoroutine(PerformFadeOut());
        }
        else
        {
            Debug.LogError("O componente Image não foi encontrado no objeto. Certifique-se de anexar o script ao objeto Image.");
        }
    }

    // Corrotina para realizar o FADE OUT manipulando a cor da imagem.
    private IEnumerator PerformFadeOut()
    {
        float time = 0f;
        
        // A cor inicial (totalmente opaca, Alpha = 1)
        Color startColor = targetImage.color; 
        
        // A cor final (totalmente transparente, Alpha = 0)
        Color endColor = new Color(baseColor.r, baseColor.g, baseColor.b, 0f); 

        // Loop de interpolação
        while (time < fadeOutDuration)
        {
            time += Time.deltaTime;
            
            // Interpola a cor de startColor (Alpha 1) para endColor (Alpha 0).
            targetImage.color = Color.Lerp(startColor, endColor, time / fadeOutDuration);
            
            yield return null; // Espera o próximo frame.
        }

        // Garante que a cor final seja exatamente a cor transparente (Alpha = 0).
        targetImage.color = endColor;

        // Opcional: Desativa o objeto após ele desaparecer completamente.
        // gameObject.SetActive(false); 

        Debug.Log("Fade Out da Imagem concluído.");
    }
}