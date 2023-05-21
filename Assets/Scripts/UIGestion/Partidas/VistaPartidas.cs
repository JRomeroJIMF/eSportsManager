using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VistaPartidas : MonoBehaviour
{
    //ContainerPartidas
    [SerializeField] private Transform containerPartidas;
    [SerializeField] private GameObject partidaPrefab;
    [SerializeField] private ControladorMenu controladorMenu;
    
    private List<GameObject> partidasInstanciadas = new List<GameObject>();
    private List<Partidas> listaPartidasVista = new List<Partidas>();
    
    private int equipoUsuario;
    
    
    void OnEnable()
    {
        equipoUsuario = UsuarioManager.Instance.CogerEquipoUsuario();

        listaPartidasVista.Clear();
        listaPartidasVista = DBManager.Instance.CogerPartidasDelEquipo(0,false);

        for (int i = partidasInstanciadas.Count-1; i >= 0; i--)
        {
            DestroyImmediate(partidasInstanciadas[i]);
        }

        for (int i = 0; i < listaPartidasVista.Count; i++)
        {
            GameObject partida = GameObject.Instantiate(partidaPrefab, containerPartidas);
            partidasInstanciadas.Add(partida);
            partida.GetComponent<FilaPartida>().RellenarFilaPartidas(listaPartidasVista[i], controladorMenu);
            
            Debug.Log(listaPartidasVista[i].tipo + " " + listaPartidasVista[i].fecha + " " + listaPartidasVista[i].lugar);
        }

    }
    
}
