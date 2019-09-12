using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    GameManager gm;
    UserInterface ui;

    public int toCollect;

    public int gananciaMaxima = 100;
    public bool Obligatorio = false;

    private void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
        ui = GameObject.FindObjectOfType<UserInterface>();

        toCollect = FindObjectsOfType<Collectable>().Length;
    }

    void Update()
    {
        if (toCollect <= 0)
        {
            gm.VictoryCondition();
            ui.Victory(gananciaMaxima);
        }
    }
}
