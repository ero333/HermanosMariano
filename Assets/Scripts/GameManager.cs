﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    int levelIndex;

    [Header("Datos que se mantienen")]
    public int maxEnergy = 6;
    public int maxLives = 3;

    [Header("Datos del nivel")]
    int resetCount = 0;
    int energy = 10;
    int lives = 3;

    private void Awake()
    {
        //No tocar, permite mantener al gamemanager en todas las escenas y poder probar todos los niveles sin tener que empezar desde el menu
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        //levelIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnLevelWasLoaded(int level)
    {       
        levelIndex = SceneManager.GetActiveScene().buildIndex;

        if (resetCount == 0)
        {
            lives = GameManager.instance.maxLives;
            energy = GameManager.instance.energy;
        }
        
        //Si hay una, Buscar condicion de victoria. Cuando gano --> resetCount = 0;

    }

    void Update()
    {
        if (lives <= 0)
        {
            //game over
        }

        if (energy <= 0)
        {
            lives -= 1;
            Reset();
        }
    }    

    public void Reset()
    {
        resetCount += 1;
        SceneManager.LoadScene(levelIndex);
        
        //Si toco un checkpoint...
    }
}
