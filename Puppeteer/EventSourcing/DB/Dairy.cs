using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace Puppeteer.EventSourcing.DB
{
    public enum DatabaseType { SQLServer, MySQL };

    internal class Dairy
    {

        private readonly string connectionString;
        private readonly string name;
        private readonly DairyStorage dairyStorage;

        internal Dairy(DatabaseType dbType, string connectionString, string name)
        {
            if (String.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString));
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            this.connectionString = connectionString;
            this.name = name;
            if (dbType == DatabaseType.MySQL)
            {
                dairyStorage = new DairyStorageMySQL(connectionString, name);
            }
            else if (dbType == DatabaseType.SQLServer)
            {
                dairyStorage = null;
            }
        }

        internal void RecuperarEstado(Actor actor)
        {
            if (actor == null) throw new ArgumentNullException(nameof(actor));
            if (dairyStorage == null) throw new Exception("El Actor no puede guardar ni recuperar su último estado pues no se estableció conexión a ninguna Base de Datos.");
            dairyStorage.RecuperarEstado(actor);
        }

        internal void EscribirEnDairy (string script)
        {
            dairyStorage.EscribirEnDairy(script);
        }

        private abstract class DairyStorage
        {
            protected readonly string connectionString;
            protected readonly string name;
            protected bool estaLevantando = true;

            protected DairyStorage(string connectionString, string name)
            {
                this.connectionString = connectionString;
                this.name = name;
            }

            protected internal abstract void RecuperarEstado(Actor actor);

            protected internal abstract void EscribirEnDairy(string script);
        }

        private class DairyStorageMySQL : DairyStorage
        {
            private readonly MySqlConnection connection;

            internal DairyStorageMySQL(string connectionString, string name):base(connectionString, name)
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
            }

            private bool ProbarSiEsTablaNueva()
            {
                bool esTablaNueva = false;
                string sql = "SELECT 1 FROM " + base.name + " LIMIT 1";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    try
                    {
                        var dataReader = command.ExecuteReader();
                        dataReader.Close();
                    }
                    catch
                    {
                        esTablaNueva = true;
                    }
                }
                return esTablaNueva;
            }

            private void CrearDairy(String nombreDelDiario)
            {
                StringBuilder statement = new StringBuilder();

                statement
                    .Append("create table ").Append(nombreDelDiario)
                    .Append("(")
                    .Append("id BIGINT UNSIGNED NOT NULL AUTO_INCREMENT,")
                    .Append("FechaHora DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,")
                    //.Append("IP CHAR(15) NOT NULL,")
                    //.Append("Browser SMALLINT NOT NULL DEFAULT 0,")
                    //.Append("Correo VARCHAR(45) NOT NULL,")
                    //.Append("ClienteId VARCHAR(32) NOT NULL,")
                    //.Append("Canal TINYINT NOT NULL DEFAULT 0,")
                    .Append("Script TEXT NOT NULL,")
                    .Append("PRIMARY KEY (id)")
                    .Append(") ENGINE=ARCHIVE CHARSET=utf8;");
                string sql = statement.ToString();
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }

             protected internal override void RecuperarEstado(Actor actor)
             {
                //string sql = "SELECT id, FechaHora, IP, Browser, Correo, ClienteId, Canal, Script FROM " + base.name + " ORDER BY id";
                if (estaLevantando)
                {
                    if (ProbarSiEsTablaNueva())
                    {
                        CrearDairy(name);
                    }
                    estaLevantando = true;
                    string sql = "SELECT FechaHora, Script FROM " + base.name + " ORDER BY id";
                    MySqlCommand command = new MySqlCommand(sql, connection);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime fechaHora = reader.GetDateTime("FechaHora");
                            string script = reader.GetString("Script");

                            var now = fechaHora;
                            var ahora = new Libraries.FechaHora(now.Day, now.Month, now.Year, now.Hour, now.Minute, now.Second);
                            actor.Perform(script, ahora);
                        }
                        reader.Close();
                    }
                    estaLevantando = false;
                }
                else
                {
                    throw new Exception($"Ya {base.name} habia recuperado su estado. No puede cargarse de nuevo.");
                }
            }

            protected override internal void EscribirEnDairy(string script)
            {
                if (String.IsNullOrEmpty(script)) throw new ArgumentNullException(nameof(script));
                if (estaLevantando) return;
                StringBuilder statement = new StringBuilder()
                    .Append("insert into ").Append(base.name)
                    .Append("( ")
                        //.Append("FechaHora,")
                        //.Append("IP,")
                        //.Append("Browser,")
                        //.Append("Correo,")
                        //.Append("ClienteId,")
                        //.Append("Canal,")
                        .Append("Script")
                    .Append(") values (")
                        //.Append("FechaHora,")
                        //.Append("IP,")
                        //.Append("Browser,")
                        //.Append("Correo,")
                        //.Append("ClienteId,")
                        //.Append("Canal,")
                        .Append('"' + script.Replace("\"","\\\"") + '"')
                    .Append(")");
                string sql = statement.ToString();
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        throw new Exception("Error al tratar de escribir el Script en el Dairy: " + e.Message);
                    }
                }
            }
        }

    }


}
