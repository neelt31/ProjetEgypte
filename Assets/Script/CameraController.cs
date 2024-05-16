using UnityEngine;
using Photon.Pun;

public class CameraController : MonoBehaviourPun
{
    public GameObject playerCamera;
    public float sensitive = 50;
    private float ClampAngle = 60f;

    private float verticalRotation;
    private float horizontalRotation;

    public float sensibilite;

    private PlayerController playerController; // Référence au contrôleur du joueur

    void Start()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            playerCamera.SetActive(false);
        }

        this.verticalRotation = this.transform.localEulerAngles.x;
        this.horizontalRotation = this.transform.localEulerAngles.y;

        // Obtenir une référence au contrôleur du joueur
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerController = playerObject.GetComponent<PlayerController>();
        }
    }

    void Update()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            return;
        }

        Look();
        Debug.DrawRay(this.transform.position, this.transform.forward * 2, Color.red);
    }

    private void Look()
    {
        float mouseVertical = -Input.GetAxis("Mouse Y");
        float mouseHorizontal = Input.GetAxis("Mouse X");

        if (playerController != null && playerController.canMove)
        {
            this.verticalRotation += mouseVertical * (this.sensitive / 4) * Time.deltaTime;
            this.horizontalRotation += mouseHorizontal * this.sensitive * Time.deltaTime;
        }

        this.verticalRotation = Mathf.Clamp(this.verticalRotation, -this.ClampAngle / 1.2f, this.ClampAngle);

        this.transform.localRotation = Quaternion.Euler(this.verticalRotation, 0f, 0f);
        transform.parent.rotation = Quaternion.Euler(0f, this.horizontalRotation, 0f);
    }
}
