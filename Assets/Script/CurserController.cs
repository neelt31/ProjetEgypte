using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D cursorTexture;

    void Start()
    {
        // Masquer le curseur syst�me
        Cursor.visible = false;
        
        // D�finir le point chaud du curseur personnalis� (assurez-vous de l'avoir d�fini dans l'Inspector)
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }

    void OnGUI()
    {
        // Dessiner le curseur personnalis� au milieu de l'�cran
        GUI.DrawTexture(new Rect((Screen.width - cursorTexture.width) / 2, (Screen.height - cursorTexture.height) / 2, cursorTexture.width, cursorTexture.height), cursorTexture);
    }
}
