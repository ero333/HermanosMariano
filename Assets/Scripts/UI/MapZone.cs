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
        gm = GameManager.instance;

        gm.SaveData();

        //evento mapa

        for (int i = 0; i < Zonas.Length; i++)
        {
            Zonas[i].GetComponent<Selectable>().interactable = false;

            if (i <= GameManager.zoneProgress - 1)
            {
                Zonas[i].GetComponent<Selectable>().interactable = true;
            }

            if (i < GameManager.seenDialogues)
            {
                Zonas[i].GetComponent<DialogueManagerMap>().seen = true;
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
        if(GameManager.seenDialogues < GameManager.zoneProgress)
        {
            DialogueManagerMap dialog = Zonas[GameManager.zoneProgress - 1].GetComponent<DialogueManagerMap>();
            dialog.background.gameObject.SetActive(true);
            dialog.active = true;

            //ver dialogo
        }
        else
        {
            int levelNumber = GameManager.instance.LevelIndexer(levelName);

            //cargar nivel
            GameManager.instance.MapaClicNivel(levelNumber);

            SceneManager.LoadScene(levelName);
        }        
    }

    private void Update()
    {
        ahorrostexto.text = "" + GameManager.ahorros;

        if (Input.GetKeyDown(KeyCode.L))
        {
            GameManager.ahorros += 10000;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            gm.Cheat();

            for (int i = 0; i < Zonas.Length; i++)
            {
                Zonas[i].GetComponent<Selectable>().interactable = false;

                if (i <= GameManager.zoneProgress - 1)
                {
                    Zonas[i].GetComponent<Selectable>().interactable = true;
                }

                if (i < GameManager.seenDialogues)
                {
                    Zonas[i].GetComponent<DialogueManagerMap>().seen = true;
                }
            }

        }
    }
}
