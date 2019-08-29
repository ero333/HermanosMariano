using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    GameManager gm;
    bool active = false;
    public Animator anim;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !active)
        {
            active = true;
            anim.SetTrigger("CheckpointColl");
        }
        gm.lastCheckpos = transform.position;
    }   
    void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
        anim = GetComponent<Animator>();
    }
}