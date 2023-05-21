using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsuarioManager : MonoBehaviour
{
    public static UsuarioManager Instance;

    public Usuarios usuario;

    public Clubes equipoUsuario;

    public List<Jugadores> jugadoresUsuario;

    // Start is called before the first frame update
    void Start()
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
        
        CogerDatosUsuario();

        //Usuario.fecha --> sacar de BD
        //TimeManagerScript.Instance.Inicializar(usuario.fecha);
        
        Debug.Log("Test FECHAINICIAL: " + usuario.fechaInicial);
        
        TimeManagerScript.Instance.Inicializar(DateTime.Parse(usuario.fechaInicial));
    }
    
    public void CogerDatosUsuario()
    {
        usuario = DBManager.Instance.CogerUsuario();

        Debug.Log("testeo " +usuario.club);
        
        equipoUsuario = DBManager.Instance.CogerClubEspecifico(usuario.club.ToString(), false)[1];
        
        jugadoresUsuario = DBManager.Instance.CogerJugadoresEquipoEspecifico(usuario.club.ToString(), false);

    }

    public int CogerEquipoUsuario()
    {
        int equipoUsuario;

        equipoUsuario = DBManager.Instance.CogerUsuario().club;
        
        return equipoUsuario;
    }
    
    public string CogerNombreUsuario()
    {

        return usuario.nombre + " " + usuario.apellido;
    }


}
