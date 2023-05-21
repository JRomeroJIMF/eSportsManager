using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptMenu : MonoBehaviour
{
    [SerializeField] private Button BotonCargarPartida;

    IEnumerator Start()
    {
        //Damos un tiempo para comprobar si no no funcionará correctamente en el start
        yield return new WaitForSeconds(0.5f);
        
        ComprobarPartidaExistente();
    }

    void ComprobarPartidaExistente()
    {
        //No dejará cargar la partida si no hay otra partida jugada
        if (DBManager.Instance.CogerUsuario()!=null)
        {
            BotonCargarPartida.interactable = true;
        }
        else
        {
            BotonCargarPartida.interactable = false;
        }
    }
}
