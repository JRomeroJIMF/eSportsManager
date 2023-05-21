using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Correos
{
    /*
    [PrimaryKey,NotNull]
    public int idCorreo { get; set; }
    public string titulo { get; set; }
    public string cuerpo { get; set; }
    public string fecha { get; set; }
    public string tipo { get; set; }
    public int leido { get; set; }
    */
    
    public string titulo;
    public string fecha;
    public string cuerpo;

    public Correos(string titulo, string fecha, string cuerpo)
    {
        this.titulo = titulo;
        this.fecha = fecha;
        this.cuerpo = cuerpo;
    }

}
