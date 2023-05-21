using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VistaCompeticiones : VistaMenuBase
{
    //NO USADO -> No hay competiciones
    
    //Lista de competiciones con la clase VistaCompeticion
    [SerializeField] private List<VistaCompeticion> listaCompeticiones;
    
    public void AbrirDetallesCompeticion()
    {

        CambioMenu_Evento nuevoEvento = new CambioMenu_Evento();

        // nuevoEvento.nuevoMenu = TipoMenus.Competicion; --> Cuando se incluya la UI de competicion sola

        ControladorEventos.TriggerEvent<CambioMenu_Evento>(nuevoEvento);
       
    }

    public override void InicializarMenu(string dataId)
    {
        // base.InicializarMenu();  --> Para llamar al InicializarMenu de su clase base
    }
}
