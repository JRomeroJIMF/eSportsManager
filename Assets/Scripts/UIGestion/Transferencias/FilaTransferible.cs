using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class FilaTransferible : MonoBehaviour
{
    [SerializeField] private Button btnJugador;
    [SerializeField] private Text nick;
    [SerializeField] private Text posicion;
    [SerializeField] private Text habilidad;
    [SerializeField] private Text valor;
    [SerializeField] private Text salario;
    [SerializeField] private Button btnFichar;
    
    //Fichaje del jugador
    private int equipoUsuario;

    private List<Clubes> listaClubes = new List<Clubes>();
    private Finanzas finanzasEquipo = new Finanzas();

    public List<Correos> listaCorreos;

    public void RellenarFilaTransferibles(Jugadores jugadores, ControladorMenu controladorMenu)
    {
        nick.text = jugadores.nick;
        posicion.text = jugadores.posicion;
        valor.text = jugadores.valor+ "€";
        salario.text = jugadores.salario + "€";
        
        CargarHabilidadJugador(jugadores);

        //Patron Observer
        btnJugador.onClick.RemoveAllListeners();
        
        btnJugador.onClick.AddListener(() =>
        {
            controladorMenu.CambiarVista(9);
            controladorMenu.InicializarMenuJugador(jugadores);
        });
        
        btnFichar.onClick.AddListener(() =>
        {
            FicharJugador(jugadores);
        });
    }
    
    void CargarHabilidadJugador(Jugadores jugadores)
    {
        //Suma de todos los atributos del jugador * 100 /  total maximo de los atributos(180)
        float sumaAtributos;
        float total;
        int totalInt;

        sumaAtributos = jugadores.reflejos + jugadores.reaccion + jugadores.precision + jugadores.concentracion +
                        jugadores.juegoEquipo + jugadores.determinacion + jugadores.comunicacion + jugadores.disciplina +
                        jugadores.liderazgo;
        total = (sumaAtributos * 100) / 180;

        //Pasar de float a int
        totalInt = (int)total;
    
        habilidad.text = totalInt.ToString();

    }

    public void FicharJugador(Jugadores jugador)
    {
        Debug.Log("TEST fichaje: " + jugador.fidClub);
        equipoUsuario = UsuarioManager.Instance.CogerEquipoUsuario();

        listaClubes = DBManager.Instance.CogerClubes();
        finanzasEquipo = DBManager.Instance.CogerFinanzasEquipoEspecifico(equipoUsuario.ToString(),false);
        
        Debug.Log( "TEST Valor jugador: "+ jugador.valor);
        Debug.Log("TEST Presupuesto transpaso :" + UsuarioManager.Instance.equipoUsuario.presupuestoFichaje);

        if (jugador.valor < UsuarioManager.Instance.equipoUsuario.presupuestoFichaje)
        { 
            jugador.fidClub = equipoUsuario;
            
            UsuarioManager.Instance.equipoUsuario.presupuestoFichaje -= jugador.valor;
            finanzasEquipo.perdidaTransferenciaJugadores += jugador.valor;
            
            DBManager.Instance.ActualizarFinanzas(finanzasEquipo);

            DBManager.Instance.ActualizarClub(UsuarioManager.Instance.equipoUsuario);

            //Eliminar directamente al jugador de la fila
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Fichaje no permitido, no hay presupuesto suficiente");
        }

        Debug.Log("TTEST fichaje despues: " + jugador.fidClub);
        
        DBManager.Instance.ActualizarJugador(jugador);
        
        UsuarioManager.Instance.CogerDatosUsuario();

    }

}
