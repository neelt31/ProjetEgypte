using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CreateAndJoinRoom : MonoBehaviourPunCallbacks
{
    public InputField createInput;
    public InputField joinInput;

    

    

    public void CreateRoom()
    {
        string roomName = createInput.text.ToUpper(); // Convertit le texte en majuscules
        if (!string.IsNullOrWhiteSpace(roomName) && roomName.Length < 10 && !ContainsSpace(roomName)) // Vérifie s'il y a un texte valide, sa longueur est inférieure à 10 et ne contient pas d'espace
        {
            PhotonNetwork.CreateRoom(roomName);
        }
    }

    bool ContainsSpace(string text)
    {
        foreach (char c in text)
        {
            if (c == ' ')
            {
                return true;
            }
        }
        return false;
    }

    public void JoinRoom() 
    {
        string roomName = joinInput.text.ToUpper();
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Jeu");
    }
	public void LeaveGame()
    {
        PhotonNetwork.LeaveRoom();
        Application.Quit();
	}



}

