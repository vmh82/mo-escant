using DataModel;
using DataModel.DTO;
using DataModel.DTO.Administracion;
using DataModel.DTO.Clientes;
using DataModel.DTO.Iteraccion;
using DataModel.DTO.Productos;
using ManijodaServicios.Resources.Texts;
using Escant_App.ViewModels;
using Escant_App.Views;
using Escant_App.Views.Configuracion;
using Escant_App.Views.GestionBDD;
using Escant_App.Views.Seguridad;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Escant_App.AppSettings
{
    public static class Settings
    {
        
        private static ISettings AppSettings => CrossSettings.Current;        
        public static Empresa DefaultEmpresa => new Empresa { Descripcion = TextsTranslateManager.Translate("AllEmpresa"), Id = null };
        public static Item DefaultItem => new Item { Descripcion = TextsTranslateManager.Translate("AllItem"), Id = null };
        public static Cliente DefaultCliente => new Cliente { Nombre = TextsTranslateManager.Translate("AllCustomers"), Id = null };
        public static Catalogo DefaultCatalogo => new Catalogo { Nombre = TextsTranslateManager.Translate("AllCatalogo"), Id = null };
        public static ColorAplicacion DefaultColor => new ColorAplicacion { Nombre = TextsTranslateManager.Translate("AllColor"), Id = null };
        public static FuenteIconos DefaultFuenteIconos => new FuenteIconos { Fuente = TextsTranslateManager.Translate("AllFuenteIconos"),  Id = null };        

        #region MessagingCenter keys
        public static string SelectedUnitMainView = "SelectedUnitMainView";
        public static string SelectedUnitStockInView = "SelectedUnitStockInView";
        public static string SelectedUnitInventoryView = "SelectedUnitInventoryView";
        public static string SelectedUnitProductsView = "SelectedUnitProductsView";
        public static string SelectedUnitAddProductView = "SelectedUnitAddProductView";
        public static string SelectedCategoryMainView = "SelectedCategoryMainView";
        public static string SelectedCategoryStockInView = "SelectedCategoryStockInView";
        public static string SelectedCategoryInventoryView = "SelectedCategoryInventoryView";
        public static string SelectedCategoryProductsView = "SelectedCategoryProductsView";
        public static string SelectedCategoryAddProductView = "SelectedCategoryAddProductView";
        public static string SelectedCustomerMainView = "SelectedCustomerMainView";
        public static string SelectedCustomerSaleOrderView = "SelectedCustomerSaleOrderView";
        public static string SelectedCustomerProductsView = "SelectedCustomerProductsView";
        //public static string SelectedSupplierProductView = "SelectedSupplierProductView";
        public static string SelectedSupplierAddProductView = "SelectedSupplierAddProductView";
        public static string SelectedSupplierStockInView = "SelectedSupplierStockInView";
        public static string SelectedSupplierPurchaseOrderView = "SelectedSupplierPurchaseOrderView";
        public static string SelectedSupplierProductsView = "SelectedSupplierProductsView";
        public static string PurchaseOrderItemsChanged = "PurchaseOrderItemsChanged";
        public static string SaleOrderItemsChanged = "SaleOrderItemsChanged";
        public static string EditedProduct = "EditedProduct";
        public static string ZXingRequestCameraFailed = "ZXingRequestCameraFailed";
        public static string SyncImmediately = "SyncImmediately";
        public static string TimerRequestStart = "TimerRequestStart";
        public static string TimerRequestStop = "TimerRequestStop";
        public static string ImageDonwloadCompleted = "ImageDonwloadCompleted";

        public static string EmpresaSeleccionadaAddItemView = "EmpresaSeleccionadaAddItemView";
        public static string EmpresaSeleccionadaStockInView = "EmpresaSeleccionadaStockInView";
        public static string EmpresaSeleccionadaAddClienteView = "EmpresaSeleccionadaAddClienteView";
        public static string EmpresaSeleccionadaIngresoComprasView = "EmpresaSeleccionadaIngresoComprasView";
        public static string CatalogoSeleccionadoAddEmpresaView = "CatalogoSeleccionadoAddEmpresaView";

        public static string CatalogoSeleccionadoGeneroAddClienteView = "CatalogoSeleccionadoGeneroAddClienteView";

        public static string CatalogoSeleccionadoPerfilAddUsuarioView = "CatalogoSeleccionadoPerfilAddUsuarioView";
        public static string CatalogoSeleccionadoAddItemView = "CatalogoSeleccionadoAddItemView";
        public static string CatalogoSeleccionadoAddGestionView = "CatalogoSeleccionadoAddGestionView";

        public static string ClienteSeleccionadaStockInView = "ClienteSeleccionadoStockInView";
        public static string ClienteSeleccionadaIngresoVentasView = "ClienteSeleccionadaIngresoVentasView";

        public static string ColorSeleccionadoPopupAddCatalogoView = "ColorSeleccionadoPopupAddCatalogoView";
        public static string ColorSeleccionadoGaleriaView = "ColorSeleccionadoGaleriaView";

        public static string FuenteIconosSeleccionadoAddCatalogoView = "FuenteIconosSeleccionadoAddCatalogoView";

        public static string ItemSeleccionadaStockInView = "ItemSeleccionadaStockInView";

        public static string PdfViewerIngresoComprasView = "PdfViewerIngresoComprasView";
        public static string PdfViewerIngresaVentasView = "PdfViewerIngresaVentasView";
        #endregion

        public static List<OrganizationUnitViewModel> OrganizationUnits => new List<OrganizationUnitViewModel>
        {
            new OrganizationUnitViewModel {Id = 1, Name = TextsTranslateManager.Translate("Person")},
            new OrganizationUnitViewModel {Id = 2, Name = TextsTranslateManager.Translate("Group")}
        };

        public static List<GenderViewModel> Genders => new List<GenderViewModel>
        {
            new GenderViewModel { Id = 1, Name = TextsTranslateManager.Translate("Male")},
            new GenderViewModel { Id = 2, Name = TextsTranslateManager.Translate("Female")}
        };
        public static List<DiscountRateViewModel> DiscountRates = new List<DiscountRateViewModel>
        {   
            new DiscountRateViewModel { DiscountRate = 0, Name = "0%" },
            new DiscountRateViewModel { DiscountRate = 5, Name = "5%" },
            new DiscountRateViewModel { DiscountRate = 10, Name = "10%"},
            new DiscountRateViewModel { DiscountRate = 15, Name = "15%"},
            new DiscountRateViewModel { DiscountRate = 20, Name = "20%"}
        };
        public static List<SelectItem> Languages = new List<SelectItem>
        {
            new SelectItem { Code ="en", Name ="English"},
            new SelectItem { Code ="es", Name ="Spanish"},
            new SelectItem { Code ="pt", Name ="Portuguese"},
            new SelectItem { Code ="hi", Name ="Bangla"},
            new SelectItem { Code ="fr", Name ="Franch"},
            new SelectItem { Code ="vi", Name ="Vietnamese"}
        };
        public static string DefaultLanguage = "es-EC";
        public static List<SelectItem> TipoAmbiente = new List<SelectItem>
        {
            new SelectItem { Code ="1", Name ="Pruebas"},
            new SelectItem { Code ="2", Name ="Producción"}
        };
        public static string DefaultTipoAmbiente = "1";
       
        public static bool IsFirstTimeLoggedIn
        {
            get => AppSettings.GetValueOrDefault(nameof(IsFirstTimeLoggedIn), true);
            set => AppSettings.AddOrUpdateValue(nameof(IsFirstTimeLoggedIn), value);
        }
        public static bool IsUseDefaultCurrencyFormat
        {
            get => AppSettings.GetValueOrDefault(nameof(IsUseDefaultCurrencyFormat), true);
            set => AppSettings.AddOrUpdateValue(nameof(IsUseDefaultCurrencyFormat), value);
        }
        public static string CurrentCurrencyCustomFormat
        {
            get => AppSettings.GetValueOrDefault(nameof(CurrentCurrencyCustomFormat), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(CurrentCurrencyCustomFormat), value);
        }
        public static string LanguageSelected
        {
            get => AppSettings.GetValueOrDefault(nameof(LanguageSelected), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(LanguageSelected), value);
        }
        public static string CustomCurrencySymbol
        {
            get => AppSettings.GetValueOrDefault(nameof(CustomCurrencySymbol), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(CustomCurrencySymbol), value);
        }
        public static string TipoAmbienteSeleccionado
        {
            get => AppSettings.GetValueOrDefault(nameof(TipoAmbienteSeleccionado), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(TipoAmbienteSeleccionado), value);
        }
        public static string AutoridadCertificaSeleccionado
        {
            get => AppSettings.GetValueOrDefault(nameof(AutoridadCertificaSeleccionado), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(AutoridadCertificaSeleccionado), value);
        }
        public static string TokenSeleccionado
        {
            get => AppSettings.GetValueOrDefault(nameof(TokenSeleccionado), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(TokenSeleccionado), value);
        }
        public static string CertificadoSeleccionado
        {
            get => AppSettings.GetValueOrDefault(nameof(CertificadoSeleccionado), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(CertificadoSeleccionado), value);
        }

        public static Regex MobileRegex => new Regex("^\\+?\\d{0,2}\\-?\\d{4,5}\\-?\\d{5,6}");
        public static Regex EmailRegex => new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        //public static Regex PasswordRegex => new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$");
        public static Regex PasswordRegex => new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$");
        public static Regex NumerosRegex => new Regex(@"^[0-9]+$");
        public static Regex MontosRegex => new Regex(@"^(\d|-)?(\d|,)*\.?\d*$");

    }
}
