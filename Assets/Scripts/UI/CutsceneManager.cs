using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public VideoManager videoManager;
    public DialogueManager DM;
    public NewDialogueManager newDM;

    [HideInInspector]
    public bool activeCutscenes = true;

    public bool DMactive = false;
    public bool newDMactive = false;

    [HideInInspector]
    public bool playerAnim = false;

    private void Awake()
    {
        if (DM != null)
        {
            if (DM.Dialogues.Length > 0)
            {
                DMactive = true;
            }
            else
            {
                DM.gameObject.SetActive(false);
            }
        }

        if (newDM != null)
        {
            if (newDM.StartCutscene != null)
            {
                newDMactive = true;
            }
        }

        if (videoManager.videoName != "")
        {
            videoManager.gameObject.SetActive(true);

            if (DMactive)
            {
                DM.gameObject.SetActive(false);
            }

            if (newDMactive)
            {
                newDM.gameObject.SetActive(false);
            }
        }
        else
        {
            videoManager.gameObject.SetActive(false);

            if (DMactive)
            {
                DM.gameObject.SetActive(true);
            }

            if (newDMactive)
            {
                newDM.gameObject.SetActive(true);
            }

            if (!DMactive && !DMactive)
            {
                activeCutscenes = false;
            }
        }
    }

    public void VideoEnded()
    {
        if (DMactive)
        {
            DM.gameObject.SetActive(true);
            activeCutscenes = true;
        }

        if (newDMactive)
        {
            newDM.gameObject.SetActive(true);
            activeCutscenes = true;
        }

        if (!DMactive && !DMactive)
        {
            activeCutscenes = false;
        }
    }

    public void IntroCutsceneEnded()
    {
        activeCutscenes = false;
    }

    public void onVictory()
    {
        UserInterface ui = FindObjectOfType<UserInterface>();
        
        //El jugador esta reproducciendo su animacion de victoria?
        if (playerAnim)
        {
            //Si es asi, llamar hasta que termine de reproducirse
            InvokeRepeating("WaitForPlayerAnim", 0.2f, 0.5f);
        }
        else
        {
            if (DMactive)
            {
                if (DM.DialoguesEnd.Length > 0)
                {
                    DM.gameObject.SetActive(true);
                }
                else
                {
                    DM.gameObject.SetActive(false);
                    ui.Victory(ui.gananciaMaxBK);
                }
            }

            if (newDMactive)
            {
                if (newDM.EndCutscene != null)
                {
                    newDM.gameObject.SetActive(true);
                }
                else
                {
                    newDM.gameObject.SetActive(false);
                    ui.Victory(ui.gananciaMaxBK);
                }
            }

            if (!DMactive && !newDMactive)
            {
                ui.Victory(ui.gananciaMaxBK);
            }
        }
    }

    void WaitForPlayerAnim()
    {
        if (!playerAnim)
        {
            onVictory(); //volver a llamar
            CancelInvoke();
        }
    }

    public void EndCutsceneStarted()
    {
        activeCutscenes = true;
    }
}
