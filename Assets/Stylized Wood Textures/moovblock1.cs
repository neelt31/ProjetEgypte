using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moovblock1 : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float speed = 2.0f;
    private bool movingToEnd = true;

    void Start()
    {
        transform.position = startPosition;
    }

    void FixedUpdate()
    {
        // Détermine la direction du mouvement en fonction de la position actuelle
        Vector3 targetPosition = movingToEnd ? endPosition : startPosition;

        // Déplace la plateforme vers la position cible
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Vérifie si la plateforme est arrivée à sa destination
        if (transform.position == targetPosition)
        {
            // Inverse la direction du mouvement
            movingToEnd = !movingToEnd;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Vérifie si l'objet entrant est le joueur
        if (other.CompareTag("Player"))
        {
            // Attache le joueur au bloc
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Vérifie si l'objet sortant est le joueur
        if (other.CompareTag("Player"))
        {
            // Détache le joueur du bloc
            other.transform.SetParent(null);
        }
    }
}
