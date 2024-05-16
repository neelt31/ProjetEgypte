using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class boutontombe : MonoBehaviourPun
{
   private Renderer rend;

    // Variable pour stocker la couleur de l'objet
    public string couleurObjet;

    // D�finissez vos deux mat�riaux dans l'inspecteur Unity
    public Material materiauVert;
    public Material materiauRouge;

    void Start()
    {
        // R�cup�rer le Renderer de l'objet
        rend = GetComponent<Renderer>();

        // Appeler la fonction pour changer le mat�riau de l'objet
        ChangerMateriauObjet();
    }

    void Update()
    {
        if (photonView.IsMine) // V�rifie si c'est le joueur local
        {
            // Envoyer la couleur actuelle du bouton aux autres joueurs
            photonView.RPC("SynchroniserCouleurObjet", RpcTarget.OthersBuffered, couleurObjet);
        }
    }

    [PunRPC]
    void SynchroniserCouleurObjet(string nouvelleCouleur)
    {
        // Mettre � jour la couleur du bouton avec la nouvelle couleur re�ue
        couleurObjet = nouvelleCouleur;
        ChangerMateriauObjet();
    }

    void ChangerMateriauObjet()
    {
        // Utilisez un bloc switch pour basculer entre les mat�riaux en fonction de la valeur de couleurObjet
        switch (couleurObjet)
        {
            case "vert":
                rend.material = materiauVert;
                break;
            case "rouge":
                rend.material = materiauRouge;
                break;
            // Ajoutez d'autres cas si n�cessaire pour d'autres couleurs
            default:
                // Mat�riau par d�faut si la couleur n'est pas reconnue
                rend.material = materiauVert;
                break;
        }
    }
}
