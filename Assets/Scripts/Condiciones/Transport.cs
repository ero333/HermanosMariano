using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transport : MonoBehaviour
{
    GameManager gm;
    UserInterface ui;

    public int gananciaMaxima = 100;

    public bool Obligatorio = false;
    public int zonaActual;

    public int toTransport;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
        ui = GameObject.FindObjectOfType<UserInterface>();

        toTransport = FindObjectsOfType<Collectable>().Length;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player player = collision.GetComponent<Player>();

            if (player.transport)
            {
                toTransport -= 1;
                player.transport = false;
                collision.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
            }

            if (toTransport <= 0)
            {
                ui.gananciaMaxBK = gananciaMaxima;
                FindObjectOfType<Player>().Win();
                GameManager.instance.VictoryCondition();

                if (Obligatorio)
                {
                    FindObjectOfType<CutsceneManager>().obli = true;
                    GameManager.instance.UnlockZone(zonaActual);
                }

                Destroy(gameObject);
            }            
        }
    }
}
