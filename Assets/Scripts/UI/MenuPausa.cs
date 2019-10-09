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

        textoNivel.text = "" + SceneManager.GetActiveScene().name;

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
        Time.timeScale = 1f;
        GameManager.instance.SaveData();
        SceneManager.LoadScene("MenuInicio");        
    }
    public void VolverAlMapa()
    {
        Time.timeScale = 1f;
        GameManager.instance.BackToMap();
    }

    public void ReiniciarNivel()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.LevelReset();
    }

}   


