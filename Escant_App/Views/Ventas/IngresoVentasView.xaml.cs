using DataModel.DTO.Compras;
using DataModel.DTO.Productos;
using ManijodaServicios.Resources.Texts;
using Escant_App.ViewModels.Ventas;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Plugin.Vibrate;
using Syncfusion.XForms.ProgressBar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace Escant_App.Views.Ventas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IngresoVentasView : ContentPage
    {
        #region 0. Declaración de Variables

        #endregion 0. Declaración de Variables
        #region 1. Vinculación View con ViewModel
        public IngresoVentasViewModel Context => (IngresoVentasViewModel)this.BindingContext;
        #endregion 1. Vinculación View con ViewModel
        #region 2. Constructor
        public IngresoVentasView()
        {
            InitializeComponent();
            Context.ImagenBotonOrden = "bag.png";
            Context.AbreOrden = ((obj) =>
            {
                abreOrden(2);
            });
            Context.AbreOrdenEstado = ((obj) =>
            {
                abreOrdenEstado(2);
            });
        }
        #endregion 2. Constructor
        #region 3. Métodos Propios FrontEnd        
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Context.ListView = productList;
            await Context.InitializeAsync(null);
        }
        #endregion 3. Métodos Propios FrontEnd
        #region 4. Métodos Personalizados FrontEnd
        private void OpenOrder(object sender, EventArgs e)
        {
            var param = 1;// Convert.ToInt32(((TappedEventArgs)e).Parameter);
            //Context.IsVisibleEstadoOrden = false;
            Context.IsVisibleEstadoOrden = true;
            Context.IsVisibleDetalleCarrito = true;
            abreOrden(param);
        }
        private void abreOrden(int origen)
        {
            //Cierro Orden Estado
            if (EstadoOrdenView.TranslationX == 0)
            {
                Action<double> callback = input => EstadoOrdenView.TranslationX = input;
                EstadoOrdenView.Animate("anim", callback, 0, 170, 16, 300, Easing.CubicOut);
            }
            switch (origen)
            {
                case 1:
                    if (OrderView.TranslationX >= 170)
                    {

                        Action<double> callback = input => OrderView.TranslationX = input;
                        OrderView.Animate("anim", callback, 170, 0, 16, 300, Easing.CubicInOut);
                    }
                    if (OrderView.TranslationX == 0)
                    {
                        Action<double> callback = input => OrderView.TranslationX = input;
                        OrderView.Animate("anim", callback, 0, 170, 16, 300, Easing.CubicOut);
                    }
                    if (Context.ImagenBotonOrden == "bag.png")
                    {
                        Context.ImagenBotonOrden = "left_arrow_black.png";
                    }
                    else
                    {
                        Context.ImagenBotonOrden = "bag.png";
                    }
                    break;
                case 2:
                    //Abro Orden Carrito en caso de no estar abierto
                    if (OrderView.TranslationX >= 170)
                    {

                        Action<double> callback = input => OrderView.TranslationX = input;
                        OrderView.Animate("anim", callback, 170, 0, 16, 300, Easing.CubicInOut);
                    }
                    Context.ImagenBotonOrden = "left_arrow_black.png";
                    break;
            }
        }
        /*
        private void OpenOrder(object sender, EventArgs e)
        {
            Context.IsVisibleEstadoOrden = false;
            Context.IsVisibleDetalleCarrito = true;
            if (OrderView.TranslationX >= 170)
            {

                Action<double> callback = input => OrderView.TranslationX = input;
                OrderView.Animate("anim", callback, 170, 0, 16, 300, Easing.CubicInOut);
            }
            if (OrderView.TranslationX == 0)
            {
                Action<double> callback = input => OrderView.TranslationX = input;
                OrderView.Animate("anim", callback, 0, 170, 16, 300, Easing.CubicOut);
            }
            if (Context.ImagenBotonOrden == "bag.png")
            {
                Context.ImagenBotonOrden = "left_arrow_black.png";
            }
            else
            {
                Context.ImagenBotonOrden = "bag.png";
            }

        }
        */
        private void OpenOrdenEstado(object sender, EventArgs e)
        {
            var param = 1;
            //Context.IsVisibleEstadoOrden = false;
            Context.IsVisibleEstadoOrden = true;
            Context.IsVisibleDetalleCarrito = true;
            abreOrdenEstado(param);
        }
        private async void abreOrdenEstado(int origen)
        {
            //Cierro Orden Carrito            
            if (OrderView.TranslationX == 0)
            {
                Action<double> callback = input => OrderView.TranslationX = input;
                OrderView.Animate("anim", callback, 0, 170, 16, 300, Easing.CubicOut);
                Context.ImagenBotonOrden = "bag.png";
            }
            var escogeTipoCondicion = await Context.escogeTipoCondicion();
            if (escogeTipoCondicion)
            {
                switch (origen)
                {
                    case 1:
                        if (EstadoOrdenView.TranslationX >= 170)
                        {

                            Action<double> callback = input => EstadoOrdenView.TranslationX = input;
                            EstadoOrdenView.Animate("anim", callback, 170, 0, 16, 300, Easing.CubicInOut);
                            animacionProgreso();
                        }
                        if (EstadoOrdenView.TranslationX == 0)
                        {
                            Action<double> callback = input => EstadoOrdenView.TranslationX = input;
                            EstadoOrdenView.Animate("anim", callback, 0, 170, 16, 300, Easing.CubicOut);
                        }
                        break;
                    case 2:
                        //Abro Orden Estado en caso de no estar abierto
                        if (EstadoOrdenView.TranslationX >= 170)
                        {

                            Action<double> callback = input => EstadoOrdenView.TranslationX = input;
                            EstadoOrdenView.Animate("anim", callback, 170, 0, 16, 300, Easing.CubicInOut);                            
                        }
                        animacionProgreso();
                        break;
                }
            }
        }
        /*
        private async void OpenOrdenEstado(object sender, EventArgs e)
        {
            var escogeTipoCondicion = await Context.escogeTipoCondicion();
            if (escogeTipoCondicion)
            {
                Context.IsVisibleEstadoOrden = true;
                Context.IsVisibleDetalleCarrito = false;
                if (EstadoOrdenView.TranslationX >= 170)
                {

                    Action<double> callback = input => EstadoOrdenView.TranslationX = input;
                    EstadoOrdenView.Animate("anim", callback, 170, 0, 16, 300, Easing.CubicInOut);
                }
                if (EstadoOrdenView.TranslationX == 0)
                {
                    Action<double> callback = input => EstadoOrdenView.TranslationX = input;
                    EstadoOrdenView.Animate("anim", callback, 0, 170, 16, 300, Easing.CubicOut);
                }
                animacionProgreso();
            }
        }
        */

        private async void CancelaOrdenEstado(object sender, EventArgs e)
        {
            Context.cancelaTipoCondicion();
        }

        private void Themes_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "BackgroundColor" && stepProgress != null && stepProgress.Children.Count > 0)
            {
                for (int i = 0; i < stepProgress.Children.Count; i++)
                {
                    SetStepViewPrimaryTextColor(stepProgress.Children[i] as StepView);
                }
            }
        }
        void SetStepViewPrimaryTextColor(StepView view)
        {
            if (view.Status == StepStatus.Completed)
            {
                view.PrimaryFormattedText.Spans[0].TextColor = stepProgress.CompletedStepStyle.FontColor;
                view.PrimaryFormattedText.Spans[3].TextColor = stepProgress.CompletedStepStyle.FontColor;
                view.PrimaryFormattedText.Spans[5].TextColor = stepProgress.CompletedStepStyle.FontColor;
            }

            if (view.Status == StepStatus.NotStarted)
            {
                view.PrimaryFormattedText.Spans[0].TextColor = Color.FromHex("#6E6E6E");
                view.PrimaryFormattedText.Spans[3].TextColor = Color.Transparent;
                view.PrimaryFormattedText.Spans[5].TextColor = Color.Transparent;
            }
        }
        private void CloseOrder(object sender, EventArgs e)
        {
            if (OrderView.TranslationX == 0)
            {
                Action<double> callback = input => OrderView.TranslationX = input;
                OrderView.Animate("anim", callback, 0, 150, 16, 300, Easing.CubicOut);
            }
        }
        private async void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            /*if (!string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                Context.ScannedText = e.NewTextValue;
                await Context.SearchProductWhenScan();
            }*/
            if (productList.DataSource != null)
            {
                productList.DataSource.Filter = FilterContacts;
                productList.DataSource.RefreshFilter();
            }
            productList.RefreshView();
        }

        private void ProductList_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            CrossVibrate.Current.Vibration(TimeSpan.FromMilliseconds(100));
            if (e.ItemData is Item product)
            {
                ((IngresoVentasViewModel)this.BindingContext).AddItemOrdenCompra(product);
                //Context.ScannedText = string.Empty;
                productList.SelectedItem = null;
            }
        }
        private void Category_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            /*if (((FilterPicker)sender).SelectedItem is Category selectedCategory)
            {
                Context.SelectedCategory = selectedCategory;
                Task.Run(async () =>
                {
                    await Context.SearchProduct();
                });
            }*/
        }

        public async void ScanBarcode(object sender, EventArgs e)
        {
            var camStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<CameraPermission>();
            if (camStatus != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera))
                {
                    // await Context.ShowCamErrorDialog();
                }

                camStatus = await CrossPermissions.Current.RequestPermissionAsync<CameraPermission>();
            }
            if (camStatus == PermissionStatus.Granted)
                ScanContinuouslyAsync();
        }

        public async void ScanContinuouslyAsync()
        {
            var options = new MobileBarcodeScanningOptions
            {
                AutoRotate = false,
                DisableAutofocus = false,
                //DelayBetweenContinuousScans = 3000
            };
            options.PossibleFormats.Add(BarcodeFormat.QR_CODE);
            //options.PossibleFormats.Add(BarcodeFormat.All_1D);
            options.PossibleFormats.Add(BarcodeFormat.CODE_128);
            options.PossibleFormats.Add(BarcodeFormat.CODE_93);
            options.PossibleFormats.Add(BarcodeFormat.CODE_39);
            options.PossibleFormats.Add(BarcodeFormat.EAN_13);
            options.PossibleFormats.Add(BarcodeFormat.EAN_8);
            options.PossibleFormats.Add(BarcodeFormat.CODE_39);
            //options.PossibleFormats.Add(BarcodeFormat.DATA_MATRIX);
            //options.PossibleFormats.Add(BarcodeFormat.CODABAR);
            //options.PossibleFormats.Add(BarcodeFormat.AZTEC);
            //options.PossibleFormats.Add(BarcodeFormat.IMB);
            //options.PossibleFormats.Add(BarcodeFormat.ITF);
            //options.PossibleFormats.Add(BarcodeFormat.MAXICODE);
            //options.PossibleFormats.Add(BarcodeFormat.MSI);

            var overlay = new ZXingDefaultOverlay
            {
                ShowFlashButton = true,
                BottomText = "Đưa mã vạch vào khung quét...",
                TopText = "Bật/Tắt Flash"
            };

            overlay.BindingContext = overlay;

            var scanPage = new ZXingScannerPage(options, overlay)
            {
                Title = TextsTranslateManager.Translate("ScannerTitle"),
                //DefaultOverlayTopText = "Canh chỉnh mã vạch trong khung"
            };
            scanPage.OnScanResult += (result) =>
            {
                CrossVibrate.Current.Vibration(TimeSpan.FromMilliseconds(100));
                //DependencyService.Get<ISoundService>().PlayAudioFile("scanner_sound.mp3");
                scanPage.IsScanning = false;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                    //Context.ScannedText = result.Text;
                });
            };
            await Navigation.PushAsync(scanPage);

            overlay.FlashButtonClicked += (s, ed) =>
            {
                scanPage.ToggleTorch();
            };
        }

        public async void GotoPurchaseOrder(object sender, EventArgs e)
        {
            /*var purchaseorderInfo = new PurchaseOrderInfo
            {
                ProductInPurchaseOrder = Context.ProductInPurchaseOrders,
                SelectedSupplier = Context.SelectedSupplier
            };
            await Context.GotoPurchaseOrderAsync(purchaseorderInfo);*/
        }

        void Handle_Refreshing(object sender, System.EventArgs e)
        {
            Task.Run(async () =>
            {
                //Context.IsRefreshing = true;
                //await Context.LoadProducts();
                //Context.IsRefreshing = false;
            });
        }
        private void ProductList_SelectionChanged(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems[0] is Item product)
            {
                Context.OpenItemAdjust(product);
                productList.SelectedItem = null;
            }
        }

        private void OpenDetalleOrden(object sender, EventArgs e)
        {
            var tappedEvgs = e as TappedEventArgs;
            var data = (OrdenDetalleCompra)tappedEvgs.Parameter;
            if (data != null)
            {
                Context.OpenDetalleOrdenAsync(1,data);
            }
        }


        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchTerm = e.NewTextValue;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = string.Empty;
            }

            searchTerm = searchTerm.ToLowerInvariant();

            var filteredItems = Context.OrdenSeleccionado.OrdenDetalleCompras.Where(x => x.NombreItem.ToLowerInvariant().Contains(searchTerm)).ToList();

            /*foreach (var value in Context.OrdenSeleccionado.OrdenDetalleCompras)
            {
                if (!filteredItems.Contains(value))
                {
                    MyItems.Remove(value);
                }
                else if (!MyItems.Contains(value))
                {
                    MyItems.Add(value);
                }
            }*/
        }

        void animacionProgreso()
        {
            StepView stepView;
            int step = Convert.ToInt32(Context.OrdenSeleccionado.IdEstado);
            stepProgress.ProgressAnimationDuration = 1;
            for (int i = 0; i < stepProgress.Children.Count; i++)
            {
                stepProgress.ProgressAnimationDuration = 1000;
                stepView = stepProgress.Children[i] as StepView;
                if (stepView.Status == StepStatus.Completed)
                {
                    stepView.PrimaryFormattedText.Spans[0].TextColor = stepProgress.CompletedStepStyle.FontColor;
                    stepView.PrimaryFormattedText.Spans[3].TextColor = stepProgress.CompletedStepStyle.FontColor;
                    stepView.PrimaryFormattedText.Spans[5].TextColor = stepProgress.CompletedStepStyle.FontColor;
                }

                if (stepView.Status == StepStatus.NotStarted)
                {
                    stepView.PrimaryFormattedText.Spans[0].TextColor = Color.FromHex("#6E6E6E");
                    stepView.PrimaryFormattedText.Spans[3].TextColor = Color.Transparent;
                    stepView.PrimaryFormattedText.Spans[5].TextColor = Color.Transparent;
                }
            }

            //coloresProgreso();
        }

        void coloresProgreso()
        {
            if (stepProgress != null && stepProgress.Children.Count > 0)
            {
                for (int i = 0; i < stepProgress.Children.Count; i++)
                {
                    SetStepViewPrimaryTextColor(stepProgress.Children[i] as StepView);
                }
            }
        }

        private bool FilterContacts(object obj)
        {
            if (barraBusqueda == null || barraBusqueda.Text == null)
                return true;

            var taskInfo = obj as Item;
            return (taskInfo.Nombre.ToLower().Contains(barraBusqueda.Text.ToLower())
                || taskInfo.Unidad.ToLower().Contains(barraBusqueda.Text.ToLower())
                || taskInfo.Marca.ToLower().Contains(barraBusqueda.Text.ToLower())
                || taskInfo.Categoria.ToLower().Contains(barraBusqueda.Text.ToLower())
                || ConvertidoServicio(taskInfo.EsServicio).Contains(barraBusqueda.Text.ToLower())
                );
        }

        private string ConvertidoServicio(int valor)
        {
            return valor==1 ? "SERVICIO" : "ITEM";
        }

        #endregion 4. Métodos Personalizados FrontEnd

    }
}