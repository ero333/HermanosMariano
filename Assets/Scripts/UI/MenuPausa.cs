using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPausa : MonoBehaviour
{
    public bool juegoPausado = false;
    public GameObject MenuPausaUI;
    public Text textoNivel;

    private void Start()
    {
        juegoPausado = false;
        Time.timeScale = 1f;

        if(textoNivel.text == "Nivel")
        {
            textoNivel.text = GameManager.instance.level;
        }        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado)
            {
                Continuar();
            }
            else
            {
                Pausar();
            }
        }
    }

    public void Continuar()
    {
        MenuPausaUI.SetActive(false);
        Time.timeScale = 1f;
        juegoPausado = false;
    }
    void Pausar()
    {
        MenuPausaUI.SetActive(true);
        Time.timeScale = 0f;
        juegoPausado = true;
    }
    public void MenuPrincipal()
    {
        GameManager.instance.ExitLevelAnalyticsEvent();
        Time.timeScale = 1f;
        GameManager.instance.SaveData();
        SceneManager.LoadScene("MenuInicio");
    }
    public void VolverAlMapa()
    {
        GameManager.instance.ExitLevelAnalyticsEvent();
        Time.timeScale = 1f;
        GameManager.instance.BackToMap();
    }

    public void ReiniciarNivel()
    {
        GameManager.instance.ResetLevelPauseMenu();
        GameManager.instance.LevelReset();
    }

    public void ReiniciarNivelGameOver()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //Debug.Log("ReiniciarNivelGameOver");
        GameManager.instance.ReiniciarNivelGameOver();
        GameManager.instance.LevelReset();
    }

}   


