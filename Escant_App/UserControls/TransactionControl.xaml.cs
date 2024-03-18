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
    public partial class TransactionControl : ContentView
    {
        public TransactionControl()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            { }
        }
        public ImageSource Image
        {
            get { return img.Source; }
            set { img.Source = value; }

        }
        public string Text
        {
            get { return lbl.Text; }
            set { lbl.Text = value; }

        }

        public Color BGColor
        {
            get { return frame.BackgroundColor; }
            set { frame.BackgroundColor = value; }
        }
    }
}