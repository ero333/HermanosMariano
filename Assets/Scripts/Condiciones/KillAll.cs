using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAll : MonoBehaviour
{
    GameManager gm;
    UserInterface ui;

    public int gananciaMaxima = 100;

    public bool Obligatorio = false;

    public int toKill;

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
            gm.VictoryCondition();
            ui.Victory(gananciaMaxima);

            toKill = 100;

            if (Obligatorio)
            {
                GameManager.zoneProgress += 1;
                GameManager.maxEnergy += 1;
            }

            Destroy(gameObject);
        }
    }
}
