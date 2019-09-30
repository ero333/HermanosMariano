using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    GameManager gm;
    UserInterface ui;

    //[HideInInspector]
    public float currentTime = 0.0f;

    public float startingTime;

    public bool canStart = false;

    //public Text countdownText;


    // Start is called before the first frame update
    void Start()
    {
        currentTime = startingTime;
        gm = GameManager.instance;
        ui = FindObjectOfType<UserInterface>();

        //if(FindObjectOfType<DialogueManager>().Dialogues.Length == 0)
        //{
        //    canStart = true;
        //}

        //ui.transform.GetChild(0).gameObject.SetActive(true);
    }



    // Update is called once per frame
    void Update()
    {
        if (canStart)
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
