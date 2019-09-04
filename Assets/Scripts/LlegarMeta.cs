using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LlegarMeta : MonoBehaviour
{
    GameManager gm;
    UserInterface ui;

    public int gananciaMaxima = 100;

    private void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
        ui = GameObject.FindObjectOfType<UserInterface>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gm.VictoryCondition();
        ui.Victory(gananciaMaxima);
    }
}
