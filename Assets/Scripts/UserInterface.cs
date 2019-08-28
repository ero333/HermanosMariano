using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    GameManager gm;
    public Text LiveText;
    public Image[] EnergyBar;

    // Start is called before the first frame update
    void Start()
    {
        gm = GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
