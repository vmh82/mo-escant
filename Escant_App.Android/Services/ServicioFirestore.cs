using System;
using System.Diagnostics.Tracing;
using System.Threading.Tasks;
using Android.Gms.Extensions;
using DataModel.DTO.Iteraccion;
using DataModel.DTO.Seguridad;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Firestore.Core;
using Google.Firestore.V1;
using Java.Util.Concurrent;
using Escant_App.Droid.Helpers;
using Escant_App.Droid.ServiceListeners;
using Escant_App.Droid.Services;
using ManijodaServicios.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(ServicioFirestore))]
namespace Escant_App.Droid.Services
{
    public class ServicioFirestore : PhoneAuthProvider.OnVerificationStateChangedCallbacks, IServicioFirestore
    {
        const int OTP_TIMEOUT = 30; // seconds
        private TaskCompletionSource<bool> _phoneAuthTcs;
        private string _verificationId;

        public ServicioFirestore()
        {
        }        

        public async Task<bool> LoginAsync(string username, string password)
        {
            /*
            var tcs = new TaskCompletionSource<bool>();
                FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(username, password)
                    .ContinueWith((task) => OnAuthCompleted(task, tcs));
            return tcs.Task.Result;
            */
            var retorno = false;
            try
            {
                await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(username, password);
                retorno = true;
            }
            catch (FirebaseAuthWeakPasswordException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (FirebaseAuthInvalidCredentialsException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (FirebaseAuthInvalidUserException ex)
            {
                throw new Exception("There is no user record corresponding to this identifier");
            }
            catch (Exception ex)
            {
                throw new Exception("There was an unkown Error.");
            }
            return retorno;
        }

        public async Task<bool> ResetPasswordAsync(string email)
        {
            await FirebaseAuth.Instance.SendPasswordResetEmailAsync(email);
            return true;
        }



        public override void OnVerificationCompleted(PhoneAuthCredential credential)
        {
            System.Diagnostics.Debug.WriteLine("PhoneAuthCredential created Automatically");
        }

        public override void OnVerificationFailed(FirebaseException exception)
        {
            System.Diagnostics.Debug.WriteLine("Verification Failed: " + exception.Message);
            _phoneAuthTcs?.TrySetResult(false);
        }

        public override void OnCodeSent(string verificationId, PhoneAuthProvider.ForceResendingToken forceResendingToken)
        {
            base.OnCodeSent(verificationId, forceResendingToken);
            _verificationId = verificationId;
            _phoneAuthTcs?.TrySetResult(true);
        }

        /*public Task<bool> SendOtpCodeAsync(string phoneNumber)
        {
            _phoneAuthTcs = new TaskCompletionSource<bool>();
            PhoneAuthProvider.Instance.VerifyPhoneNumber(
                phoneNumber,
                OTP_TIMEOUT,
                TimeUnit.Seconds,
                Platform.CurrentActivity,
                this);
            return _phoneAuthTcs.Task;
        }*/

        private void OnAuthCompleted(Task task, TaskCompletionSource<bool> tcs)
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                // something went wrong
                tcs.SetResult(false);
                return;
            }
            _verificationId = null;
            tcs.SetResult(true);
        }
        /*
        public Task<bool> VerifyOtpCodeAsync(string code)
        {
            if (!string.IsNullOrWhiteSpace(_verificationId))
            {
                var credential = PhoneAuthProvider.GetCredential(_verificationId, code);
                var tcs = new TaskCompletionSource<bool>();
                FirebaseAuth.Instance.SignInWithCredentialAsync(credential)
                    .ContinueWith((task) => OnAuthCompleted(task, tcs));
                return tcs.Task;
            }
            return Task.FromResult(false);
        }
        
        public Task<Usuario> GetUserAsync()
        {
            var tcs = new TaskCompletionSource<Usuario>();

            FirebaseFirestore.Instance
                .Collection("usuario")
                .Document(FirebaseAuth.Instance.CurrentUser.Uid)
                .Get()
                .AddOnCompleteListener(new OnCompleteListener(tcs));

            return tcs.Task;
        }
        */
        public async Task<SecuencialNumerico> GetIncremental(string secuencial)
        {
            //var tcs = new TaskCompletionSource<SecuencialNumerico>();
            var tcs1 = new TaskCompletionSource<SecuencialNumerico>();
            SecuencialNumerico envio = new SecuencialNumerico();
            try
            {
                /*
                var valorActual = await FirebaseFirestore.Instance
                .Collection("Secuenciales")
                .Document("Secuencial")
                .Get()
                .AddOnCompleteListener(new OnCompleteListener_SecuencialNumerico(tcs,secuencial));
                //var y = tcs.Task.Result;
                envio.SecuencialRecibo = tcs.Task.Result.SecuencialRecibo;*/

                await FirebaseFirestore.Instance
               .Collection("Secuenciales")
               .Document("Secuencial")
               .Update(secuencial, FieldValue.Increment(1));

                var valorIncrementado = await FirebaseFirestore.Instance
                .Collection("Secuenciales")
                .Document("Secuencial")
                .Get()
                .AddOnCompleteListener(new OnCompleteListener_SecuencialNumerico(tcs1,secuencial));

                envio.SecuencialReciboIncrementado = tcs1.Task.Result.SecuencialRecibo;
                //envio.SecuencialRecibo = valorActual.Data["Recibo"].ToString()

            }
            catch (Exception ex)
            { }
            //return tcs.Task;
            return envio;
        }       
    }

}