using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class ImageFadeIn : MonoBehaviour
{
    // A referência ao componente Image que será manipulado.
    private Image targetImage;

    // A duração total do fade in em segundos.
    public float fadeInDuration = 1.5f;
    
    // Cor de destino (normalmente branco ou a cor original da imagem).
    // Usamos [HideInInspector] pois vamos apenas pegar o Alpha.
    [HideInInspector] public Color targetColor; 

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
            targetColor = targetImage.color;

            // 2. IMPORTANTE: Define a cor inicial da imagem como totalmente transparente (Alpha = 0).
            // Mantendo os valores RGB originais.
            targetImage.color = new Color(targetColor.r, targetColor.g, targetColor.b, 0f);

            // 3. Inicia a Corrotina de Fade In.
            StartCoroutine(PerformFadeIn());
        }
        else
        {
            Debug.LogError("O componente Image não foi encontrado no objeto. Certifique-se de anexar o script ao objeto Image.");
        }
    }

    // Corrotina para realizar o FADE IN manipulando a cor da imagem.
    private IEnumerator PerformFadeIn()
    {
        float time = 0f;
        
        // A cor inicial (totalmente transparente)
        Color startColor = targetImage.color; 
        
        // A cor final (totalmente opaca, Alpha = 1)
        Color endColor = new Color(targetColor.r, targetColor.g, targetColor.b, 1f); 

        // Loop de interpolação
        while (time < fadeInDuration)
        {
            time += Time.deltaTime;
            
            // Interpola a cor de startColor para endColor ao longo do tempo.
            targetImage.color = Color.Lerp(startColor, endColor, time / fadeInDuration);
            
            yield return null; // Espera o próximo frame.
        }

        // Garante que a cor final seja exatamente a cor opaca (Alpha = 1).
        targetImage.color = endColor;

        Debug.Log("Fade In da Imagem concluído.");
    }
}