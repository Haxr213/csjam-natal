using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
   [SerializeField] private int idCenaJogo;
   [SerializeField] private int idCenaCreditos;

   [SerializeField] private GameObject botaoAudioOn;
   [SerializeField] private GameObject botaoAudioOff;
   
   public void Jogar()
   {
      SceneManager.LoadScene(idCenaJogo);
   }
   
   public void Credits()
   {
      SceneManager.LoadScene(idCenaCreditos);
   }
   
   public void Sair()
   {
      Debug.Log("Sair do Jogo está funcional");
      Application.Quit();
   }

   public void AtivarAudio()
   {
      botaoAudioOn.SetActive(true); 
      botaoAudioOff.SetActive(false);
      
   }

   public void DesativarAudio()
   {
      botaoAudioOn.SetActive(false); 
      botaoAudioOff.SetActive(true);
   }
   
}
