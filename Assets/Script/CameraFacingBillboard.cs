using UnityEngine;

public class CameraFacingBillboard : MonoBehaviour
{



    


    // Update is called once per frame
     void Update()
    {

        
        Camera cam =Camera.main;
        if(cam is not null){
            transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,    cam.transform.rotation * Vector3.up);
        }
        
    }

}
