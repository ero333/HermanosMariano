using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    GameManager gm;
    UserInterface ui;
    CutsceneManager cutM;

    //[HideInInspector]
    public float startingTime;
    public float currentTime = 100;

    //bool canStart = false;

    //public Text countdownText;

    private void Awake()
    {
        //if (!canStart)
        //{
        //    gameObject.SetActive(false);
        //}
    }

    void Start()
    {
        currentTime = startingTime;
        gm = GameManager.instance;
        ui = FindObjectOfType<UserInterface>();
        cutM = FindObjectOfType<CutsceneManager>();

        //if (!cutM.activeCutscenes)
        //{
        //    canStart = true;
        //}
        //else
        //{
        //    canStart = false;
        //}

        //if(FindObjectOfType<DialogueManager>())
        //{
        //    if(FindObjectOfType<DialogueManager>().Dialogues.Length > 0)
        //    {
        //        canStart = true;
        //    }
        //}
        //else
        //{
        //    canStart = true;
        //}

        //ui.transform.GetChild(0).gameObject.SetActive(true);
    }



    // Update is called once per frame
    void Update()
    {
        if (!cutM.activeCutscenes)
        {
            currentTime -= 1 * Time.deltaTime;
            //countdownText.text = currentTime.ToString("0");

            if (currentTime <= 0)
            {
                currentTime = 0;
                Time.timeScale = 0f;

                if (!gm.victory)
                {
                    ui.GameOverScreen.SetActive(true);
                }
                else
                {
                    gameObject.SetActive(false);
                }

            }

        }
    }
}
