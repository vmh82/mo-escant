using System;
using System.Collections.Generic;
using System.Text;

namespace Escant_App.Validations
{
    public class IsNotNullOrEmptyRule<T> : IValidationRule<T>
    {
        public IsNotNullOrEmptyRule()
        {
            ValidationMessage = "Không được để trống";
        }

        public IsNotNullOrEmptyRule(string entidad)
        {
            ValidationMessage = entidad + "Không được để trống";
        }

        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }

            var str = value as string;

            return !string.IsNullOrWhiteSpace(str);
        }
    }
}
