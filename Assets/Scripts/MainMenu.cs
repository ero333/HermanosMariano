using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    GameManager gm;

    public int newMaxEnergy;
    public int newMaxLives;
    

    void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
    }

    public void NewGame()
    {
        gm.ahorros = 0;
        gm.maxLives = newMaxLives;
        gm.maxEnergy = newMaxEnergy;
        gm.zoneProgress = 1;

        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("CurrentZone", 1);
        PlayerPrefs.SetInt("Ahorros", 0);
        PlayerPrefs.SetInt("MaxLives", newMaxLives);
        PlayerPrefs.SetInt("MaxEnergy", newMaxEnergy);

        SceneManager.LoadScene("NivelIntroduccion");
    }

    public void Continue()
    {
        if (PlayerPrefs.HasKey("CurrentZone"))
        {
            gm.zoneProgress = PlayerPrefs.GetInt("CurrentZone");
            gm.ahorros = PlayerPrefs.GetInt("Ahorros");
            gm.maxLives = PlayerPrefs.GetInt("MaxLives");
            gm.maxEnergy = PlayerPrefs.GetInt("MaxEnergy");

            SceneManager.LoadScene("MapaZonas");
        }        
    }

    
}
