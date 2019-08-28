using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    GameManager gm;
    public Text LiveText;


    public int Energy;
    public int AllEnergy;
    public Image[] EnergyBar;
    public Sprite EnergyComplete;
    public Sprite EnergyEmpty;
    
    void Start()
    {
        gm = GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Energy > AllEnergy)
        {
            Energy = AllEnergy;
        } 

        for (int i = 0; i < EnergyBar.Length; i++)
        {
            if (i < Energy)
            {
                EnergyBar[i].sprite = EnergyComplete;
            }
            else{
                EnergyBar[i].sprite = EnergyEmpty;
            }


            if (i < AllEnergy) {
                EnergyBar[i].enabled = true;
            }
            else {
                EnergyBar[i].enabled = false;
            }
        }
    }

    
}
