using System.Collections;
using System.Collections.Generic;
using SimpleSQL;
using UnityEngine;

public class Finanzas
{
    [PrimaryKey,NotNull]
    public int idFinanza { get; set; }
    public int fidClub { get; set; }
    public float beneficioMerchandising { get; set; }
    public float beneficioPatrocinadores { get; set; }
    public float beneficioTorneos { get; set; }
    public float beneficioTransferenciaJugadores { get; set; }
    public float perdidaCompra { get; set; }
    public float perdidaGaminghouse { get; set; }
    public float perdidaSalarios { get; set; }
    public float perdidaTransferenciaJugadores { get; set; }

   
    
    
}


