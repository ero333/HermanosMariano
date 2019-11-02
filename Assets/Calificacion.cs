using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Calificacion : MonoBehaviour
{
    public void Calificar(int stars)
    {
        Dictionary<string, object> dictionary = new Dictionary<string, object>
        {
          {"Nota", stars }

        };

        Debug.Log("Nota; " + stars);
        //Analytics.CustomEvent("Calificar", dictionary);

        SceneManager.LoadScene("MenuInicio");
    }
}
