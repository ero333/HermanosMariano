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

        for (int i = 1; i < Zonas.Length; i++)
        {
            Zonas[i].GetComponent<Selectable>().interactable = false;

            if (i <= GameManager.zoneProgress)
            {
                Zonas[i].GetComponent<Selectable>().interactable = true;
            }
        }

        for (int i = 0; i < Deudas.Length; i++)
        {
            if (i < GameManager.paidDeudas)
            {
                Deudas[i].transform.parent.GetComponent<Button>().interactable = true;
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
        ahorrostexto.text = "" + GameManager.ahorros;

        if (Input.GetKeyDown(KeyCode.L))
        {
            GameManager.ahorros += 10000;
        }
    }
}
