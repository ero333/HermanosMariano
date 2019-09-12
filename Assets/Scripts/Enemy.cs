using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    Player player;

    [Header("Debug, no tocar")]
    public Vector2 playerDistance;
    public Vector2 playerDistanceAbs;
    public Vector2 playerDirection;
    public bool onGround;
    //public bool onWall;
    public bool fRight = true;    

    //Con esto sabe si se va a caer si continua caminando
    public bool onRightFloor;
    public bool onLeftFloor;

    [Header("Detector de colisiones")]
    public LayerMask groundLayer;
    public LayerMask playerLayer;
    public float collisionRadius = 0.25f;
    public Vector2 bottomOffset, rightOffset, leftOffset;
    public Vector2 bottomSize;
    private Color debugCollisionColor = Color.red;

    [Space]
    public Vector2 triggerOffset, triggerSize;
    public bool ShowRange;
    public bool trigger = false;

    [Header("Vida")]
    public int lives;
    public float hitStunTimer = 1f;

    [Header("Perseguir")]
    public bool chase = true;    
    public float speed = 10f;    
    bool canChase = true;
    bool nirvana = false;
    bool canFlip = true;

    [Header("Huir")]
    public bool flee = false;
    public float speedFlee = 5f;
    public float safeDistance = 10f;
    bool canFlee = true;
    bool fleeActive = false;
    bool cornered = false;

    [Header("Golpear")]
    public int meleeDamage = 2;
    public float meleeDelay = 1;
    public Vector2 meleeHitBoxSize;
    public Vector2 meleeHitBoxOffset;
    bool hit = false;
    bool hited = false;
    bool canAttack = true;    
    public bool frameDamage = false;

    [Header("Disparar")]
    public int shootDamage = 1;
    bool canShoot = true;
    public float shootDelay = 5f;
    public float minShootDis = 15f;
    public GameObject bullet;
    public GameObject spawnBullet;

    IEnumerator generalActionsDelay;
    IEnumerator ShootDelay;
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset, triggerOffset, meleeHitBoxOffset };
        var sizes = new Vector2[] { bottomSize, meleeHitBoxSize, triggerSize };

        //Colisiones
        Gizmos.DrawWireCube((Vector2)transform.position + bottomOffset, (Vector2)bottomSize);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);

        //Trigger size
        if (ShowRange)
        {
            Gizmos.DrawWireCube((Vector2)transform.position + triggerOffset, (Vector2) triggerSize);
        }

        //Melee
        Gizmos.DrawWireCube((Vector2)transform.position + meleeHitBoxOffset, (Vector2) meleeHitBoxSize);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindObjectOfType<Player>();

        if (transform.rotation.y == 180)
        {
            fRight = false;
        }

        //que comience volteado
        if (fRight == false)
        {
            transform.Rotate(0f, 180f, 0f);

            //volteo del hitbox y trigger
            meleeHitBoxOffset.x *= -1;
            triggerOffset.x *= -1;
        }        
    }

    private void FixedUpdate()
    {
        DetectCollisions();
    }

    void Update()
    {
        getPlayerDir();
        
        if (lives <= 0)
        {
            anim.SetBool("Death", true);

            StartCoroutine(DeathDelay(1f));
        }

        //si el jugador me salta por arriba, freno
        if (playerDirection.y == 1 && playerDirection.x == 0)
        {
            generalActionsDelay = ActionsDelay(0.6f);

            StopCoroutine(generalActionsDelay);
            StartCoroutine(generalActionsDelay);
        }


        //Comportamientos
        if (trigger && lives > 0)
        {
            //saltar (y frenar cuando se encuentra un precipicio) NO CAMBIAR DE LUGAR NI PONER OTROS COMPORTAMIENTOS ARRIBA
            if(onGround && (!onLeftFloor || !onRightFloor))
            {
                generalActionsDelay = ActionsDelay(0.6f);
              
                //cuando huye, es invertido
                if (flee && fleeActive && ((playerDirection.x == -1 && !onRightFloor) || (playerDirection.x == 1 && !onLeftFloor)) )
                {
                    //saltar

                    cornered = true;
                    playerDirection.x = 0;

                    //Debug.Log("No puedo huir hasta el vacio");

                } //cuando lo persigue
                else if (chase && ((playerDirection.x == 1 && !onRightFloor) || (playerDirection.x == -1 && !onLeftFloor)) )
                {
                    //saltar

                    playerDirection.x = 0;

                    //Debug.Log("No puedo perseguir hasta el vacio");
                }

                //if (meleeDamage > 0 && !fleeActive)
                //{
                //    canAttack = false;
                //}
            }
            else if (onLeftFloor && onRightFloor)
            {
                cornered = false;
            }

            //evitar o FLEE
            if (flee && canFlee && onGround && playerDistanceAbs.x < safeDistance)
            {
                //Debug.Log("Estoy huyendo");

                fleeActive = true;

                rb.velocity = new Vector2( - playerDirection.x * speedFlee, rb.velocity.y);               

                nirvana = false;
            }
            else if (flee && playerDistanceAbs.x > safeDistance)
            {
                fleeActive = false;
            }
            
            //perseguir (el onGround puede joder el salto ¿sacarlo podria arreglarlo?)
            if (chase && canChase && onGround && playerDirection.x != 0 && !fleeActive)
            {
                if (flee && canFlee && (playerDistanceAbs.x > safeDistance && playerDistanceAbs.x < safeDistance + 1f) )
                {
                    nirvana = true;
                    //Debug.Log("Me quedo tranqui");
                }
                else
                {
                    //Debug.Log("Estoy persiguiendo");
                    nirvana = false;
                    rb.velocity = new Vector2(playerDirection.x * speed, rb.velocity.y);
                }                                
            }
            
            //disparar
            if (shootDamage > 0 && canShoot && canAttack && playerDistanceAbs.x <= minShootDis)
            {
                generalActionsDelay = ActionsDelay(1f);
                StopCoroutine(generalActionsDelay);
                StartCoroutine(generalActionsDelay);

                rb.velocity = new Vector2(0, 0);

                ShootDelay = ShootDelayCou(shootDelay);
                StopCoroutine(ShootDelay);
                StartCoroutine(ShootDelay);

                if (playerDirection.x == -1 && fRight)
                {
                    fRight = false;
                    transform.Rotate(0f, 180f, 0f);

                    //volteo del hitbox y trigger
                    meleeHitBoxOffset.x *= -1;
                    triggerOffset.x *= -1;
                }
                else if (playerDirection.x == 1 && !fRight)
                {
                    fRight = true;
                    transform.Rotate(0f, 180f, 0f);

                    //volteo del hitbox y trigger
                    meleeHitBoxOffset.x *= -1;
                    triggerOffset.x *= -1;
                }

                anim.SetTrigger("Shoot");                

                GameObject bulletInst = Instantiate(bullet, spawnBullet.transform.position, spawnBullet.transform.rotation);
                bulletInst.GetComponent<Bullet>().damage = shootDamage;
            }

        }

        SetAnim();
    }
    
    //Sistema de deteccion de jugador
    void getPlayerDir()
    {
        float xDis = player.transform.position.x - transform.position.x;
        float yDis = player.transform.position.y - transform.position.y;

        int xDir = Mathf.FloorToInt(Mathf.Clamp(xDis, -1, 1));
        int yDir = Mathf.FloorToInt(Mathf.Clamp(yDis, -1, 1));

        playerDistance = new Vector2 (xDis, yDis);
        playerDistanceAbs = new Vector2(Mathf.Abs(xDis), Mathf.Abs(yDis));
        playerDirection = new Vector2(xDir, yDir);
    }

    void DetectCollisions()
    {
        //Colisiones con el piso:
        onGround = Physics2D.OverlapBox((Vector2)transform.position + bottomOffset, (Vector2)bottomSize, 0f, groundLayer);
        
        onRightFloor = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        onLeftFloor = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);

        //trigger
        if (!trigger)
        {
            trigger = Physics2D.OverlapBox((Vector2)transform.position + triggerOffset, (Vector2)triggerSize, 0f, playerLayer);
        }        

        //Atacks        
        Collider2D[] hitBox = Physics2D.OverlapBoxAll((Vector2)transform.position + meleeHitBoxOffset, (Vector2)meleeHitBoxSize, 0f, playerLayer);

        
        //evento de colision con el jugador en la hitbox, golpear        
        if (hitBox.Length > 0 && meleeDamage > 0)
        {
            //Debug.Log("Player Hit");
            hit = true;
            if (canAttack)
            {
                generalActionsDelay = ActionsDelay(meleeDelay);

                StopCoroutine(generalActionsDelay);
                StartCoroutine(generalActionsDelay);
                anim.SetTrigger("MeleeAttack");
                
                canAttack = false;
            }               
                               
        }
        else if (hitBox.Length == 0 && meleeDamage > 0)
        {
            hit = false;
        }

        //hacer daño cuando el jugador este en la hitbox y el enemigo este en el frame de la animacion correcta
        if (hit && frameDamage && !hited && meleeDamage > 0)
        {
            player.TakeDamage(meleeDamage, playerDirection.x);
            hited = true;
        }
                      
    }

    void SetAnim()
    {
        IEnumerator Flip = DelayFlip();

        anim.SetBool("OnGround", onGround);

        //caminar
        if ( trigger && ( (chase && canChase) || (flee && canFlee && fleeActive) ) && onGround && playerDirection.x != 0 && !nirvana)
        {
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);
        }

        //volteos
        if (trigger && canFlip)
        {
            if (flee && fleeActive && !cornered)
            {
                //darle la espalda al jugador
                if (playerDirection.x < 0 && !fRight)
                {
                    fRight = true;
                    StartCoroutine(Flip);
                }
                else if (playerDirection.x > 0 && fRight)
                {
                    fRight = false;
                    StartCoroutine(Flip);
                }
            }
            else
            {
                //voltear segun donde este el jugador
                if (playerDirection.x > 0 && !fRight)
                {
                    fRight = true;

                    StartCoroutine(Flip);

                }
                else if (playerDirection.x < 0 && fRight)
                {
                    fRight = false;

                    StartCoroutine(Flip);
                }
                else if (cornered)
                {
                    if (playerDistance.x > 0.01 && !fRight)
                    {
                        fRight = true;

                        StartCoroutine(Flip);

                    }
                    else if (playerDistance.x < -0.01 && fRight)
                    {
                        fRight = false;

                        StartCoroutine(Flip);
                    }
                }
            }            
            
        }

        IEnumerator DelayFlip()
        {
            yield return new WaitForSeconds(0.2f);
            transform.Rotate(0f, 180f, 0f);

            //volteo del hitbox y trigger
            meleeHitBoxOffset.x *= -1;
            triggerOffset.x *= -1;
        }
    }

    //si choco con algo
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Fall")
        {
            Destroy(gameObject);
        }
    }

    //frenar todas las acciones y comportamientos designados y resumirlos luego de un tiempo
    IEnumerator ActionsDelay(float delay)
    {
        bool chse = false;
        bool atk = false;
        bool fle = false;

        if (chase)
        {
            canChase = false;
            chse = true;
        }

        if (meleeDamage > 0)
        {
            canAttack = false;
            atk = true;
        }
        
        if (flee)
        {
            canFlee = false;
            fle = true;
        }

        canFlip = false;

        yield return new WaitForSeconds(delay);

        if (chse)
        {
            canChase = true;
        }
        
        if (atk)
        {
            canAttack = true;
            hited = false;
        }

        if (fle)
        {
            canFlee = true;
        }

        canFlip = true;
        //hit = false;
    }

    IEnumerator ShootDelayCou(float delay)
    {
        canShoot = false;
        yield return new WaitForSeconds(delay);
        canShoot = true;
    }

    IEnumerator DeathDelay (float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    public void TakeDamage(int damage, float dir)
    {
        trigger = true;

        lives -= damage;
        anim.SetTrigger("GetHit");

        rb.AddForce(new Vector2(dir * 150, 200));

        generalActionsDelay = ActionsDelay(hitStunTimer);

        StopCoroutine(generalActionsDelay);
        StartCoroutine(generalActionsDelay);
    }
}
