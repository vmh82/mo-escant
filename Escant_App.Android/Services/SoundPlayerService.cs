using System;
using Android.Media;
using Escant_App.Droid.Services;
using Escant_App.Services;
using Escant_App.Services.Impl;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency (typeof(SoundPlayerService))]
namespace Escant_App.Droid.Services
{
    public class SoundPlayerService : ISoundService
    {
        public SoundPlayerService()
        {
        }

        public void PlayAudioFile(string fileName)
        {
            var player = new MediaPlayer();
            var fd = global::Android.App.Application.Context.Assets.OpenFd(fileName);
            player.Prepared += (s, e) =>
            {
                player.Start();
            };
            player.SetDataSource(fd.FileDescriptor, fd.StartOffset, fd.Length);
            player.Prepare();
        }
    }
}