using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    Player player;
    Collider2D col;

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

    public bool onRightWall;
    public bool onLeftWall;

    public bool colLeft;
    public bool colRight;
    public bool colUp;
    public bool colDown;

    [Header("Drops")]
    public GameObject dropOnHit;
    public GameObject dropOnDeath;
    bool droped = false;
    Vector2 safeGuard;

    public GameObject MoneyObject;
    public int MoneyValue = 0;
    //public float dropMoney = 0;

    [Header("Detector de colisiones")]
    public LayerMask groundLayer;
    public LayerMask playerLayer;

    public Vector2 bottomOffset, bottomSize;

    [Space]
    public float collisionRadius = 0.25f;
    public Vector2 rightFloor, leftFloor;
    public Vector2 rightWall, leftWall;

    [Space]
    public Vector2 triggerOffset, triggerSize;
    public bool ShowRange;
    public bool trigger = false;

    [Header("Vida")]
    public int lives;
    public float hitStunTimer = 1f;
    public bool modoSuicida = false;

    [Header("Patrullar")]
    public bool patrol;
    public float patrolSpeed;
    bool canPatrol = true;

    [Header("Perseguir y correr")]
    public bool chase = true;
    public float speed = 10f;
    bool canChase = true;
    bool nirvana = false;
    bool canFlip = true;

    public bool sprint = false;
    public float sprintSpeed = 8f;
    public float sprintDuration = 3f;
    public float sprintDelay = 5f;
    bool canSprint = true;
    bool sprintOn = false;
    
    [Header("Huir")]
    public bool flee = false;
    public float speedFlee = 5f;
    public float safeDistance = 10f;
    bool canFlee = true;
    bool fleeActive = false;
    bool cornered = false;

    [Header("Saltar")]
    public bool JumpObstacles = false;    
    public float jumpForce = 8;
    bool isJumping = false;

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

    //enumeradores
    IEnumerator generalActionsDelay;
    IEnumerator ShootDelay;
    IEnumerator SprintDelay;
    IEnumerator Flip;

    //[HideInInspector]
    public string culpable;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var positions = new Vector2[] { bottomOffset, rightFloor, leftFloor, rightWall, leftWall, triggerOffset, meleeHitBoxOffset };
        var sizes = new Vector2[] { bottomSize, meleeHitBoxSize, triggerSize };

        //Colisiones
        Gizmos.DrawWireCube((Vector2)transform.position + bottomOffset, (Vector2)bottomSize);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightFloor, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftFloor, collisionRadius);

        Gizmos.DrawWireSphere((Vector2)transform.position + rightWall, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftWall, collisionRadius);

        //Trigger size
        if (ShowRange)
        {
            Gizmos.DrawWireCube((Vector2)transform.position + triggerOffset, (Vector2)triggerSize);
        }

        //Melee
        Gizmos.DrawWireCube((Vector2)transform.position + meleeHitBoxOffset, (Vector2)meleeHitBoxSize);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindObjectOfType<Player>();
        col = GetComponent<BoxCollider2D>();

        //currentTime = Time.time + timeToIncrease;

        if (transform.rotation.y == 180)
        {
            fRight = false;

            //volteo del hitbox y trigger
            meleeHitBoxOffset.x *= -1;
            triggerOffset.x *= -1;
        }

        //que comience volteado
        if (fRight == false)
        {
            transform.Rotate(0f, 180f, 0f);

            //volteo del hitbox y trigger
            meleeHitBoxOffset.x *= -1;
            triggerOffset.x *= -1;
        }

        if (dropOnDeath != null)
        {
            if (dropOnDeath.GetComponent<Collectable>())
            {
                safeGuard = transform.position;
            }
        }

        if (dropOnHit != null)
        {
            if (dropOnHit.GetComponent<Collectable>())
            {
                safeGuard = transform.position;
            }
        }

        PhysicsMaterial2D physicMaterial = new PhysicsMaterial2D(gameObject.name);
        physicMaterial.bounciness = 0;
        physicMaterial.friction = 1;

        rb.sharedMaterial = physicMaterial;
    }

    void Update()
    {
        DetectCollisions();

        getPlayerDir();

        //si el jugador me salta por arriba, freno
        if (playerDirection.y == 1 && playerDirection.x == 0)
        {
            generalActionsDelay = ActionsDelay(0.6f);

            StopCoroutine(generalActionsDelay);
            StartCoroutine(generalActionsDelay);
        }

        //material fisico que no se trabe en las paredes pero que no resbale en el piso
        if (onGround)
        {
            rb.sharedMaterial.friction = 1;
        }
        else if (!onGround)
        {
            rb.sharedMaterial.friction = 0;
        }

        //que cuando salte no pueda cambiar de direccion
        if(!onGround && isJumping)
        {
            int dir;
            if (fRight && !onRightWall && !colRight)
            {
                dir = 1;
            }
            else if (!onLeftWall && !colLeft)
            {
                dir = -1;
            }
            else
            {
                dir = 0;
            }

            rb.velocity = new Vector2(dir * speed, rb.velocity.y);
        }
        else if(onGround && isJumping)
        {
            isJumping = false;
        }

        //Comportamientos
        if (trigger && lives > 0)
        {
            patrol = false;

            //saltar (y frenar cuando se encuentra un precipicio) NO CAMBIAR DE LUGAR NI PONER OTROS COMPORTAMIENTOS ARRIBA
            if (onGround && ( (!onLeftFloor || !onRightFloor) || (onLeftWall || onRightWall) ) )
            {
                generalActionsDelay = ActionsDelay(0.6f);

                //cuando huye, es invertido
                if (flee && canFlee && fleeActive)
                {                
                if ( (playerDirection.x == -1 && !onRightFloor) || (playerDirection.x == 1 && !onLeftFloor) || 
                     (playerDirection.x == -1 && onRightWall) || (playerDirection.x == 1 && onLeftWall) )
                {
                    //saltar
                    if (JumpObstacles && !isJumping && onGround)
                    {
                        isJumping = true;
                        rb.velocity = new Vector2(rb.velocity.x, 0);
                        rb.velocity += Vector2.up * jumpForce;                        
                    }
                    else //acorralado, se queda quieto
                    {
                        cornered = true;
                        playerDirection.x = 0;
                        //Debug.Log("No puedo huir hasta el vacio");
                    }

                }
                }
                //cuando lo persigue
                else if (chase && (playerDirection.x == 1 && !onRightFloor) || (playerDirection.x == -1 && !onLeftFloor) || (playerDirection.x == -1 && onLeftWall) || (playerDirection.x == 1 && onRightWall) )
                {
                    //saltar
                    if (JumpObstacles && !isJumping && onGround)
                    {
                        isJumping = true;
                        rb.velocity = new Vector2(rb.velocity.x, 0);
                        rb.velocity += Vector2.up * jumpForce;
                    }
                    else if (!modoSuicida)
                    {
                        playerDirection.x = 0;
                        //Debug.Log("No puedo perseguir hasta el vacio");
                    }
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

                rb.velocity = new Vector2(-playerDirection.x * speedFlee, rb.velocity.y);

                nirvana = false;
            }
            else if (flee && playerDistanceAbs.x > safeDistance)
            {
                fleeActive = false;
            }

            //perseguir (el onGround puede joder el salto ¿sacarlo podria arreglarlo?)
            if (chase && canChase && onGround && playerDirection.x != 0 && !fleeActive)
            {
                if (flee && canFlee && (playerDistanceAbs.x > safeDistance && playerDistanceAbs.x < safeDistance + 1f))
                {
                    nirvana = true;
                    //Debug.Log("Me quedo tranqui");
                }
                else
                {
                    //Debug.Log("Estoy persiguiendo");
                    nirvana = false;

                    if (sprint && canSprint)
                    {
                        //Debug.Log("corriendo");
                        rb.velocity = new Vector2(playerDirection.x * sprintSpeed, rb.velocity.y);

                        if (!sprintOn)
                        {
                            sprintOn = true;
                            SprintDelay = Sprint(sprintDelay, sprintDuration);
                            StopCoroutine(SprintDelay);
                            StartCoroutine(SprintDelay);                        
                        }
                    }
                    else
                    {
                        //Debug.Log("caminando");
                        rb.velocity = new Vector2(playerDirection.x * speed, rb.velocity.y);
                    }
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
                bulletInst.GetComponent<Bullet>().shooter = gameObject.name;
            }

        }
        else if (patrol && canPatrol)
        {
            if (onGround &&
                ((fRight && !onRightFloor) || (!fRight && !onLeftFloor))
                || ((fRight && onRightWall) || (!fRight && onLeftWall)))
            {
                generalActionsDelay = ActionsDelay(0.6f);

                Flip = DelayFlip();
                StopCoroutine(Flip);
                StartCoroutine(Flip);

                generalActionsDelay = ActionsDelay(0.4f);
                StopCoroutine(generalActionsDelay);
                StartCoroutine(generalActionsDelay);
            }

            if (fRight)
            {
                rb.velocity = new Vector2(1 * patrolSpeed, rb.velocity.y);
            }
            else if (!fRight)
            {
                rb.velocity = new Vector2(-1 * patrolSpeed, rb.velocity.y);
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

        playerDistance = new Vector2(xDis, yDis);
        playerDistanceAbs = new Vector2(Mathf.Abs(xDis), Mathf.Abs(yDis));
        playerDirection = new Vector2(xDir, yDir);
    }

    void DetectCollisions()
    {
        //Colisiones con el piso:
        onGround = Physics2D.OverlapBox((Vector2)transform.position + bottomOffset, (Vector2)bottomSize, 0f, groundLayer);

        onRightFloor = Physics2D.OverlapCircle((Vector2)transform.position + rightFloor, collisionRadius, groundLayer);
        onLeftFloor = Physics2D.OverlapCircle((Vector2)transform.position + leftFloor, collisionRadius, groundLayer);

        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightWall, collisionRadius, groundLayer);
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftWall, collisionRadius, groundLayer);

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
        if (hit && frameDamage && !hited && meleeDamage > 0 && lives > 0)
        {
            

            player.TakeDamage(meleeDamage, playerDirection.x, gameObject.name);
            hited = true;
        }

    }

    void SetAnim()
    {
        Flip = DelayFlip();

        anim.SetBool("OnGround", onGround);

        //caminar
        if (((trigger && ((chase && canChase) || (flee && canFlee && fleeActive)))
            || (patrol && canPatrol))
            && onGround && playerDirection.x != 0 && !nirvana)
        {
            anim.SetBool("Run", true);
            if (sprint && canSprint && !patrol)
            {
                anim.SetBool("Sprint", true);
            }
        }
        else
        {
            anim.SetBool("Run", false);
            anim.SetBool("Sprint", false);
        }

        if(!onGround && isJumping && lives > 0)
        {
            anim.SetTrigger("Jump");
        }

        //volteos
        if (trigger && canFlip)
        {
            if (flee && fleeActive && !cornered)
            {
                //darle la espalda al jugador
                if (playerDirection.x < 0 && !fRight)
                {
                    StopCoroutine(Flip);
                    StartCoroutine(Flip);
                }
                else if (playerDirection.x > 0 && fRight)
                {
                    StopCoroutine(Flip);
                    StartCoroutine(Flip);
                }
            }
            else
            {
                //voltear segun donde este el jugador
                if (playerDirection.x > 0 && !fRight)
                {
                    StopCoroutine(Flip);
                    StartCoroutine(Flip);
                }
                else if (playerDirection.x < 0 && fRight)
                {
                    StopCoroutine(Flip);
                    StartCoroutine(Flip);
                }
                else if (cornered)
                {
                    if (playerDistance.x > 0.01 && !fRight)
                    {
                        StopCoroutine(Flip);
                        StartCoroutine(Flip);
                    }
                    else if (playerDistance.x < -0.01 && fRight)
                    {
                        StopCoroutine(Flip);
                        StartCoroutine(Flip);
                    }
                }
            }
        }
    }

    //si choco con algo
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //deteccion absoluta e indicriminada de colisiones con superficies solidas
        if(collision.gameObject.layer == 8)
        {
            foreach (ContactPoint2D hitPos in collision.contacts)
            {
                //Debug.Log(hitPos.normal);
                colDown = hitPos.normal.y == 1;
                colUp = hitPos.normal.y == -1;
                colLeft = hitPos.normal.x == 1;
                colRight = hitPos.normal.x == -1;
            }
        }

        if (collision.gameObject.tag == "Fall")
        {

            culpable = "Abismo";
            if (dropOnDeath != null && !droped)
            {
                if (dropOnDeath.GetComponent<Collectable>())
                {
                    Instantiate(dropOnDeath, safeGuard, Quaternion.identity);
                }
            }

            if (dropOnHit != null && !droped)
            {
                if (dropOnHit.GetComponent<Collectable>())
                {
                    Instantiate(dropOnHit, safeGuard, Quaternion.identity);
                }
            }

            droped = true;

            TakeDamage(1000, 0);
        }
    }

    IEnumerator DelayFlip()
    {
        if (fRight)
        {
            fRight = false;
        }
        else if (!fRight)
        {
            fRight = true;
        }        

        yield return new WaitForSeconds(0.2f);

        transform.Rotate(0f, 180f, 0f);

        //volteo del hitbox y trigger
        meleeHitBoxOffset.x *= -1;
        triggerOffset.x *= -1;
    }

    //frenar todas las acciones y comportamientos designados y resumirlos luego de un tiempo
    IEnumerator ActionsDelay(float delay)
    {
        if (chase)
        {
            canChase = false;
        }
        
        if (meleeDamage > 0)
        {
            canAttack = false;
        }
                
        if (flee)
        {
            canFlee = false;
        }

        canFlip = false;

        if (patrol && !trigger)
        {
            canPatrol = false;
        }

        yield return new WaitForSeconds(delay);

        if (chase)
        {
            canChase = true;
        }
        
        if (meleeDamage > 0)
        {
            canAttack = true;
            hited = false;
        }

        if (flee)
        {
            canFlee = true;
        }

        canFlip = true;
       
        if (patrol && !trigger)
        {
            canPatrol = true;
        }
    }

    IEnumerator ShootDelayCou(float delay)
    {
        canShoot = false;
        yield return new WaitForSeconds(delay);
        canShoot = true;
    }

    IEnumerator Sprint(float delay, float duration)
    {
        //canSprint = true;
        sprintOn = true;

        yield return new WaitForSeconds(duration);
        canSprint = false;

        yield return new WaitForSeconds(delay);
        canSprint = true;
        sprintOn = false;

    }

    IEnumerator DeathDelay(float delay)
    {

        GameManager.instance.MatarEnemigo(transform.position.x, transform.position.y, culpable);

        if (dropOnDeath != null && !droped)
        {
            droped = true;
            Instantiate(dropOnDeath, transform.position += new Vector3(0, 1.2f), Quaternion.identity);
        }

        if(MoneyValue > 0)
        {
            Money money = Instantiate(MoneyObject, transform.position += new Vector3(0, 1.2f), Quaternion.identity).GetComponent<Money>();
            money.value = MoneyValue;
            MoneyValue = -1;
        }

        yield return new WaitForSeconds(delay);

        if (FindObjectOfType<KillAll>() != null)
        {
            FindObjectOfType<KillAll>().toKill -= 1;
        }

        Destroy(gameObject);
    }

    public void TakeDamage(int damage, float dir)
    {
        trigger = true;

        lives -= damage;

        if (lives == 0)
        {
            anim.SetBool("Death", true);

            //if (FindObjectOfType<KillAll>() != null)
            //{
            //    FindObjectOfType<KillAll>().toKill -= 1;
            //}

            lives = -1;

            StopAllCoroutines();
            StartCoroutine(DeathDelay(1f));
        }
        else if (lives > 0)
        {
            anim.SetTrigger("GetHit");

            rb.AddForce(new Vector2(dir * 150, 200));

            generalActionsDelay = ActionsDelay(hitStunTimer);

            StopCoroutine(generalActionsDelay);
            StartCoroutine(generalActionsDelay);
        }
        else if(lives < 0 && !anim.GetBool("Death"))
        {
            StopAllCoroutines();
            StartCoroutine(DeathDelay(0.1f));
        }

        if (dropOnHit != null && !droped)
        {
            droped = true;
            Instantiate(dropOnHit, transform.position + new Vector3(0, 1.2f), Quaternion.identity);
        }
    }

    public void Stun(float timer)
    {
        anim.SetTrigger("Stun");

        //bc2D.size = new Vector2(0, 1.5f);
        //bc2D.offset = new Vector2(0, 0.5f);
        if (meleeDamage > 0)
        {
            canAttack = false;
        }

        generalActionsDelay = ActionsDelay(timer);
        StopCoroutine(generalActionsDelay);
        StartCoroutine(generalActionsDelay);

        if (shootDamage > 0)
        {
            canShoot = false;

            ShootDelay = ShootDelayCou(shootDelay);
            StopCoroutine(ShootDelay);
            StartCoroutine(ShootDelay);
        }
    }

    public void Panic()
    {
        chase = false;
        sprint = true;
        sprintDuration = 10;

        flee = true;
        safeDistance = 50;

        JumpObstacles = true;
    }
}
