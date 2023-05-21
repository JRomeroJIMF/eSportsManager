using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilaPartida : MonoBehaviour
{
    [SerializeField] private Image logoEquipoRival;
    [SerializeField] private Text nombreEquipoRival;
    [SerializeField] private Text fecha;
    [SerializeField] private Text horario;
    [SerializeField] private Text lugar;
    [SerializeField] private Text tipo;
    
    private List<Clubes> listaEquipos = new List<Clubes>();

    private int equipoRival;
    private int equipoUsuario;

    public void RellenarFilaPartidas(Partidas partida, ControladorMenu controladorMenu)
    {
        listaEquipos = DBManager.Instance.CogerClubes();
        
        equipoUsuario = UsuarioManager.Instance.CogerEquipoUsuario();

        //Imagen del equipo a traves de una ruta
        string rutaImagen = "ImagenesLogos/" + listaEquipos[partida.equipoVisitante].logo;
        logoEquipoRival.overrideSprite = Resources.Load<Sprite>(rutaImagen);
        Debug.Log(rutaImagen);

        nombreEquipoRival.text = listaEquipos[partida.equipoVisitante].nombreClub;
        fecha.text = partida.fecha;
        horario.text = partida.horario;
        lugar.text = partida.lugar;
        tipo.text = partida.tipo;
    }
}
