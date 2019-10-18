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
    public bool isEnding = false;

    public UserInterface ui;
    //public DialogueManager dialog;
    Image back;
    public Text SpaceoZ;

    GameManager gm;
    

    private void Awake()
    {
        gm = GameManager.instance;

        videoPlayer = GetComponent<VideoPlayer>();
        if (!GameManager.sound)
        {
            videoPlayer.SetDirectAudioMute(0, true);
        }
        else
        {
            videoPlayer.SetDirectAudioMute(0, false);
        }

        if (videoName != "" && !isEnding)
        {
            ui.menuPausa.enabled = false;           
            videoPlayer.url = Path.Combine(Application.streamingAssetsPath, videoName + ".mp4");
            videoPlayer.loopPointReached += EndVideo;
            GetComponent<Image>().enabled = true;
        }
        else if(videoName != "" && isEnding)
        {
            ui.menuPausa.enabled = false;
            videoPlayer.url = Path.Combine(Application.streamingAssetsPath, videoName + ".mp4");
            videoPlayer.loopPointReached += EndVideo;
            GetComponent<Image>().enabled = true;
        }
        else
        {
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
    }

    public void startVideo()
    {
        videoPlayer.targetCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        videoPlayer.Play();
        Time.timeScale = 0f;
        cutM = FindObjectOfType<CutsceneManager>();
    }

    private void Update()
    {
        if (!GameManager.sound)
        {
            videoPlayer.SetDirectAudioMute(0, true);
        }
        else
        {
            videoPlayer.SetDirectAudioMute(0, false);
        }


        if ( (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space)) && videoName != "")
        {
            EndVideo(videoPlayer);
        }
    }

    void EndVideo(VideoPlayer vp)
    {
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
