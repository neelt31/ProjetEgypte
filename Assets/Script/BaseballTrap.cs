using System.Collections;
using UnityEngine;
using Photon.Pun;

public class BaseballTrap : MonoBehaviourPun, IInteractable
{
    public cubechargementscript Pilier;
    public changerbouton bouton;
    public Animator anim;
    public Animator animbutton;
    public float animationSpeed = 1.0f;
    public float activationDelay = 9.0f;
    private bool trapActivated = false;

    public GameObject ImagePRess;
    void Start()
    {
        bouton.couleurObjet = "vert";
        ImagePRess.SetActive(true);
    }




    public void Interact()
    {

        if (bouton.couleurObjet == "vert" && !trapActivated)
        {
            photonView.RPC("ActivateTrapRPC", RpcTarget.All);
        }
    }

    [PunRPC]
    private void ActivateTrapRPC()
    {
        StartCoroutine(ActivateTrap());
        trapActivated = true;
        bouton.photonView.RPC("SynchroniserCouleurObjet", RpcTarget.All, "rouge");
        Pilier.photonView.RPC("AnnimPilier", RpcTarget.All);

    }

    private IEnumerator ActivateTrap()
    {
        animbutton.SetBool("push", true);
        anim.speed = animationSpeed;
        anim.SetBool("uplav", true);
        Invoke("CloseTrap", 5f);
        yield return new WaitForSeconds(activationDelay);
        anim.SetBool("downlav", false);
        trapActivated = false;
        bouton.couleurObjet = "vert";
        bouton.photonView.RPC("SynchroniserCouleurObjet", RpcTarget.All, "vert");
    }

    private void CloseTrap()
    {
        anim.SetBool("downlav", true);
        animbutton.SetBool("push", false);
        anim.SetBool("uplav", false);


    }
}
