using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survive : MonoBehaviour
{
    GameManager gm;
    UserInterface ui;

    public float countDown = 60;

    public int gananciaMaxima = 100;

    public bool Obligatorio = false;
    public int zonaActual;

    private void Start()
    {
        gm = GameManager.instance;
        ui = GameObject.FindObjectOfType<UserInterface>();

        CountDown timer = GetComponent<CountDown>();
        timer.enabled = false;
    }

    private void FixedUpdate()
    {
        if(countDown > 0)
        {
            countDown -= Time.deltaTime;
        }
        else
        {
            GameManager.instance.VictoryCondition();
            ui.Victory(gananciaMaxima);

            if (Obligatorio)
            {
                GameManager.instance.UnlockZone(zonaActual);
            }

            gameObject.SetActive(false);
        }        
    }
}
