using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using Photon.Realtime;
public class MenuManager : MonoBehaviourPunCallbacks
{
    public GameObject PanelChoixskinvigil;
    public GameObject PanelChoixskinvoleur;


    public GameObject PanelVigilWin;
    public GameObject PanelVoleurWin;
    public GameObject PanelPseudo;
    public GameObject MurTransparent;
    public Text TextDecompte;
    public PlayerController playerController;
    public GameState mygameState;
    public Crosshair crosshair;
    //timer

    public float SecondeRestante;
    public Text Timer;
    //pseudo

    public InputField pseudoinput;

    public AudioController audio;

    public bool isvigil=false;
    public bool isvoleur=false;

    //capa///

    public bool isclow=false;
    public GameObject PanelCLown;
    public bool ActiveCapa=true;
    public GameObject PanelteteClown;
    public GameObject ImageClown;

    public bool isNinja=false;

    public GameObject ImageSaut;



    public GameObject ImageCoeur;

    ////////

    void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }
    
    void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged(GameState state)
    {
        mygameState=state;
        if(state==GameState.encour){
            StartGame();
        }
        if(state==GameState.Victoirevoleur){
            Victoirdesvoleurs();
        }
        if(state==GameState.Victoirevigil){
            Victoirdesvigils();
        }
        if(state==GameState.Choixrole){
            MurTransparent.SetActive(true);
            SecondeRestante=100;
        }
        if(state==GameState.Clown){
            afficheScreenNoir();
        }
        
        
    }

    void Start()
    {
        PanelChoixskinvoleur.SetActive(false);
        PanelChoixskinvigil.SetActive(false);
        TextDecompte.enabled=false;
        Timer.enabled=false;
        MurTransparent.SetActive(true);
        PanelVigilWin.SetActive(false);
        PanelVoleurWin.SetActive(false);
        PanelPseudo.SetActive(true);
        ImageClown.SetActive(false);
        ImageSaut.SetActive(false);
    }
        
    void Update()
    {
        Debug.Log((GameManager.instance.Vigiles.Count,GameManager.instance.Voleurs.Count));
        if (Input.GetKeyDown(KeyCode.Return) && PhotonNetwork.IsMasterClient && mygameState==GameState.Choixrole )
        {
            if(GameManager.instance.Players.Count==(GameManager.instance.Vigiles.Count+GameManager.instance.Voleurs.Count)){
                GameManager.instance.UpdateNewState(GameState.encour);
                
            }
            else{
                Debug.Log("joueurs ayant role non attribue : "+ (GameManager.instance.Players.Count-GameManager.instance.Vigiles.Count-GameManager.instance.Voleurs.Count) );
            }
        }
        if (isclow && Input.GetKeyDown(KeyCode.F))
        {
            // Réalisez ici les actions que vous souhaitez effectuer lorsque la touche F est pressée
            Debug.Log("Touche F appuyée !");
            activecapa();
        }
        if(isNinja){
            UpdateImageDOubleSaut(ImageSaut);
        }
        UpdateImageCoeur(ImageCoeur);

        UpdateTimerDisplay();
    }

    void StartGame()
    {
        TextDecompte.enabled=true;
        // Lance la coroutine pour le décompte
        StartCoroutine(DecompteCoroutine());
        
    }



    private IEnumerator StartCountdown()
    {
        while (SecondeRestante > 0 && mygameState==GameState.encour)
        {
            // Diminue le temps restant
            SecondeRestante -= Time.deltaTime;

            // Attend la fin du frame avant de réduire le temps restant à nouveau
            yield return null;
        }
        if( SecondeRestante<=0){
            GameManager.instance.UpdateNewState(GameState.Victoirevigil);
        }
        Timer.enabled=false;
        // Le temps est écoulé, vous pouvez déclencher des actions supplémentaires ici si nécessaire

    }

    private void UpdateTimerDisplay()
    {
        // Met à jour l'affichage du décompte
        Timer.text = FormatTime(SecondeRestante);
    }

    private string FormatTime(float timeInSeconds)
    {
        // Convertit le temps en minutes et secondes
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);

        // Formate le temps dans le format mm:ss
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    IEnumerator DecompteCoroutine()
    {
        float decompteTimer = 4f;
        
        while (decompteTimer > 0f)
        {

            TextDecompte.text = Mathf.CeilToInt(decompteTimer).ToString() +".."; 
            yield return new WaitForSeconds(1f);

            // Diminue le timer
            decompteTimer -= 1f;
        }
        TextDecompte.text = "Go !!!";
        audio.StartSound();
        yield return new WaitForSeconds(1f);
        TextDecompte.enabled=false;
        MurTransparent.SetActive(false);
        Timer.enabled=true;
        Debug.Log("gi");
        StartCoroutine(StartCountdown());
    }


    void Victoirdesvoleurs()
    {
        

        Timer.enabled=false;
        PanelVoleurWin.SetActive(true); // Affiche le panneau des voleurs
        if(!isvigil){
            audio.VictoireSound();
        }
        
        if(playerController.death==true){
            playerController.DesactiveModeSpec();
            playerController.photonView.RPC("DesactiveModeSpec", RpcTarget.All);
        }
        StartCoroutine(ReturnPlayersAfterDelay(3f,PanelVoleurWin)); // Attend 5 secondes avant de retourner les joueurs à la position spécifiée
        playerController.ChangeSkin(0);

    }

    IEnumerator ReturnPlayersAfterDelay(float delay,GameObject panel)
    {
        yield return new WaitForSeconds(delay);

        // Récupère tous les joueurs

        // Ramène chaque joueur à la position spécifiée
        GameObject joueurLocal = GameObject.FindWithTag("Player");

        // Vérifier si le joueur local existe
        if (joueurLocal != null)
        {
            // Ramener le joueur local à la position spécifiée
            joueurLocal.transform.position = new Vector3(-20, 8, -29);
            isclow=false;
            isvigil=false;
            isvoleur=false;
            
        }

        else
        {
            Debug.LogError("Impossible de trouver le joueur local.");
        }
        panel.SetActive(false);
        GameManager.instance.UpdateNewState(GameState.Choixrole);


    }

    void Victoirdesvigils()
    {
        if(isvigil){
            audio.VictoireSound();
        }

        PanelVigilWin.SetActive(true); // Affiche le panneau des vgigil
        if(playerController.death==true){
            playerController.DesactiveModeSpec();
            playerController.photonView.RPC("DesactiveModeSpec", RpcTarget.All);
        }
        StartCoroutine(ReturnPlayersAfterDelay(3f,PanelVigilWin)); // Attend 5 secondes avant de retourner les joueurs à la position spécifiée
        playerController.ChangeSkin(0); 
    }

    public void changerpseudodemonjoueur()
    {
        if(pseudoinput.text.Length<=10)
        {
            string newPseudo = pseudoinput.text;
            playerController.changeName(newPseudo);
            PanelPseudo.SetActive(false);
            GameManager.instance.photonView.RPC("RPC_UpdateScoreBoard", RpcTarget.All);
            Cursor.visible = false;
            crosshair.showCrosshair = true;
            }

    }

    public void ModeChoixskin(string type){
        if(type=="vigil"){
            isvigil=true;
            changerskin(1);
        }
        else{
            PanelChoixskinvoleur.SetActive(true);
            isvigil=false;
        }
        crosshair.showCrosshair = false;
        Cursor.visible = true;
    }

    public void changerskin(int indiceskin){
        PanelteteClown.SetActive(true);
        if(indiceskin==2){
            isclow=true;
            ImageClown.SetActive(true);
        }
        if(indiceskin==3){
            isNinja=true;
            playerController.isNinja=true;
            ImageSaut.SetActive(true);
        }
        if(indiceskin==4){
            ImageCoeur.SetActive(true);
            playerController.isCaval=true;
            playerController.Deuxiemevie=true;
        }
        playerController.ChangeSkin(indiceskin);
        PanelChoixskinvigil.SetActive(false);
        PanelChoixskinvoleur.SetActive(false);
        crosshair.showCrosshair = true;
        Cursor.visible = false;
    }
    public void  TppostionVoleur(){
        playerController.transform.position = new Vector3(-183.96f, -7.68f, 89.98f);
    }

    public void activecapa(){
        GameManager.instance.photonView.RPC("RPC_activeCapaClown", RpcTarget.All);
        ActiveCapa=false;
        StartTransition(ImageClown,8f);
    }

    public void afficheScreenNoir()
    {
    StartCoroutine(AfficherPanneauNoir());
    }

    private IEnumerator AfficherPanneauNoir()
    {
        PanelCLown.SetActive(isvigil);
        yield return new WaitForSeconds(3f); // Attendre 3 secondes
        PanelCLown.SetActive(false); // Désactiver le panneau après 3 secondes
    }
    public void StartTransition(GameObject targetObject, float duration)
    {
        StartCoroutine(TransitionCoroutine(targetObject, duration));

    }

    private IEnumerator TransitionCoroutine(GameObject targetObject, float duration)
    {
        CanvasGroup canvasGroup = targetObject.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = targetObject.AddComponent<CanvasGroup>();
        }

        float time = 0;

        // Set initial alpha to 0.2
        canvasGroup.alpha = 0.2f;

        // Transition to opaque
        while (time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(0.2f, 0.7f, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1;
        ActiveCapa = true;
        Debug.Log("cest bon");
    }

    public void UpdateImageDOubleSaut(GameObject targetObject)
    {
       


        CanvasGroup canvasGroup = targetObject.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = targetObject.AddComponent<CanvasGroup>();
        }

        // Set initial alpha to 0.2
        canvasGroup.alpha = 0.3f;

        // Transition to opaque
        if(playerController.jumpCount<1)
        {   
            
            canvasGroup.alpha = 1;

        }
    }
    public void UpdateImageCoeur(GameObject targetObject)
    {
       


        CanvasGroup canvasGroup = targetObject.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = targetObject.AddComponent<CanvasGroup>();
        }

        // Set initial alpha to 0.2
        canvasGroup.alpha = 0.3f;

        // Transition to opaque
        if(playerController.Deuxiemevie)
        {   
            
            canvasGroup.alpha = 1;

        }
    }
        

    


    
}
