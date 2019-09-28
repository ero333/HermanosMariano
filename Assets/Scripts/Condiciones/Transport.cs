using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transport : MonoBehaviour
{
    GameManager gm;
    UserInterface ui;

    public int gananciaMaxima = 100;

    public bool Obligatorio = false;

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
            }

            if (toTransport <= 0)
            {
                GameManager.instance.VictoryCondition();
                ui.Victory(gananciaMaxima);

                if (Obligatorio)
                {
                    GameManager.zoneProgress += 1;
                    GameManager.maxEnergy += 1;
                }

                Destroy(gameObject);
            }            
        }
    }
}
