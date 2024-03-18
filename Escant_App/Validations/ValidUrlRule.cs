using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using ManijodaServicios.Resources.Texts;

namespace Escant_App.Validations
{
    public class ValidUrlRule<T> : IValidationRule<T>
    {
        public string _campo = "";
        public ValidUrlRule(string campo)
        {
            ValidationMessage = "Phải là một URL";
            _campo = campo;
        }

        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
                ValidationMessage = _campo != "" ? TextsTranslateManager.Translate(_campo) + ": " + TextsTranslateManager.Translate("EmailRequired") : TextsTranslateManager.Translate("EmailRequired");
                return false;
            }

            var str = value as string;

            if (str == "")
            {
                ValidationMessage = _campo != "" ? TextsTranslateManager.Translate(_campo) + ": " + TextsTranslateManager.Translate("EmailRequired") : TextsTranslateManager.Translate("EmailRequired");
                return false;
            }
            return new UrlAttribute().IsValid(value);
        }
    }
}
