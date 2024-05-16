using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;
    public GameState State;
    public int playerCount;

    [SerializeField]public List<PlayerController> Voleurs{get ;set;}
    [SerializeField]public List<PlayerController> Vigiles{get ;set;}
    [SerializeField]public List<PlayerController> Players{get ;set;}

    public TextMeshProUGUI ScoreBoard1er2eme;
    public TextMeshProUGUI ScoreBoardPlayer;
    public TextMeshProUGUI ScoreBoardPoint;



    public int nbvoleurdead;
    public static event System.Action<GameState> OnGameStateChanged;
    public bool roomstarted=false;
    //--------------------------------------------------------------------------------------------------//

    void Awake()
    {
        instance = this;
        Voleurs = new List<PlayerController>();
        Vigiles = new List<PlayerController>();
        Players = new List<PlayerController>();
        nbvoleurdead=0;
    }

    void Start()
    {
        UpdateNewState(GameState.Choixrole);
    }

    public void NewVigile(PlayerController vigile)
    {
        Vigiles.Add(vigile);
    }

    public void NewVoleur(PlayerController voleur)
    {
        Voleurs.Add(voleur);

    }

    public void UpdateNewState(GameState newState)
    {   if(newState!=GameState.Clown)
            State = newState;
        if(!roomstarted && newState!=GameState.Choixrole && newState!=GameState.Clown){
            roomstarted=true;
    
        }
        if(newState==GameState.Choixrole){
            foreach(PlayerController player in Voleurs){
                player.death=false;
                player.canMove=true;
                photonView.RPC("RPC_UpdateScoreBoard", RpcTarget.All);
            }
            foreach(PlayerController player2 in Vigiles){
                player2.death=false;
                player2.canMove=true;

            }
            Voleurs = new List<PlayerController>();
            Vigiles = new List<PlayerController>();
            nbvoleurdead=0;
        }
        if(newState ==GameState.Victoirevoleur){
            foreach(PlayerController voleur in Voleurs){
                voleur.score+=Voleurs.Count;
            }
            foreach(PlayerController vigil in Vigiles){
                vigil.score+=nbvoleurdead;
            }
            
        }
        
        
        if(newState ==GameState.Victoirevigil){
            foreach(PlayerController vigil in Vigiles)
                {
                        vigil.score+=nbvoleurdead;
                }   
        }
        // Envoie un événement de changement d'état à tous les clients
        photonView.RPC("RPC_UpdateGameState", RpcTarget.All, newState);
    }

    [PunRPC]
    private void RPC_UpdateGameState(GameState newState)
    {
        State = newState;
        OnGameStateChanged?.Invoke(newState);
    }
    [PunRPC]
    public void RPC_activeCapaClown(){
        UpdateNewState(GameState.Clown);
    }
    
    [PunRPC]
    private void RPC_UpdateScoreBoard()
    {
        // Trie les joueurs par score, du plus élevé au plus bas
        List<PlayerController> sortedPlayers = Players.OrderByDescending(player => player.score).ToList();

        // Initialise une variable pour stocker le texte du tableau des scores
        string rankText = "";
        string playerText = "";
        string scoreText = "";

        // Parcourt les joueurs triés pour les ajouter au texte du tableau des scores avec leur classement, pseudo et score
        for (int i = 0; i < sortedPlayers.Count; i++)
        {
            // Classement du joueur (1er, 2eme, 3eme, etc.)
            string rank = (i + 1) switch
            {
                1 => "1er     :",
                2 => "2eme :",
                3 => "3eme :",
                _ => (i + 1) + "eme :",
            };

            // Ajoute le classement, le pseudo et le score à leur propre chaîne de texte
            rankText += $"{rank}\n\n";
            playerText += $"{sortedPlayers[i].currentPseudo}\n\n";
            scoreText += $"{sortedPlayers[i].score}\n\n";
        }

        // Met à jour le texte du tableau des scores dans l'interface utilisateur
        ScoreBoard1er2eme.text = rankText;
        ScoreBoardPlayer.text = playerText;
        ScoreBoardPoint.text = scoreText;

        // Affiche le tableau des scores dans la console à des fins de débogage
    }


    [PunRPC]
    public void NewPlayer(int playerViewID)
    {
        // Trouver le joueur avec l'ID de vue spécifié
        PhotonView playerView = PhotonView.Find(playerViewID);
        if (playerView != null)
        {
            PlayerController player = playerView.GetComponent<PlayerController>();
            if (player != null)
            {
                Players.Add(player);
            }
            else
            {
                Debug.LogError("Le composant PlayerController n'a pas été trouvé sur le joueur.");
            }
        }
        else
        {
             Debug.LogError("La vue Photon avec l'ID spécifié n'a pas été trouvée.");
        }
    }

    [PunRPC]
    public void AddPlayerToVoleursList(int playerViewID)
    {
        // Trouver le joueur avec l'ID de vue spécifié
        PhotonView playerView = PhotonView.Find(playerViewID);
        if (playerView != null)
        {
            PlayerController player = playerView.GetComponent<PlayerController>();
            if (player != null)
            {
                NewVoleur(player);
            }
        }
    }

    [PunRPC]
    public void AddPlayerToVigilesList(int playerViewID)
    {
        PhotonView playerView = PhotonView.Find(playerViewID);
        if (playerView != null)
        {
            PlayerController player = playerView.GetComponent<PlayerController>();
            if (player != null)
            {
                NewVigile(player);
            }
        }
    }

    [PunRPC]
    public void VoleurDead (int playerViewID){
        PhotonView playerView = PhotonView.Find(playerViewID);
        if (playerView != null)
        {
            PlayerController player = playerView.GetComponent<PlayerController>();
            if (player != null)
            {
                nbvoleurdead++;
                player.death=true;
                if(nbvoleurdead==Voleurs.Count)
                {
                    
                    UpdateNewState(GameState.Victoirevigil);
                    
                }
            }
            
        }
    }

    [PunRPC]
    public void WinVoleur(){
         UpdateNewState(GameState.Victoirevoleur);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (State!=GameState.Choixrole)
        {
            // Informer le nouveau joueur que la partie est en cours
            photonView.RPC("InformGameStarted", newPlayer);
        }
        Debug.Log("oe");
        photonView.RPC("RPC_UpdateScoreBoard", RpcTarget.All);
    }
    
    [PunRPC]
    private void InformGameStarted()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Lobby");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
    
        // Récupérer le joueur qui a quitté la chambre
        PlayerController pc = GetPlayerController(otherPlayer.UserId);
    
        // Vérifier si le PlayerController existe
        if (pc != null)
        {
        
            // Appeler la RPC pour signaler que le joueur a quitté, en passant le UserId
            photonView.RPC("RPCPlayerLeave", RpcTarget.All, otherPlayer.UserId);
        }   
    }

    [PunRPC]
    public void RPCPlayerLeave(string userId)
    {
        // Trouver le joueur avec le UserId spécifié
        PlayerController player = GetPlayerController(userId);
        if (player != null)
        {
            Players.Remove(player);
            if (Voleurs.Contains(player))
        {
            Voleurs.Remove(player);
        }

        // Vérifier s'il est un vigile et le retirer de la liste des vigiles si c'est le cas
        if (Vigiles.Contains(player))
        {
            Vigiles.Remove(player);
        }
        }
        else
        {
            Debug.LogError("Le composant PlayerController n'a pas été trouvé pour le joueur avec l'UserId : " + userId);
        }
    }
    
    private PlayerController GetPlayerController(string userId)
    {
        // Parcourir tous les PlayerControllers dans la liste Players
        foreach (PlayerController pc in Players)
        {
            // Vérifier si le PlayerController correspond au joueur spécifié
            if (pc.photonView.Owner.UserId == userId)
            {
                // Si le PlayerController correspond, le retourner
                return pc;
            }
        }
        return null;
    }
    public  PlayerController GetPlayerControllerNotDeath()
    {
        // Parcourir tous les PlayerControllers dans la liste Players
        foreach (PlayerController pc in Players)
        {
            // Vérifier si le PlayerController correspond au joueur spécifié
            if(!pc.death){
                return pc;
            }
        }
        return null;
    }


}

public enum GameState
{
    Choixrole,
    encour,
    Victoirevoleur,
    Victoirevigil,
    Clown,
}
