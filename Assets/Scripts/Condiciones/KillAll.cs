using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAll : MonoBehaviour
{
    GameManager gm;
    UserInterface ui;

    public int gananciaMaxima = 100;

    public int toKill = 0;

    public bool Obligatorio = false;
    public int zonaActual;    

    void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
        ui = GameObject.FindObjectOfType<UserInterface>();

        if(toKill <= 0)
        {
            toKill = FindObjectsOfType<Enemy>().Length;
        }        
    }

    void Update()
    {
        if (toKill <= 0)
        {
            ui.gananciaMaxBK = gananciaMaxima;
            FindObjectOfType<Player>().Win();
            GameManager.instance.VictoryCondition();            

            toKill = 100;

            if (Obligatorio)
            {
                FindObjectOfType<CutsceneManager>().obli = true;
                GameManager.instance.UnlockZone(zonaActual);
            }

            Destroy(gameObject);
        }
    }
}
