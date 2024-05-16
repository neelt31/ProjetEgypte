using UnityEngine;

public class ChangeCubeColor : MonoBehaviour
{
    // Référence au Renderer du cube
    private Renderer cubeRenderer;
    private float triggerEnterTime = 0f;
    private bool playerInside = false;
    private float delaytime = 5;

    void Start()
    {
        // Obtenez le Renderer attaché à ce GameObject (le cube)
        cubeRenderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            triggerEnterTime = Time.time;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (CompareTag("Player"))
        {
            playerInside = false;
        }
    }

    void Update()
    {
        if (playerInside && Time.time - triggerEnterTime >= (delaytime - 1))
        {
            cubeRenderer.material.color = Color.red;
        }

        if (playerInside && Time.time - triggerEnterTime >= delaytime)
        {
            Destroy(gameObject);
        }
    }
}
