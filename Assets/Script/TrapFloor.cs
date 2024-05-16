using System.Collections;
using UnityEngine;
using Photon.Pun;

public class TrapFloor : MonoBehaviourPun, IInteractable
{
    public cubechargementscript Pilier;
    public changerbouton bouton;
    public GameObject[] blocks; 
    public Animator animbutton;
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
        foreach (GameObject block in blocks)
        {
            block.SetActive(false);
        }
        Invoke("ReapperBlock", 3f);
        yield return new WaitForSeconds(activationDelay);
        trapActivated = false;
        bouton.couleurObjet = "vert";
        bouton.photonView.RPC("SynchroniserCouleurObjet", RpcTarget.All, "vert");
    }

    private void ReapperBlock()
    {
        foreach (GameObject block in blocks)
        {
            block.SetActive(true);
        }
        animbutton.SetBool("push", false);

    }
}
