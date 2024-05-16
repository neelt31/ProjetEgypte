using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Skinscripte : MonoBehaviour
{
    public int current_index = 2;
    public int maxindex = 4;
    [SerializeField]public List<GameObject> ListImage = new List<GameObject>();
    
    [SerializeField]public MenuManager menuManager;

    void Start(){
        ListImage[current_index-2].SetActive(true);

    }

    public void Suivant()
    {
        ListImage[current_index-2].SetActive(false);

        if (current_index == maxindex)
        {
            current_index = 2;
        }
        else
        {
            current_index++;
        }
        ListImage[current_index-2].SetActive(true);

    }

    public void Precedent()
    {
        ListImage[current_index-2].SetActive(false);
 
        if (current_index == 2)
        {
            current_index = maxindex;
        }
        else
        {
            current_index--;
        }
        ListImage[current_index-2].SetActive(true);
    }

    public void Valider(){
        menuManager.changerskin(current_index);

    }

}
