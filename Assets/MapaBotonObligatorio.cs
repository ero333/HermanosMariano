using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapaBotonObligatorio : MonoBehaviour
{
    GameManager gm;
    public GameObject Deuda;
    
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
    }

    public void Unlock (int deuda)
    {
        if(gm.ahorros >= deuda)
        {
            gm.ahorros -= deuda;
            gameObject.GetComponent<Button>().interactable = true;
            gm.paidDeudas += 1;
            Deuda.SetActive(false);
        }
        else
        {
            Deuda.GetComponent<Button>().interactable = false;
        }
    }
}
