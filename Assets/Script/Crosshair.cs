using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public Texture2D crosshairTexture;
    public Texture2D crosshairTexture2;
    public float crosshairSize = 32f;
    public Interactor interactor;
    public bool showCrosshair = false;



    void OnGUI()
    {
        if (showCrosshair)
        {
            float xMin = Screen.width / 2 - (crosshairSize / 2);
            float yMin = Screen.height / 2 - (crosshairSize / 2);
            if (interactor.detect)
            {
                GUI.DrawTexture(new Rect(xMin, yMin, crosshairSize, crosshairSize), crosshairTexture);
            }
            else
            {
                GUI.DrawTexture(new Rect(xMin, yMin, crosshairSize, crosshairSize), crosshairTexture2);
            }
        }
    }
}
