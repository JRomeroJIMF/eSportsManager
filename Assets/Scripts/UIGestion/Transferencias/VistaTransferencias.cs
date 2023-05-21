using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class VistaTransferencias : MonoBehaviour
{
    //Fichajes
    [SerializeField] public List<Jugadores> listaJugadoresContratados = new List<Jugadores>();

    //Ventas
    [SerializeField] private List<VistaJugador> listaJugadoresVendidos;

    //ListaJugadoresTransferibles
        //ContainerJugadores
    [SerializeField] private Transform containerJugadores;
    [SerializeField] private GameObject JugadorPrefab;
    [SerializeField] private ControladorMenu controladorMenu;
    
    // [SerializeField] private List<VistaJugador> listaJugadoresTotales;
    private List<GameObject> jugadoresInstanciados = new List<GameObject>();
    private List<Jugadores> listaJugadoresVista = new List<Jugadores>();

    private int equipoUsuario;

    void OnEnable()
    {
        listaJugadoresVista = DBManager.Instance.CogerJugadoresTransferibles(); //Seria la 0 en la BD

        for (int i = jugadoresInstanciados.Count-1; i >= 0; i--)
        {
            DestroyImmediate(jugadoresInstanciados[i]);
        }

        for (int i = 0; i < listaJugadoresVista.Count; i++)
        {
            GameObject Jugador = GameObject.Instantiate(JugadorPrefab, containerJugadores);
            jugadoresInstanciados.Add(Jugador);
            Jugador.GetComponent<FilaTransferible>().RellenarFilaTransferibles(listaJugadoresVista[i], controladorMenu);
            
            Debug.Log(listaJugadoresVista[i].nombre + " " + listaJugadoresVista[i].nick + " " + listaJugadoresVista[i].posicion);
        }
    }
}
