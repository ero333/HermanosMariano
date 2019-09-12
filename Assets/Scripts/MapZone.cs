using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapZone : MonoBehaviour
{
    GameManager gm;
    public GameObject[] Zonas;
    public GameObject[] Deudas;

    public Text ahorrostexto;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();

        gm.SaveData();

        for (int i = 0; i < Zonas.Length; i++)
        {
            Zonas[i].GetComponent<Selectable>().interactable = false;

            if (i <= gm.zoneProgress)
            {
                Zonas[i].GetComponent<Selectable>().interactable = true;
            }
        }

        //for (int i = 0; i < gm.zoneProgress; i++)
        //{

        //}

        for (int i = 0; i < Deudas.Length; i++)
        {
            if (i < gm.paidDeudas)
            {
                Deudas[i].SetActive(false);
            }
        }
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    private void Update()
    {
        ahorrostexto.text = "" + gm.ahorros;
    }
}
