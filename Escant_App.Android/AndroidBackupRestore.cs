using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataModel.DTO;
using Escant_App.Droid;
using ManijodaServicios.Offline.Interfaz;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;
using System.IO;
using Permission = Plugin.Permissions.Abstractions.Permission;

[assembly: Dependency(typeof(AndroidBackupRestore))]
namespace Escant_App.Droid
{
    public class AndroidBackupRestore : IBackupRestore
    {
        public async Task<bool> BackupDb(string backupFileName, string dbFileName)
        {
            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
            try
            {
                //var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Storage);
                //if (status != PermissionStatus.Granted)
                //{
                //    var results = await CrossPermissions.Current.RequestPermissionsAsync(Plugin.Permissions.Abstractions.Permission.Storage);
                //    if (results.ContainsKey(Plugin.Permissions.Abstractions.Permission.Storage))
                //        status = results[Plugin.Permissions.Abstractions.Permission.Storage];
                //}
                var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
                if (storageStatus != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                    {
                        taskCompletionSource.SetResult(false);
                    }

                    storageStatus = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                }

                if (storageStatus == PermissionStatus.Granted)
                {
                    var sourceDbFile = GetDbLocalFilePath(dbFileName);
                    var destDbFile = GetExternalStorageFilePath(backupFileName);
                    var source = System.IO.File.ReadAllBytes(sourceDbFile);
                    System.IO.File.WriteAllBytes(destDbFile, source);
                    //System.IO.File.Copy(sourceDbFile, destDbFile, true);
                    taskCompletionSource.SetResult(true);
                }
                else if (storageStatus != PermissionStatus.Unknown)
                {
                    //Can not continue
                    taskCompletionSource.SetResult(false);
                }
            }
            catch (Exception ex)
            {
                taskCompletionSource.SetResult(false);
            }
            return await taskCompletionSource.Task;
        }

        public async Task<bool> RestoreDb(string restoreFileName, string destDbFileName)
        {
            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
            try
            {
                //var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Storage);
                //if (status != PermissionStatus.Granted)
                //{
                //    var results = await CrossPermissions.Current.RequestPermissionsAsync(Plugin.Permissions.Abstractions.Permission.Storage);
                //    if (results.ContainsKey(Plugin.Permissions.Abstractions.Permission.Storage))
                //        status = results[Plugin.Permissions.Abstractions.Permission.Storage];
                //}
                var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
                if (storageStatus != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                    {
                        taskCompletionSource.SetResult(false);
                    }

                    storageStatus = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                }

                if (storageStatus == PermissionStatus.Granted)
                {
                    var originalSourceDbFile = GetExternalStorageFilePath(restoreFileName); //from external storage
                    var sourceTempDbFile = GetExternalStorageFilePath("EscantApp.db");
                    System.IO.File.Copy(originalSourceDbFile, sourceTempDbFile, true);

                    var destDbFile = GetDbLocalFilePath(destDbFileName); //special folder
                    System.IO.File.Copy(sourceTempDbFile, destDbFile, true);
                    Java.IO.File deletedFile = new Java.IO.File(sourceTempDbFile);
                    deletedFile.Delete();
                    taskCompletionSource.SetResult(true);
                }
                else if (storageStatus != PermissionStatus.Unknown)
                {
                    //Can not continue
                    taskCompletionSource.SetResult(false);
                }
            }
            catch (Exception ex)
            {
                taskCompletionSource.SetResult(false);
            }
            return await taskCompletionSource.Task;
        }

        public async Task<List<FileDto>> GetAllBackupFiles()
        {
            TaskCompletionSource<List<FileDto>> taskCompletionSource = new TaskCompletionSource<List<FileDto>>();
            try
            {
                Java.IO.File backupDir = new Java.IO.File(GetRootPath() + "/Escant_App/backup_db");
                var allFiles = backupDir.ListFiles();
                var backupFiles = allFiles.Where(x => x.Name.Contains(".db")).Select(x => new FileDto
                {
                    FileName = x.Name,
                    LastModified = (new DateTime(1970, 1, 1).AddMilliseconds(x.LastModified())).ToLocalTime(),
                    FileSize = GetFileSize(x.Length())
                }).ToList();
                taskCompletionSource.SetResult(backupFiles);
            }
            catch(Exception ex)
            {
                taskCompletionSource.SetResult(new List<FileDto>());
            }
            return await taskCompletionSource.Task;
        }
        /// <summary>
        /// Método para obtener todos los certificados
        /// </summary>
        /// <returns></returns>
        public async Task<List<FileDto>> GetAllCertificados()
        {
            TaskCompletionSource<List<FileDto>> taskCompletionSource = new TaskCompletionSource<List<FileDto>>();
            try
            {
                Java.IO.File backupDir = new Java.IO.File(GetRootPath() + "/Escant_App");
                var allFiles = backupDir.ListFiles();
                var backupFiles = allFiles.Where(x => x.Name.Contains(".p12")).Select(x => new FileDto
                {
                    FileName = x.Name,
                    LastModified = (new DateTime(1970, 1, 1).AddMilliseconds(x.LastModified())).ToLocalTime(),
                    FileSize = GetFileSize(x.Length())
                }).ToList();
                taskCompletionSource.SetResult(backupFiles);
            }
            catch (Exception ex)
            {
                taskCompletionSource.SetResult(new List<FileDto>());
            }
            return await taskCompletionSource.Task;
        }
        public string GetFileSize(long size)
        {
            string[] sizesLabel = { "B", "KB", "MB", "GB" };
            int order = 0;
            while (size >= 1024 && order < sizesLabel.Length - 1)
            {
                order++;
                size = size / 1024;
            }
            return String.Format("{0:0.##} {1}", size, sizesLabel[order]);
        }
        private string GetDbLocalFilePath(string fileName)
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return System.IO.Path.Combine(path, fileName);
        }

        private string GetExternalStorageFilePath(string fileName)
        {
            Java.IO.File parentDir = new Java.IO.File(GetRootPath() + "/Escant_App");
            if (!parentDir.Exists())
                parentDir.Mkdir();

            Java.IO.File backupDir = new Java.IO.File(GetRootPath() + "/Escant_App/backup_db");
            if(!backupDir.Exists())
                backupDir.Mkdir();
            return System.IO.Path.Combine(backupDir.Path, fileName);
        }

        public async Task<string> GetScriptTexts()
        {
            TaskCompletionSource<string> taskCompletionSource = new TaskCompletionSource<string>();
            try
            {
                var currentActivity = Plugin.CurrentActivity.CrossCurrentActivity.Current;
                var assets = currentActivity.Activity.Assets;
                using (StreamReader sr = new StreamReader(assets.Open("Escant_App_sample_data.sql")))
                {
                    string content = sr.ReadToEnd();
                    taskCompletionSource.SetResult(content);
                }
            }
            catch (Java.IO.IOException e)
            {
                taskCompletionSource.SetResult("");
            }
            return await taskCompletionSource.Task;
        }

        private string GetRootPath()
        {
            string root = null;
            if (Android.OS.Environment.IsExternalStorageEmulated)
            {
                root = Android.OS.Environment.ExternalStorageDirectory.ToString();
            }
            else
            {
                root = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            return root;
        }
    }
}
