using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewDialogueManager : MonoBehaviour
{
    UserInterface ui;
    CutsceneManager cutM;

    [Header("Start")]
    public DialogueData StartCutscene;

    [Header("End")]
    public DialogueData EndCutscene;

    [Header("Referencias")]
    public Text Speaker;
    public Text dialogueText;
    public Image charA;
    public Image charB;
    public Image background;

    int index = 0;
    int indexAna = 0;

    [HideInInspector]
    public bool end = false;

    [HideInInspector]
    public bool obli = false;


    private void Awake()
    {
        if (StartCutscene.Dialogues.Length == 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;

            //background = GetComponent<Image>();

            charA.sprite = StartCutscene.charAstart;
            charB.sprite = StartCutscene.charBstart;
        }

        
    }
    void Start()
    {
        ui = FindObjectOfType<UserInterface>();
        cutM = FindObjectOfType<CutsceneManager>();

        if (StartCutscene.Dialogues.Length > 0)
        {
            ui.menuPausa.enabled = false;
            Time.timeScale = 0;
        }        
    }
    
    void Update()
    {
        if(Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space))
        {
            index += 1;
            indexAna = index - 1;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1;
            ui.menuPausa.enabled = true;
            indexAna = index;
            index += 100;
            //gameObject.SetActive(false);            
        }

        if (!GameManager.instance.victory)
        {
            if (index < StartCutscene.Dialogues.Length)
            {
                if(StartCutscene.Dialogues[index].background != null)
                {
                    background.sprite = StartCutscene.Dialogues[index].background;
                }                
                
                if(StartCutscene.Dialogues[index].Speaker != null)
                {
                    Speaker.text = StartCutscene.Dialogues[index].Speaker;
                }
                else
                {
                    Speaker.text = StartCutscene.Dialogues[index - 2].Speaker;
                }

                dialogueText.text = StartCutscene.Dialogues[index].Dialogue;
                
                if (StartCutscene.Dialogues[index].character == DialogueClass.PosibleCharacters.CharA)
                {
                    charA.sprite = StartCutscene.Dialogues[index].CharArt;
                    charA.color = new Color(1, 1, 1, 1f);

                    charB.color = new Color(1, 1, 1, 0.5f);
                }
                else
                {
                    charB.sprite = StartCutscene.Dialogues[index].CharArt;
                    charB.color = new Color(1, 1, 1, 1f);

                    charA.color = new Color(1, 1, 1, 0.5f);
                }

                if (charA.sprite == null)
                {
                    charA.color = new Color(1, 1, 1, 0f);
                }

                if(charB.sprite == null)
                {
                    charB.color = new Color(1, 1, 1, 0f);
                }

            }
            else
            {
                if(index < 100)
                {
                    GameManager.instance.CompletarCutscene("intro", StartCutscene.Dialogues.Length, indexAna);
                }
                else
                {
                    GameManager.instance.SaltearCutscene("intro", StartCutscene.Dialogues.Length, indexAna);
                }
                

                Time.timeScale = 1;
                index = 0;
                ui.menuPausa.enabled = true;

                cutM.IntroCutsceneEnded();

                gameObject.SetActive(false);
            }
        }
        else if (GameManager.instance.victory)
        {
            if (index < EndCutscene.Dialogues.Length && EndCutscene.Dialogues.Length > 0)
            {
                cutM.EndCutsceneStarted();

                if (EndCutscene.Dialogues[index].background != null)
                {
                    background.sprite = EndCutscene.Dialogues[index].background;
                }

                if (EndCutscene.Dialogues[index].Speaker != "")
                {
                    Speaker.text = EndCutscene.Dialogues[index].Speaker;
                }
                else
                {
                    Speaker.text = EndCutscene.Dialogues[index - 2].Speaker;
                }

                dialogueText.text = EndCutscene.Dialogues[index].Dialogue;

                if (EndCutscene.Dialogues[index].character == DialogueClass.PosibleCharacters.CharA)
                {
                    charA.sprite = EndCutscene.Dialogues[index].CharArt;
                    charA.color = new Color(1, 1, 1, 1f);

                    charB.color = new Color(1, 1, 1, 0.5f);
                }
                else
                {
                    charB.sprite = EndCutscene.Dialogues[index].CharArt;
                    charB.color = new Color(1, 1, 1, 1f);

                    charA.color = new Color(1, 1, 1, 0.5f);
                }

                if (charA.sprite == null)
                {
                    charA.color = new Color(1, 1, 1, 0f);
                }

                if (charB.sprite == null)
                {
                    charB.color = new Color(1, 1, 1, 0f);
                }
            }
            else
            {
                if (index < 100)
                {
                    GameManager.instance.CompletarCutscene("final", EndCutscene.Dialogues.Length, indexAna);
                }
                else
                {
                    GameManager.instance.SaltearCutscene("final", EndCutscene.Dialogues.Length, indexAna);
                }                

                end = true;
                Debug.Log("Ended end dialogue");
                //Time.timeScale = 1;
                ui.Victory(ui.gananciaMaxBK, obli);
                gameObject.SetActive(false);
            }
        }
    }
}
