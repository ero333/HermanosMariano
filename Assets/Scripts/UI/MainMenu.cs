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
        GameManager.instance.maxLives = newMaxLives;
        GameManager.maxEnergy = newMaxEnergy;
        GameManager.zoneProgress = 1;
        GameManager.paidDeudas = 0;
        GameManager.seenDialogues = 0;

        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("CurrentZone", GameManager.zoneProgress);
        PlayerPrefs.SetInt("DeudasPagadas", GameManager.paidDeudas);
        PlayerPrefs.SetInt("Ahorros", GameManager.ahorros);
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
            GameManager.instance.maxLives = PlayerPrefs.GetInt("MaxLives");
            GameManager.maxEnergy = PlayerPrefs.GetInt("MaxEnergy");

            GameManager.seenDialogues = GameManager.zoneProgress - 1;

            SceneManager.LoadScene("MapaZonas");
        }        
    }   
}
