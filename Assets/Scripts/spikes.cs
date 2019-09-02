using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikes : MonoBehaviour
{
    public int damage;
    GameManager gm;

    private void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        float dir = Mathf.FloorToInt(Mathf.Clamp(collision.transform.position.x - gameObject.transform.position.x, -1, 1));

        if (collision.gameObject.tag == "Player")
        {            
            collision.GetComponent<Player>().TakeDamage(damage, dir * 2);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            collision.GetComponent<Enemy>().TakeDamage(damage, dir * 2);
        }
    }
    
}
