using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    LineRenderer lineRenderer;

    public float speed = 16f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
    }
    void Start()
    {
        rb.velocity = transform.right * speed;
        lineRenderer.SetPosition(0, new Vector3 (0,0,0));
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
        Destroy(gameObject);
    }
}
