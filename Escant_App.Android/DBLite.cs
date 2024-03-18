using System.IO;
using SQLite;
using Escant_App.Droid;
using DataModel.Infraestructura.Offline.DB;
using Xamarin.Forms;
using System;
using System.Collections.Generic;

[assembly: Dependency(typeof(DBLite))]
namespace Escant_App.Droid
{
    public class DBLite : IDBLite
    {
        public string DatabasePath()
        {
            //var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), DB3.DATABASE_NAME);
            var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), DB3.DATABASE_NAME);

            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }

            return path;
        }
        public SQLiteAsyncConnection GetConnection()
        {
            var ruta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            // Se crea la base de datos
            var path = Path.Combine(ruta, DB3.DATABASE_NAME);
            return new SQLiteAsyncConnection(path);
        }

        public bool CopyDBToSdCard()
        {
            bool respuesta = false;
            try
            {
                string fileName = DependencyService.Get<IDBLite>().DatabasePath();
                //string backupfile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "backup.db3");
                string backupfile = Path.Combine(Android.OS.Environment.DirectoryDownloads, DbContext.DB_NAME);
                File.Copy(fileName, backupfile, true);
                respuesta = true;
            }
            catch (Exception ex)
            {
                respuesta = false;
            }
            return respuesta;
        }

        public Dictionary<string, string> CopyDBToTarjeta()
        {
            string respuesta = "false";
            string mensaje = "";
            Dictionary<string, string> miRespuesta =
                       new Dictionary<string, string>();
            try
            {
                string fileName = DependencyService.Get<IDBLite>().DatabasePath();
                //string backupfile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "backup.db3");               ;
                //string backupfile = Path.Combine(GetLoggerDir(Android.OS.Environment.DirectoryDownloads,1), "backup" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + ".db3");
                string backupfile = Path.Combine(GetLoggerDir(Android.OS.Environment.DirectoryDownloads, 1), DbContext.DB_NAME);
                if (File.Exists(backupfile)) File.Delete(backupfile);
                File.Copy(fileName, backupfile, true);
                respuesta = "true";
                mensaje = "El respaldo se ha realizado exitosamente";
            }
            catch (Exception ex)
            {
                respuesta = "false";
                mensaje = "El respaldo tuvo errores" + ex.Message.ToString();
            }
            miRespuesta.Add("respuesta", mensaje);
            return miRespuesta;

        }

        public Dictionary<string, string> RestoreDBTarjeta()
        {
            string respuesta = "false";
            string mensaje = "";
            Dictionary<string, string> miRespuesta =
                       new Dictionary<string, string>();
            try
            {

                //string backupfile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "backup.db3");
                string fileName = Path.Combine(GetLoggerDir(Android.OS.Environment.DirectoryDownloads, 0), DbContext.DB_NAME);
                string backupfile = DependencyService.Get<IDBLite>().DatabasePath();
                if (fileName != "")
                {
                    if (File.Exists(backupfile)) File.Delete(backupfile);
                    File.Copy(fileName, backupfile, true);
                    respuesta = "true";
                    mensaje = "La restauración se ha realizado exitosamente";
                }
                else
                {
                    mensaje = "Error, No existe archivo para restaurar ";

                }
            }
            catch (Exception ex)
            {
                respuesta = "false";
                mensaje = "La restauración tuvo errores" + ex.Message.ToString();
            }
            miRespuesta.Add("respuesta", mensaje);
            return miRespuesta;

        }


        public static string GetLoggerDir(string path, int back)
        {
            Java.IO.File dataDir = new Java.IO.File(path);
            if (back == 1) //Si es backup crea
            {
                if (!dataDir.Exists())
                {

                    //var folderPath = GetExternalFilesDir(null).AbsolutePath; var dir = Path.Combine(folderPath, "MyBakcup"); if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);


                    dataDir = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory + "/" + "BackupEscant" + "/");
                    if (!dataDir.Exists())
                        dataDir.Mkdirs();
                }
            }
            else
            {
                if (!dataDir.Exists())
                {
                    dataDir = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory + "/" + "BackupEscant" + "/");
                    if (!dataDir.Exists())
                        return "";
                }
            }
            return dataDir.ToString();
        }



    }
}