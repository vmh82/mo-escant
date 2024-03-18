using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Gms.Tasks;
using DataModel.DTO.Iteraccion;
using DataModel.DTO.Seguridad;
using Firebase.Firestore;

namespace Escant_App.Droid.ServiceListeners
{
    public class OnCompleteListener : Java.Lang.Object, IOnCompleteListener
    {
        private TaskCompletionSource<Usuario> _tcs;

        public OnCompleteListener(TaskCompletionSource<Usuario> tcs)
        {
            _tcs = tcs;
        }

        public void OnComplete(Android.Gms.Tasks.Task task)
        {
            if (task.IsSuccessful)
            {
                // process document
                var result = task.Result;
                if (result is DocumentSnapshot doc)
                {
                    var user = new Usuario();
                    user.Id = doc.Id;
                    user.Nombres = doc.GetString("Nombres");
                    user.Apellidos = doc.GetString("Apellidos");
                    _tcs.TrySetResult(user);
                    return;
                }
            }
            // something went wrong
            _tcs.TrySetResult(default(Usuario));
        }
    }
    public class OnCompleteListener_SecuencialNumerico : Java.Lang.Object, IOnCompleteListener
    {
        private TaskCompletionSource<SecuencialNumerico> _tcs;
        private string _secuencial;

        public OnCompleteListener_SecuencialNumerico(TaskCompletionSource<SecuencialNumerico> tcs,string secuencial)
        {
            _tcs = tcs;
            _secuencial = secuencial;
        }

        public void OnComplete(Android.Gms.Tasks.Task task)
        {
            if (task.IsSuccessful)
            {
                // process document
                var result = task.Result;
                if (result is DocumentSnapshot doc)
                {
                    var user = new SecuencialNumerico();
                    user.SecuencialRecibo = doc.Data[_secuencial].ToString();
                    _tcs.TrySetResult(user);
                    return;
                }
            }
            // something went wrong
            _tcs.TrySetResult(default(SecuencialNumerico));
        }
    }
}