using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VistaCompeticion : MonoBehaviour
{
    //NO USADO -> No hay competiciones 
    
    [SerializeField] private Text tituloCompeticion;
    [SerializeField] private Image logoCompeticion;
    [SerializeField] private Image equipo1;
    [SerializeField] private Image equipo2;
    
    //Informacion
    [SerializeField] private Text fecha;
    [SerializeField] private Text horario;
    [SerializeField] private Text lugar;
    [SerializeField] private Text etapa;
    
    //Expectativas
    [SerializeField] private Slider expectativaTorneo;
    [SerializeField] private Slider expectativaPartida;

    public void Update()
    {
        Debug.Log("Expectativas torneo "+ tituloCompeticion.text + ": " + expectativaTorneo.value);
        Debug.Log("Expectativas partida " + tituloCompeticion.text + ": " + expectativaPartida.value);

        //Expectativas torneo
        if (expectativaTorneo.value<0.3)
        {
            Debug.Log("Expectativas de torneo bajas en " + tituloCompeticion.text);
        } 
        else if (expectativaTorneo.value>0.7)
        {
            Debug.Log("Expectativas de torneo altas en " + tituloCompeticion.text);
        }
        else
        {
            Debug.Log("Expectativas de torneo medias en " + tituloCompeticion.text);
        }
        
        //Expectativas de partida
        if (expectativaPartida.value<0.3)
        {
            Debug.Log("Expectativas de partida bajas en " + tituloCompeticion.text);
        } 
        else if (expectativaPartida.value>0.7)
        {
            Debug.Log("Expectativas de partida altas en " + tituloCompeticion.text);
        }
        else
        {
            Debug.Log("Expectativas de partida medias en " + tituloCompeticion.text);
        }
    }

    public void Inicializar()
    {
        
    }
}
