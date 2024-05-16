using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class cubechargementscript : MonoBehaviourPun
    
{
    public float temps = 0f;
    public float maxtemps = 10f;

    public Image healthBarImage1;
    public Image healthBarImage2;
    public Image healthBarImage3;
    public Image healthBarImage4;




    [PunRPC]
    public void AnnimPilier()
    {
        // Réinitialise la valeur de temps
        temps = 0f;
        Debug.Log("cest partie");
        // Démarre la coroutine pour l'animation
        StartCoroutine(AnimatePillar());
    }

    IEnumerator AnimatePillar()
    {
        // Temps total pour l'animation (10 secondes)
        float animationDuration = 9f;

        // Temps écoulé pendant l'animation
        float elapsedTime = 0f;

        // Boucle jusqu'à ce que le temps écoulé atteigne la durée totale de l'animation
        while (elapsedTime < animationDuration)
        {
            // Calcule la progression de l'animation en fonction du temps écoulé
            float progress = elapsedTime / animationDuration;

            // Met à jour la valeur fillAmount proportionnellement à la progression
            healthBarImage1.fillAmount = progress;
            healthBarImage2.fillAmount = progress;
            healthBarImage3.fillAmount = progress;
            healthBarImage4.fillAmount = progress;

            // Incrémente le temps écoulé avec le temps écoulé depuis le dernier frame
            elapsedTime += Time.deltaTime;

            // Attend la fin du frame avant de continuer à la prochaine itération de la boucle
            yield return null;
        }

        // Assure que la barre de chargement soit remplie à 100% à la fin de l'animation
        healthBarImage1.fillAmount = 1f;
        healthBarImage2.fillAmount = 1f;
        healthBarImage3.fillAmount = 1f;
        healthBarImage4.fillAmount = 1f;
    }
}