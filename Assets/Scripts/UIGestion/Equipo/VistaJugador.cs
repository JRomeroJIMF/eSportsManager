using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VistaJugador : MonoBehaviour
{
    [SerializeField] private Text nombreJugador;
    [SerializeField] private Image imagenJugador;
    private string txtLogoEquipo;
    [SerializeField] private Image logoEquipo;

    private List<Clubes> equipo = new List<Clubes>();
    
    //Informacion
    [SerializeField] private Text posicion;
    [SerializeField] private Text edad;
    [SerializeField] private Text nacionalidad;
    [SerializeField] private Text altura;
    [SerializeField] private Text personalidad;
    
    //Atributos
    [SerializeField] private Text reflejos;
    [SerializeField] private Text reaccion;
    [SerializeField] private Text precision;
    [SerializeField] private Text concentracion;
    [SerializeField] private Text juegoEquipo;
    [SerializeField] private Text determinacion;
    [SerializeField] private Text comunicacion;
    [SerializeField] private Text disciplina;
    [SerializeField] private Text liderazgo;
    
    //Habilidad
    [SerializeField] private Text porcentajePistola;
    [SerializeField] private Text porcentajeSubfusil;
    [SerializeField] private Text porcentajeFusil;
    [SerializeField] private Text porcentajeAwp;
    
    //Estadisticas
        //Amistosos
    [SerializeField] private Text partidasAmistoso;
    [SerializeField] private Text asesinatosAmistoso;
    [SerializeField] private Text muertesAmistoso;
    [SerializeField] private Text asistenciasAmistoso;
    [SerializeField] private Text kdaRatioAmistoso;
    [SerializeField] private Text headshotAmistoso;
        //Competitivo
    [SerializeField] private Text partidasCompetitivo;
    [SerializeField] private Text asesinatosCompetitivo;
    [SerializeField] private Text muertesCompetitivo;
    [SerializeField] private Text asistenciasCompetitivo;
    [SerializeField] private Text kdaRatioCompetitivo;
    [SerializeField] private Text headshotCompetitivo;
        //Total
    [SerializeField] private Text partidasTotal;
    [SerializeField] private Text asesinatosTotal;
    [SerializeField] private Text muertesTotal;
    [SerializeField] private Text asistenciasTotal;
    [SerializeField] private Text kdaRatioTotal;
    [SerializeField] private Text headshotTotal;

    public void Inicializar(Jugadores jugadores)
    {
        equipo = DBManager.Instance.CogerClubEspecifico(jugadores.fidClub.ToString(), false);

        txtLogoEquipo = equipo[0].logo;
        
        nombreJugador.text = jugadores.nombre + " \"" + jugadores.nick + "\"";
        
        //Imagen del jugador a traves de una ruta
        string rutaImagen = "ImagenesJugadores/" + jugadores.imagenJugador;
        imagenJugador.overrideSprite = Resources.Load<Sprite>(rutaImagen);
        Debug.Log(rutaImagen);

        Debug.Log("TEST DE LOGO JUGADOR - " + txtLogoEquipo);
        //Imagen del club a traves de una ruta
        string rutaImagenLogo = "ImagenesLogos/" + txtLogoEquipo;
        logoEquipo.overrideSprite = Resources.Load<Sprite>(rutaImagenLogo);
        Debug.Log(rutaImagenLogo);
        
        //Atributos
        reflejos.text = Mathf.RoundToInt(jugadores.reflejos).ToString();
        reaccion.text = Mathf.RoundToInt(jugadores.reaccion).ToString();
        precision.text = Mathf.RoundToInt(jugadores.precision).ToString();
        concentracion.text = Mathf.RoundToInt(jugadores.concentracion).ToString();
        juegoEquipo.text = Mathf.RoundToInt(jugadores.juegoEquipo).ToString();
        determinacion.text = Mathf.RoundToInt(jugadores.determinacion).ToString();
        comunicacion.text = Mathf.RoundToInt(jugadores.comunicacion).ToString();
        disciplina.text = Mathf.RoundToInt(jugadores.disciplina).ToString();
        liderazgo.text = Mathf.RoundToInt(jugadores.liderazgo).ToString();
        
        //Informacion
        posicion.text = jugadores.posicion;
        edad.text = jugadores.edad.ToString();
        nacionalidad.text = jugadores.nacionalidad;
        altura.text = jugadores.altura + "cm";
        personalidad.text = jugadores.personalidad;
        
        //Habilidad
        porcentajePistola.text = jugadores.porcentajePistola + "%";
        porcentajeSubfusil.text = jugadores.porcentajeSubfusil + "%";
        porcentajeFusil.text = jugadores.porcentajeFusil + "%";
        porcentajeAwp.text = jugadores.porcentajeAwp + "%";
        
        //Estadisticas
            //Amistosos
        partidasAmistoso.text = jugadores.partidasAmistoso.ToString();
        asesinatosAmistoso.text = jugadores.asesinatosAmistoso.ToString();
        muertesAmistoso.text = jugadores.muertesAmistoso.ToString();
        asistenciasAmistoso.text = jugadores.asistenciasAmistoso.ToString();
        kdaRatioAmistoso.text = CalcularKdaRatioAmistoso(jugadores).ToString();
        headshotAmistoso.text = jugadores.headshotAmistoso.ToString();
            //Competitivos
        partidasCompetitivo.text = jugadores.partidasCompetitivo.ToString();
        asesinatosCompetitivo.text = jugadores.asesinatosCompetitivo.ToString();
        muertesCompetitivo.text = jugadores.muertesCompetitivo.ToString();
        asistenciasCompetitivo.text = jugadores.asistenciasCompetitivo.ToString();
        kdaRatioCompetitivo.text = CalcularKdaRatioCompetitivo(jugadores).ToString();
        headshotCompetitivo.text = jugadores.headshotCompetitivo.ToString();
            //Total
        partidasTotal.text = (jugadores.partidasAmistoso + jugadores.partidasCompetitivo).ToString();
        asesinatosTotal.text = (jugadores.asesinatosAmistoso + jugadores.asesinatosCompetitivo).ToString();
        muertesTotal.text = (jugadores.muertesAmistoso + jugadores.muertesCompetitivo).ToString();
        asistenciasTotal.text = (jugadores.asistenciasAmistoso + jugadores.asistenciasCompetitivo).ToString();
        kdaRatioTotal.text = CalcularKdaRatioTotal(jugadores).ToString();
        headshotTotal.text = (jugadores.headshotAmistoso + jugadores.headshotCompetitivo).ToString();

    }

    private float CalcularKdaRatioAmistoso(Jugadores jugadores)
    {
        float total;

        if (jugadores.muertesAmistoso > 0)
        {
            total = (jugadores.asesinatosAmistoso + jugadores.asistenciasAmistoso) / jugadores.muertesAmistoso;
        }
        else
        {
            total = 0;
        }

        return total;
    }
    
    private float CalcularKdaRatioCompetitivo(Jugadores jugadores)
    {
        float total;
        
        if (jugadores.muertesAmistoso > 0)
        {
            total = (jugadores.asesinatosCompetitivo + jugadores.asistenciasCompetitivo) / jugadores.muertesCompetitivo;
        }
        else
        {
            total = 0;
        }
        return total;
    }
    
    private float CalcularKdaRatioTotal(Jugadores jugadores)
    {
        float total;
        
        if (jugadores.muertesAmistoso > 0)
        {
            total = ((jugadores.asesinatosAmistoso + jugadores.asesinatosCompetitivo) + (jugadores.asistenciasAmistoso + jugadores.asistenciasCompetitivo)) / (jugadores.muertesAmistoso + jugadores.muertesCompetitivo);
        }
        else
        {
            total = 0;
        }
        return total;
    }
    
}
