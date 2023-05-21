using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilaJugador : MonoBehaviour
{
    [SerializeField] private Text nombreJugador;
    [SerializeField] private Text posicion;
    [SerializeField] private Image moral;
    [SerializeField] private Text condicion;
    [SerializeField] private Text ritmo;
    [SerializeField] private Text habilidad;
    [SerializeField] private Text headShots;
    [SerializeField] private Text valor;
    [SerializeField] private Text salario;

    [SerializeField] private Button botonFila;

    //Venta jugador
    [SerializeField] private Button botonVender;
    private int equipoUsuario;
    private List<Clubes> listaClubes = new List<Clubes>();
    private Finanzas finanzasEquipo = new Finanzas();

    public void RellenarFila(Jugadores jugadores, ControladorMenu controladorMenu)
    {
        nombreJugador.text = jugadores.nombre + " \"" + jugadores.nick + "\"";
        posicion.text = jugadores.posicion;
        condicion.text = jugadores.condicion.ToString();
        ritmo.text = jugadores.ritmo.ToString();
        valor.text = jugadores.valor+ "€";
        salario.text = jugadores.salario + "€";

        CargarMoralJugador(jugadores);

        CargarHabilidadJugador(jugadores);
        
        //Cambiar cuando lo tengamos en la BD
        headShots.text = jugadores.headshotCompetitivo.ToString();

        botonFila.onClick.RemoveAllListeners();
        
        //Agregamos llamadas para el evento cuando pulsa el boton
        botonFila.onClick.AddListener(() =>
        {
            controladorMenu.CambiarVista(9);
            controladorMenu.InicializarMenuJugador(jugadores);
        });
        
        botonVender.onClick.AddListener(() =>
        {
            VenderJugador(jugadores);
        });
    }

    void CargarMoralJugador(Jugadores jugadores)
    {
        if (jugadores.moral > 80)
        {
            moral.color = new Color(  0.0f/255.0f,125.0f/255.0f,0.0f/255.0f,255.0f/255.0f);
        }
        else if (jugadores.moral > 60)
        {
            moral.color = new Color(  85.0f/255.0f,145.0f/255.0f,0.0f/255.0f,255.0f/255.0f);
            moral.transform.rotation = Quaternion.Euler(0,0,-45f);
        }
        else if (jugadores.moral > 40)
        {
            moral.color = new Color(  210.0f/255.0f,200.0f/255.0f,0.0f/255.0f,255.0f/255.0f);
            moral.transform.rotation = Quaternion.Euler(0,0,-90f);
        }
        else if (jugadores.moral > 20)
        {
            moral.color = new Color(  210.0f/255.0f,120.0f/255.0f,0.0f/255.0f,255.0f/255.0f);
            moral.transform.rotation = Quaternion.Euler(0,0,-135f);
        }
        else
        {
            moral.color = Color.red;
            moral.transform.rotation = Quaternion.Euler(0,0,180f);
        }
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
    
    public void VenderJugador(Jugadores jugador)
    {
        equipoUsuario = UsuarioManager.Instance.CogerEquipoUsuario();
        finanzasEquipo = DBManager.Instance.CogerFinanzasEquipoEspecifico(equipoUsuario.ToString(), false);

        Debug.Log("TEST venta: " + jugador.fidClub);

        jugador.fidClub = 0;
        
        finanzasEquipo.beneficioTransferenciaJugadores += jugador.valor;
        DBManager.Instance.ActualizarFinanzas(finanzasEquipo);
        
        Debug.Log("TEST fichaje despues: " + jugador.fidClub);
        
        DBManager.Instance.ActualizarJugador(jugador);
        
        //Eliminar directamente al jugador de la fila
        gameObject.SetActive(false);
        
        UsuarioManager.Instance.CogerDatosUsuario();
        
    }
    
}
