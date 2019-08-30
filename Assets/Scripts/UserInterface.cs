using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    GameManager gm;

    public Text LivesText;
    public Image[] EnergyBars;
    public Text MoneyText;
    //public Sprite EnergyComplete;
    //public Sprite EnergyEmpty;

    public GameObject GameOverScreen;

    void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
        GameOverScreen.SetActive(false);

        for (int i = 0; i < EnergyBars.Length; i++)
        {
            if (i < gm.maxEnergy)
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

        if (gm.lives == 0)
        {
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

    
}
