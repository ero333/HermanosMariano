using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Calificacion : MonoBehaviour
{
    public void Calificar(int stars)
    {
        GameManager.instance.Calificar(stars);
        PlayerPrefs.SetInt("Calificado", 1);
        SceneManager.LoadScene("MenuInicio");
    }
}
