using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DataModel.DTO;
using Escant_App.Droid;
using Escant_App.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidImageDownloader))]
namespace Escant_App.Droid
{
    public class AndroidImageDownloader: IDownloadImageService
    {
        public async Task DownloadImage(string url, string id)
        {
            DownloadImageTask downloadImageTask = new DownloadImageTask();
            downloadImageTask.Execute(url);
            var result = downloadImageTask.GetResult();
            if(result != null)
            {
                MessagingCenter.Send<DownloadItem>(new DownloadItem(id, result), Escant_App.AppSettings.Settings.ImageDonwloadCompleted);
            }
        }
    }
}