using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaFall : MonoBehaviour
{
    //Tiempos de espera de caida
    public float FallDelay = 1f;
    public float RespawnDelay = 5f;
    //y de reaparicionde la plataforma

    private Rigidbody2D rb2d;
    private CompositeCollider2D cc2d;
    private Vector2 start;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        cc2d = GetComponent<CompositeCollider2D>();
        start = transform.position;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Invoke("Fall", FallDelay);
            
        }
    }
    void Fall()
    {
        rb2d.isKinematic = false;
        cc2d.isTrigger = true;
        Invoke("Respawn", RespawnDelay);
    }

    void Respawn()
    {
        transform.position = start;
        rb2d.isKinematic = true;
        rb2d.velocity = Vector2.zero;
        cc2d.isTrigger = false;
    }
}
