using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class choixPerso : MonoBehaviour
{
    public PlayerController playerController;

    public void Start()
    {
        playerController.canMove = false;
    }

    public void Voleur()
    {
        playerController.transform.position = new Vector3(-17.52f,4.96f,30.62f);
        playerController.canMove = true;

    }

    public void Vigil()
    {
        playerController.transform.position = new Vector3(-21.19f, 3.21f, 20.36f);
        playerController.speedplayer += 2;
        playerController.canMove = true;
    }
}
