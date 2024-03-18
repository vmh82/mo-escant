using Acr.UserDialogs;
using ManijodaServicios.Offline.Interfaz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ManijodaServicios.Offline.Implementa
{
    public class DialogService : IDialogService
    {
        public Task ShowAlertAsync(string message, string title, string buttonLabel)
        {
            return UserDialogs.Instance.AlertAsync(message, title, buttonLabel);
        }
        public void ShowAlert(string message, string title, string buttonLabel)
        {
            UserDialogs.Instance.Alert(message, title, buttonLabel);
        }
        public Task<bool> ShowConfirmedAsync(string message, string title, string okText, string cancelText)
        {
            return UserDialogs.Instance.ConfirmAsync(message, title, okText, cancelText);
        }
        public void ShowToast(string message, int duration = 5000)
        {
            var toastConfig = new ToastConfig(message);
            toastConfig.SetDuration(duration);
            // ICON
            /*
            var img = "notification_icon.png";
            var icon = await BitmapLoader.Current.LoadFromResource(img, null, null);
            toastConfig.SetIcon(icon);
            */

            toastConfig.SetMessageTextColor(System.Drawing.Color.White);
            toastConfig.SetBackgroundColor(System.Drawing.Color.FromArgb(33, 44, 55));

            UserDialogs.Instance.Toast(toastConfig);
        }
    }
}
