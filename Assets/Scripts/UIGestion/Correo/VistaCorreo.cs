using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VistaCorreo : VistaMenuBase
{
    [SerializeField] private GameObject container;
    [SerializeField] private GameObject correoPrefab;
    
   [SerializeField] private Text txtTitulo;
   [SerializeField] private Text txtFecha;
   [SerializeField] private Text txtCuerpo;

   private List<Correos> listaCorreos = new List<Correos>();

   private string nombreUsuario;
   private string equipoUsuario;
   
   void Start()
   {
       listaCorreos.Clear();
       
      nombreUsuario = UsuarioManager.Instance.CogerNombreUsuario();
      equipoUsuario = UsuarioManager.Instance.equipoUsuario.nombreClub;
      

      //Fecha seria fecha de inicio del juego
      Correos correosBienvenida = new Correos("Bienvenida al club", "01/06/2023", "Bienvenido a tu nuevo club " + nombreUsuario + ", los jugadores están deseando conocerte. \n\nComo nuevo manager de " + equipoUsuario + ", deberás hacerte cargo de la plantilla" +
                                                                            " analiza bien a tus jugadores y juntos demostrad que podeis hacer frente a cualquier rival. Si crees que necesitas algún refuerzo, el club estará encantado de ayudarte, siempre que no pases el presupuesto para fichajes. \n\nAtento a los " +
                                                                            "entrenamientos estos serán la clave para que tu equipo se haga más fuerte cada semana y podáis competir contra otros de nivel superior. \n\n¡Mucha suerte!");
      listaCorreos.Add(correosBienvenida);

      Correos correosInstrucciones = new Correos("Instrucciones", "01/06/2023", "Para jugar a eSports director, debes tener en cuenta lo siguiente: \n\n- Principal: Calendario y próximos eventos. \n\n- Correo: Buzón de correos." + 
                                                                             " \n\n- Equipo: Plantilla completa y atributos de los jugadores si haces click en ellos.\n\n- Entrenamiento: Mejoras que recibirán tus jugadores gracias a los entrenamientos." +
                                                                             " \n\n- Partidas: Resumen de las próximas partidas. \n\n- Centro de datos: Datos más importantes de tu equipo. \n\n- Club: Resumen económico y palmarés de tu club. " +
                                                                             "\n\n- Transferencias: Lista de jugadores disponibles para fichar.");
      listaCorreos.Add(correosInstrucciones);

      Correos correosNoticia =
          new Correos("Nuevo manager para " + UsuarioManager.Instance.equipoUsuario.diminutivoClub, "01/06/2023", nombreUsuario + " ha firmado por " + equipoUsuario + ", a sus " + UsuarioManager.Instance.usuario.edad + " años será la primera vez que no competirá como pro player, veremos qué tal lo hace. \n\nCon su " +
              "experiencia como jugador profesional de CS:GO, puede hacer una gran contribución en la parte técnica, lo que ayudará mucho a este equipo. \n\nAunque vemos diferencia de opiniones entre los fans de " + equipoUsuario + " por la llegada del inexperto manager, seguro que pronto se hará con el apoyo de todos. \n\nEsperamos que pueda llevar al equipo a lo más alto y lograr grandes resultados.");
      listaCorreos.Add(correosNoticia);
      
      // Recorrer la lista de correos y crear un botón para cada uno
      foreach (Correos correo in listaCorreos) {
          GameObject nuevoBoton = Instantiate(correoPrefab, container.transform);
          nuevoBoton.GetComponentInChildren<Text>().text = correo.fecha;
          nuevoBoton.GetComponentInChildren<Text>().text = correo.titulo;
          nuevoBoton.GetComponent<Button>().onClick.AddListener(() => MostrarCorreo(correo));
      }

   }

   //Metodo para añadir un correo
   public void AñadirCorreo(string titulo, DateTime fecha, string cuerpo)
   {
       Correos correos = new Correos(titulo, fecha.ToString(), cuerpo);
       listaCorreos.Add(correos);

       fecha.AddDays(7);

       DateTime test=DateTime.UtcNow;
       TimeSpan resultado;

       resultado = fecha - test;
       
   }

   //Metodo para mostrar los correos
   public void MostrarCorreo(Correos correo)
   {
       txtTitulo.text = correo.titulo;
       txtFecha.text = correo.fecha;
       txtCuerpo.text = correo.cuerpo;
   }

}
