using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VistaEntrenamiento : VistaMenuBase
{
    //Nivel de carga entrenamiento
    [SerializeField] private Text nivelCarga;
    [SerializeField] private Text nivelFelicidad;
    [SerializeField] private Text nivelFatiga;
    //Informacion entrenamiento
    [SerializeField] private Text tipoEntrenamiento;
    
    [SerializeField] private Text reflejos;
    [SerializeField] private Text reaccion;
    [SerializeField] private Text precision;
    [SerializeField] private Text concentracion;
    [SerializeField] private Text juegoEquipo;
    [SerializeField] private Text comunicacion;
    [SerializeField] private Text pistola;
    [SerializeField] private Text subfusil;
    [SerializeField] private Text fusil;
    [SerializeField] private Text awp;
    [SerializeField] private Text sumanivelCarga;
    [SerializeField] private Text sumanivelFatiga;
    
    [SerializeField] private List<Button> listaBotonesEntrenamiento;
    private List<Entrenamientos> listaEntrenamientos = new List<Entrenamientos>();

    private int botonSeleccionado;

    //Entrenamientos de los jugadores
    private List<Jugadores> listaJugadores = new List<Jugadores>();
    private int equipoUsuario;

    void OnEnable()
    {
        listaEntrenamientos.Clear();

        listaEntrenamientos = DBManager.Instance.CogerEntrenamientos();
        
        //Entrenamientos de los jugadores
        equipoUsuario = UsuarioManager.Instance.CogerEquipoUsuario();
        listaJugadores = DBManager.Instance.CogerJugadoresEquipoEspecifico(equipoUsuario.ToString(), false);

        Debug.Log("TEST jugador de la lista: "+listaJugadores[4].nombre);
        Debug.Log("TEST jugador de la lista: "+listaJugadores[4].reflejos);
        
        
        TimeManagerScript.Instance.cambioFecha.AddListener(AplicarEntrenamiento);
    }

    void OnDisable()
    {
        TimeManagerScript.Instance.cambioFecha.RemoveListener(AplicarEntrenamiento);
    }

    //Aunque no usemos la fecha del parametro es necesario ya que nos lo pide el evento
    void AplicarEntrenamiento(DateTime fecha)
    {
        for (int i = 0; i < listaJugadores.Count; i++)
        {
            if (listaJugadores[i].reflejos < 20)
            {
                listaJugadores[i].reflejos +=  float.Parse(reflejos.text);
                
                Debug.Log("TEST jugador entrenando: " + listaJugadores[i].nombre + " entrena");
                Debug.Log("TEST jugador entrenando: " + listaJugadores[i].reflejos + " despues de entrenar");
            }
            else
            {
                Debug.Log("TEST de jugador entrenado : " + listaJugadores[i].nombre + " No entrena nivel maximo");
            }
            
            if (listaJugadores[i].reaccion < 20)
            {
                listaJugadores[i].reaccion +=  float.Parse(reaccion.text);
            }
            
            if (listaJugadores[i].precision < 20)
            {
                listaJugadores[i].precision +=  float.Parse(precision.text);
            }
            
            if (listaJugadores[i].concentracion < 20)
            {
                listaJugadores[i].concentracion +=  float.Parse(concentracion.text);
            }
            
            if (listaJugadores[i].juegoEquipo < 20)
            {
                listaJugadores[i].juegoEquipo +=  float.Parse(juegoEquipo.text);
            }
            
            if (listaJugadores[i].comunicacion < 20)
            {
                listaJugadores[i].comunicacion +=  float.Parse(comunicacion.text);
            }

            DBManager.Instance.ActualizarJugador(listaJugadores[i]);
        }
        
    }
    
    public void Actualizar(int indice)
    {
        AudioManagerScript.Instance.Reproducir(4, true,true);

        //Dinamica entrenamiento
        nivelCarga.text = listaEntrenamientos[indice].nivelCarga + "%";
        nivelFelicidad.text = listaEntrenamientos[indice].nivelFelicidad + "%";
        nivelFatiga.text = listaEntrenamientos[indice].nivelFatiga + "%";
        
        //Informacion entrenamiento
        reflejos.text = listaEntrenamientos[indice].sumaReflejos.ToString();
        CambiarTexto(listaEntrenamientos[indice].sumaReflejos.ToString().Contains("-"),reflejos);
        reaccion.text = listaEntrenamientos[indice].sumaReaccion.ToString();
        CambiarTexto(listaEntrenamientos[indice].sumaReaccion.ToString().Contains("-"),reaccion);
        precision.text = listaEntrenamientos[indice].sumaPrecision.ToString();
        CambiarTexto(listaEntrenamientos[indice].sumaPrecision.ToString().Contains("-"),precision);
        concentracion.text = listaEntrenamientos[indice].sumaConcentracion.ToString();
        CambiarTexto(listaEntrenamientos[indice].sumaConcentracion.ToString().Contains("-"),concentracion);
        juegoEquipo.text = listaEntrenamientos[indice].sumaJuegoEquipo.ToString();
        CambiarTexto(listaEntrenamientos[indice].sumaJuegoEquipo.ToString().Contains("-"),juegoEquipo);
        comunicacion.text = listaEntrenamientos[indice].sumaComunicacion.ToString();
        CambiarTexto(listaEntrenamientos[indice].sumaComunicacion.ToString().Contains("-"),comunicacion);
        pistola.text = listaEntrenamientos[indice].sumaPistola.ToString();
        CambiarTexto(listaEntrenamientos[indice].sumaPistola.ToString().Contains("-"),pistola);
        subfusil.text = listaEntrenamientos[indice].sumaSubfusil.ToString();
        CambiarTexto(listaEntrenamientos[indice].sumaSubfusil.ToString().Contains("-"),subfusil);
        fusil.text = listaEntrenamientos[indice].sumaFusil.ToString();
        CambiarTexto(listaEntrenamientos[indice].sumaFusil.ToString().Contains("-"),fusil);
        awp.text = listaEntrenamientos[indice].sumaAwp.ToString();
        CambiarTexto(listaEntrenamientos[indice].sumaAwp.ToString().Contains("-"),awp);
        sumanivelCarga.text = listaEntrenamientos[indice].sumaNivelCarga.ToString();
        CambiarTexto(listaEntrenamientos[indice].sumaNivelCarga.ToString().Contains("-"),sumanivelCarga);
        sumanivelFatiga.text = listaEntrenamientos[indice].sumaFatigaGenerada.ToString();
        CambiarTexto(listaEntrenamientos[indice].sumaFatigaGenerada.ToString().Contains("-"),sumanivelFatiga);

        tipoEntrenamiento.text = "ENTRENAMIENTO DE " + listaEntrenamientos[indice].tipo.ToUpper();
    }
    
    void CambiarTexto(bool entrenamiento, Text tipo)
    {
        if (entrenamiento)
        {
            tipo.color = Color.red;
        }
        else
        {
            tipo.color = new Color(  235.0f/255.0f,165.0f/255.0f,50.0f/255.0f,255.0f/255.0f);
            tipo.text = "+" + tipo.text;
        }
    }
    
}


