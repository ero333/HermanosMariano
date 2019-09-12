using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    Collect collect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collect = FindObjectOfType<Collect>();

            collect.toCollect -= 1;

            Destroy(gameObject);
        }
    }
}
