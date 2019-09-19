﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LlegarMeta : MonoBehaviour
{
    GameManager gm;
    UserInterface ui;

    public int gananciaMaxima = 100;

    public bool Obligatorio = false;

    private void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
        ui = GameObject.FindObjectOfType<UserInterface>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gm.VictoryCondition();
            ui.Victory(gananciaMaxima);

            if (Obligatorio)
            {
                GameManager.zoneProgress += 1;
                GameManager.maxEnergy += 1;
            }
        }        
    }
}