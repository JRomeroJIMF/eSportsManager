using System.Collections;
using System.Collections.Generic;
using SimpleSQL;
using UnityEngine;

public class Partidas
{
    [PrimaryKey,NotNull]
    public int idPartida { get; set; }
    public string tipo { get; set; }
    public int equipoLocal { get; set; }
    public int equipoVisitante { get; set; }
    public string fecha { get; set; }
    public string horario { get; set; }
    public string lugar { get; set; }
    public string resultado { get; set; }
}
