using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeManagerScript : MonoBehaviour
{
    private DateTime fechaActual;
    
    public static TimeManagerScript Instance;
    
    //Patron Observer
    //Declarar evento
    public UnityEvent<DateTime> cambioFecha;

    void Awake()
    {
        //Patron Singleton
        if (Instance == null)
        {
            Instance = this; 
        }
        else
        {
            Destroy(this);
        }
    }

    public void Inicializar(DateTime fechaInicial)
    {
        fechaActual = fechaInicial;
    }

    public void SumarDias(int cantidadDias)
    {
        fechaActual = fechaActual.AddDays(cantidadDias);
        
        //Invocar evento --> llamarlo
        cambioFecha?.Invoke(fechaActual);
        
        Debug.Log("TEST fecha: " + fechaActual);
        
        //Actualizar la fecha para cargas
        UsuarioManager.Instance.usuario.fechaInicial = fechaActual.ToString();
        DBManager.Instance.ActualizarUsuario(UsuarioManager.Instance.usuario);
        
        Debug.Log("TEST fecha: "+ DBManager.Instance.CogerUsuario().fechaInicial);

    }

    public bool CompararFechaActual(DateTime fechaComparacion)
    {
        TimeSpan intervaloComparacion;

        intervaloComparacion = fechaActual - fechaComparacion;

        return intervaloComparacion.Days > 0;
    }

    public DateTime CogerFecha()
    {
        return fechaActual;
    }
}
