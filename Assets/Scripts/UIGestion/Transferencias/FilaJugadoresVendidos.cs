using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilaJugadoresVendidos : MonoBehaviour
{
    //NO IMPLEMENTADO AUN
    
    [SerializeField] private Text nickJugador;
    [SerializeField] private Text valorJugador;
    [SerializeField] private Text salarioJugador;

    public void RellenarFilaJugadorContratado(Jugadores jugador)
    {
        nickJugador.text = jugador.nick;
        valorJugador.text = jugador.valor + "€";
        salarioJugador.text = jugador.salario + "€";
    }
}
