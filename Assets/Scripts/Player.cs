﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    GameManager gm;

    //No tocar >:(
    [Header("Debug (No tocar)")]
    public bool onGround;
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    public int wallSide;

    [Header("Ajustes")]
    bool fRight = true;
    public float speed = 10f;
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Space]
    public LayerMask groundLayer;

    [Header("Detector de Collisiones")]
    public float collisionRadius = 0.25f;
    public Vector2 bottomOffset, rightOffset, leftOffset;
    private Color debugCollisionColor = Color.red;

    //animaciones
    //Idle
    private bool isIdleCount = false;
    float idleTimer = 5f;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };

        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
    }

    void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        DetectCollisions();
        
        //inputs de movimiento

        float x;
        float xRaw = Input.GetAxisRaw("Horizontal");

        if (xRaw != 0)
        {
            x = Input.GetAxis("Horizontal");
        }
        else
        {
            x = 0;
        }
        
       //Moverse
        rb.velocity = new Vector2(x * speed, rb.velocity.y);

        //Saltar
        if (Input.GetButtonDown("Jump"))
        {
            if (onGround)
            {
                anim.SetTrigger("JumpInput");

                rb.velocity = new Vector2(rb.velocity.x, 0f);
                rb.velocity += Vector2.up * jumpForce;
            }            
        }

        //Ajustar salto y gravedad para que dependan del Input (si mantengo, más alto y más lento caigo)
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        SetAnimations(x);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Choco con un Abismo
        if (collision.gameObject.tag == "Fall")
        {
            Debug.Log("hit");
            
            gm.Respawn();
        }
    }

    void SetAnimations(float xInput)
    {
        //Idle        
        IEnumerator idleTrigger = IdleTrigger(idleTimer);
        if (xInput == 0 && onGround && !Input.anyKeyDown && !isIdleCount)
        {
            isIdleCount = true;
            idleTimer = Random.Range(3.5f, 16f);            
            StartCoroutine(idleTrigger);            
        }
        else if(xInput != 0 || !onGround || Input.anyKeyDown)
        {
            StopCoroutine(idleTrigger);
        }

        //voltear, con rotate asi tambien se voltean todos los hijos si decidimos usarlos
        if (xInput > 0 && !fRight)
        {
            fRight = true;
            transform.Rotate(0f, 180f, 0f);
        }
        else if (xInput < 0 && fRight)
        {
            fRight = false;
            transform.Rotate(0f, 180f, 0f);
        }

        //correr
        if (xInput != 0)
        {
            anim.SetBool("Run", true);
            //Ajuste de la velocidad para que no "resbale los pies" al principio
            if (onGround)
            {
                anim.speed = Mathf.Clamp(Mathf.Abs(xInput), 0.5f, 1);
            }                       
        }
        else
        {
            anim.SetBool("Run", false);
        }        

        //saltar
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("onGround", onGround);
    }

    void DetectCollisions()
    {
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);
        onWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer)
            || Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);

        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);

        //wallSide = onRightWall ? -1 : 1;
    }

    //Corutinas
    IEnumerator IdleTrigger(float timer)
    {
        //Debug.Log(timer);
        yield return new WaitForSeconds(timer);
        anim.SetTrigger("IdleAlt");
        isIdleCount = false;
    }

}