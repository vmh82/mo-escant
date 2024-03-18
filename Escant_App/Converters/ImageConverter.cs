using System;
using System.Globalization;
using System.IO;
using Xamarin.Forms;

namespace Escant_App.Converters
{
    public class ImageConverter : IValueConverter
    {/*
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                byte[] bytes = (byte[])value;
                ImageSource retImageSource = ImageSource.FromStream(() => new MemoryStream(bytes));
                return retImageSource;
            }

            if (parameter != null)
            {
                string fillerIcon = (string)parameter;
                ImageSource retImageSource = ImageSource.FromFile(fillerIcon);
                return retImageSource;
            }

            return null;
        }
        */
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ImageSource retImageSource = ImageSource.FromStream(() => new MemoryStream());
            if (value != null)
            {

                if (parameter != null)
                {
                    
                    var y = value.GetType().Name;
                    switch (parameter)
                    {
                        /*case string a when parameter.GetType().Name.Contains("String"):                        
                            byte[] bytes = System.Convert.FromBase64String((string)value);
                            retImageSource = ImageSource.FromStream(() => new MemoryStream(bytes));                            
                            break;
                        //case string b when parameter.GetType().Name.Contains("CodigoEstado"):
                        */
                        case "String":
                            if (string.IsNullOrEmpty((string)value))
                            {
                                retImageSource = ImageSource.FromFile("picture_icon.png");
                            }
                            else
                            {
                                byte[] bytes = System.Convert.FromBase64String((string)value);
                                retImageSource = ImageSource.FromStream(() => new MemoryStream(bytes));
                            }
                            break;
                        case "CodigoEstado":
                            switch (value)
                            {
                                case string a when value.ToString().Contains("Pendien"):
                                    retImageSource = ImageSource.FromFile("OrdenPendiente.png");
                                    break;
                                case string b when value.ToString().Contains("Ingresa"):
                                    retImageSource = ImageSource.FromFile("OrdenIngresada.png");
                                    break;
                                case string b when value.ToString().Contains("Recibi"):
                                    retImageSource = ImageSource.FromFile("OrdenRecibida.png");
                                    break;
                                case string b when value.ToString().Contains("Cancela"):
                                    retImageSource = ImageSource.FromFile("OrdenCancelada.png");
                                    break;
                            }
                            break;
                        default:
                            string fillerIcon = (string)parameter;
                            retImageSource = ImageSource.FromFile(fillerIcon);
                            break;
                    }
                }
                else
                {
                    byte[] bytes = (byte[])value;
                    retImageSource = ImageSource.FromStream(() => new MemoryStream(bytes));

                }

            }
            else
            {
                if (parameter != null)
                {
                    switch (parameter)
                    {                     
                        case "String":
                            retImageSource = ImageSource.FromFile("picture_icon.png");
                            break;
                    }

                }
            }
            return retImageSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
