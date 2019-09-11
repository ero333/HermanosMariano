using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpGun : MonoBehaviour
{
    public int bullets = 6;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<Player>().bullets += bullets;
            Destroy(gameObject);
        }
    }
}
