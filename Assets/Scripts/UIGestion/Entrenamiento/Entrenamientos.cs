using System.Collections;
using System.Collections.Generic;
using SimpleSQL;
using UnityEngine;

public class Entrenamientos
{
    [PrimaryKey,NotNull]
    public int idEntrenamiento { get; set; }
    public string tipo { get; set; }
    public float nivelCarga { get; set; }
    public float nivelFelicidad { get; set; }
    public float nivelFatiga { get; set; }
    public float sumaReflejos { get; set; }
    public float sumaReaccion { get; set; }
    public float sumaPrecision { get; set; }
    public float sumaConcentracion { get; set; }
    public float sumaJuegoEquipo { get; set; }
    public float sumaComunicacion { get; set; }
    public float sumaPistola { get; set; }
    public float sumaSubfusil { get; set; }
    public float sumaFusil { get; set; }
    public float sumaAwp { get; set; }
    public float sumaNivelCarga { get; set; }
    public float sumaFatigaGenerada { get; set; }
    
}
