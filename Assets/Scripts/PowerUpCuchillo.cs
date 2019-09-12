using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpCuchillo : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<Player>().meleeDamage = 1;
            Destroy(gameObject);
        }
    }
}
