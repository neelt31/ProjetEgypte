using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameralaser : MonoBehaviour
{
    public GameObject visualizerPrefab; // Préfabriqué pour le rayon visuel
    public float raycastDistance = 10f; // Distance maximale du rayon

    private GameObject visualizer; // Objet visuel du rayon

    void Start()
    {
        // Instancie l'objet visuel du rayon
        visualizer = Instantiate(visualizerPrefab);
        visualizer.SetActive(false); // Désactive l'objet visuel au départ
    }

    void Update()
    {
        // Lance un rayon depuis la position de la caméra dans la direction de son regard
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        // Déclare une variable pour stocker les informations de collision
        RaycastHit hit;

        // Vérifie si le rayon touche quelque chose
        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // Active l'objet visuel et positionne-le sur le point de collision
            visualizer.SetActive(true);
            visualizer.transform.position = hit.point;
            visualizer.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }
        else
        {
            // Si le rayon ne touche rien, désactive l'objet visuel
            visualizer.SetActive(false);
        }
    }
}
