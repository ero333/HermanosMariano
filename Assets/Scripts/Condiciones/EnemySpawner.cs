using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemySpawner : MonoBehaviour
{
    public int maximumEnemies = 5;
    public int currentEnemies = 0;
    public float delay;
    public GameObject enemyToSpawn;

    bool canSpawn = false;

    [Header("Enemy Stats")]
    public bool OverwriteEnemy = true;
    [Space]
    public int moneyValue = 0;

    [Header("Vida")]
    public bool trigger = true;
    public int lives;
    public float hitStunTimer = 1f;
    public bool modoSuicida = false;

    [Header("Perseguir y correr")]
    public bool chase = true;
    public float speed = 10f;

    public bool sprint = false;
    public float sprintSpeed = 8f;
    public float sprintDuration = 3f;
    public float sprintDelay = 5f;

    [Header("Huir")]
    public bool flee = false;
    public float speedFlee = 5f;
    public float safeDistance = 10f;

    [Header("Saltar")]
    public bool JumpObstacles = false;
    public float jumpForce = 8;

    [Header("Golpear")]
    public int meleeDamage = 2;
    public float meleeDelay = 1;

    [Header("Disparar")]
    public int shootDamage = 1;
    public float shootDelay = 5f;
    public float minShootDis = 15f;

    void Start()
    {
        StartCoroutine(SpawnDelay(delay));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (enemyToSpawn != null && canSpawn && currentEnemies <= maximumEnemies)
        {
            GameObject enemy = Instantiate(enemyToSpawn, transform.position, Quaternion.identity);

            if (OverwriteEnemy)
            {
                Enemy enemyScript = enemy.GetComponent<Enemy>();

                enemyScript.MoneyValue = moneyValue;
                enemyScript.trigger = trigger;
                enemyScript.lives = lives;
                enemyScript.hitStunTimer = hitStunTimer;
                enemyScript.modoSuicida = modoSuicida;

                enemyScript.chase = chase;
                enemyScript.speed = speed;

                enemyScript.sprint = sprint;
                enemyScript.sprintSpeed = sprintSpeed;
                enemyScript.sprintDuration = sprintDuration;
                enemyScript.sprintDelay = sprintDelay;

                enemyScript.flee = flee;
                enemyScript.speedFlee = speedFlee;
                enemyScript.safeDistance = safeDistance;

                enemyScript.JumpObstacles = JumpObstacles;
                enemyScript.jumpForce = jumpForce;


                enemyScript.meleeDamage = meleeDamage;
                enemyScript.meleeDelay = meleeDelay;

                enemyScript.shootDamage = shootDamage;
                enemyScript.shootDelay = shootDelay;
                enemyScript.minShootDis = minShootDis;
            }

            canSpawn = false;

            StopAllCoroutines();
            StartCoroutine(SpawnDelay(delay));
        }

        currentEnemies = FindObjectsOfType<Enemy>().Length;        
    }

    IEnumerator SpawnDelay(float delay)
    {
        currentEnemies = FindObjectsOfType<Enemy>().Length;
        yield return new WaitForSeconds(delay);
        canSpawn = true;
        currentEnemies = FindObjectsOfType<Enemy>().Length;
    }

    public void CopyEnemyStats()
    {
        if (enemyToSpawn != null)
        {
            Enemy enemyScript = enemyToSpawn.GetComponent<Enemy>();

            moneyValue = enemyScript.MoneyValue;
            trigger = enemyScript.trigger;
            lives = enemyScript.lives;
            hitStunTimer = enemyScript.hitStunTimer;
            modoSuicida = enemyScript.modoSuicida;

            chase = enemyScript.chase;
            speed = enemyScript.speed;

            sprint = enemyScript.sprint;
            sprintSpeed = enemyScript.sprintSpeed;
            sprintDuration = enemyScript.sprintDuration;
            sprintDelay = enemyScript.sprintDelay;

            flee = enemyScript.flee;
            speedFlee = enemyScript.speedFlee;
            safeDistance = enemyScript.safeDistance;

            JumpObstacles = enemyScript.JumpObstacles;
            jumpForce = enemyScript.jumpForce;

            meleeDamage = enemyScript.meleeDamage;
            meleeDelay = enemyScript.meleeDelay;

            shootDamage = enemyScript.shootDamage;
            shootDelay = enemyScript.shootDelay;
            minShootDis = enemyScript.minShootDis;
        }
        else
        {
            Debug.LogError("No hay ningun enemigo para copiar");
        }
        
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(EnemySpawner))]
public class CopyEnemyStats : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EnemySpawner myScript = (EnemySpawner)target;
        if (GUILayout.Button("Copy Enemy Stats"))
        {
            myScript.CopyEnemyStats();
        }
    }
}
#endif