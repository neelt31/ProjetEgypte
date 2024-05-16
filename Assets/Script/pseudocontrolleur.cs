using UnityEngine;

public class Billboard : MonoBehaviour
{
    void Update()
    {
        // Orienter le texte du pseudonyme vers la cam�ra
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
            Camera.main.transform.rotation * Vector3.up);
    }
}