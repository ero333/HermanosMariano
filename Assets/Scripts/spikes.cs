using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikes : MonoBehaviour
{
    public int damage;
    GameManager gm;

    Player player;
    bool playerInside = false;
    //bool canDamage = false;

    IEnumerator damageTimer;

    private void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        float dir = Mathf.FloorToInt(Mathf.Clamp(collision.transform.position.x - gameObject.transform.position.x, -1, 1));

        if (collision.gameObject.tag == "Player")
        {
            playerInside = true;
            //canDamage = true;
            player = collision.GetComponent<Player>();
            player.TakeDamage(damage, dir, gameObject.name);

            damageTimer = DamageTimer();
            StartCoroutine(damageTimer);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            collision.GetComponent<Enemy>().culpable = gameObject.name;
            collision.GetComponent<Enemy>().TakeDamage(damage, dir * 2);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInside = false;
            //canDamage = false;
            //player = collision.GetComponent<Player>();
            damageTimer = DamageTimer();
            StopCoroutine(damageTimer);
        }
    }

    //private void FixedUpdate()
    //{
    //    if (playerInside && canDamage)
    //    {
    //        float dir = Mathf.FloorToInt(Mathf.Clamp(player.transform.position.x - gameObject.transform.position.x, -1, 1));
    //        player.TakeDamage(damage, dir);
    //        damageTimer = DamageTimer();
    //        StartCoroutine(damageTimer);
    //    }
    //}

    IEnumerator DamageTimer()
    {
        yield return new WaitForSeconds(1.8f);
        if (playerInside)
        {            
            float dir = Mathf.FloorToInt(Mathf.Clamp(player.transform.position.x - gameObject.transform.position.x, -1, 1));
            player.TakeDamage(damage, dir, gameObject.name);
            damageTimer = DamageTimer();
            StartCoroutine(damageTimer);
        }
        else
        {
            StopCoroutine(damageTimer);
        }
    }

}
