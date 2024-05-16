using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class corner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Empêcher le joueur de monter en le repoussant vers le bas
            Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                playerRigidbody.AddForce(Vector3.down * 10f, ForceMode.Impulse);
            }
        }
    }
}
