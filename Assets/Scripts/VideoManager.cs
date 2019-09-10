using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    VideoPlayer videoPlayer;
    public string videoName;

    public UserInterface ui;

    private void Awake()
    {
        if (videoName != "")
        {
            ui.menuPausa.enabled = false;
            videoPlayer = GetComponent<VideoPlayer>();
            videoPlayer.url = Path.Combine(Application.streamingAssetsPath, videoName + ".mp4");
            videoPlayer.loopPointReached += EndVideo;
        }        
    }

    void Start()
    {        
        if (videoName != "")
        {
            videoPlayer.targetCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            videoPlayer.Play();
            Time.timeScale = 0f;
        }                
    }

    private void Update()
    {
        if ( (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space)) && videoName != "")
        {
            Time.timeScale = 1f;
            videoPlayer.enabled = false;
            ui.menuPausa.enabled = true;
        }
    }

    void EndVideo(VideoPlayer vp)
    {
        ui.menuPausa.enabled = true;
        Time.timeScale = 1f;
        videoPlayer.Stop();
        Debug.Log("video ended");
    }

    
}
