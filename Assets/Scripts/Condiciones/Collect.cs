using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    GameManager gm;
    UserInterface ui;

    public int toCollect = 0;

    public int gananciaMaxima = 100;
    public bool Obligatorio = false;
    public int zonaActual;

    private void Start()
    {
        gm = GameManager.instance;
        ui = GameObject.FindObjectOfType<UserInterface>();

        toCollect += FindObjectsOfType<Collectable>().Length;

        Enemy[] enemies = FindObjectsOfType<Enemy>();

        for (int i = 0; i < enemies.Length; i++)
        {
            //Collectable colec = enemies[i].dropOnHit.GetComponent<Collectable>();

            if(enemies[i].dropOnHit != null)
            {
                if (enemies[i].dropOnHit.GetComponent<Collectable>())
                {
                    toCollect += 1;
                }
            }

            if (enemies[i].dropOnDeath != null)
            {
                if (enemies[i].dropOnDeath.GetComponent<Collectable>())
                {
                    toCollect += 1;
                }
            }

        }
    }

    void Update()
    {
        if (toCollect <= 0)
        {
            ui.gananciaMaxBK = gananciaMaxima;
            FindObjectOfType<Player>().Win();
            GameManager.instance.VictoryCondition();            

            toCollect = 100;

            if (Obligatorio)
            {
                FindObjectOfType<CutsceneManager>().obli = true;
                GameManager.instance.UnlockZone(zonaActual);
            }
        }
    }
}
