using System.Text.RegularExpressions;
using ManijodaServicios.Resources.Texts;

namespace Escant_App.Validations
{
    public class NumerosRule<T> : IValidationRule<T>
    {
        public string _campo = "";
        public NumerosRule(string campo)
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
                ValidationMessage = _campo != "" ? TextsTranslateManager.Translate(_campo) + ": " + TextsTranslateManager.Translate("EmailRequired") : TextsTranslateManager.Translate("EmailRequired");
                return false;
            }
            
            
            var str = value.GetType().FullName=="System.Single"?value.ToString():value as string;

            if (str == "")
            {
                ValidationMessage = _campo != "" ? TextsTranslateManager.Translate(_campo) + ": " + TextsTranslateManager.Translate("EmailRequired") : TextsTranslateManager.Translate("EmailRequired");
                return false;
            }


            Regex regex = AppSettings.Settings.NumerosRegex;
            Match match = regex.Match(str);

            if (!match.Success)
                ValidationMessage = _campo != "" ? TextsTranslateManager.Translate(_campo) + ": " + TextsTranslateManager.Translate("EmailNotValid") : TextsTranslateManager.Translate("EmailNotValid");

            return match.Success;
        }


    }

}
