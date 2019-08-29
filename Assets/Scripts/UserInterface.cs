﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    GameManager gm;

    public Text LivesText;
    public Image[] EnergyBars;
    //public Sprite EnergyComplete;
    //public Sprite EnergyEmpty;
    
    void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();

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
