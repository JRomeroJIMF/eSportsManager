using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DBManager : MonoBehaviour
{
    [SerializeField] private SimpleSQL.SimpleSQLManager sqlManagerDinamico;
    [SerializeField] private SimpleSQL.SimpleSQLManager sqlManagerBase;

    public static DBManager Instance;

    void Start()
    {
        //Patron Singleton
        if (Instance == null)
        {
            Instance = this;
            
            //Esto hará que no se destruya entre escenas
            DontDestroyOnLoad(this);
            
            //Cosas del SQLite
            sqlManagerBase.Initialize(false);
            
            //Cosas del SQLite
            sqlManagerDinamico.Initialize(false);
            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnApplicationQuit()
    {
        sqlManagerBase.Close();
        sqlManagerBase.Dispose();
        
        sqlManagerDinamico.Close();
        sqlManagerDinamico.Dispose();
    }
    
    void OnEnable()
    {
        Debug.Log("Test - Nombre Usuario: " + CogerUsuario().nombre);
        Debug.Log("Test - Apellido Usuario: " + CogerUsuario().apellido);
        Debug.Log("Test - Edad Usuario: " + CogerUsuario().edad);
        Debug.Log("Test - Club del Usuario: " + CogerUsuario().club);
    }

    public void ResetearBDDinamica()
    {
        try
        {
            sqlManagerDinamico.BeginTransaction();

            string sql = "DELETE FROM usuarios";
            sqlManagerDinamico.Execute(sql);
            
            sql = "DELETE FROM clubes";
            sqlManagerDinamico.Execute(sql);

            List<Clubes> clubDefault = CogerClubEspecifico("0", true);
            sql = "INSERT INTO clubes (nombreClub, logo, nacionalidad, diminutivoClub, informacion, equipacion, balance, presupuestoFichaje, cohesionEquipo, ambienteEquipo, situacionManager) VALUES (?, ?, ?, ?, ?, ?, ?,?,?,?,?)";

            sqlManagerDinamico.Execute(sql, clubDefault[0].nombreClub, clubDefault[0].logo, clubDefault[0].nacionalidad, clubDefault[0].diminutivoClub, clubDefault[0].informacion, clubDefault[0].equipacion, clubDefault[0].balance, clubDefault[0].presupuestoFichaje, clubDefault[0].cohesionEquipo, clubDefault[0].ambienteEquipo, clubDefault[0].situacionManager);

            sql = "DELETE FROM finanzas";
            sqlManagerDinamico.Execute(sql);
            
            sql = "DELETE FROM jugadores";
            sqlManagerDinamico.Execute(sql);
            
            List<Jugadores> jugadoresClub = CogerJugadoresEquipoEspecifico("0", true);
            for (int i = 0; i < jugadoresClub.Count; i++)
            {
                AñadirJugadoresDinamico(0, jugadoresClub[i].nombre, jugadoresClub[i].nick, jugadoresClub[i].imagenJugador, jugadoresClub[i].posicion, jugadoresClub[i].moral, jugadoresClub[i].condicion, jugadoresClub[i].ritmo, jugadoresClub[i].valor, jugadoresClub[i].salario, jugadoresClub[i].edad, jugadoresClub[i].nacionalidad, jugadoresClub[i].altura, jugadoresClub[i].personalidad, jugadoresClub[i].reflejos, jugadoresClub[i].reaccion, jugadoresClub[i].precision, jugadoresClub[i].concentracion, jugadoresClub[i].juegoEquipo, jugadoresClub[i].determinacion, jugadoresClub[i].comunicacion, jugadoresClub[i].disciplina, jugadoresClub[i].liderazgo, jugadoresClub[i].porcentajePistola, jugadoresClub[i].porcentajeSubfusil, jugadoresClub[i].porcentajeFusil, jugadoresClub[i].porcentajeAwp);
            }
            
            sql = "DELETE FROM partidas";
            sqlManagerDinamico.Execute(sql);

            sqlManagerDinamico.Commit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
            sqlManagerDinamico.Rollback();
            
            throw;
        }
        
    }
    
    public List<Clubes> CogerClubes()
    {
        string query = "SELECT * FROM clubes";

        List<Clubes> listaClubes = sqlManagerBase.Query<Clubes>(query);

        return listaClubes;
    }
    
    public List<Clubes> CogerClubEspecifico(string variableID, bool estatica)
    {

        List<Clubes> listaClubes = new List<Clubes>();

        string query = string.Empty;

        if (estatica)
        {
            query = "SELECT * FROM clubes WHERE idClub == "+ variableID;
            listaClubes = sqlManagerBase.Query<Clubes>(query);
        }
        else
        {
            query = "SELECT * FROM clubes";
            listaClubes = sqlManagerDinamico.Query<Clubes>(query);
        }

        return listaClubes;
    }
    
    public List<Entrenamientos> CogerEntrenamientos()
    {
        string query = "SELECT * FROM entrenamientos";

        List<Entrenamientos> listaEntrenamientos = sqlManagerBase.Query<Entrenamientos>(query);

        return listaEntrenamientos;
    }
    
    public List<Jugadores> CogerTodosJugadores()
    {
        string query = "SELECT * FROM jugadores";

        List<Jugadores> listaJugadores = sqlManagerBase.Query<Jugadores>(query);

        return listaJugadores;
    }

    public Jugadores CogerJugadorPorPosicion(string idClub, string posicion)
    {
        string query = "SELECT * FROM jugadores WHERE posicion = "+ posicion + " AND fidClub = " + idClub;

        List<Jugadores> JugadorPorID = sqlManagerBase.Query<Jugadores>(query);

        return JugadorPorID[0];
    }

    public List<Jugadores> CogerJugadoresEquipoEspecifico(string variableID, bool estatica)
    {
        string query = string.Empty;

        List<Jugadores> listaJugadores = new List<Jugadores>();

        if (estatica)
        {
            query = "SELECT * FROM jugadores WHERE fidClub =  " + variableID;
            listaJugadores = sqlManagerBase.Query<Jugadores>(query);
        }
        else
        {
            query = "SELECT * FROM jugadores where fidClub != 0";
            listaJugadores = sqlManagerDinamico.Query<Jugadores>(query); 
        }

        return listaJugadores;
    }
    
    public List<Jugadores> CogerJugadoresTransferibles()
    {
        string query = "SELECT * FROM jugadores WHERE fidClub = 0";

        List<Jugadores> listaJugadores = sqlManagerDinamico.Query<Jugadores>(query);

        return listaJugadores;
    }

    //Quitar los correos de la BD
    /*
    public List<Correo> CogerCorreos()
    {
        string query = "SELECT * FROM correos ";

        List<Correo> listaCorreos = sqlManager.Query<Correo>(query);

        return listaCorreos;
    }
    */
    
    public Finanzas CogerFinanzasEquipoEspecifico(string variableID, bool estatica)
    {
        string query = string.Empty;

        Finanzas finanzasEquipo = new Finanzas();

        if (estatica)
        {
            query = "SELECT * FROM finanzas WHERE fidClub =  " + variableID;
            finanzasEquipo = sqlManagerBase.Query<Finanzas>(query)[0];
        }
        else
        {
            query = "SELECT * FROM finanzas";
            finanzasEquipo = sqlManagerDinamico.Query<Finanzas>(query)[0]; 
        }

        return finanzasEquipo;
    }

    public Usuarios CogerUsuario()
    {
        string query = "SELECT * FROM usuarios";

        Usuarios usuarios = sqlManagerDinamico.Query<Usuarios>(query)[0];

        return usuarios;
    }
    
    public List<Usuarios> CogerUsuarios()
    {
        string query = "SELECT * FROM usuarios";

        List<Usuarios> usuarios = sqlManagerDinamico.Query<Usuarios>(query);

        return usuarios;
    }
    
  
   public List<Partidas> CogerPartidasDelEquipo(int clubId, bool estatica)
   {
       string query = "SELECT * FROM partidas where equipoLocal = " + clubId;

       List<Partidas> listaPartidas = new List<Partidas>();
       
       if (!estatica)
       {
           query =  "SELECT * FROM partidas";
           listaPartidas = sqlManagerDinamico.Query<Partidas>(query);
       }
       else
       {
           listaPartidas = sqlManagerBase.Query<Partidas>(query);
       }

       return listaPartidas;
   }

    //INSERT
    public void AñadirUsuario(string nombreUsuario, string apellidoUsuario, int edadUsuario)
    {

        string sql = "INSERT INTO usuarios (nombre, apellido, edad) VALUES (?, ?, ?)";

        sqlManagerBase.Execute(sql,nombreUsuario,apellidoUsuario,edadUsuario);
        
    }
    
    public void AñadirUsuarioTest2(Usuarios usuarios)
    {
        sqlManagerBase.Insert(usuarios);

    }
    
    public void AñadirClubDinamico(string nombreClub, string logo, string nacionalidad,string diminutivoClub, string informacion, string equipacion, float balance, float presupuestoFichaje, float cohesionEquipo, float ambienteEquipo, float situacionManager)
    {
        //Faltaria añadir el nick del usuario a la BD
        
        string sql = "INSERT INTO clubes (nombreClub, logo, nacionalidad, diminutivoClub, informacion, equipacion, balance, presupuestoFichaje, cohesionEquipo, ambienteEquipo, situacionManager) VALUES (?, ?, ?, ?, ?, ?, ?,?,?,?,?)";

        sqlManagerDinamico.BeginTransaction();

        sqlManagerDinamico.Execute(sql,nombreClub, logo, nacionalidad, diminutivoClub, informacion, equipacion, balance, presupuestoFichaje, cohesionEquipo, ambienteEquipo, situacionManager);
        
        sqlManagerDinamico.Commit();
        
    }
    
    public void AñadirJugadoresDinamico(int fidClub, string nombre, string nick, string imagenJugador, string posicion, float moral, float condicion, float ritmo, float valor, float salario, int edad, 
        string nacionalidad, float altura, string personalidad, float reflejos, float reaccion, float precision, float concentracion, float juegoEquipo, float determinacion, float comunicacion,float disciplina,
        float liderazgo, float porcentajePistola, float porcentajeSubfusil, float porcentajeFusil, float porcentajeAwp )
    {

        string sql = "INSERT INTO jugadores (fidClub, nombre, nick, imagenJugador, posicion, moral, condicion, ritmo, valor, salario, edad, nacionalidad, altura, personalidad, reflejos, reaccion, precision,concentracion, juegoEquipo, determinacion, comunicacion,disciplina,liderazgo, porcentajePistola, porcentajeSubfusil, porcentajeFusil, porcentajeAwp ) VALUES (?, ?, ?, ?, ?, ?, ?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

        sqlManagerDinamico.BeginTransaction();
        
        sqlManagerDinamico.Execute(sql,fidClub, nombre, nick, imagenJugador, posicion, moral, condicion, ritmo, valor, salario, edad, nacionalidad, altura, personalidad, reflejos, reaccion, precision,concentracion, juegoEquipo, determinacion, comunicacion,disciplina,liderazgo, porcentajePistola, porcentajeSubfusil, porcentajeFusil, porcentajeAwp);
        Debug.Log("AÑADIENDO JUGADOR CON ID CLUB = " + fidClub);
        sqlManagerDinamico.Commit();

    }

    public void AñadirUsuarioDinamico(string nombreUsuario, string apellidoUsuario, int edadUsuario, string fechaInicial )
    {

        string sql = "INSERT INTO usuarios (nombre, apellido, edad, fechaInicial) VALUES (?, ?, ?, ?)";
        
        sqlManagerDinamico.Execute(sql,nombreUsuario,apellidoUsuario,edadUsuario,fechaInicial);
        
    }
    
    public void AñadirPartidasDinamico(string tipo, int equipoLocal, int equipoVisitante, string fecha, string horario, string lugar, string resultado )
    {

        string sql = "INSERT INTO partidas (tipo, equipoLocal, equipoVisitante, fecha, horario, lugar, resultado) VALUES (?, ?, ?, ?, ?, ?, ?)";
        
        sqlManagerDinamico.Execute(sql,tipo, equipoLocal, equipoVisitante, fecha, horario, lugar, resultado);

    }
    
    public void AñadirFinanzasDinamico(int fidClub, float beneficioMerchandising, float beneficioPatrocinadores,float beneficioTorneos, float beneficioTransferenciaJugadores, float perdidaCompra, float perdidaGaminghouse, float perdidaSalarios, float perdidaTransferenciaJugadores)
    {

        string sql = "INSERT INTO finanzas (fidClub, beneficioMerchandising, beneficioPatrocinadores, beneficioTorneos, beneficioTransferenciaJugadores, perdidaCompra, perdidaGaminghouse, perdidaSalarios, perdidaTransferenciaJugadores) VALUES (?, ?, ?, ?, ?, ?, ?,?,?)";

        sqlManagerDinamico.Execute(sql,fidClub, beneficioMerchandising, beneficioPatrocinadores, beneficioTorneos, beneficioTransferenciaJugadores, perdidaCompra, perdidaGaminghouse, perdidaSalarios, perdidaTransferenciaJugadores);
        
    }
    
    
    //UPDATE
    public void ActualizarBDDinamica(Usuarios usuario)
    {
        //Metodo para hacer el transpaso de una base de datos base a una base de datos dinamica
        
        //Usuario
        sqlManagerDinamico.UpdateTable(usuario);

        //Club
        List<Clubes> clubUsuario = CogerClubEspecifico(usuario.club.ToString(),true);
        //Debug.Log( "TEST USUARIO CLUB " + usuario.club);
        AñadirClubDinamico(clubUsuario[0].nombreClub, clubUsuario[0].logo, clubUsuario[0].nacionalidad, clubUsuario[0].diminutivoClub, clubUsuario[0].informacion, clubUsuario[0].equipacion, clubUsuario[0].balance, clubUsuario[0].presupuestoFichaje,clubUsuario[0].cohesionEquipo, clubUsuario[0].ambienteEquipo, clubUsuario[0].situacionManager );

        //jugadores
        List<Jugadores> jugadoresClub = CogerJugadoresEquipoEspecifico(usuario.club.ToString(), true);
        for (int i = 0; i < jugadoresClub.Count; i++)
        {
            AñadirJugadoresDinamico(1, jugadoresClub[i].nombre, jugadoresClub[i].nick, jugadoresClub[i].imagenJugador, jugadoresClub[i].posicion, jugadoresClub[i].moral, jugadoresClub[i].condicion, jugadoresClub[i].ritmo, jugadoresClub[i].valor, jugadoresClub[i].salario, jugadoresClub[i].edad, jugadoresClub[i].nacionalidad, jugadoresClub[i].altura, jugadoresClub[i].personalidad, jugadoresClub[i].reflejos, jugadoresClub[i].reaccion, jugadoresClub[i].precision, jugadoresClub[i].concentracion, jugadoresClub[i].juegoEquipo, jugadoresClub[i].determinacion, jugadoresClub[i].comunicacion, jugadoresClub[i].disciplina, jugadoresClub[i].liderazgo, jugadoresClub[i].porcentajePistola, jugadoresClub[i].porcentajeSubfusil, jugadoresClub[i].porcentajeFusil, jugadoresClub[i].porcentajeAwp);
        }
        
        //Finanzas
        Finanzas finanzasClub = CogerFinanzasEquipoEspecifico(usuario.club.ToString(), true);
        AñadirFinanzasDinamico(finanzasClub.fidClub, finanzasClub.beneficioMerchandising, finanzasClub.beneficioPatrocinadores, finanzasClub.beneficioTorneos, finanzasClub.beneficioTransferenciaJugadores, finanzasClub.perdidaCompra, finanzasClub.perdidaGaminghouse, finanzasClub.perdidaSalarios, finanzasClub.perdidaTransferenciaJugadores );
        
        //Partidas
        List<Partidas> listaPartidas = CogerPartidasDelEquipo(usuario.club, true);
        for (int i = 0; i < listaPartidas.Count; i++)
        {
            AñadirPartidasDinamico(listaPartidas[i].tipo, listaPartidas[i].equipoLocal, listaPartidas[i].equipoVisitante, listaPartidas[i].fecha, listaPartidas[i].horario, listaPartidas[i].lugar, listaPartidas[i].resultado);
        }
        
    }
    
    public void ActualizarUsuario(Usuarios usuario)
    {
        string sql = "UPDATE usuarios " +
                     "SET nombre = ?, " +
                     "apellido = ?, " +
                     "club = ?, " +
                     "edad = ?, " +
                     "fechaInicial = ?, " +
                     "equipoFavorito = ?, " +
                     "juegoFavorito = ? " +
                     "WHERE " +
                     "idUsuario = ?";
        sqlManagerDinamico.Execute(sql, usuario.nombre, usuario.apellido, usuario.club, usuario.edad, usuario.fechaInicial,usuario.equipoFavorito,usuario.juegoFavorito,usuario.idUsuario);

        /*
        sqlManagerDinamico.BeginTransaction();
            
        sqlManagerDinamico.UpdateTable(usuario);
            
        sqlManagerDinamico.Commit();
        */

    }

    public void ActualizarJugador(Jugadores jugador)
    {
        try
        {
            sqlManagerDinamico.BeginTransaction();
            
            sqlManagerDinamico.UpdateTable(jugador);
            
            sqlManagerDinamico.Commit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
            sqlManagerDinamico.Rollback();
            
            throw;
        }
        
    }

    public void ActualizarFinanzas(Finanzas finanzas)
    {
        try
        {
            sqlManagerDinamico.BeginTransaction();

            sqlManagerDinamico.UpdateTable(finanzas);
            
            sqlManagerDinamico.Commit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
            sqlManagerDinamico.Rollback();
            
            throw;
        }
    }
    
    public void ActualizarClub(Clubes club)
    {
        try
        {
            sqlManagerDinamico.BeginTransaction();

            sqlManagerDinamico.UpdateTable(club);
            
            sqlManagerDinamico.Commit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
            sqlManagerDinamico.Rollback();
            
            throw;
        }
    }
    
    //DELETE
    /*
    public void BorrarUsuarios()
    {
        string sql = "DELETE FROM usuarios";
        sqlManagerBase.Execute(sql);
    }
    */
    
    public void BorrarPartidas(int idPartida)
    {
        string sql = "DELETE FROM partidas WHERE idPartida = " + idPartida;

        try
        {
            sqlManagerDinamico.BeginTransaction();

            sqlManagerDinamico.Execute(sql);
            
            sqlManagerDinamico.Commit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
            sqlManagerDinamico.Rollback();
            
            throw;
        }
    }

}