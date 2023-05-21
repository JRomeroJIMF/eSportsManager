using System.Collections;
using System.Collections.Generic;
using SimpleSQL;
using UnityEngine;

public class Jugadores
{
    [PrimaryKey,NotNull]
    public int idJugador { get; set; }
    public int fidClub { get; set; }
    public string nombre { get; set; }
    public string nick { get; set; }
    public string imagenJugador { get; set; }
    public string posicion { get; set; }
    public float moral { get; set; }
    public float condicion { get; set; }
    public float ritmo { get; set; }
    public float valor { get; set; }
    public float salario { get; set; }
    public int edad { get; set; }
    public string nacionalidad { get; set; }
    public float altura { get; set; }
    public string personalidad { get; set; }
    public float reflejos { get; set; }
    public float reaccion { get; set; }
    public float precision { get; set; }
    public float concentracion { get; set; }
    public float juegoEquipo { get; set; }
    public float determinacion { get; set; }
    public float comunicacion { get; set; }
    public float disciplina { get; set; }
    public float liderazgo { get; set; }
    public float porcentajePistola  { get; set; }
    public float porcentajeSubfusil  { get; set; }
    public float porcentajeFusil  { get; set; }
    public float porcentajeAwp  { get; set; }
    public int partidasAmistoso  { get; set; }
    public int asesinatosAmistoso  { get; set; }
    public int muertesAmistoso  { get; set; }
    public int asistenciasAmistoso  { get; set; }
    public float kdaRatioAmistoso  { get; set; }
    public float headshotAmistoso  { get; set; }
    public int partidasCompetitivo  { get; set; }
    public int asesinatosCompetitivo  { get; set; }
    public int muertesCompetitivo  { get; set; }
    public int asistenciasCompetitivo  { get; set; }
    public float kdaRatioCompetitivo  { get; set; }
    public float headshotCompetitivo  { get; set; }
    
}
