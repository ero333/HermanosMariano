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
    private PolygonCollider2D pc2d;
    private Vector2 start;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        pc2d = GetComponent<PolygonCollider2D>();
        start = transform.position;

    }


    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Invoke("Fall",FallDelay);
            Invoke("Respawn", FallDelay + RespawnDelay);
        }
    }
    void Fall()
    {
        rb2d.isKinematic = false;
        pc2d.isTrigger = true;
    }

    void Respawn()
    {
        transform.position = start;
        rb2d.isKinematic = true;
        rb2d.velocity = Vector2.zero;
        pc2d.isTrigger = false;
    }
}
