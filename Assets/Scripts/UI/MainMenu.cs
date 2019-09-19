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
        GameManager.ahorros = 0;
        gm.maxLives = newMaxLives;
        GameManager.maxEnergy = newMaxEnergy;
        GameManager.zoneProgress = 0;
        GameManager.paidDeudas = 0;

        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("CurrentZone", 0);
        PlayerPrefs.SetInt("DeudasPagadas", 0);
        PlayerPrefs.SetInt("Ahorros", 0);
        PlayerPrefs.SetInt("MaxLives", newMaxLives);
        PlayerPrefs.SetInt("MaxEnergy", newMaxEnergy);

        SceneManager.LoadScene("NivelIntroduccion");
    }

    public void Continue()
    {
        if (PlayerPrefs.HasKey("CurrentZone"))
        {
            GameManager.zoneProgress = PlayerPrefs.GetInt("CurrentZone");
            GameManager.paidDeudas = PlayerPrefs.GetInt("DeudasPagadas");
            GameManager.ahorros = PlayerPrefs.GetInt("Ahorros");
            gm.maxLives = PlayerPrefs.GetInt("MaxLives");
            GameManager.maxEnergy = PlayerPrefs.GetInt("MaxEnergy");

            SceneManager.LoadScene("MapaZonas");
        }        
    }

    
}
