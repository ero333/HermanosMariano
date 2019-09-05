using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapZone : MonoBehaviour
{
    GameManager gm;
    public GameObject[] Zonas;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();

        for (int i = 0; i < Zonas.Length; i++)
        {
            Zonas[i].GetComponent<Selectable>().interactable = false;
        }

        for (int i = 0; i < gm.zoneProgress; i++)
        {
            Zonas[i].GetComponent<Selectable>().interactable = true;
        }
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
   
}
