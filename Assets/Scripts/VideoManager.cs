using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    VideoPlayer videoPlayer;
    public string videoName;

    private void Awake()
    {
        if (videoName != "0")
        {
            videoPlayer = GetComponent<VideoPlayer>();
            videoPlayer.url = Path.Combine(Application.streamingAssetsPath, videoName + ".mp4");
            videoPlayer.loopPointReached += EndVideo;
        }        
    }

    void Start()
    {
        
        if (videoName != "0")
        {
            videoPlayer.targetCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            videoPlayer.Play();
        }                
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space))
        {
            videoPlayer.enabled = false;
        }
    }

    void EndVideo(VideoPlayer vp)
    {
        videoPlayer.Stop();
        Debug.Log("video ended");
    }

    
}
