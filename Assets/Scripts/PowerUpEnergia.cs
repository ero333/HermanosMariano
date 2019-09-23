using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpEnergia : MonoBehaviour
{
    private void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
    }
    GameManager gm;
    public int CantidadDeEnergia;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gm.energy += CantidadDeEnergia;
            Destroy(gameObject);
        }
    }
}
