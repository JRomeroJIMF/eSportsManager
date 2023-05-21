using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class VistaJuego : MonoBehaviour
{
    private int puntuacionLocal=0;
    private int puntuacionVisitante=0;
    private int rondaActual;
    private float tiempoEntreTiradas = 2.0f;
    
    //InformacionPartida
    [SerializeField] private Image logoEquipoUsuario;
    [SerializeField] private Image logoEquipoRival;
    [SerializeField] private Text diminutivoEquipoUsuario;
    [SerializeField] private Text diminutivoEquipoRival;
    [SerializeField] private Text puntuacionEquipoUsuario;
    [SerializeField] private Text puntuacionEquipoRival;
    [SerializeField] private Text cuentaRonda;

    //InformacionJugadores
    [SerializeField] private List<Image> imagenesEquipoLocal;
    [SerializeField] private List<Text> nombresEquipoLocal;
    [SerializeField] private List<Text> habilidadesEquipoLocal;
    private List<int> valoresHabilidadesEquipoLocal = new List<int>(5);
    [SerializeField] private List<Text> tiradasEquipoLocal;
    private List<bool> aciertosEquipoLocal = new List<bool>(5);
    [SerializeField] private List<Image> resultadosEquipoLocal;
    private List<int> numeroHeadShot = new List<int>(5);


    [SerializeField] private List<Image> imagenesEquipoVisitante;
    [SerializeField] private List<Text> nombresEquipoVisitante;
    [SerializeField] private List<Text> habilidadesEquipoVisitante;
    private List<int> valoresHabilidadesEquipoVisitante = new List<int>(5);
    [SerializeField] private List<Text> tiradasEquipoVisitante;
    private List<bool> aciertosEquipoVisitante = new List<bool>(5);
    [SerializeField] private List<Image> resultadosEquipoVisitante;

    [SerializeField] private Color Fallo =  new Color(200.0f / 255.0f, 30.0f / 255.0f, 30.0f / 255.0f, 0.0f / 255.0f);
    [SerializeField] private Color Acierto =  new Color(200.0f / 255.0f, 30.0f / 255.0f, 30.0f / 255.0f, 255.0f / 255.0f);
    
    //Botones siguiente y continuar
    private Button botonSiguienteRonda;
    private Button botonContinuar;

    //Aumentos
    [SerializeField] private Button aumentoAnimar;
    [SerializeField] private Button aumentoMotivar;
    [SerializeField] private Button aumentoEstrategia;
    
    private bool MotivarUsado;
    private bool AnimarUsado;
    private bool EstrategiaUsado;

    [SerializeField] private Image sumatorio;
    int contadorEstrategia;
    
    //RellenarJuego
    private List<Partidas> listaPartidas = new List<Partidas>();
    private List<Clubes> listaEquipos = new List<Clubes>();
    
    private List<Jugadores> listaJugadoresUsuario = new List<Jugadores>();
    private List<Jugadores> listaJugadoresRival = new List<Jugadores>();
    
    private int equipoUsuario;
    private int equipoRival;

    // Start is called before the first frame update
    void Start()
    {
        //Obtenemos referencias de botones y hacemos el continuar no usable
        botonSiguienteRonda = GameObject.Find("BtnSiguienteRonda").GetComponent<Button>();
        botonContinuar = GameObject.Find("BtnContinuar").GetComponent<Button>();
        botonContinuar.interactable = false;

        //Botones usados
        MotivarUsado = false;
        AnimarUsado = false;
        EstrategiaUsado = false;
        
        sumatorio.color = Fallo;
        contadorEstrategia = 0;
        
        listaPartidas.Clear();
        listaPartidas = DBManager.Instance.CogerPartidasDelEquipo(0,false);

        RellenarJuego(CogerPartida());
        
        DBManager.Instance.BorrarPartidas(CogerPartida().idPartida);
        
    }

    void RellenarJuego(Partidas partida)
    {
        listaEquipos = DBManager.Instance.CogerClubes();

        listaEquipos[0] = DBManager.Instance.CogerClubEspecifico(partida.equipoLocal.ToString(), false)[1];
        listaEquipos[1] = DBManager.Instance.CogerClubEspecifico(partida.equipoVisitante.ToString(), true)[0];
        
        //Imagen del equipoUsuario a traves de una ruta
        string rutaImagen = "ImagenesLogos/" + listaEquipos[0].logo;
        logoEquipoUsuario.overrideSprite = Resources.Load<Sprite>(rutaImagen);

        //Imagen del equipo a traves de una ruta
        string rutaImagen2 = "ImagenesLogos/" + listaEquipos[1].logo;
        logoEquipoRival.overrideSprite = Resources.Load<Sprite>(rutaImagen2);

        diminutivoEquipoUsuario.text = listaEquipos[0].diminutivoClub;
        diminutivoEquipoRival.text = listaEquipos[1].diminutivoClub;

        puntuacionEquipoUsuario.text = "0";
        puntuacionEquipoRival.text = "0";

        cuentaRonda.text = "0";

        RellenarJugadores(partida);

        for (int i = 0; i < 5; i++)
        {
            tiradasEquipoLocal[i].text = "";
            tiradasEquipoVisitante[i].text = "";

            resultadosEquipoLocal[i].color = Fallo;
            resultadosEquipoVisitante[i].color = Fallo;
            
            aciertosEquipoLocal.Add(false);
            aciertosEquipoVisitante.Add(false);

        }

        sumatorio.color = Fallo;
    }

    void RellenarJugadores(Partidas partida)
    {

        listaJugadoresUsuario = DBManager.Instance.CogerJugadoresEquipoEspecifico(partida.equipoLocal.ToString(), false);
        listaJugadoresRival = DBManager.Instance.CogerJugadoresEquipoEspecifico(partida.equipoVisitante.ToString(), true);

        for (int i = 0; i < listaJugadoresUsuario.Count; i++)
        {
            //Sin esto da error por el indice de la lista
            valoresHabilidadesEquipoLocal.Add(0);
            numeroHeadShot.Add(0);

            //Busqueda en la lista de jugadores por posicion
            Jugadores jugador = listaJugadoresUsuario.Find(x => x.posicion.Equals(((Posiciones) i).ToString()));
            // Debug.Log("TEST jugador cogido: " + jugador.nombre + " posicion: "+ jugador.posicion + " Habilidad" + CargarHabilidadJugador(jugador));

            string rutaImgJugador = "ImagenesJugadores/" + jugador.imagenJugador;
            imagenesEquipoLocal[i].overrideSprite = Resources.Load<Sprite>(rutaImgJugador);

            nombresEquipoLocal[i].text = jugador.nombre + " \"" + jugador.nick + "\"";

            valoresHabilidadesEquipoLocal[i] = CargarHabilidadJugador(jugador);
            habilidadesEquipoLocal[i].text  = valoresHabilidadesEquipoLocal[i].ToString();
        }

        for (int i = 0; i < listaJugadoresRival.Count; i++)
        {
            //Sin esto da error por el indice de la lista
            valoresHabilidadesEquipoVisitante.Add(0);
            
            Jugadores jugador = listaJugadoresRival.Find(x => x.posicion.Equals(((Posiciones) i).ToString()));
            // Debug.Log("TEST jugador cogido: " + jugador.nombre + " posicion: "+ jugador.posicion); 
            
            string rutaImgJugador = "ImagenesJugadores/" + jugador.imagenJugador;
            imagenesEquipoVisitante[i].overrideSprite = Resources.Load<Sprite>(rutaImgJugador);

            nombresEquipoVisitante[i].text = jugador.nombre + " \"" + jugador.nick + "\"";
            
            valoresHabilidadesEquipoVisitante[i] = CargarHabilidadJugador(jugador);
            habilidadesEquipoVisitante[i].text  = valoresHabilidadesEquipoVisitante[i].ToString();
        }
    }
    
    int CargarHabilidadJugador(Jugadores jugador)
    {
        //Suma de todos los atributos del jugador * 100 /  total maximo de los atributos(180)
        float sumaAtributos;
        float total;
        int totalInt;

        sumaAtributos = jugador.reflejos + jugador.reaccion + jugador.precision + jugador.concentracion +
                        jugador.juegoEquipo + jugador.determinacion + jugador.comunicacion + jugador.disciplina +
                        jugador.liderazgo;
        total = (sumaAtributos * 100) / 180;

        //Pasar de float a int
        totalInt = (int)total;
    
        return totalInt;
    }
    
    public IEnumerator CalcularResultadoRonda(float tiempoTirada)
    {
        int aciertosLocales = 0;
        int aciertosVisitante = 0;

        //Inhabilito el boton hasta el fin del metodo
        botonSiguienteRonda.interactable = false;
        botonContinuar.interactable = false;

        //Cambio de ronda
        rondaActual++;
        cuentaRonda.text = rondaActual.ToString();
        
        for (int i = 0; i < listaJugadoresUsuario.Count; i++)
        {
            Debug.Log( "TEST habilidades: " + habilidadesEquipoLocal[i].text);
            Debug.Log( "TEST valores de habilidades: " + valoresHabilidadesEquipoLocal[i]);
        }

        //Esto limpiará los resultados y tiradas al principio de cada ronda
        for (int i = 0; i < listaJugadoresUsuario.Count; i++)
        {
            resultadosEquipoLocal[i].color = Fallo;
            resultadosEquipoVisitante[i].color = Fallo;

            tiradasEquipoLocal[i].text = " ";
            tiradasEquipoVisitante[i].text = " ";
        }

        for (int i = 0; i < listaJugadoresUsuario.Count; i++)
        {
            yield return new WaitForSeconds(tiempoTirada);
            
            //Turno del jugador Usuario
            int resultadoUsuario = Random.Range(20, 101);
            tiradasEquipoLocal[i].text = resultadoUsuario.ToString();

            aciertosEquipoLocal[i] = valoresHabilidadesEquipoLocal[i] >= resultadoUsuario;
            
            if (aciertosEquipoLocal[i]) {
                resultadosEquipoLocal[i].color = Acierto;
                numeroHeadShot[i]++;
                AudioManagerScript.Instance.Reproducir(2, true,true);
                
                Debug.Log("Acierto: " + resultadosEquipoLocal[i].color + " del jugador " + listaJugadoresUsuario[i].nombre);
            } else {
                resultadosEquipoLocal[i].color = Fallo;
                Debug.Log("Fallo: " + resultadosEquipoLocal[i].color + " del jugador " + listaJugadoresUsuario[i].nombre);
            }
            
            if (aciertosEquipoLocal[i])
            {
                aciertosLocales++;
            }
            
            yield return new WaitForSeconds(tiempoTirada);
            
            //Turno del jugador Rival
            int resultadoRival = Random.Range(20, 101);
            tiradasEquipoVisitante[i].text = resultadoRival.ToString();

            aciertosEquipoVisitante[i] = valoresHabilidadesEquipoVisitante[i] >= resultadoRival;
            
            if (aciertosEquipoVisitante[i]) {
                resultadosEquipoVisitante[i].color = Acierto;
                AudioManagerScript.Instance.Reproducir(2, true,true);
                Debug.Log("Acierto: " + resultadosEquipoVisitante[i].color + " del jugador " + listaJugadoresRival[i].nombre);
            } else {
                resultadosEquipoVisitante[i].color = Fallo;
                Debug.Log("Fallo: " + resultadosEquipoVisitante[i].color + " del jugador " + listaJugadoresRival[i].nombre);
            }
            
            if (aciertosEquipoVisitante[i])
            {
                aciertosVisitante++;
            }
            
            //Sumatoria de un acierto si se pulsa el boton de estrategia
            if (EstrategiaUsado==true && contadorEstrategia ==0)
            {
                aciertosLocales++;
                //Contador para que solo sume una vez por partida
                contadorEstrategia++;
            }
        }
        
        //Test de aciertos y de numero de headshots totales de cada jugador
        Debug.Log("TEST Aciertos locales: "+aciertosLocales.ToString());
        Debug.Log("TEST Aciertos visitantes: "+aciertosVisitante.ToString());
        for (int i = 0; i < listaJugadoresUsuario.Count; i++)
        {
            Debug.Log("TEST headshot: " + typeof(Posiciones).ToString() + " HeadShots TOTALES: " + numeroHeadShot[i]);
        }
        
        for(int i = 0; i < Enum.GetValues(typeof(Posiciones)).Length; i++)
        {
            string posicion = Enum.GetName(typeof(Posiciones), i);
            Debug.Log("TEST headshot: " + posicion + " HeadShots TOTALES: " + numeroHeadShot[i]);
        }

        if (aciertosLocales > aciertosVisitante)
        {
            puntuacionLocal++;
        }
        else if(aciertosLocales < aciertosVisitante)
        {
            puntuacionVisitante++;
        }

        puntuacionEquipoUsuario.text = puntuacionLocal.ToString();
        puntuacionEquipoRival.text = puntuacionVisitante.ToString();

        //En la ronda 5 los botones cambian para que no podamos jugar más.
        if (rondaActual<5)
        {
            //Boton vuelve a ser usable
            botonSiguienteRonda.interactable = true;
        }
        else
        {
            if (puntuacionLocal==puntuacionVisitante)
            {
                //Boton vuelve a ser usable
                botonSiguienteRonda.interactable = true;
                cuentaRonda.text = "D";
            }
            else
            {
                botonSiguienteRonda.interactable = false;
                botonContinuar.interactable = true;
            }
            
        }
        
        //Rellenar los jugadores en cada ronda quitando los aumentos
        RellenarJugadores(listaPartidas[0]);
        
        //Quitar la señal de sumatorio por si está activada
        sumatorio.color = Fallo;

    }
    
    //Este metodo está para que lo pueda llamar el boton de siguiente Ronda ya que no puede llamar un IEnumerator
    //Y hemos tenido que hacer IEnumerator el metodo CalcularResultadoRonda para añadirle tiempo entre fase de cada ronda
    public void SiguienteRonda()
    {
        AudioManagerScript.Instance.Reproducir(3, true,true);
        StartCoroutine(CalcularResultadoRonda(1.5f));
    }
    
    public void AnimarEquipo()
    {
        AudioManagerScript.Instance.Reproducir(5, true,true);

        if (!AnimarUsado)
        {
            AnimarUsado = true;
            for (int i = 0; i < listaJugadoresUsuario.Count; i++)
            {
                valoresHabilidadesEquipoLocal[i] = (int.Parse(habilidadesEquipoLocal[i].text) + 1);
                habilidadesEquipoLocal[i].text = ((int.Parse(habilidadesEquipoLocal[i].text) + 1)).ToString();
            }

            aumentoAnimar.interactable = false;
        }
    }
    
    public void MotivarEquipo()
    {
        AudioManagerScript.Instance.Reproducir(5, true,true);
        
        int resultado = Random.Range(0, 2);
        
        if (!MotivarUsado)
        {
            if (resultado==0)
            {
                MotivarUsado = true;
                for (int i = 0; i < listaJugadoresUsuario.Count; i++)
                {
                    valoresHabilidadesEquipoLocal[i] = (int.Parse(habilidadesEquipoLocal[i].text) - 2);
                    habilidadesEquipoLocal[i].text = (int.Parse(habilidadesEquipoLocal[i].text) - 2).ToString();
                }
            }
            else
            {
                MotivarUsado = true;
                for (int i = 0; i < listaJugadoresUsuario.Count; i++)
                {
                    valoresHabilidadesEquipoLocal[i] = (int.Parse(habilidadesEquipoLocal[i].text) + 2);
                    habilidadesEquipoLocal[i].text = (int.Parse(habilidadesEquipoLocal[i].text) + 2).ToString();
                }
            }
            aumentoMotivar.interactable = false;
        }
        
    }
    
    public void EstrategiaEquipo()
    {
        AudioManagerScript.Instance.Reproducir(5, true,true);

        if (!EstrategiaUsado)
        {
            EstrategiaUsado = true;
            
            aciertosEquipoLocal.Add(true);

            sumatorio.color = Acierto;

            aumentoEstrategia.interactable = false;
        }
        
    }

    public Partidas CogerPartida()
    {
        DateTime fechaActual = DateTime.Parse(UsuarioManager.Instance.usuario.fechaInicial);
        
        for (int i = 0; i < listaPartidas.Count; i++)
        {
            TimeSpan intervaloComparacion;

            intervaloComparacion = fechaActual - DateTime.Parse(listaPartidas[i].fecha);
            
            if (intervaloComparacion.Days > 0)
            {
                return listaPartidas[i];
            }
        }

        return listaPartidas[0];
    }

    public void FinalizarPartida()
    {
        AudioManagerScript.Instance.Reproducir(1, true,true);
        
        for (int i = 0; i < listaJugadoresUsuario.Count; i++)
        {
            Jugadores jugador = listaJugadoresUsuario.Find(x => x.posicion.Equals(((Posiciones) i).ToString()));
            
            Debug.Log("TEST FINALIZAR jugador antes: " + jugador.nombre + " moral: " + jugador.moral+ " ritmo: " + jugador.ritmo+ " condicion: " + jugador.condicion + " nº HS :" + jugador.headshotCompetitivo);
            
            if (puntuacionLocal>puntuacionVisitante)
            {
                //Victoria
                if (jugador.moral<95)
                {
                    jugador.moral += 5;
                }
                if (jugador.ritmo<95)
                {
                    jugador.ritmo += 3;
                }
                if (jugador.condicion>15)
                {
                    jugador.condicion -= 1;
                }
                
                jugador.headshotCompetitivo += numeroHeadShot[i];
            }
            else
            {
                //Derrota
                if (jugador.moral>10)
                {
                    jugador.moral -= 3;
                }
                if (jugador.ritmo<95) 
                {
                    jugador.ritmo += 1;
                }
                if (jugador.condicion>15) 
                {
                    jugador.condicion -= 2;
                }

                jugador.headshotCompetitivo += numeroHeadShot[i];
            }
            
            DBManager.Instance.ActualizarJugador(jugador);
            
            Debug.Log("TEST Finalizar jugador despues: " + jugador.nombre + " moral: " + jugador.moral+ " ritmo: " + jugador.ritmo+ " condicion: " + jugador.condicion + " nº HS :" + jugador.headshotCompetitivo);
        }
    }
    
    public enum Posiciones
    {
        Entry, 
        Lurker,
        Support,
        AWPer,
        IGL
    }

}
