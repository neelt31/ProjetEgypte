using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("osssssseeee");
        if (other.CompareTag("Player"))
        {
            Debug.Log("oeeee");
            PlayerController playerController = other.GetComponent<PlayerController>();
            playerController.lastCheckpoint=transform.position;

        }
    }
}
