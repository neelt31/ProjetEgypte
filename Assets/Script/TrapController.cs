using System.Collections;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    public GameObject CubeMoov;
    public Animator anim;

    void OnTriggerEnter()
    {
        anim.SetBool("detect", true);
    }

}