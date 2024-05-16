using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioClip AudioLeave;
    [SerializeField] private AudioClip AudioWin;

    [SerializeField] private AudioClip AudioStart;
    public  AudioSource perso_AudioSource;

    
    // Start is called before the first frame update


    // Update is called once per frame
    public void LeaveRoomSound(){
        perso_AudioSource.PlayOneShot(AudioLeave);
    }

    public void VictoireSound(){
        perso_AudioSource.PlayOneShot(AudioWin);
        
    }

    public void StartSound(){
        perso_AudioSource.PlayOneShot(AudioStart);
        Debug.Log("oe");
    }
}
