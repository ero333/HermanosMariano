using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    GameManager gm;
    Animator anim;
    bool active = false;

    void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !active)
        {
            active = true;
            anim.SetTrigger("CheckpointColl");

            GameManager.instance.lastCheckpos = transform.position;
        }        
    }    
}