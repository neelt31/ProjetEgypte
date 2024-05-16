using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using Photon.Realtime;

public class SoloController : MonoBehaviour
{
    public bool modesoloactive=false;
    public float SecondeRestante=60f;
    public Text TextDecompte;
    public GameObject MurTransparent;
    public Text Timer;
    public bool active=false;

    public AudioController audio;

    public bool started=false;

    public GameObject Bot;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !started && modesoloactive)
        {
            started=true;
            StartGame();
        }
        UpdateTimerDisplay();
    }
    void StartGame()
    {
        
        TextDecompte.enabled=true;
        // Lance la coroutine pour le décompte
        StartCoroutine(DecompteCoroutine());
        
    }
    private IEnumerator StartCountdown()
    {
        while (SecondeRestante > 0 )
        {
            // Diminue le temps restant
            SecondeRestante -= Time.deltaTime;


            // Attend la fin du frame avant de réduire le temps restant à nouveau
            yield return null;
        }
        Timer.enabled=false;
        // Le temps est écoulé, vous pouvez déclencher des actions supplémentaires ici si nécessaire

    }
    private void UpdateTimerDisplay()
    {
        // Met à jour l'affichage du décompte

        Timer.text = FormatTime(SecondeRestante);
        Debug.Log(Timer.text);
    }
    private string FormatTime(float timeInSeconds)
    {
        // Convertit le temps en minutes et secondes
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);

        // Formate le temps dans le format mm:ss
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    IEnumerator DecompteCoroutine()
    {
        float decompteTimer = 4f;
        
        while (decompteTimer > 0f)
        {

            TextDecompte.text = Mathf.CeilToInt(decompteTimer).ToString() +".."; 
            yield return new WaitForSeconds(1f);

            // Diminue le timer
            decompteTimer -= 1f;
        }
        TextDecompte.text = "Go !!!";
        audio.StartSound();
        yield return new WaitForSeconds(1f);
        TextDecompte.enabled=false;
        MurTransparent.SetActive(false);
        Timer.enabled=true;
        Bot.SetActive(true);
        StartCoroutine(StartCountdown());
    }


    
}
