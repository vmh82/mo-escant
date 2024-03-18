using DataModel.DTO.Iteraccion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Escant_App.UserControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FrameView : ContentView
    {
        public FrameView()
        {
            InitializeComponent();
        }

        public class Transaccion 
        {
            public ImageSource Imagen { get; set; }
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
            public string CantidadTotal { get; set; }
            public string CantidadHoy { get; set; }
        }

    }
}