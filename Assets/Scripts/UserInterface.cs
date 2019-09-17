using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    GameManager gm;
    Player player;

    public Text LivesText;
    public Image[] EnergyBars;
    public Text MoneyText;
    //public Sprite EnergyComplete;
    //public Sprite EnergyEmpty;

    public GameObject GameOverScreen;
    public GameObject VictoryScreen;
    public Text ganancias;
    public Text impuestos;
    public Text total;
    public Text bullets;
    public MenuPausa menuPausa;

    public DialogueManager dialogueManager;

    public int gananciaMaxBK;

    private void Awake()
    {
        menuPausa = GetComponent<MenuPausa>();
    }

    void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
        GameOverScreen.SetActive(false);

        for (int i = 0; i < EnergyBars.Length; i++)
        {
            if (i < GameManager.maxEnergy)
            {
                EnergyBars[i].gameObject.SetActive(true);
            }
            else
            {
                EnergyBars[i].gameObject.SetActive(false);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
        LivesText.text = "" + gm.lives;
        MoneyText.text = "" + gm.money;
        bullets.text = "" + player.bullet;

        if (gm.lives == 0)
        {
            Time.timeScale = 0f;
            GameOverScreen.SetActive(true);
        }


        for (int i = 0; i < EnergyBars.Length; i++)
        {
            if (i < gm.energy)
            {
                EnergyBars[i].enabled = true;
            }
            else
            {
                EnergyBars[i].enabled = false;
            }            
        }
    }

    public void BackToMap()
    {
        Time.timeScale = 1f;
        gm.BackToMap();
    }

    public void Victory(int gananciaMaxima)
    {
        int impuesto = 0;

        Time.timeScale = 0f;

        if(dialogueManager.DialoguesEnd.Length > 0 && !dialogueManager.end)
        {
            gananciaMaxBK = gananciaMaxima;

            dialogueManager.gameObject.SetActive(true);
        }
        else
        {
            VictoryScreen.SetActive(true);
            ganancias.text = "" + gm.money;

            Debug.Log(gm.money);


            if (gm.money > gananciaMaxima)
            {
                impuesto = gm.money - gananciaMaxima;
            }
            else if (gm.money == gananciaMaxima)
            {
                impuesto = ((gananciaMaxima * 5) / 100);
            }
            else
            {
                impuesto = ((gananciaMaxima * 10) / 100);
            }

            impuestos.text = "" + impuesto;
            Debug.Log(impuesto);
            total.text = "" + (gm.money - impuesto);
            GameManager.ahorros += gm.money - impuesto;
        }        
    }
    
}
