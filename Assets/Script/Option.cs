using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Option : MonoBehaviourPunCallbacks
{
    public GameObject Panel;
    public GameObject Panelcommande;
    public PlayerController playerController;
    public CameraController cameraController;
    public Crosshair crosshair;
    private bool scenecommande = false;
    bool visible = false;
    public Slider slider;
    public Text TxtSensi;

    

    public AudioController audio;


    // Update is called once per frame

    void Start()
    {
        Panelcommande.SetActive(false);
        Panel.SetActive(false);
        slider.value = 0.6f;

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            crosshair.showCrosshair = false;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            crosshair.showCrosshair = true;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            Cursor.visible = false;
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Cursor.visible = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (scenecommande)
            {
                Panelcommande.SetActive(false);
                scenecommande = false;
                Cursor.visible = false;
                crosshair.showCrosshair = false;
            }
            else
            {
                if (visible)
                {
                    Panel.SetActive(false);
                    visible = false;
                    Cursor.visible = false;
                    playerController.canMove = true;
                    crosshair.showCrosshair = true;
                }
                else
                {
                    Panel.SetActive(true);
                    visible = true;
                    Cursor.visible = true;
                    crosshair.showCrosshair = false;
                    if (playerController.canMove)
                    {
                        playerController.canMove = false;
                    }
                }
            }
        }

        Sliderchange();
    }
    public void Affichecommande()
    {
        crosshair.showCrosshair = false;
        Panelcommande.SetActive(true);
        scenecommande = true;
        Cursor.visible = true;
    }

    public void DisconnectPlayer()
    {

        audio.LeaveRoomSound();

        StartCoroutine(DisconnectAndLoad());
    }

    IEnumerator DisconnectAndLoad()
    {
        yield return new WaitForSeconds(1.6f);
        PhotonNetwork.LeaveRoom();

        while (PhotonNetwork.InRoom)
        {
            yield return null;
        }

        SceneManager.LoadScene("Lobby");
    }

    public void sensichanger()
    {


    }

    public void Sliderchange()
    {
        cameraController.sensitive = slider.value * 500;
        TxtSensi.text = "Sensibilite " + (slider.value * 100).ToString("00") + "% :";
    }

}
