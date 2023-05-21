using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VistaClub : VistaMenuBase
{
    //Finanzas
        //Beneficios
    [SerializeField] private Text beneficioMerchandising;
    [SerializeField] private Text beneficioPatrocinadores;
    [SerializeField] private Text beneficioTorneos;
    [SerializeField] private Text beneficioVentaJugadores;
    [SerializeField] private Text beneficioTotal;
        //Perdidas
    [SerializeField] private Text perdidaCompra;
    [SerializeField] private Text perdidaGaminghouse;
    [SerializeField] private Text perdidaSalarios;
    [SerializeField] private Text perdidaFichajes;
    [SerializeField] private Text perdidaTotal;
        //Balance
    [SerializeField] private Text balanceTotal;
    [SerializeField] private Text presupuestoTranspaso;
    
    private Finanzas finanzasEquipo = new Finanzas();
    
    //Equipación
    [SerializeField] private Image equipacion;
    
    private List<Clubes> listaClubes = new List<Clubes>();
    private int equipoUsuario;
    public static VistaClub Instance;
    
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
    }


    void OnEnable()
    {
        equipoUsuario = UsuarioManager.Instance.CogerEquipoUsuario();

        finanzasEquipo = DBManager.Instance.CogerFinanzasEquipoEspecifico(equipoUsuario.ToString(), false);
        
        RellenarCampos();

        UsuarioManager.Instance.equipoUsuario.presupuestoFichaje = CalcularPresupuestoFichaje(CalcularBalanceTotal());
        
        //Hacer update de las finanzas para que se quedé correcto antes de empezar
        DBManager.Instance.ActualizarFinanzas(finanzasEquipo);
        DBManager.Instance.ActualizarClub(listaClubes[equipoUsuario]);
    }

    public void RellenarCampos()
    {
        //Finanzas
        beneficioMerchandising.text = finanzasEquipo.beneficioMerchandising+"€";
        beneficioPatrocinadores.text = finanzasEquipo.beneficioPatrocinadores+"€";
        beneficioTorneos.text = finanzasEquipo.beneficioTorneos+"€";
        beneficioVentaJugadores.text = finanzasEquipo.beneficioTransferenciaJugadores+"€";
        beneficioTotal.text = finanzasEquipo.beneficioMerchandising + finanzasEquipo.beneficioPatrocinadores +
                              finanzasEquipo.beneficioTorneos + finanzasEquipo.beneficioTransferenciaJugadores + "€";

        perdidaCompra.text = finanzasEquipo.perdidaCompra+"€";
        perdidaGaminghouse.text = finanzasEquipo.perdidaGaminghouse+"€";
        perdidaSalarios.text = CalcularSalarioEquipo() + "€";
        perdidaFichajes.text = finanzasEquipo.perdidaTransferenciaJugadores+"€";
        perdidaTotal.text = finanzasEquipo.perdidaCompra + finanzasEquipo.perdidaGaminghouse +
                            CalcularSalarioEquipo() + finanzasEquipo.perdidaTransferenciaJugadores + "€";

        balanceTotal.text = CalcularBalanceTotal()+"€";
        presupuestoTranspaso.text = CalcularPresupuestoFichaje(CalcularBalanceTotal()) + "€";

        listaClubes.Clear();
        listaClubes = DBManager.Instance.CogerClubes();
        
        //Imagen del logo a traves de una ruta
        string rutaImagenEquipacion = "ImagenesEquipaciones/" + listaClubes[equipoUsuario].equipacion;
        equipacion.overrideSprite = Resources.Load<Sprite>(rutaImagenEquipacion);
        Debug.Log(rutaImagenEquipacion);
    }

    public float CalcularBalanceTotal()
    {
        float total;
        total = (finanzasEquipo.beneficioMerchandising + finanzasEquipo.beneficioPatrocinadores +
                 finanzasEquipo.beneficioTorneos+finanzasEquipo.beneficioTransferenciaJugadores) - (finanzasEquipo.perdidaCompra + finanzasEquipo.perdidaGaminghouse + CalcularSalarioEquipo() + finanzasEquipo.perdidaTransferenciaJugadores );

        return total;
    }

    public float CalcularPresupuestoFichaje(float Balance)
    {
        //40% del balance total
        float total;

        total = (Balance * 40) / 100;

        return total;
        
    }
    
    public float CalcularSalarioEquipo()
    {
        float salarioTotalEquipo = 0f;
        List<Jugadores> listaJugadores = DBManager.Instance.CogerJugadoresEquipoEspecifico(equipoUsuario.ToString(), false);

        for (int i = 0; i < listaJugadores.Count; i++)
        {
            salarioTotalEquipo += listaJugadores[i].salario;
        }

        salarioTotalEquipo *= 12;

        return salarioTotalEquipo;
    }
}
