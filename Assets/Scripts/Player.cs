using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    GameManager gm;
    CinemachineVirtualCamera cam;
    CinemachineFramingTransposer composer;

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
    bool inven = false;

    [Space]
    public Vector2 stompBoxOffset;
    public Vector2 stompBoxSize;

    public bool transport;

    [Header("Ataque")]
    public LayerMask EnemyLayer;
    public int meleeDamage = 2;
    public Vector2 meleeHitBoxSize;
    public Vector2 meleeHitBoxOffset;
    bool hitInput = false;
    bool attackLock;

    [Header("Disparar")]
    public int bullets = 0;
    public int shootDamage = 2;
    public GameObject spawnBullet;
    public GameObject bullet;

    [Header("Detector de Collisiones")]
    public LayerMask groundLayer;
    public float collisionRadius = 0.25f;
    public Vector2 bottomOffset, rightOffset, leftOffset;
    public Vector2 bottomSize;
    //private Color debugCollisionColor = Color.red;

    //corrutinas
    IEnumerator hitDelay; /*= HitDelay(0.3f);*/
    IEnumerator invenTimer;

    //animaciones
    //Idle
    private bool isIdleCount = false;
    float idleTimer = 5f;
    //correr sincro
    [HideInInspector]
    public float runTimeSinc;

    //para bajar la camara
    float camTimer = 1.2f;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };
        var sizes = new Vector2[] { bottomSize };
                
        Gizmos.DrawWireCube((Vector2)transform.position + bottomOffset, (Vector2)bottomSize);
        //Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        //Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position + meleeHitBoxOffset, (Vector2)meleeHitBoxSize);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube((Vector2)transform.position + stompBoxOffset, (Vector2)stompBoxSize);
    }

    void Start()
    {
        gm = GameManager.instance;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        cam = FindObjectOfType<CinemachineVirtualCamera>();
        composer = cam.GetCinemachineComponent<CinemachineFramingTransposer>();

        if (gm.resetCount == 0)
        {
            GameManager.instance.lastCheckpos = transform.position;
        }
        else if (gm.resetCount > 0) //ya no sirve, pero dejemoslo por las dudas
        {
            transform.position = GameManager.instance.lastCheckpos;
        }

        PhysicsMaterial2D physicMaterial = new PhysicsMaterial2D(gameObject.name);
        physicMaterial.bounciness = 0;
        physicMaterial.friction = 0;

        rb.sharedMaterial = physicMaterial;        
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
        else if (anim.GetBool("Win"))
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
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
        if (Input.GetButtonDown("Fire1") && !attackLock && meleeDamage > 0)
        {
            hitInput = true;
            anim.SetTrigger("MeleeInput");
            attackLock = true;
            if (onGround)
            {
                rb.velocity = new Vector2(0f, 0f);
                rb.AddForce(new Vector2(xRaw * 30, 0f));
            }            

            /* IEnumerator */
            hitDelay = HitDelay(0.3f);
            StopCoroutine(hitDelay);
            StartCoroutine(hitDelay);
        }

        //disparar
        if (Input.GetButtonDown("Fire2") && !attackLock && bullets > 0)
        {
            GameObject firedbullet = Instantiate(bullet, spawnBullet.transform.position, spawnBullet.transform.rotation);
            firedbullet.GetComponent<Bullet>().damage = shootDamage;
            firedbullet.GetComponent<Bullet>().shooter = "Player";

            bullets -= 1;
            gm.bulletCounter += 1;
            //rb.velocity = new Vector2(0f, 0f);
            runTimeSinc = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
            anim.SetTrigger("Shoot");

            hitDelay = HitDelay(0.1f);
            StopCoroutine(hitDelay);
            StartCoroutine(hitDelay);
        }

        if(Input.GetButtonDown("Crouch") && onGround)
        {            
            anim.SetTrigger("Crouch");
            invenTimer = InvenTimer(0.3f);
            StopCoroutine(invenTimer);
            StartCoroutine(invenTimer);
            rb.velocity = new Vector2(0, 0);
        }

        if(Input.GetButton("Crouch") && onGround)
        {
            camTimer -= Time.deltaTime;
            if(camTimer < 0)
            {                
                composer.m_ScreenY = 0.4f;
            }
        }
        else
        {
            camTimer = 0.8f;
            composer.m_ScreenY = 0.55f;
        }

        SetAnimations(x);
    }

    public void TakeDamage (int damage, float dir, string name)
    {
        if (!inven)
        {
            GameManager.instance.energy -= damage;
            anim.SetTrigger("TookDamage");
            rb.velocity = new Vector2(0f, 0f);
            rb.AddForce(new Vector2(dir * 150, 100f));
            
            /*IEnumerator*/invenTimer = InvenTimer(0.6f);
            StopCoroutine(invenTimer);
            StartCoroutine(invenTimer);

            gm.ultimoCulpable = name;
        }        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Choco con un Abismo
        if (collision.gameObject.tag == "Fall")
        {
            gm.ultimoCulpable = "Abismo";
            //gm.ultimoCulpableTipo = "Abismo";

            //Debug.Log("Death");
            GameManager.instance.lives -= 1;
            GameManager.instance.Reset();            
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
        if (xInput != 0 && /*!attackLock &&*/ onGround)
        {
            anim.SetBool("Run", true);
            //Ajuste de la velocidad para que no "resbale los pies" al principio
            if (onGround && anim.GetBool("Run") && !hitInput)
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


        //Hitbox de cuchillo
        Collider2D[] hitBox = Physics2D.OverlapBoxAll((Vector2)transform.position + meleeHitBoxOffset, (Vector2)meleeHitBoxSize, 0f, EnemyLayer);

        //evento de colision con el jugador
        if (hitBox.Length > 0 && hitInput)
        {
            hitInput = false;
            //do damage
            for (int i = 0; i < hitBox.Length; i++)
            {
                Enemy enemy = hitBox[i].GetComponent<Enemy>();

                float dir;
                if (transform.position.x < enemy.transform.position.x)
                {
                    dir = 1;
                }
                else
                {
                    dir = -1;
                }

                if(enemy.lives > 0)
                {
                    enemy.culpable = "Player";
                    enemy.TakeDamage(meleeDamage, dir);                    
                }                
            }
        }

        //hitbox de salto tipo mario
        Collider2D[] stompBox = Physics2D.OverlapBoxAll((Vector2)transform.position + stompBoxOffset, (Vector2)stompBoxSize, 0f, EnemyLayer);

        if (stompBox.Length > 0)
        {
            for (int i = 0; i < stompBox.Length; i++)
            {
                //stun
                stompBox[i].GetComponent<Enemy>().Stun(3);
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.velocity += Vector2.up * 8;

                anim.SetTrigger("JumpInput");                
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
        attackLock = true;
        yield return new WaitForSeconds(delay);
        attackLock = false;
        hitInput = false;
    }

    IEnumerator InvenTimer(float timer)
    {
        inven = true;
        attackLock = true;
        yield return new WaitForSeconds(timer);
        attackLock = false;
        inven = false;
    }


    public void Win()
    {
        StopAllCoroutines();
        attackLock = true;
        inven = true;
        anim.SetBool("Win", true);
        FindObjectOfType<CutsceneManager>().playerAnim = true;
    }
}
