using Android.App;
using Android.Content;
using Android.Telephony;
using MNJD_NetBilling.Droid.Receivers;
using MNJD_NetBilling.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(SendSms))]

namespace MNJD_NetBilling.Droid.Receivers
{
    public class SendSms : ISendSms
    {
        public void Send(string address, string message)
        {
            var pendingIntent = PendingIntent.GetActivity(Android.App.Application.Context, 0, new Intent(Android.App.Application.Context, typeof(MainActivity)).AddFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask), PendingIntentFlags.NoCreate);

            SmsManager smsM = SmsManager.Default;
            smsM.SendTextMessage(address, null, message, pendingIntent, null);
        }
    }
}