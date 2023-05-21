using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilaPartidaJugada : MonoBehaviour
{
    [SerializeField] private Text fecha;
    [SerializeField] private Text diminutivoClub;
    [SerializeField] private Text tipoPartida;
    [SerializeField] private Text resultadoPartida;
    
    private List<Clubes> listaClubes = new List<Clubes>();

    public void RellenarFilaPartidaJugada(Partidas partida)
    {
        listaClubes = DBManager.Instance.CogerClubes();

        fecha.text = partida.fecha;
        diminutivoClub.text = listaClubes[partida.equipoVisitante].diminutivoClub;
        tipoPartida.text = partida.tipo;
        resultadoPartida.text = partida.resultado;
    }
}
