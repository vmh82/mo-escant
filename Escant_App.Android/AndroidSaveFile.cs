using System;
using System.IO;
using System.Threading.Tasks;
using Android.Content;
using AndroidX.Core.Content;
using Java.IO;
using Xamarin.Forms;
using Escant_App.Droid;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Android.App;
using Android.Graphics;
using Escant_App.Interfaces;
using System.Text;

[assembly: Dependency(typeof(AndroidSaveFile))]
namespace Escant_App.Droid
{
    public class AndroidSaveFile : ISaveFile
    {
        public AndroidSaveFile()
        {
        }
        public async Task<bool> WriteBackExcelFile(string filePath, string contentType, MemoryStream stream)
        {
            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
            string exception = string.Empty;
            
            /*Obsolete*/
            //var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Storage);
            //if (status != PermissionStatus.Granted)
            //{
            //    var results = await CrossPermissions.Current.RequestPermissionsAsync(Plugin.Permissions.Abstractions.Permission.Storage);
            //    if (results.ContainsKey(Plugin.Permissions.Abstractions.Permission.Storage))
            //        status = results[Plugin.Permissions.Abstractions.Permission.Storage];
            //}

            //if (status == PermissionStatus.Granted)
            //{
            //    Java.IO.File file = new Java.IO.File(filePath);
            //    if (file.Exists())
            //    {
            //        file.Delete();
            //    }
            //    try
            //    {
            //        FileOutputStream outs = new FileOutputStream(file);
            //        outs.Write(stream.ToArray());
            //        outs.Flush();
            //        outs.Close();
            //    }
            //    catch (Exception e)
            //    {
            //        exception = e.ToString();
            //    }
            //    finally
            //    {
            //        if (contentType != "application/html")
            //        {
            //            stream.Dispose();
            //        }
            //    }

            //    if (file.Exists() && contentType != "application/html")
            //    {
            //        Android.Net.Uri path = Android.Net.Uri.FromFile(file);
            //        string extension = Android.Webkit.MimeTypeMap.GetFileExtensionFromUrl(Android.Net.Uri.FromFile(file).ToString());
            //        string mimeType = Android.Webkit.MimeTypeMap.Singleton.GetMimeTypeFromExtension(extension);
            //        Intent intent = new Intent(Intent.ActionView);
            //        intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
            //        path = FileProvider.GetUriForFile(Android.App.Application.Context, Android.App.Application.Context.PackageName + ".fileprovider", file);
            //        intent.SetDataAndType(path, mimeType);
            //        intent.AddFlags(ActivityFlags.GrantReadUriPermission);
            //        Forms.Context.StartActivity(Intent.CreateChooser(intent, "Choose App"));
            //    }
            //    taskCompletionSource.SetResult(true);
            //}
            //else if (status != PermissionStatus.Unknown)
            //{
            //    taskCompletionSource.SetResult(false);
            //}

            try
            {
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
                    Java.IO.File file = new Java.IO.File(filePath);
                    if (file.Exists())
                    {
                        file.Delete();
                    }
                    try
                    {
                        FileOutputStream outs = new FileOutputStream(file);
                        outs.Write(stream.ToArray());
                        outs.Flush();
                        outs.Close();
                    }
                    catch (Exception e)
                    {
                        exception = e.ToString();
                    }
                    finally
                    {
                        if (contentType != "application/html")
                        {
                            stream.Dispose();
                        }
                    }

                    if (file.Exists() && contentType != "application/html")
                    {
                        Android.Net.Uri path = Android.Net.Uri.FromFile(file);
                        string extension = Android.Webkit.MimeTypeMap.GetFileExtensionFromUrl(Android.Net.Uri.FromFile(file).ToString());
                        string mimeType = Android.Webkit.MimeTypeMap.Singleton.GetMimeTypeFromExtension(extension);
                        Intent intent = new Intent(Intent.ActionView);
                        intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask | ActivityFlags.MultipleTask);
                        path = FileProvider.GetUriForFile(Android.App.Application.Context, Android.App.Application.Context.PackageName + ".fileprovider", file);
                        intent.SetDataAndType(path, mimeType);

                        intent.AddFlags(ActivityFlags.GrantReadUriPermission | ActivityFlags.NewTask | ActivityFlags.MultipleTask);
                        Forms.Context.StartActivity(Intent.CreateChooser(intent, "Choose App"));
                    }
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

        public async Task<bool> Save(string fileName, string contentType, MemoryStream stream)
        {
            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
            string exception = string.Empty;
            string root = string.Empty;
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Storage);
            if (status != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(Plugin.Permissions.Abstractions.Permission.Storage);
                if (results.ContainsKey(Plugin.Permissions.Abstractions.Permission.Storage))
                    status = results[Plugin.Permissions.Abstractions.Permission.Storage];
            }

            if (status == PermissionStatus.Granted)
            {
                if (Android.OS.Environment.IsExternalStorageEmulated)
                {
                    root = Android.OS.Environment.ExternalStorageDirectory.ToString();
                }
                else
                {
                    root = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }

                Java.IO.File myDir = new Java.IO.File(System.IO.Path.Combine(root + "/Escant_App"));
                myDir.Mkdir();

                Java.IO.File file = new Java.IO.File(myDir, fileName);

                if (file.Exists())
                {
                    file.Delete();
                }

                try
                {
                    FileOutputStream outs = new FileOutputStream(file);
                    outs.Write(stream.ToArray());
                    outs.Flush();
                    outs.Close();
                }
                catch (Exception e)
                {
                    exception = e.ToString();
                }
                finally
                {
                    if (contentType != "application/html")
                    {
                        stream.Dispose();
                    }
                }

                if (file.Exists() && contentType != "application/html")
                {
                    Android.Net.Uri path = Android.Net.Uri.FromFile(file);
                    string extension = Android.Webkit.MimeTypeMap.GetFileExtensionFromUrl(Android.Net.Uri.FromFile(file).ToString());
                    string mimeType = Android.Webkit.MimeTypeMap.Singleton.GetMimeTypeFromExtension(extension);
                    Intent intent = new Intent(Intent.ActionView);
                    intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                    path = FileProvider.GetUriForFile(Android.App.Application.Context, Android.App.Application.Context.PackageName + ".fileprovider", file);
                    intent.SetDataAndType(path, mimeType);
                    intent.AddFlags(ActivityFlags.GrantReadUriPermission);
                    Forms.Context.StartActivity(Intent.CreateChooser(intent, "Choose App"));
                }
                taskCompletionSource.SetResult(true);
            }
            else if (status != PermissionStatus.Unknown)
            {
                //Can not continue
                taskCompletionSource.SetResult(false);
            }
            return await taskCompletionSource.Task;
        }

        public string SaveMemory(MemoryStream stream)
        {
            string root = null;
            string fileName = "SavedDocument.pdf";
            //Get the root folder of the application
            if (Android.OS.Environment.IsExternalStorageEmulated)
            {
                root = Android.OS.Environment.ExternalStorageDirectory.ToString();
            }
            else
            {
                root = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            //Create a new folder with name Syncfusion
            Java.IO.File myDir = new Java.IO.File(System.IO.Path.Combine(root + "/Syncfusion"));
            myDir.Mkdir();
            //Create a new file with the name fileName in the folder Syncfusion
            Java.IO.File file = new Java.IO.File(myDir, fileName);
            string filePath = file.Path;
            //If the file already exists delete it
            if (file.Exists()) file.Delete();
            Java.IO.FileOutputStream outs = new Java.IO.FileOutputStream(file);
            //Save the input stream to the created file
            outs.Write(stream.ToArray());
            var ab = file.Path;
            outs.Flush();
            outs.Close();
            return filePath;
        }

        public string SaveMemory2(MemoryStream stream)
        {
            string text = "hello world";
            string fileName = "SavedDocument.pdf";
            byte[] data = Encoding.ASCII.GetBytes(text);
            string rootPath = System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DataDirectory.ToString());
            var filePathDir = System.IO.Path.Combine(rootPath, "/Syncfusion");
            if (!System.IO.File.Exists(filePathDir))
            {
                Directory.CreateDirectory(filePathDir);
            }
            string filePath = System.IO.Path.Combine(filePathDir, fileName);
            System.IO.File.WriteAllBytes(filePath, data);
            return filePath;
        }

        public void SaveFile(string fileName, String contentType, MemoryStream s)
        {
            string root = null;
            if (Android.OS.Environment.IsExternalStorageEmulated)
            {
                root = Android.OS.Environment.ExternalStorageDirectory.ToString();
            }
            else
                root = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            Java.IO.File myDir = new Java.IO.File(root + "/Escant_App");
            myDir.Mkdir();

            Java.IO.File file = new Java.IO.File(myDir, fileName);

            if (file.Exists()) file.Delete();

            try
            {
                FileOutputStream outs = new FileOutputStream(file);
                outs.Write(s.ToArray());

                outs.Flush();
                outs.Close();

            }
            catch (Exception e)
            {

            }
            if (file.Exists())
            {
                Android.Net.Uri path = Android.Net.Uri.FromFile(file);
                string extension = Android.Webkit.MimeTypeMap.GetFileExtensionFromUrl(Android.Net.Uri.FromFile(file).ToString());
                string mimeType = Android.Webkit.MimeTypeMap.Singleton.GetMimeTypeFromExtension(extension);
                Intent intent = new Intent(Intent.ActionView);
                intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                path = FileProvider.GetUriForFile(Android.App.Application.Context, Android.App.Application.Context.PackageName + ".fileprovider", file);
                intent.SetDataAndType(path, mimeType);
                intent.AddFlags(ActivityFlags.GrantReadUriPermission);
                Forms.Context.StartActivity(Intent.CreateChooser(intent, "Choose App"));
            }
        }

        public static Activity activity { get; set; }

        public async System.Threading.Tasks.Task<byte[]> CaptureAsync()
        {
            var activity1 = Forms.Context as Activity;

            var view = activity1.Window.DecorView;
            view.DrawingCacheEnabled = true;

            Bitmap bitmap = view.GetDrawingCache(true);

            byte[] bitmapData;

            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 0, stream);
                bitmapData = stream.ToArray();
            }

            return bitmapData;
        }
    }
}
