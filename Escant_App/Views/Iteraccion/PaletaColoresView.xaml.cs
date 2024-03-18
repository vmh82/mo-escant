using DataModel.DTO.Iteraccion;
using Escant_App.ViewModels.Iteraccion;
using Escant_App.Views.Administracion;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Escant_App.Views.Iteraccion
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaletaColoresView : PopupPage
    {
        #region 0.DeclaraciónVariables
        private ColorAplicacion _colorSeleccionado;
        #endregion 0.DeclaraciónVariables
        #region 1.Constructor
        public PaletaColoresView()
        {
            _colorSeleccionado = new ColorAplicacion();
            InitializeComponent();
        }
        #endregion 1.Constructor
        #region 2.VinculaClaseFrontEnd
        public PaletaColoresViewModel Context => (PaletaColoresViewModel)this.BindingContext;
        #endregion 2.VinculaClaseFrontEnd
        #region 3.Métodos FrontEnd
        /// <summary>
        /// Evento de seleccción de Color
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SfRadialMenuItem_ItemTapped(object sender, Syncfusion.SfRadialMenu.XForms.ItemTappedEventArgs e)
        {
            Syncfusion.SfRadialMenu.XForms.SfRadialMenuItem item = sender as Syncfusion.SfRadialMenu.XForms.SfRadialMenuItem;
            _colorSeleccionado.Codigo= HexConverter((Color)item.BackgroundColor);
            _colorSeleccionado.Codigo1 = RGBConverter((Color)item.BackgroundColor);
            administraSalida(); 

        }
        /// <summary>
        /// Método de Conversión de Color en Hexadecimal
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static String HexConverter(System.Drawing.Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }
        /// <summary>
        /// Conversión de color en RGB
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static String RGBConverter(System.Drawing.Color c)
        {
            return "RGB(" + c.R.ToString() + "," + c.G.ToString() + "," + c.B.ToString() + ")";
        }
        /// <summary>
        /// Método para retornar el Color Seleccionado a la página de llamada
        /// </summary>
        async void administraSalida()
        {

            MessagingCenter.Send<ColorAplicacion>(_colorSeleccionado, "OnDataSourceChanged");
            await Context.GoReturn();
            
        }
        /// <summary>
        /// Metodo que se dispara al presionar botón de Retorno
        /// </summary>
        /// <returns></returns>
        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();
        }
        /// <summary>
        /// Método para animar la aparición de la Pantalla
        /// </summary>
        protected override void OnAppearingAnimationBegin()
        {
            base.OnAppearingAnimationBegin();

        }
        #endregion 3.Métodos FrontEnd
    }
}