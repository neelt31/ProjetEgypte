using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


interface IInteractable
{
    public void Interact();
}


public class Interactor : MonoBehaviour
{
    public Transform interactorSource;
    public LayerMask interactableLayer;
    public float interactRange = 3f;
    public bool detect = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(interactorSource.position, interactorSource.forward, out hit, interactRange, interactableLayer))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }

        detect = Physics.Raycast(interactorSource.position, interactorSource.forward, interactRange, interactableLayer);

    }

    void OnDrawGizmos()
    {
        Gizmos.color = detect ? Color.green : Color.red; 
        Gizmos.DrawLine(interactorSource.position, interactorSource.position + interactorSource.forward * interactRange);
    }
}

