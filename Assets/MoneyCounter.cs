#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.EditorTools;


[ExecuteInEditMode]
public class MoneyCounter : MonoBehaviour
{
    public int MaximusMoney;
    Money[] allMoney;
    Enemy[] allEnemies;

    // Start is called before the first frame update
    void Start()
    {
        allMoney = FindObjectsOfType<Money>();
        allEnemies = FindObjectsOfType<Enemy>();
        MaximusMoney = 0;
        for (int i = 0; i < allMoney.Length; i++)
        {
            MaximusMoney += allMoney[i].value;
        }
        for (int i = 0; i < allEnemies.Length; i++)
        {
            MaximusMoney += allEnemies[i].MoneyValue;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
        {
            enabled = false;
        }
        else
        {
            allMoney = FindObjectsOfType<Money>();
            allEnemies = FindObjectsOfType<Enemy>();
            MaximusMoney = 0;
            for (int i = 0; i < allMoney.Length; i++)
            {
                MaximusMoney += allMoney[i].value;
            }

            for (int i = 0; i < allEnemies.Length; i++)
            {
                MaximusMoney += allEnemies[i].MoneyValue;
            }
        }        
    }
}
#endif