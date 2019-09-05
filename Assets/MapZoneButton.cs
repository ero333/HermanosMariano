using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapZoneButton : MonoBehaviour
{
    public GameObject[] levelBotones;

    private void Start()
    {
        foreach (var levelBoton in levelBotones)
        {
            levelBoton.SetActive(false);
        }
    }

    private void OnMouseEnter()
    {
        foreach (var levelBoton in levelBotones)
        {
            levelBoton.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        foreach (var levelBoton in levelBotones)
        {
            levelBoton.SetActive(false);
        }
    }
}
