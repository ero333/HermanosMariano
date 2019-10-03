using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapZoneButton : MonoBehaviour
{
    public GameObject[] levelBotones;
    public bool unlocked;

    private void Start()
    {
        unlocked = GetComponent<Selectable>().interactable;
        
        foreach (var levelBoton in levelBotones)
        {
            levelBoton.SetActive(false);
        }
                
    }

    private void OnMouseEnter()
    {
        unlocked = GetComponent<Selectable>().interactable;

        if (unlocked)
        {
            foreach (var levelBoton in levelBotones)
            {
                levelBoton.SetActive(true);
            }
        }        
    }

    private void OnMouseExit()
    {
        unlocked = GetComponent<Selectable>().interactable;

        if (unlocked)
        {
            foreach (var levelBoton in levelBotones)
            {
                levelBoton.SetActive(false);
            }
        }        
    }
}
