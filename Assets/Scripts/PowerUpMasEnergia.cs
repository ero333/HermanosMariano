using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpMasEnergia: MonoBehaviour
{
    private void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
    }

    GameManager gm;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gm.energy = 6;
            Destroy(gameObject);
        }
    }
}
