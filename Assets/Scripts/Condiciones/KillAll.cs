using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAll : MonoBehaviour
{
    GameManager gm;
    UserInterface ui;

    public int gananciaMaxima = 100;

    public int toKill;

    public bool Obligatorio = false;
    public int zonaActual;    

    void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
        ui = GameObject.FindObjectOfType<UserInterface>();

        toKill = FindObjectsOfType<Enemy>().Length;
    }

    void Update()
    {
        if (toKill <= 0)
        {
            ui.gananciaMaxBK = gananciaMaxima;
            GameManager.instance.VictoryCondition();            

            toKill = 100;

            if (Obligatorio)
            {
                GameManager.instance.UnlockZone(zonaActual);
            }

            Destroy(gameObject);
        }
    }
}
