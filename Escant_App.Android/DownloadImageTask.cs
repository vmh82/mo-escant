using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using Java.Net;

namespace Escant_App.Droid
{
    public class DownloadImageTask : AsyncTask<string, string, byte[]>
    {
        protected override byte[] RunInBackground(params string[] @params)
        {
            try
            {
                URL url = new URL(@params[0]);
                URLConnection urlConnection = url.OpenConnection();
                urlConnection.Connect();
                int lengthOfFile = urlConnection.ContentLength;
                InputStream input = new BufferedInputStream(url.OpenStream(), lengthOfFile);
                ByteArrayOutputStream output = new ByteArrayOutputStream();
                byte[] byteBuffer = new byte[1024];
                long total = 0;
                int count;
                while ((count = input.Read(byteBuffer)) != -1)
                {
                    total += count;
                    output.Write(byteBuffer, 0, count);
                }
                var data = output.ToByteArray();
                output.Flush();
                output.Close();
                input.Close();
                return data;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        protected override void OnPostExecute(byte[] result)
        {
            base.OnPostExecute(result);
        }
    }
}