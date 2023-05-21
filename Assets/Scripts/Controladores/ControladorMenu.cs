
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class ControladorMenu : MonoBehaviour
{
    [SerializeField] private List<Button> listaBotones;
    [SerializeField] private List<GameObject> prefabsMenus;
    
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject prefabAnterior;
    

    public void Start()
    {
        ControladorEventos.AddListener<CambioMenu_Evento>(CambiarMenu);
        
        //Con esto hacemos que Principal aparezca como seleccionado al iniciar el juego
        listaBotones[0].Select();
    }

    public void CambiarVista(GameObject prefab)
    {
        //Activamos y desactivamos los prefabs para que cambien con su respectivo boton de  menu
        prefabAnterior.SetActive(false);
        prefab.SetActive(true);
        prefabAnterior = prefab;
        
        AudioManagerScript.Instance.Reproducir(0,false,true);

        /*
        //Test
        Debug.Log(prefab.name);
        Debug.Log(prefabAnterior.name);
        */
    }
    
    public void CambiarVista(int prefabIndice)
    {
        //Metodo sobrecargado para mostrar jugadores desde eventos
        
        prefabAnterior.SetActive(false);
        prefabsMenus[prefabIndice].SetActive(true);
        prefabAnterior = prefabsMenus[prefabIndice];
        
    }

    void CambiarMenu(CambioMenu_Evento cambioEvento)
    {
        switch (cambioEvento.nuevoMenu)
        {
            case TipoMenus.Principal:
                CambiarVista(prefabsMenus[0]);
                break;
            
            case TipoMenus.Correo:
                CambiarVista(prefabsMenus[1]);
                break;
            
            case TipoMenus.Equipo:
                CambiarVista(prefabsMenus[2]);
                break;
            
            case TipoMenus.Entrenamiento:
                CambiarVista(prefabsMenus[3]);
                break;
            
            case TipoMenus.Partidas:
                CambiarVista(prefabsMenus[4]);
                break;
            
            case TipoMenus.CentroDatos:
                CambiarVista(prefabsMenus[5]);
                break;
            
            case TipoMenus.Club:
                CambiarVista(prefabsMenus[6]);
                break;
            
            case TipoMenus.Transferencias:
                CambiarVista(prefabsMenus[7]);
                break;
            
            case TipoMenus.Social:
                CambiarVista(prefabsMenus[8]);
                break;
            
            case TipoMenus.Jugador:
                CambiarVista(prefabsMenus[9]);
                break;
        }
        
        AudioManagerScript.Instance.Reproducir(0,false,true);
    }

    public void InicializarMenuJugador(Jugadores jugadores)
    {
        prefabAnterior.GetComponent<VistaJugador>().Inicializar(jugadores);
    }

}

public enum TipoMenus{
    //Con esto enumeramos los valores del menu, para no tener que estar haciendolo con su valor
    // y quede mas claro a la hora de, por ejemplo, hacer el switch
    Principal,
    Correo,
    Equipo,
    Entrenamiento,
    Partidas,
    CentroDatos,
    Club,
    Transferencias,
    Social,
    Jugador
}

public class CambioMenu_Evento
{
    public TipoMenus nuevoMenu;
}

