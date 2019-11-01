using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Collectable : MonoBehaviour
{
    Collect collect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GameManager.instance.RecolectarObjeto(gameObject.name, transform.position.x, transform.position.y);

            if(GameObject.FindObjectOfType<Collect>() != null)
            {
                collect = FindObjectOfType<Collect>();

                collect.toCollect -= 1;

                Destroy(gameObject);
            }
            else if (GameObject.FindObjectOfType<Transport>() != null)
            {
                Player player = FindObjectOfType<Player>();

                if (!player.transport)
                {
                    Sprite mySprite = GetComponentInChildren<SpriteRenderer>().sprite;
                    player.transport = true;
                    player.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = mySprite;
                    Destroy(gameObject);
                }                
            }       
        }
    }
}
