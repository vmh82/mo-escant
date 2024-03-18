using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ManijodaServicios.Resources.Texts;

namespace Escant_App.Validations
{
    public class PasswordRule<T> : IValidationRule<T>
    {
        public string _campo = "";
        public PasswordRule(string campo)
        {
            ValidationMessage = "Phải là một địa chỉ email";
            _campo = campo;
        }

        public string ValidationMessage { get; set; }

        /*public bool Check(string value)
        {
            return new EmailAddressAttribute().IsValid(value);
        }
        */
        public bool Check(T value)
        {

            if (value == null)
            {
                ValidationMessage = _campo != "" ? TextsTranslateManager.Translate(_campo) + ": " + TextsTranslateManager.Translate("PasswordRequired") : TextsTranslateManager.Translate("PasswordRequired");
                return false;
            }

            var str = value as string;

            if (str == "")
            {
                ValidationMessage = _campo != "" ? TextsTranslateManager.Translate(_campo) + ": " + TextsTranslateManager.Translate("PasswordRequired") : TextsTranslateManager.Translate("PasswordRequired");
                return false;
            }


            Regex regex = AppSettings.Settings.PasswordRegex;
            Match match = regex.Match(str);

            if (!match.Success)
                ValidationMessage = _campo != "" ? TextsTranslateManager.Translate(_campo) + ": " + TextsTranslateManager.Translate("PasswordNotValid") : TextsTranslateManager.Translate("PasswordNotValid");

            return match.Success;
        }


    }

}
