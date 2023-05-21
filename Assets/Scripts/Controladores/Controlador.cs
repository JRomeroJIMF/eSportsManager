using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controlador : MonoBehaviour
{
    //NO USADO
    
    [SerializeField] private int escena;

    // método para cambiar de escena con botón
    public void Cambiar(int escena)
    {
        SceneManager.LoadScene(escena);
    }

    // método para el botón de salir
    public void Salir()
    {
        Application.Quit();
    }
}
