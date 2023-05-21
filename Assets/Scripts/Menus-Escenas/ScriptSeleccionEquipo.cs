using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.UI;

public class ScriptSeleccionEquipo : MonoBehaviour
{
    [SerializeField] private Text nombreClub;
    [SerializeField] private Image logo;
    
    //Plantilla
    [SerializeField] private Text nombreJugador1;
    [SerializeField] private Text habilidadJugador1;
    [SerializeField] private Text nombreJugador2;
    [SerializeField] private Text habilidadJugador2;
    [SerializeField] private Text nombreJugador3;
    [SerializeField] private Text habilidadJugador3;
    [SerializeField] private Text nombreJugador4;
    [SerializeField] private Text habilidadJugador4;
    [SerializeField] private Text nombreJugador5;
    [SerializeField] private Text habilidadJugador5;
    
    //Informacion
    [SerializeField] private Text nacionalidad;
    [SerializeField] private Text diminutivo;
    [SerializeField] private Text informacion;
    
    //Equipacion
    [SerializeField] private Image equipacion;
    
    //Finanzas
    [SerializeField] private Text balance;
    [SerializeField] private Text presupuestoFichaje;
    private Finanzas finanzasClub = new Finanzas();
    
    private List<Clubes> listaClubes = new List<Clubes>();
    private List<Jugadores> jugadoresEquipo = new List<Jugadores>();
    //Test
    private List<Jugadores> jugadoresTotal = new List<Jugadores>();
    
    //Usuario
    private Usuarios usuario = new Usuarios();

    private int clubSeleccionado;
    private int indiceActual;

    void OnEnable()
    {
        listaClubes.Clear();
        listaClubes = DBManager.Instance.CogerClubes();
        
        clubSeleccionado = listaClubes[1].idClub;
        indiceActual = 1;

        jugadoresEquipo = DBManager.Instance.CogerJugadoresEquipoEspecifico(clubSeleccionado.ToString(), true);

        jugadoresTotal = DBManager.Instance.CogerTodosJugadores();

        usuario = DBManager.Instance.CogerUsuario();
        
        //Test de usuario
        Debug.Log("TEST Nombre usuario: " + usuario.nombre + usuario.edad + usuario.equipoFavorito);
        Debug.Log("TEST Lista clubes" + listaClubes.Count);
        Debug.Log("TEST Jugadores totales" + jugadoresTotal.Count);

        rellenarCamposSeleccion();
        
    }

    public void ConfirmarSeleccion()
    {
        usuario.club = clubSeleccionado;
        
        DateTime fechaInicial = new DateTime(2023, 06, 1);

        usuario.fechaInicial = fechaInicial.ToString();

        DBManager.Instance.ActualizarBDDinamica(usuario);

        finanzasClub = DBManager.Instance.CogerFinanzasEquipoEspecifico(UsuarioManager.Instance.equipoUsuario.idClub.ToString(), false);
        UsuarioManager.Instance.equipoUsuario.presupuestoFichaje = CalcularPresupuestoTranspaso(CalcularBalanceTotal());
        DBManager.Instance.ActualizarClub( UsuarioManager.Instance.equipoUsuario);
        
        UsuarioManager.Instance.CogerDatosUsuario();
        
    }

    public void CambiarSeleccionClub(int nuevoIndice)
    {
        //1 o -1 si es derecha o izq.

        if (nuevoIndice==1)
        {
            indiceActual++;
            
            Debug.Log("if " + indiceActual);

            if (indiceActual>=listaClubes.Count)
            {
                
                indiceActual = 1;
                
                // Debug.Log("2º if " + indiceActual);
            }
            
        }
        else
        {
            indiceActual--;
            
            Debug.Log("else " + indiceActual);
            
            if (indiceActual<=0)
            {
                indiceActual = listaClubes.Count - 1;
                
                // Debug.Log("else + if " + indiceActual);
            }
        }
        
        // Debug.Log("fin " + indiceActual);

        clubSeleccionado = listaClubes[indiceActual].idClub;
        
        jugadoresEquipo = DBManager.Instance.CogerJugadoresEquipoEspecifico(clubSeleccionado.ToString(), true);

        rellenarCamposSeleccion();
    }

    void rellenarCamposSeleccion()
    {
        //Imagen del logo a traves de una ruta
        string rutaImagenLogo = "ImagenesLogos/" + listaClubes[indiceActual].logo;
        logo.overrideSprite = Resources.Load<Sprite>(rutaImagenLogo);
        Debug.Log(rutaImagenLogo);

        nombreClub.text = listaClubes[indiceActual].nombreClub;
        
        nombreJugador1.text = jugadoresEquipo[0].nombre + " \"" + jugadoresEquipo[0].nick + "\"";
        habilidadJugador1.text = CargarHabilidadJugador(jugadoresEquipo[0]);
        nombreJugador2.text = jugadoresEquipo[1].nombre + " \"" + jugadoresEquipo[1].nick + "\"";
        habilidadJugador2.text = CargarHabilidadJugador(jugadoresEquipo[1]);
        nombreJugador3.text = jugadoresEquipo[2].nombre + " \"" + jugadoresEquipo[2].nick + "\"";
        habilidadJugador3.text = CargarHabilidadJugador(jugadoresEquipo[2]);
        nombreJugador4.text = jugadoresEquipo[3].nombre + " \"" + jugadoresEquipo[3].nick + "\"";
        habilidadJugador4.text = CargarHabilidadJugador(jugadoresEquipo[3]);
        nombreJugador5.text = jugadoresEquipo[4].nombre + " \"" + jugadoresEquipo[4].nick + "\"";
        habilidadJugador5.text = CargarHabilidadJugador(jugadoresEquipo[4]);
        
        nacionalidad.text = listaClubes[indiceActual].nacionalidad;
        diminutivo.text = listaClubes[indiceActual].diminutivoClub;
        informacion.text = listaClubes[indiceActual].informacion;
        
        //Imagen de la equipacion a traves de una ruta
        string rutaImagenEquipacion = "ImagenesEquipaciones/" + listaClubes[indiceActual].equipacion;
        equipacion.overrideSprite = Resources.Load<Sprite>(rutaImagenEquipacion);
        Debug.Log(rutaImagenEquipacion);

        balance.text = listaClubes[indiceActual].balance + "€";
        presupuestoFichaje.text = listaClubes[indiceActual].presupuestoFichaje + "€";
       
    }
    
    string CargarHabilidadJugador(Jugadores jugadores)
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

        return totalInt.ToString();

    }
    
    public float CalcularBalanceTotal()
    {
        float total;
        total = (finanzasClub.beneficioMerchandising + finanzasClub.beneficioPatrocinadores +
                 finanzasClub.beneficioTorneos+finanzasClub.beneficioTransferenciaJugadores) - (finanzasClub.perdidaCompra + finanzasClub.perdidaGaminghouse + CalcularSalarioEquipo() + finanzasClub.perdidaTransferenciaJugadores );

        return total;
    }

    public float CalcularPresupuestoTranspaso(float Balance)
    {
        //40% del balance total
        float total;

        total = (Balance * 40) / 100;

        return total;
        
        //Otra idea es añadirlo a la BD y que cada equipo tenga su presupuesto de fichajes segun sus condiciones 
        // o añadir a la BD un % que cada club destine a su presupuesto.
    }
    
    float CalcularSalarioEquipo()
    {
        float salarioTotalEquipo = 0f;
        List<Jugadores> listaJugadores = DBManager.Instance.CogerJugadoresEquipoEspecifico("1", false);

        for (int i = 0; i < listaJugadores.Count; i++)
        {
            salarioTotalEquipo += listaJugadores[i].salario;
        }

        salarioTotalEquipo *= 12;

        return salarioTotalEquipo;
    }

}
