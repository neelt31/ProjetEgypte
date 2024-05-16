using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;


public class Victory : MonoBehaviourPunCallbacks
{



    void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.photonView.RPC("WinVoleur", RpcTarget.AllBuffered);
            Cursor.visible = true;
        }
    }

    public void DisconnectPlayer()
    {
        StartCoroutine(DisconnectAndLoad());
    }

    IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.LeaveRoom();

        while (PhotonNetwork.InRoom)
        {
            yield return null;
        }

        SceneManager.LoadScene("Lobby");
    }

}
