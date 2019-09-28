using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    public int value;
    GameManager gm;

    private void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            GameManager.instance.money += value;
            Destroy(gameObject);
        }
    }
   
}
