using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sierra : MonoBehaviour
{
    public int damage;
    public Transform target1;
    public float speed;
    GameManager gm;
    Player player;
    bool playerInside = false;

    IEnumerator damageTimer;

    private Vector3 start, end;

    // Start is called before the first frame update
    void Start()
    {
        if (target1 != null)
        {
            target1.parent = null;
            start = transform.position;
            end = target1.position;
        }

        gm = GameObject.FindObjectOfType<GameManager>();
    }

    private void FixedUpdate()
    {
        if (target1 != null)
        {
            float fixedSpeed = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target1.position, speed);
        }

        if (transform.position == target1.position)
        {
            target1.position = (target1.position == start) ? end : start;
        }
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
