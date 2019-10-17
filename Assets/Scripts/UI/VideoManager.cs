using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoManager : MonoBehaviour
{
    CutsceneManager cutM;
    VideoPlayer videoPlayer;
    public string videoName;

    public UserInterface ui;
    //public DialogueManager dialog;
    Image back;
    public Text SpaceoZ;
    

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        if (!GameManager.sound)
        {
            videoPlayer.SetDirectAudioMute(0, true);
        }

        if (videoName != "")
        {
            ui.menuPausa.enabled = false;           
            videoPlayer.url = Path.Combine(Application.streamingAssetsPath, videoName + ".mp4");
            videoPlayer.loopPointReached += EndVideo;
            GetComponent<Image>().enabled = true;
        }
        else
        {
            //if (dialog.Dialogues.Length > 0)
            //{
            //    dialog.gameObject.SetActive(true);
            //}
            gameObject.SetActive(false);
        }
    }

    void Start()
    {        
        if (videoName != "")
        {
            videoPlayer.targetCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            videoPlayer.Play();
            Time.timeScale = 0f;
            cutM = FindObjectOfType<CutsceneManager>();
        }
        //else if (dialog.Dialogues.Length == 0)
        //{
        //    FindObjectOfType<CountDown>().canStart = true;
        //}             
    }

    private void Update()
    {
        if ( (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space)) && videoName != "")
        {
            EndVideo(videoPlayer);
        }
    }

    void EndVideo(VideoPlayer vp)
    {
        //if (dialog.Dialogues.Length > 0)
        //{
        //    dialog.gameObject.SetActive(true);
        //}
        //else
        {
            ui.menuPausa.enabled = true;
            Time.timeScale = 1f;
        }
        
        videoPlayer.Stop();
        cutM.VideoEnded();
        gameObject.SetActive(false);
        Debug.Log("video ended");
    }

    
}
