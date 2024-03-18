using DataModel.DTO;
using DataModel.DTO.Productos;
using ManijodaServicios.Resources.Texts;
using Escant_App.Interfaces;
using Escant_App.ViewModels.Inventario;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Plugin.Vibrate;
using Syncfusion.SfNumericUpDown.XForms;
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

namespace Escant_App.Views.Inventario
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetalleInventarioView : ContentPage
    {

        #region 0. Declaración de Variables

        #endregion 0. Declaración de Variables

        #region 1. Vinculación View con ViewModel
        public DetalleInventarioViewModel Context => (DetalleInventarioViewModel)this.BindingContext;
        #endregion 1. Vinculación View con ViewModel

        #region 2. Constructor
        public DetalleInventarioView()
        {
            try { 
            InitializeComponent();
            Context.ImagenBotonOrden = "bag.png";            
            Context.AbreOrdenEstado = ((obj) =>
            {
                abreOrdenEstado(1);
            });
            Context.SeteaNotificacion = ((obj) =>
            {
                SeteaValorNotificacion();
            });
                MessagingCenter.Unsubscribe<string>(this, Escant_App.AppSettings.Settings.ZXingRequestCameraFailed);
            MessagingCenter.Subscribe<string>(this, Escant_App.AppSettings.Settings.ZXingRequestCameraFailed, (arg) =>
            {
                Navigation.PopToRootAsync();
            });
            }
            catch(Exception ex)
            {

            }

        }
        #endregion 2. Constructor

        #region 3. Métodos Propios FrontEnd        
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            //await Context.InitializeAsync(null);
        }
        #endregion 3. Métodos Propios FrontEnd

        #region 4. Métodos Personalizados FrontEnd
               

        private void OpenOrdenEstado(object sender, EventArgs e)
        {
            var param = 1;
            //Context.IsVisibleEstadoOrden = false;                       
            abreOrdenEstado(param);
        }
        private void abreOrdenEstado(int origen)
        {
            Context.IsVisibleEstadoOrden = true;
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
                        animacionProgreso();
                    }
                    break;
            }
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
        
        private async void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (registroListView.DataSource != null)
            {
                registroListView.DataSource.Filter = FilterContacts;
                registroListView.DataSource.RefreshFilter();
            }
            registroListView.RefreshView();
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
                ScanAsync();            


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
                //await Context.LoadData();
            });
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
                || taskInfo.Marca.ToLower().Contains(barraBusqueda.Text.ToLower()));
        }

        #endregion 4. Métodos Personalizados FrontEnd







        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            CrossVibrate.Current.Vibration(TimeSpan.FromMilliseconds(100));
        }
        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            
        }

        public async void ScanAsync()
        {
            var options = new MobileBarcodeScanningOptions
            {
                AutoRotate = false,
                //TryInverted = true,
                TryHarder = true,
                DisableAutofocus = false,
                PossibleFormats = new List<BarcodeFormat>()
            };
            options.PossibleFormats.Add(BarcodeFormat.QR_CODE);
            //options.PossibleFormats.Add(BarcodeFormat.All_1D);
            options.PossibleFormats.Add(BarcodeFormat.CODE_128);
            options.PossibleFormats.Add(BarcodeFormat.CODE_93);
            options.PossibleFormats.Add(BarcodeFormat.CODE_39);
            options.PossibleFormats.Add(BarcodeFormat.EAN_13);
            options.PossibleFormats.Add(BarcodeFormat.EAN_8);
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
                //DefaultOverlayTopText = "Canh chỉnh mã vạch trong khung",
                //DefaultOverlayBottomText = string.Empty,
                Title = TextsTranslateManager.Translate("ScannerTitle"),
            };

            scanPage.OnScanResult += (result) => {
                // Play scanner sound
                //DependencyService.Get<ISoundService>().PlayAudioFile("scanner_sound.mp3");
                CrossVibrate.Current.Vibration(TimeSpan.FromMilliseconds(100));

                // Stop scanning
                scanPage.IsScanning = false;
                
            };
            overlay.FlashButtonClicked += (s, ed) =>
            {
                scanPage.ToggleTorch();
            };

            // Navigate to our scanner page
            await Navigation.PushAsync(scanPage);
        }

        private void Handle_SelectionChanged(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            var pages = Navigation.NavigationStack.ToList();
            var previousPage = pages.ElementAt(pages.Count - 2);
            
        }

        private void Handle_ValueChanged(object sender, Syncfusion.SfNumericUpDown.XForms.ValueEventArgs e)
        {            
            var IdItem=((SfNumericUpDown)sender).ClassId;
            var item = Context.Registros.Where(x => x.Id == IdItem).FirstOrDefault();
            if (!string.IsNullOrEmpty(e.Value.ToString()))
                {
                
                Context.AddItemOrdenCompra(item);                
            }
        }

        private void SeteaValorNotificacion()
        {
            if (ToolbarItems.Count > 0)
            {
                var cantidad = String.Format("{0:0}", Context.CantidadOrden);
                DependencyService.Get<IToolbarItemBadgeService>().SetBadge(this, ToolbarItems.First(), cantidad.ToString(), Color.Red, Color.White);
            }
        }

        private void registroListView_SelectionChanging(object sender, Syncfusion.ListView.XForms.ItemSelectionChangingEventArgs e)
        {

        }

        private void registroListView_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {

        }
    }
}