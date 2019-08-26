using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    int levelIndex;

    [Header("Datos Permanentes")]
    public int maxEnergy = 6;
    public int maxLives = 3;

    private void Awake()
    {
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
        levelIndex = SceneManager.GetActiveScene().buildIndex;
    }

    void Update()
    {
        
    }

    private void OnLevelWasLoaded(int level)
    {
        
    }

    public void Respawn()
    {
        SceneManager.LoadScene(levelIndex);
    }
}
