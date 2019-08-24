using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    int levelIndex;

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
