using System.Collections;
using System.Collections.Generic;
using SimpleSQL;
using UnityEngine;

public class Usuarios 
{
    [PrimaryKey,NotNull]
    public int idUsuario { get; set; }
    public string nombre { get; set; }
    public string apellido { get; set; }
    public int club { get; set; }
    public int edad { get; set; }
    public string fechaInicial { get; set; }
    public string equipoFavorito { get; set; }
    public string juegoFavorito { get; set; }
}
