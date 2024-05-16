using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnPlayers : MonoBehaviourPunCallbacks
{

    public GameObject playerPrefab;
    public Option option;

    public MenuManager menu;

    public AudioController audio;

    
    




    void Start()
    {
        Vector3 persoPosition = new Vector3(-20, 8, -29);
        GameObject Player = PhotonNetwork.Instantiate(playerPrefab.name, persoPosition, Quaternion.identity);
        option.playerController = Player.GetComponent<PlayerController>();



        menu.playerController=Player.GetComponent<PlayerController>();
        audio.perso_AudioSource=Player.GetComponent<AudioSource>();
        PlayerController pc=Player.GetComponent<PlayerController>();
        
        
        Camera playerCamera = Player.GetComponentInChildren<Camera>();
        option.cameraController = playerCamera.GetComponent<CameraController>();
        menu.crosshair = playerCamera.GetComponent<Crosshair>();
        option.crosshair = playerCamera.GetComponent<Crosshair>();
        GameManager.instance.photonView.RPC("NewPlayer", RpcTarget.AllBuffered,pc.photonView.ViewID);
    }

}
