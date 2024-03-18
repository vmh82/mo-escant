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
    public partial class DashboardTemplateView : ContentView
    {
        public DashboardTemplateView()
        {
            InitializeComponent();
        }
    }
}