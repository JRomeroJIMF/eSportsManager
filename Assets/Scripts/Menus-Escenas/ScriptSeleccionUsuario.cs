using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class ScriptSeleccionUsuario : MonoBehaviour
{
    [SerializeField] private Text txtNombre;
    [SerializeField] private Text txtApellido;
    [SerializeField] private Text txtNick;
    [SerializeField] private Text txtEdad;
    [SerializeField] private Image imagenEquipo;
    [SerializeField] private Image imagenJuego;
    
    [SerializeField] private List<Sprite> listaEquipos;
    [SerializeField] private List<Sprite> listaJuegos;

    [SerializeField] private ControladorEscena _controlEscena;
    
    private int indiceActualEquipo;
    private int indiceActualJuego;
    
    private List<Usuarios> listaUsuarios = new List<Usuarios>();

    [SerializeField] private GameObject validacionPrefab;

    void OnEnable()
    {
        //Menu de seleccion de equipo preferido
        indiceActualEquipo = 0;

    }

    public void CrearUsuario()
    {
        bool nombreValidado = ValidacionTexto(txtNombre.text, 15, 3, false);
        bool apellidoValidado = ValidacionTexto(txtApellido.text, 15, 3, false);
        bool nickValidado = ValidacionTexto(txtNick.text, 10, 5, true);
        bool edadValidado = ValidacionEdad(txtEdad.text, 99, 10);
        
        if (!nombreValidado || !apellidoValidado || !nickValidado || !edadValidado)
        {
            validacionPrefab.SetActive(true);
            
        }
        else
        {
            DBManager.Instance.ResetearBDDinamica();
        
            DateTime fechaInicial = new DateTime(2023,06,01);
            DBManager.Instance.AñadirUsuarioDinamico(txtNombre.text, txtApellido.text,int.Parse(txtEdad.text), fechaInicial.ToString());
        
            UsuarioManager.Instance.usuario = DBManager.Instance.CogerUsuario();
        
            TimeManagerScript.Instance.Inicializar(DateTime.Parse(UsuarioManager.Instance.usuario.fechaInicial));

            _controlEscena.Cambiar(2);
        }
        
    }

    
    //Validaciones
    bool ValidacionTexto(string texto, int longMax, int longMin, bool caracteres)
    {
        bool valido = true;
        
        if (string.IsNullOrEmpty(texto))
        {
            valido = false;
            Debug.Log("El campo no puede estar vacio");
        }
        else if (texto.Length < longMin || texto.Length > longMax)
        {
            valido = false;
            Debug.Log("El campo debe estar entre " + longMin + " y " + longMax + " caracteres");
        }else if (!caracteres && Regex.IsMatch(texto, @"[^a-zA-Z ]"))
        {
            //@"[^a-zA-Z ]" -> esta expresión permite letras y el espacio(ya que deja un espacio al final, si quiero que agregue numeros puedo poner 0-9 despues de la Z)
            
            valido = false;
            Debug.Log("El campo no puede contener caracteres especiales o numeros");
        }

        return valido;
    }
    
    bool ValidacionEdad(string texto, int edadMax, int edadMin)
    {
        bool valido = true;

        if (string.IsNullOrEmpty(texto))
        {
            valido = false;
            Debug.Log("El campo no puede estar vacío");
        }
        else if (!int.TryParse(texto, out int edad))
        {
            valido = false;
            Debug.Log("El campo de edad debe ser un número");
        }
        else if (edad < edadMin || edad > edadMax)
        {
            valido = false;
            Debug.Log("El campo debe estar entre" + edadMax + " y " + edadMin + " años");
        }

        return valido;
    }

    public void CambiarEquipo(int nuevoIndice)
    {
        //Para el equipo favorito
        
        //1 o -1 si es derecha o izq.
        if (nuevoIndice==1)
        {
            indiceActualEquipo++;

            if (indiceActualEquipo>=listaEquipos.Count)
            {
                indiceActualEquipo = 0;
            }
            
        }
        else
        {
            indiceActualEquipo--;

            if (indiceActualEquipo<0)
            {
                indiceActualEquipo = listaEquipos.Count - 1;

            }
        }

        imagenEquipo.sprite = listaEquipos[indiceActualEquipo];

    }
    
    public void CambiarJuego(int nuevoIndice)
    {
        //Para el juego favorito
        
        //1 o -1 si es derecha o izq.
        if (nuevoIndice==1)
        {
            indiceActualJuego++;

            if (indiceActualJuego>=listaJuegos.Count)
            {
                indiceActualJuego = 0;
            }
            
        }
        else
        {
            indiceActualJuego--;

            if (indiceActualJuego<0)
            {
                indiceActualJuego = listaJuegos.Count - 1;

            }
        }

        imagenJuego.sprite = listaJuegos[indiceActualJuego];

    }
    
    public void Entendido(GameObject prefab)
    {
        prefab.SetActive(false);
        
    }
}
