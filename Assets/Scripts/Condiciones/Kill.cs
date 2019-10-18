using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill : MonoBehaviour
{
    GameManager gm;
    UserInterface ui;

    public GameObject toKill;

    public int gananciaMaxima = 100;

    public bool Obligatorio = false;
    public int zonaActual;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;
        ui = GameObject.FindObjectOfType<UserInterface>();

        if(toKill == null)
        {
            Debug.Log("No hay forma de ganar, asigne un objetivo a matar");
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(toKill == null)
        {
            ui.gananciaMaxBK = gananciaMaxima;
            FindObjectOfType<Player>().Win();
            GameManager.instance.VictoryCondition();            

            if (Obligatorio)
            {
                FindObjectOfType<CutsceneManager>().obli = true;
                GameManager.instance.UnlockZone(zonaActual);
            }

            gameObject.SetActive(false);
        }
    }
}
