using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilaProximaPartida : MonoBehaviour
{
    [SerializeField] private Text fecha;
    [SerializeField] private Text diminutivoClub;
    [SerializeField] private Text tipoPartida;

    private List<Clubes> listaClubes = new List<Clubes>();

    public void RellenarFilaProximaPartida(Partidas partida)
    {
        listaClubes = DBManager.Instance.CogerClubes();

        fecha.text = partida.fecha;
        diminutivoClub.text = listaClubes[partida.equipoVisitante].diminutivoClub;
        tipoPartida.text = partida.tipo;

    }
}
