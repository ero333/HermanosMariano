using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    LineRenderer lineRenderer;

    public float speed = 16f;
    public int damage = 2;
    public string shooter;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
    }
    void Start()
    {
        rb.velocity = transform.right * speed;
        lineRenderer.SetPosition(0, new Vector3 (0,0,0));

        Destroy(gameObject, 8f);
    }

    void Update()
    {
        if(lineRenderer.GetPosition(1).x > -4f)
        {
            lineRenderer.SetPosition(1, new Vector3(lineRenderer.GetPosition(1).x - 0.5f, 0, 0));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        float dir = Mathf.Clamp(transform.position.x - collision.transform.position.x, -1, 1);

        if(collision.tag == "Enemy")
        {
            if(collision.GetComponent<Enemy>().lives > 0)
            {
                collision.GetComponent<Enemy>().culpable = "Bullet (" + shooter + ")";
                collision.GetComponent<Enemy>().TakeDamage(damage, dir);
            }            
            Destroy(gameObject);
        }
        else if (collision.tag == "Player")
        {
            collision.GetComponent<Player>().TakeDamage(damage, dir, "Bullet ("+shooter+")");
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }        
    }
}
