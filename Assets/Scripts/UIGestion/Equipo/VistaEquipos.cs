using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VistaEquipos : VistaMenuBase
{
    //ContainerJugadores
    [SerializeField] private Transform containerJugadores;
    [SerializeField] private GameObject JugadorPrefab;
    [SerializeField] private ControladorMenu controladorMenu;
    
    private List<GameObject> jugadoresInstanciados = new List<GameObject>();
    private List<Jugadores> listaJugadoresVista = new List<Jugadores>();
    
    //DinamicaEquipo
    [SerializeField] private Text cohesionEquipo;
    [SerializeField] private Text ambiente;
    [SerializeField] private Text situacionManager;
    
    private List<Clubes> listaEquipos = new List<Clubes>();
    private int equipoUsuario;


    void OnEnable()
    {
        equipoUsuario = UsuarioManager.Instance.CogerEquipoUsuario();

        listaJugadoresVista.Clear();
        listaJugadoresVista = DBManager.Instance.CogerJugadoresEquipoEspecifico(equipoUsuario.ToString(), false);

        for (int i = jugadoresInstanciados.Count-1; i >= 0; i--)
        {
            DestroyImmediate(jugadoresInstanciados[i]);
        }

        for (int i = 0; i < listaJugadoresVista.Count; i++)
        {
            if(listaJugadoresVista[i].fidClub == 0) continue;
            
            GameObject Jugador = GameObject.Instantiate(JugadorPrefab, containerJugadores);
            jugadoresInstanciados.Add(Jugador);
            Jugador.GetComponent<FilaJugador>().RellenarFila(listaJugadoresVista[i], controladorMenu);
            
            Debug.Log(listaJugadoresVista[i].nombre + " " + listaJugadoresVista[i].nick + " " + listaJugadoresVista[i].posicion);
        }
        
        //DinamicaEquipo
        listaEquipos.Clear();
        listaEquipos = DBManager.Instance.CogerClubEspecifico(equipoUsuario.ToString(), false);
        
        listaEquipos.RemoveAt(0);
        
        cohesionEquipo.text = listaEquipos[0].cohesionEquipo + "%";
        ambiente.text = listaEquipos[0].ambienteEquipo + "%";
        situacionManager.text = listaEquipos[0].situacionManager + "%";

    }
    

    public void AbrirDetallesJugador()
    {
        CambioMenu_Evento nuevoEvento = new CambioMenu_Evento();
        nuevoEvento.nuevoMenu = TipoMenus.Jugador;
        ControladorEventos.TriggerEvent<CambioMenu_Evento>(nuevoEvento);
    }

    public override void InicializarMenu(string dataID)
    {
       // base.InicializarMenu();  --> Para llamar al InicializarMenu de su clase base si necesario
    }
    
}
