using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D cursorTexture;

    void Start()
    {
        // Masquer le curseur système
        Cursor.visible = false;
        
        // Définir le point chaud du curseur personnalisé (assurez-vous de l'avoir défini dans l'Inspector)
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }

    void OnGUI()
    {
        // Dessiner le curseur personnalisé au milieu de l'écran
        GUI.DrawTexture(new Rect((Screen.width - cursorTexture.width) / 2, (Screen.height - cursorTexture.height) / 2, cursorTexture.width, cursorTexture.height), cursorTexture);
    }
}
