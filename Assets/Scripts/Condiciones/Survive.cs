using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survive : MonoBehaviour
{
    GameManager gm;
    UserInterface ui;
    CutsceneManager cutM;

    public float countDown = 60;

    public int gananciaMaxima = 100;

    public bool Obligatorio = false;
    public int zonaActual;

    private void Start()
    {
        gm = GameManager.instance;
        ui = GameObject.FindObjectOfType<UserInterface>();
        cutM = FindObjectOfType<CutsceneManager>();
    }

    private void Update()
    {
        if (!cutM.activeCutscenes)
        {
            if (countDown > 0f)
            {
                countDown -= 1 * Time.deltaTime;
            }
            else
            {
                GameManager.instance.VictoryCondition();
                ui.Victory(gananciaMaxima, Obligatorio);

                if (Obligatorio)
                {
                    GameManager.instance.UnlockZone(zonaActual);
                }

                gameObject.SetActive(false);
            }
        }               
    }
}
