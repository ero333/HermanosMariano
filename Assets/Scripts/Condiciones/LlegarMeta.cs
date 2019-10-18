using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LlegarMeta : MonoBehaviour
{
    UserInterface ui;

    public int gananciaMaxima = 100;

    public bool Obligatorio = false;
    public int zonaActual;

    private void Start()
    {
        ui = GameObject.FindObjectOfType<UserInterface>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {            
            ui.gananciaMaxBK = gananciaMaxima;
            collision.GetComponent<Player>().Win();
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
