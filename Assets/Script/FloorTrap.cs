using System.Collections;
using UnityEngine;

public class FloorTrap : MonoBehaviour
{
    GameObject joueur;
    public Animator anim;
    public float animationSpeed = 1.0f;
    public float activationDelay = 2.0f;
    private bool trapActivated = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !trapActivated)
        {
            StartCoroutine(ActivateTrap());
        }
    }
    private IEnumerator ActivateTrap()
    {
        trapActivated = true;
        anim.speed = animationSpeed;
        anim.SetTrigger("detect");
        Invoke("CloseTrap", 1f);
        yield return new WaitForSeconds(activationDelay);
        anim.SetBool("close", false);
        trapActivated = false;
    }
    
    private void CloseTrap()
    {
        anim.SetBool("close", true);
    }

    
}
