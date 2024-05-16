using System.Collections;
using UnityEngine;

public class trapController : MonoBehaviour
{
    GameObject joueur;
    public Animator anim;
    public float animationSpeed = 1.0f;
    public float activationDelay = 2.0f;
    private bool trapActivated = false; 
    public Vector3 teleportPosition; 
    public bool playerInZone = false; 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !trapActivated)
        {
            StartCoroutine(ActivateTrap());
            playerInZone = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (playerInZone && collision.gameObject.CompareTag("Walltrap"))
        {
            TeleportPlayer(collision.gameObject); 
        }
    }

    private IEnumerator ActivateTrap()
    {
        trapActivated = true;
        anim.speed = animationSpeed;
        anim.SetTrigger("detect");
        yield return new WaitForSeconds(activationDelay);
        trapActivated = false;
    }

    private void TeleportPlayer(GameObject player)
    {
        player.transform.position = teleportPosition;
        playerInZone = false; 
    }
}
