using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("Start")]
    public Sprite charAstart;
    public Sprite charBstart;

    public DialogueClass[] Dialogues;

    [Header("Referencias")]
    public Text characterText;
    public Text dialogueText;
    public Image charA;
    public Image charB;

    int index = 0;

    void Start()
    {
        if (Dialogues.Length == 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            charA.sprite = charAstart;
            charB.sprite = charBstart;
        }
    }

   
    void Update()
    {
        if(Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space))
        {
            index += 1;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }

        if (index < Dialogues.Length)
        {
            dialogueText.text = Dialogues[index].Dialogue;

            if (Dialogues[index].character == DialogueClass.PosibleCharacters.CharA)
            {
                charA.sprite = Dialogues[index].CharArt;
                charA.color = new Color(1, 1, 1, 1f);

                charB.color = new Color(1,1,1,0.5f);
            }
            else
            {
                charB.sprite = Dialogues[index].CharArt;
                charB.color = new Color(1, 1, 1, 1f);

                charA.color = new Color(1, 1, 1, 0.5f);
            }
        }
        else
        {
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }


    }
}
