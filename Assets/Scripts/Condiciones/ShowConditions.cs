using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowConditions : MonoBehaviour
{
    [Header("Referencias")]
    public Text time;
    public Text condition;

    Player player;

    LlegarMeta llegarMeta;
    Collect collect;
    Transport transport;
    KillAll killAll;
    Kill kill;
    Survive survive;

    void Start()
    {
        player = FindObjectOfType<Player>();
        GameManager gm = GameManager.instance;

        if(FindObjectOfType<LlegarMeta>() != null)
        {
            llegarMeta = FindObjectOfType<LlegarMeta>();
            gm.CondicionDeVictoria = "Meta";
        }
        else if(FindObjectOfType<Collect>() != null)
        {
            collect = FindObjectOfType<Collect>();
            gm.CondicionDeVictoria = "Recolectar";
        }
        else if(FindObjectOfType<Transport>() != null)
        {
            transport = FindObjectOfType<Transport>();
            gm.CondicionDeVictoria = "Transportar";
        }
        else if(FindObjectOfType<KillAll>() != null)
        {
            killAll = FindObjectOfType<KillAll>();
            gm.CondicionDeVictoria = "MatarATodos";
        }
        else if (FindObjectOfType<Kill>() != null)
        {
            kill = FindObjectOfType<Kill>();
            gm.CondicionDeVictoria = "Matar";
        }
        else if (FindObjectOfType<Survive>() != null)
        {
            survive = FindObjectOfType<Survive>();
            gm.CondicionDeVictoria = "Sobrevivir";
        }
    }

    void Update()
    {
        if(llegarMeta != null)
        {
            condition.text = "META: " + Mathf.Round( Vector2.Distance(player.transform.position, llegarMeta.transform.position) );
        }
        else if (collect != null)
        {
            condition.text = "FALTAN: " + collect.toCollect;
        }
        else if (transport != null)
        {
            condition.text = "FALTAN: " + transport.toTransport;
        }
        else if (killAll != null)
        {
            condition.text = "MATAR: " + killAll.toKill;
        }
        else if (kill != null)
        {
            if(kill.toKill != null)
            {
                condition.text = "MATAR OBJETIVO: " + Mathf.Round(Vector2.Distance(player.transform.position, kill.toKill.transform.position));
            }            
        }
        else if (survive != null)
        {
            condition.text = "AGUANTAR";
        }
    }
}
