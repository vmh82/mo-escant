﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Escant_App.Eventos
{
    public class EventosGlobales
    {
        public static event EventHandler<SMSEventArgs> OnSMSReceived;

        public static void OnSMSReceived_Event(object sender, SMSEventArgs sms)
        {
            OnSMSReceived?.Invoke(sender, sms);
        }
    }

    public class SMSEventArgs : EventArgs
    {
        public string PhoneNumber { get; set; }

        public string Message { get; set; }
    }
}
