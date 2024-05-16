
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChoixVoleur : MonoBehaviourPunCallbacks
{
    public MenuManager menu;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            playerController.transform.position = new Vector3(-1700.52f, 4.96f, 30.62f);
            menu.isvoleur=true;
            menu.isvigil=false;
            GameManager.instance.photonView.RPC("AddPlayerToVoleursList", RpcTarget.AllBuffered, playerController.photonView.ViewID);

            menu.ModeChoixskin("voleur");
            
        }
    }
    public void DebutvoleurPosition(){
        transform.position = new Vector3(-17.52f, 4.96f, 30.62f);
    }
}
