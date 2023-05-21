using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class VistaSuperior : MonoBehaviour
{
    [SerializeField] private Image logo;
    [SerializeField] private Text nombreClub;

    private List<Clubes> listaClubes = new List<Clubes>();

    private int equipoUsuario;

    //Fecha
    [SerializeField] private Text fecha;
    [SerializeField] private Text diaSemana;

    private string fechaTraducida;

    private List<Partidas> listaPartidas = new List<Partidas>();
    
    [SerializeField] private GameObject validacionPrefab;

    void OnEnable()
    {
        Debug.Log("TEST coger equipo: "+ UsuarioManager.Instance.CogerEquipoUsuario());
        
        equipoUsuario = 1; 

        listaClubes.Clear();
        listaClubes = DBManager.Instance.CogerClubEspecifico("0", false);

        listaPartidas = DBManager.Instance.CogerPartidasDelEquipo(0, false);

        //Imagen del logo a traves de una ruta
        string rutaImagenLogo = "ImagenesLogos/" + listaClubes[equipoUsuario].logo;
        logo.overrideSprite = Resources.Load<Sprite>(rutaImagenLogo);
        Debug.Log(rutaImagenLogo);

        nombreClub.text = listaClubes[equipoUsuario].nombreClub;

        ActualizarFecha();

        TimeManagerScript.Instance.cambioFecha.AddListener(IrAPartida);
        
    }

    void OnDisable()
    {
        TimeManagerScript.Instance.cambioFecha.RemoveListener(IrAPartida);
    }

    void ActualizarFecha()
    {
        DateTime fechaActual = TimeManagerScript.Instance.CogerFecha();
        DayOfWeek diaActual = fechaActual.DayOfWeek;

        switch (diaActual)
        {
            case DayOfWeek.Monday:
                fechaTraducida = "Lunes";
                break;
            case DayOfWeek.Tuesday:
                fechaTraducida = "Martes";
                break;
            case DayOfWeek.Wednesday:
                fechaTraducida = "Miercoles";
                break;
            case DayOfWeek.Thursday:
                fechaTraducida = "Jueves";
                break;
            case DayOfWeek.Friday:
                fechaTraducida = "Viernes";
                break;
            case DayOfWeek.Saturday:
                fechaTraducida = "Sábado";
                break;
            case DayOfWeek.Sunday:
                fechaTraducida = "Domingo";
                break;

        }
        
        diaSemana.text = fechaTraducida;
        
        fecha.text = TimeManagerScript.Instance.CogerFecha().ToString();
    }
    
    public void AñadirDias(int diaSumado)
    {
        
        for (int i = 0; i < 5; i++)
        {
            if (UsuarioManager.Instance.jugadoresUsuario.Find(x => x.posicion.Equals(((VistaJuego.Posiciones)i).ToString()))==null)
            {
                validacionPrefab.SetActive(true);

                Debug.Log("Falta la posicion " + ((VistaJuego.Posiciones) i).ToString());

                return;
            }
            
        }

        if (UsuarioManager.Instance.jugadoresUsuario.Count>5)
        {
            Debug.Log(UsuarioManager.Instance.jugadoresUsuario.Count);
            Debug.Log("Demasiados jugadores en el equipo");
            
            validacionPrefab.SetActive(true);

            return;
        }

        AudioManagerScript.Instance.Reproducir(1,false,true);
        
        TimeManagerScript.Instance.SumarDias(diaSumado);

        ActualizarFecha();
    }

    public void IrAPartida(DateTime fechaActual)
    {
        for (int i = 0; i < listaPartidas.Count; i++)
        {
            if (TimeManagerScript.Instance.CompararFechaActual(DateTime.Parse(listaPartidas[i].fecha)))
            {
                SceneManager.LoadScene(4);
            }
        }
    }
    
    public void Entendido(GameObject prefab)
    {
        prefab.SetActive(false);
        
    }
}
