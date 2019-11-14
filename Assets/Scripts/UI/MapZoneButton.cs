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

            string name = gameObject.name;
            int zone = 0;
            for (int i = 0; i < name.Length; i++)
            {
                if (char.IsNumber(name[i]))
                {
                    zone = name[i] - '0';
                }
            }

            FindObjectOfType<MapZone>().clicZone = zone;
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

    private void OnMouseDown()
    {
        string name = gameObject.name;
        int zone = 0;
        for (int i = 0; i < name.Length; i++)
        {
            if(char.IsNumber(name[i]) )
            {                
                zone = name[i] - '0';
            }
        }

        StartCoroutine(SendEvent(zone));
              
    }

    IEnumerator SendEvent(int zone)
    {
        yield return new WaitForSeconds(0.5f);

        if (!GetComponent<DialogueManagerMap>().active)
        {
            GameManager.instance.MapaClicErroneo(zone);
        }
    }
}
