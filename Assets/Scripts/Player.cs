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
    //public int wallSide;

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
    bool attackLock;
    

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
        //Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        //Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);

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
        else if (gm.resetCount > 0) //ya no sirve, pero dejemoslo por las dudas
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
        
        if (!attackLock)
        {
            //Moverse
            rb.velocity = new Vector2(x * speed, rb.velocity.y);
            

            //Saltar
            if (Input.GetButtonDown("Jump"))
            {
                if (onGround)
                {
                    anim.SetTrigger("JumpInput");

                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.velocity += Vector2.up * jumpForce;
                }
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

        //golpear
        if (Input.GetButtonDown("Fire1") && onGround && !attackLock)
        {
            hit = true;
            anim.SetTrigger("MeleeInput");
            attackLock = true;
            rb.velocity = new Vector2(0f, 0f);
            rb.AddForce(new Vector2(xRaw * 30, 0f));
            IEnumerator hitDelay = HitDelay(0.3f);
            StopCoroutine(hitDelay);
            StartCoroutine(hitDelay);
        }

        SetAnimations(x);
    }

    public void TakeDamage (int damage, float dir)
    {
        gm.energy -= damage;
        anim.SetTrigger("TookDamage");
        attackLock = true;
        rb.velocity = new Vector2(0f, 0f);
        rb.AddForce(new Vector2(dir * 150, 100f));
        IEnumerator hitDelay = HitDelay(0.3f);
        StopCoroutine(hitDelay);
        StartCoroutine(hitDelay);
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
            StopCoroutine(idleTrigger);
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
            meleeHitBoxOffset.x *= -1;
        }
        else if (xInput < 0 && fRight)
        {
            fRight = false;
            transform.Rotate(0f, 180f, 0f);
            meleeHitBoxOffset.x *= -1;
        }

        //correr
        if (xInput != 0 && !attackLock)
        {
            anim.SetBool("Run", true);
            //Ajuste de la velocidad para que no "resbale los pies" al principio
            if (onGround && anim.GetBool("Run") && !hit)
            {
                anim.speed = Mathf.Clamp(Mathf.Abs(xInput), 0.5f, 1);
            }
            else
            {
                anim.speed = 1;
            }                 
        }
        else
        {
            anim.SetBool("Run", false);
            anim.speed = 1;
        }

        //saltar
        if (onGround)
        {
            anim.SetFloat("yVelocity", 0f);
        }
        else
        {
            anim.SetFloat("yVelocity", rb.velocity.y);
        }
        
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


        //Hitbox
        Collider2D[] hitBox = Physics2D.OverlapBoxAll((Vector2)transform.position + meleeHitBoxOffset, (Vector2)meleeHitBoxSize, 0f, EnemyLayer);

        //evento de colision con el jugador
        if (hitBox.Length > 0 && hit)
        {
            hit = false;

            //do damage
            for (int i = 0; i < hitBox.Length; i++)
            {
                float dir;
                if (transform.position.x < hitBox[i].transform.position.x)
                {
                    dir = 1;
                }
                else
                {
                    dir = -1;
                }

                hitBox[i].GetComponent<Enemy>().TakeDamage(meleeDamage, dir);
                Debug.Log("enemy Hit");
            }
        }
    }

    //Corutinas

    //activar la idle alternativa
    IEnumerator IdleTrigger(float timer)
    {
        //Debug.Log(timer);
        yield return new WaitForSeconds(timer);
        anim.SetTrigger("IdleAlt");
        isIdleCount = false;
    }


    IEnumerator HitDelay(float delay)
    {        
        yield return new WaitForSeconds(delay);
        attackLock = false;
    }

}
