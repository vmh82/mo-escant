using System;
using System.Collections.Generic;
using System.Text;

namespace Escant_App.Validations
{
    public interface IValidationRule<T>
    {
        string ValidationMessage { get; set; }

        bool Check(T value);
    }
}
