using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    public static bool juegoPausado = false;
    public GameObject MenuPausaUI;

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
        SceneManager.LoadScene("MenuPrincipal");
    }
    public void VolverAlMapa()
    {
        SceneManager.LoadScene("VolverAlMapa");
    }
}   


