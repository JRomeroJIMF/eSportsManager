using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VistaPrincipal : VistaMenuBase
{
   [SerializeField] private Transform containerProximaPartida;
   [SerializeField] private GameObject proximaPartidaPrefab;

   private List<GameObject> partidasInstanciadas = new List<GameObject>();
   private List<Partidas> listaProximaPartidaVista = new List<Partidas>();

   private int equipoUsuario;

   void OnEnable()
   {
      equipoUsuario = UsuarioManager.Instance.CogerEquipoUsuario();
      listaProximaPartidaVista = DBManager.Instance.CogerPartidasDelEquipo(0, false);

      for (int i = partidasInstanciadas.Count-1; i >= 0; i--)
      {
         DestroyImmediate(partidasInstanciadas[i]);
      }

      for (int i = 0; i < listaProximaPartidaVista.Count; i++)
      {
         GameObject partida = GameObject.Instantiate(proximaPartidaPrefab, containerProximaPartida);
         partidasInstanciadas.Add(partida);
         partida.GetComponent<FilaProximaPartida>().RellenarFilaProximaPartida(listaProximaPartidaVista[i]);
            
         Debug.Log(listaProximaPartidaVista[i].fecha +  " " + listaProximaPartidaVista[i].tipo);
      }

   }
}
