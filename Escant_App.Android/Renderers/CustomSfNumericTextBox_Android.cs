using System;
using Escant_App.Droid.Renderers;
using Escant_App.UserControls;
using Syncfusion.SfNumericTextBox.XForms.Droid;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(CustomSfNumericTextBox), typeof(CustomSfNumericTextBox_Android))]
namespace Escant_App.Droid.Renderers
{
    public class CustomSfNumericTextBox_Android : SfNumericTextBoxRenderer
    {
        
    }
}

