using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpVida : MonoBehaviour
{
    private void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
    }

    GameManager gm;
    public int Vida;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gm.lives += Vida;
            Destroy(gameObject);
        }
    }
}
