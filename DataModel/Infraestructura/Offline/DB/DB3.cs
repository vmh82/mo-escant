using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace DataModel.Infraestructura.Offline.DB
{
    public class DB3
    {
        public const string DATABASE_NAME = "EscantApp.db";

        public const SQLiteOpenFlags Flags =
           SQLiteOpenFlags.ReadWrite | // open the database in read/write mode
           SQLiteOpenFlags.Create |    // create the database if it doesn't exist
           SQLiteOpenFlags.SharedCache;// enable multi-threaded database access
    }
}
