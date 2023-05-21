using System.Collections;
using System.Collections.Generic;
using SimpleSQL;
using UnityEngine;

public class Clubes
{
    [PrimaryKey,NotNull]
    public int idClub { get; set; }
    public string nombreClub { get; set; }
    public string logo { get; set; }
    public string nacionalidad { get; set; }
    public string diminutivoClub { get; set; }
    public string informacion { get; set; }
    public string equipacion { get; set; }
    public float balance { get; set; }
    public float presupuestoFichaje { get; set; }
    public float cohesionEquipo { get; set; }
    public float ambienteEquipo { get; set; }
    public float situacionManager { get; set; }
    
}
