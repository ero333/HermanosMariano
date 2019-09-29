using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    GameManager gm;
    UserInterface ui;

    float currentTime = 0.0f;
    public float startingTime;

    public Text countdownText;


    // Start is called before the first frame update
    void Start()
    {
        currentTime = startingTime;
        gm = GameObject.FindObjectOfType<GameManager>();
        ui = FindObjectOfType<UserInterface>();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        countdownText.text = currentTime.ToString("0");

        if (currentTime <= 0)
        {
            currentTime = 0;
            Time.timeScale = 0f;
            ui.GameOverScreen.SetActive(true);
        }
    }
}
