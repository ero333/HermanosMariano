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

    bool DMactive = false;
    bool newDMactive = false;

    private void Awake()
    {
        if(DM != null)
        {
            if(DM.Dialogues.Length > 0)
            {
                DMactive = true;
            }
            else
            {
                DM.gameObject.SetActive(false);
            }
        }

        if(newDM != null)
        {
            if(newDM.StartCutscene != null)
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

            if(!DMactive && !DMactive)
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
        if (DMactive)
        {
            DM.gameObject.SetActive(true);
        }

        if (newDMactive)
        {
            newDM.gameObject.SetActive(true);
        }
    }

    public void EndCutsceneStarted()
    {
        activeCutscenes = true;
    }
}
