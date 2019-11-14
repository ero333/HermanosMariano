using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManagerMap : MonoBehaviour
{
    public bool seen = false;
    public string loadLevel = "MenuInicio";

    [Header("Start")]
    public Sprite charAstart;
    public Sprite charBstart;

    public DialogueClass[] Dialogues;

    [Header("Referencias")]
    public Text Speaker;
    public Text dialogueText;
    public Image charA;
    public Image charB;
    public Image background;
    public Text SpaceoZ;
    public Text ESC;

    int index = 0;
    int indexAN = 0;

    public bool active = false;

    private void Awake()
    {
        if (Dialogues.Length == 0 || seen)
        {
            background.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            charA.sprite = charAstart;
            charB.sprite = charBstart;            
        }
    }
    
    void Update()
    {
        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space))
        {
            index += 1;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            indexAN = index;

            Time.timeScale = 1;
            index += 100;
            seen = true;
        }

        if (index < Dialogues.Length && active && !seen)
        {
            if (Dialogues[index].background != null)
            {
                background.sprite = Dialogues[index].background;
            }

            if (Dialogues[index].Speaker != null)
            {
                Speaker.text = Dialogues[index].Speaker;
            }
            else
            {
                Speaker.text = Dialogues[index - 2].Speaker;
            }

            dialogueText.text = Dialogues[index].Dialogue;

            if (Dialogues[index].character == DialogueClass.PosibleCharacters.CharA)
            {
                charA.sprite = Dialogues[index].CharArt;
                charA.color = new Color(1, 1, 1, 1f);

                charB.color = new Color(1, 1, 1, 0.5f);
            }
            else
            {
                charB.sprite = Dialogues[index].CharArt;
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
        else if (active)
        {
            string name = gameObject.name;
            int zone = 0;
            for (int i = 0; i < name.Length; i++)
            {
                if (char.IsNumber(name[i]))
                {
                    zone = name[i] - '0';
                }
            }

            if (index < 100) //leyo los dialogos
            {   
                GameManager.instance.MapaCompletarCutscene(Dialogues.Length, loadLevel, zone);
            }
            else //se los salteo
            {
                GameManager.instance.MapaSaltearCutscene(Dialogues.Length, indexAN, loadLevel, zone);
            }
            
            Time.timeScale = 1;            
            index = 0;
            active = false;
            seen = true;
            GameManager.seenDialogues = GameManager.zoneProgress;
            GameManager.instance.SaveData();
            background.gameObject.SetActive(false);
            GameManager.instance.LoadScene(loadLevel);
        }
    }
}
