using SQLite;
using DataModel.DTO;
using DataModel.DTO.Administracion;
using DataModel.DTO.Clientes;
using DataModel.DTO.Seguridad;
using DataModel.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;


namespace DataModel.Infraestructura.Offline.DB
{
    public class DbContext
    {
        public static string DB_NAME = "EscantApp.db";
        public static string LocalFilePath;
        private static readonly Type[] tableTypes = new Type[]
        {
            typeof(Empresa),
            typeof(Catalogo),
            typeof(Cliente),
        };
        static bool initialized = false;
        static bool crea = true;
        public SQLiteAsyncConnection Database { get; }
        /// <summary>
        /// Initialized a new DbContext
        /// </summary>
        public DbContext()
        {
            Database = new SQLiteAsyncConnection(Path.Combine(LocalFilePath, DB_NAME));  
            if(crea)
            {
                TaskFunciones.CreateDatabase(initialized, tableTypes).SafeFireAndForget(false);
                crea = false;
            }
        }
        

        /// <summary>
        /// Gets a table by it's type from the db.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public AsyncTableQuery<T> Set<T>() where T : new()
        {
            return Database.Table<T>();
        }
    }
}
