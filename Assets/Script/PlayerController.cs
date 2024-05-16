using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPunCallbacks
{
    public float sensi=25;
    
    public LayerMask layerMask;
    public bool grounded;
    private Rigidbody rb;
    public float speedplayer;
    public float forcesaut;
    public bool canMove = true;
    public Vector3 teleportPosition;
    public bool death = false;
    public bool isvoleur=false;

    public Text pseudotext;

    public string currentPseudo = "Player";
    public float activatedistance;

    public int score=0;
    public Texture2D cursorTexture; 

    public Camera CameraSpec;
    public Camera MainCam;


    public GameObject[] playerElements;

    public Animator[] animations;
    public GameObject[] skins;


    public Animator anim;


    [SerializeField] private AudioClip AudioDance=null;

    private AudioSource perso_AudioSource;


    private float Secondeleftdance=6f;

    public int jumpCount = 0;
    private int maxJumpCount = 2;

    public  bool isNinja=false;

    public bool isCaval=false;


    public Vector3 lastCheckpoint=new Vector3(-156.86f, 6.85f, 37.99f);

    
    public bool Deuxiemevie=false;

    PhotonView view;

    // Start is called before the first frame update
    void Start()
    {
        perso_AudioSource =GetComponent<AudioSource>();
        anim=animations[0];
        this.rb = GetComponent<Rigidbody>();
        view = GetComponent<PhotonView>();
        CameraSpec.enabled=false;
        if (view.IsMine)
        {
            pseudotext.enabled=false;
        }
        CameraSpec.enabled=false;
		
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
           ChangeSkin(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
           ChangeSkin(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeSkin(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChangeSkin(4);
        }
        
        if (view.IsMine)
        {

            Grounded();
            if (canMove)
            {
                Jump();
                Move();
                Dance();
            }

            Playdeath();
            if(anim.GetBool("danse")){
                Secondeleftdance-= Time.deltaTime;
                if(Secondeleftdance<=0f){
                    view.RPC("StopDanceRPC", RpcTarget.All);
                    

                }
            }
        }


    }
    public void changeName(string name){
        view.RPC("SyncPseudo", RpcTarget.All, name);
    }
    [PunRPC]
    private void SyncPseudo(string newPseudo)
    {
        // Mettre à jour le pseudo actuel
        currentPseudo = newPseudo;
        // Mettre à jour le pseudo localement
        UpdatePseudoLocally(newPseudo);
    }
    private void UpdatePseudoLocally(string newPseudo)
    {
        // Mettre à jour le texte affichant le pseudo
        pseudotext.text = newPseudo;
    }

    // Lorsqu'un joueur rejoint la salle
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("nouveau joeur dans la room ");
        base.OnPlayerEnteredRoom(newPlayer);
        
        // Envoyer le pseudo actuel au nouveau joueur
        photonView.RPC("SyncPseudo", newPlayer, currentPseudo);

        int currentSkinIndex = GetCurrentSkinIndex();
        ChangeSkin(currentSkinIndex);
    }






    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (grounded || (isNinja && jumpCount < maxJumpCount))
            {
                view.RPC("JumpRPC", RpcTarget.All);
                jumpCount++;
            }
        }


    }

    [PunRPC] 
    private void JumpRPC()
    {
        Debug.Log(jumpCount);
        if(jumpCount==0){
            this.rb.AddForce(Vector3.up * forcesaut, ForceMode.Impulse);
            
        }
        else{
            this.rb.AddForce(Vector3.up *( forcesaut+0), ForceMode.Impulse);
            Debug.Log("gros saut");
        }
        
        
        this.anim.Play("Jump");
        if(anim.GetBool("danse")){
            view.RPC("StopDanceRPC", RpcTarget.All);
        }
        
    }

    public void Dance(){
        
        if(Input.GetKeyDown(KeyCode.P) && this.grounded){
            view.RPC("DanceRPC", RpcTarget.All);
            
            
        }
        
    }
    [PunRPC] 
    public void DanceRPC() 
    {
        if (anim.GetBool("danse")) 
        {

        }
        else 
        {
            anim.SetBool("danse", true);
            perso_AudioSource.PlayOneShot(AudioDance);

        }
    }

    [PunRPC] 
    public void StopDanceRPC(){
        anim.SetBool("danse", false);
        perso_AudioSource.Stop();
        Secondeleftdance = 6f;
    }




    private void Grounded()
    {
        if (Physics.CheckSphere(this.transform.position + Vector3.down, 0.2f, layerMask))
        {
            this.grounded = true;
            jumpCount=0;
        }
        else
        {
            this.grounded = false;
        }

    }


    private void Move()
    {
        float verticalAxis = Input.GetAxis("Vertical");
        float horizontalAxis = Input.GetAxis("Horizontal");

        if (canMove)
        {
            Vector3 movement = this.transform.forward * verticalAxis + this.transform.right * horizontalAxis;
            movement.Normalize();

            rb.MovePosition(transform.position + movement * speedplayer * Time.fixedDeltaTime);

            this.anim.SetFloat("Vertical", verticalAxis);
            this.anim.SetFloat("Horizontal", horizontalAxis);
            if (movement != Vector3.zero &&anim.GetBool("danse") )
            {
            view.RPC("StopDanceRPC", RpcTarget.All);
            }
            
        }
        

    }




    private void Playdeath()
{
    if (death)
    {
        death = false;
        anim.Play("Death");

        // Attendre 2 secondes
        StartCoroutine(WaitForDeathAnimation());
    }
}

private IEnumerator WaitForDeathAnimation()
{
    yield return new WaitForSeconds(2f);

    // Code à exécuter après l'attente
    if (canMove)
    {

        if (isCaval && Deuxiemevie)
        {

            transform.position = lastCheckpoint;

            Deuxiemevie = false;
            death = false;
        }
        else
        {

            canMove = false;
            GameManager.instance.photonView.RPC("VoleurDead", RpcTarget.AllBuffered, this.photonView.ViewID);
            //faut attendre un peu ici 
            MainCam.enabled = false;
            CameraSpec.enabled = true;
            view.RPC("ActiveModeSpec", RpcTarget.All);
        }
    }
}

    [PunRPC] 
    public void ActiveModeSpec(){
        foreach (GameObject element in playerElements)
        {
            element.SetActive(false);
        }
        
        PlayerController? playeralive=GameManager.instance.GetPlayerControllerNotDeath();
        if(playeralive is not null){
            CameraSpec.transform.position=playeralive.MainCam.transform.position;
        }
    }


    [PunRPC] 
    public void  DesactiveModeSpec(){

        CameraSpec.enabled=false;
        MainCam.enabled=true;
        foreach (GameObject element in playerElements)
        {
            element.SetActive(true);
        }
    }
    public void ChangeSkin(int indice_skin){
        view.RPC("NewSkinforall", RpcTarget.All,indice_skin);
    }

    [PunRPC]
    public void NewSkinforall(int indice_skin){
        foreach(GameObject skin in skins){
            skin.SetActive(false);
        }
        skins[indice_skin].SetActive(true);
        anim=animations[indice_skin];
    }

    private int GetCurrentSkinIndex()
    {
        for (int i = 0; i < skins.Length; i++)
        {
            if (skins[i].activeSelf)
            {
                return i;
            }
        }
        return 0;
    }

    
}
