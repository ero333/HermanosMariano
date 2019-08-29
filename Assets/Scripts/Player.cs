using System.Collections;
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
    public bool fRight = true;
    public float speed = 10f;
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Ataque")]
    public LayerMask EnemyLayer;
    public int meleeDamage = 2;
    public Vector2 meleeHitBoxSize;
    public Vector2 meleeHitBoxOffset;
    bool hit = false;

    [Header("Detector de Collisiones")]
    public LayerMask groundLayer;
    public float collisionRadius = 0.25f;
    public Vector2 bottomOffset, rightOffset, leftOffset;
    public Vector2 bottomSize;
    private Color debugCollisionColor = Color.red;

    //animaciones
    //Idle
    private bool isIdleCount = false;
    float idleTimer = 5f;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };
        var sizes = new Vector2[] { bottomSize };

        Gizmos.DrawWireCube((Vector2) transform.position + bottomOffset, (Vector2) bottomSize);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);

        Gizmos.DrawWireCube((Vector2)transform.position + meleeHitBoxOffset, (Vector2)meleeHitBoxSize);
    }

    void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (gm.resetCount == 0)
        {
            gm.lastCheckpos = transform.position;
        }
        else if (gm.resetCount > 0)
        {
            transform.position = gm.lastCheckpos;
        }
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

                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
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

    public void TakeDamage (int damage)
    {
        gm.energy -= damage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Choco con un Abismo
        if (collision.gameObject.tag == "Fall")
        {
            Debug.Log("Death");
            gm.lives -= 1;
            gm.Reset();
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
        onGround = Physics2D.OverlapBox((Vector2)transform.position + bottomOffset, (Vector2)bottomSize, 0f, groundLayer);
        onWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer)
            || Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);

        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);

        //wallSide = onRightWall ? -1 : 1;

        Collider2D[] hitBox = Physics2D.OverlapBoxAll((Vector2)transform.position + meleeHitBoxOffset, (Vector2)meleeHitBoxSize, 0f, EnemyLayer);

        //evento de colision con el jugador
        if (hitBox.Length > 0 && !hit)
        {
            Debug.Log("enemy Hit");
            hit = true;

            anim.SetTrigger("MeleeAttack");
            //do damage
            //hitBox[0].GetComponent<Enemy>().TakeDamage(meleeDamage);
        }
        else if (hitBox.Length == 0 && hit)
        {
            hit = false;
        }
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
