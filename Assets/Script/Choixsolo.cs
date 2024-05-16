using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choixsolo : MonoBehaviour
{
    // Start is called before the first frame update
    public MenuManager menu;
    public SoloController solo;

    public SoloController sc;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(GameManager.instance.Players.Count==1)
            {
                PlayerController playerController = other.GetComponent<PlayerController>();
                playerController.transform.position = new Vector3(-21.19f, 3.21f, 20.36f);
                playerController.speedplayer += 2;
                menu.ModeChoixskin("vigil");
                solo.active=true;
                sc.modesoloactive=true;
            }
        }
    }
}
