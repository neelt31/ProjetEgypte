using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Piegepic1 : MonoBehaviourPun, IInteractable
{
    public cubechargementscript Pilier;
    public changerbouton bouton;
    public Animator anim;
    public Animator animbutton;
    public float animationSpeed = 1.0f;
    public float activationDelay = 9.0f;
    private bool trapActivated = false;

    void Start()
    {
        bouton.couleurObjet = "vert";
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
        anim.SetTrigger("detect");
        Invoke("CloseTrap", 2f);
        yield return new WaitForSeconds(activationDelay);
        anim.SetBool("close", false);
        trapActivated = false;
        bouton.couleurObjet = "vert";
        bouton.photonView.RPC("SynchroniserCouleurObjet", RpcTarget.All, "vert");
    }

    private void CloseTrap()
    {
        anim.SetBool("close", true);
        animbutton.SetBool("push", false);

    }
}
