using System;
using System.Collections.Generic;
using System.Text;

namespace Escant_App.Interfaces
{
    public interface ISendSms
    {
        void Send(string address, string message);
    }
}
