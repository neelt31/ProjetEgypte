using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChoixVigil : MonoBehaviourPunCallbacks
{
    public MenuManager menu;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            playerController.transform.position = new Vector3(-108.44f, 10.54f, 29.62f);
            playerController.speedplayer += 15;
            menu.isvigil=true;
            menu.isvoleur=false;

            GameManager.instance.photonView.RPC("AddPlayerToVigilesList", RpcTarget.AllBuffered, playerController.photonView.ViewID);
            menu.ModeChoixskin("vigil");

        }
    }
}
