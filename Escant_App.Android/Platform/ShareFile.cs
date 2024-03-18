using System;
using System.IO;
using System.Threading.Tasks;
using Android.Content;
using Android.Support.V4.Content;
using Java.IO;
using Xamarin.Forms;
using Escant_App.Droid.Platform;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Android.App;
using Android.Graphics;
using Escant_App.Interfaces;


[assembly: Dependency(typeof(ShareFile))]
namespace Escant_App.Droid.Platform
{
    public class ShareFile : IShareFile
    {
        private readonly Context _context;
        public ShareFile()
        {
            _context = Android.App.Application.Context;
        }

        public Task Show(string title, string message, string filePath)
        {
            var extension = filePath.Substring(filePath.LastIndexOf(".") + 1).ToLower();
            var contentType = string.Empty;

            switch (extension)
            {
                case "pdf":
                    contentType = "application/pdf";
                    break;
                case "png":
                    contentType = "image/png";
                    break;
                default:
                    contentType = "application/octetstream";
                    break;
            }

            var intent = new Intent(Intent.ActionSend);
            intent.SetType(contentType);
            intent.PutExtra(Intent.ExtraStream, Android.Net.Uri.Parse("file://" + filePath));
            intent.PutExtra(Intent.ExtraText, message ?? string.Empty);
            intent.PutExtra(Intent.ExtraSubject, title ?? string.Empty);

            var chooserIntent = Intent.CreateChooser(intent, title ?? string.Empty);
            chooserIntent.SetFlags(ActivityFlags.ClearTop);
            chooserIntent.SetFlags(ActivityFlags.NewTask);
            _context.StartActivity(chooserIntent);

            return Task.FromResult(true);           


        }
    }
}