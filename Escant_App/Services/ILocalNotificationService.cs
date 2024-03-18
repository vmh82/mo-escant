using System;
using System.Collections.Generic;
using System.Text;

namespace Escant_App.Services
{
    public interface ILocalNotificationService
    {
        void LocalNotification(string title, string body, int id, DateTime notifyTime);
        void Cancel(int id);
    }
}
