

using Escant_App.Services;
using Escant_App.Services.Impl;
using Escant_App.ViewModels.Configuracion;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using TinyIoC;
using Xamarin.Forms;
using DataModel.Infraestructura.Offline.DB;
using DataModel.Infraestructura.Offline.Interfaces;
using ManijodaServicios.Offline.Interfaz;
using ManijodaServicios.Offline.Implementa;
using Escant_App.ViewModels.Administracion;
using static ManijodaServicios.Offline.Implementa.ServicioIteraccion;
using Escant_App.ViewModels.Clientes;
using Escant_App.ViewModels.Iteraccion;
using Escant_App.ViewModels.Seguridad;
using Escant_App.ViewModels.GestionBDD;
using static DataModel.Infraestructura.Offline.Interfaces.ProductosRepositorio;
using Escant_App.ViewModels.Productos;
using Escant_App.ViewModels.Compras;
using ManijodaServicios.Interfaces;
using Escant_App.ViewModels.Documentos;
using Escant_App.ViewModels.Ventas;
using Escant_App.ViewModels.Cajas;
using Escant_App.ViewModels.Inventario;
using Escant_App.ViewModels.Principal;

namespace Escant_App.ViewModels.Base
{
    public static class ViewModelLocator
    {
        private static TinyIoCContainer _container;

        public static readonly BindableProperty AutoWireViewModelProperty =
            BindableProperty.CreateAttached("AutoWireViewModel", typeof(bool), typeof(ViewModelLocator), default(bool), propertyChanged: OnAutoWireViewModelChanged);

        public static bool GetAutoWireViewModel(BindableObject bindable)
        {
            return (bool)bindable.GetValue(ViewModelLocator.AutoWireViewModelProperty);
        }

        public static void SetAutoWireViewModel(BindableObject bindable, bool value)
        {
            bindable.SetValue(ViewModelLocator.AutoWireViewModelProperty, value);
        }

        static ViewModelLocator()
        {
            _container = new TinyIoCContainer();

            // View models - by default, TinyIoC will register concrete classes as multi-instance.            
            _container.Register<LoginViewModel>();
            _container.Register<OrdenMaestroCompraViewModel>();
            _container.Register<ManageDatabaseViewModel>();
            _container.Register<StoreSettingViewModel>();
            _container.Register<NavigationViewModel>();
            _container.Register<AppSettingsViewModel>();
            _container.Register<ReceiptPrinterViewModel>();
            _container.Register<MasterViewModel>();
            _container.Register<PrecioViewModel>();
            _container.Register<PopupAddPrecioItemViewModel>();
            _container.Register<ItemViewModel>();
            _container.Register<AddItemViewModel>();
            _container.Register<ComprobanteSettingViewModel>();
            _container.Register<CatalogoViewModel>();
            _container.Register<PopupAddCatalogoViewModel>();
            _container.Register<EmpresaViewModel>();
            _container.Register<AddEmpresaViewModel>();
            _container.Register<ClienteViewModel>();
            _container.Register<AddClienteViewModel>();
            _container.Register<GaleriaViewModel>();
            _container.Register<PaletaColoresViewModel>();
            _container.Register<UsuarioViewModel>();
            _container.Register<AddUsuarioViewModel>();
            _container.Register<PerfilMenuViewModel>();
            _container.Register<ComprasViewModel>();
            _container.Register<IngresoComprasViewModel>();            
            _container.Register<PopupActualizaItemOrdenComprasViewModel>();            
            _container.Register<PopupReceptaItemOrdenComprasViewModel>();            
            _container.Register<PopupActualizaOrdenComprasViewModel>();            
            _container.Register<SecuencialesSettingViewModel>();
            _container.Register<PdfViewerViewModel>();            
            _container.Register<VentasViewModel>();
            _container.Register<OrdenMaestroVentaViewModel>();
            _container.Register<IngresoVentasViewModel>();
            _container.Register<PopupActualizaOrdenVentasViewModel>();
            _container.Register<PopupDetalleOrdenViewModel>();
            _container.Register<PopupValidaViewModel>();
            _container.Register<PopupOrdenTablaPagoViewModel>();
            _container.Register<DetalleInventarioViewModel>();
            _container.Register<PopupActualizaDetalleInventarioViewModel>();
            _container.Register<PopupActualizaOrdenInventarioViewModel>();
            _container.Register<PopupDistribuyeItemOrdenInventarioViewModel>();
            _container.Register<PrincipalViewModel>();
            _container.Register<PasswordOlvidadoViewModel>();


            _container.Register<IServicioFirestore>(DependencyService.Get<IServicioFirestore>());
            //DbContext
            _container.Register<DbContext>();

            //Repositories
            _container.Register<IEmpresaRepositorio, EmpresaRepositorio>();
            _container.Register<ICatalogoRepositorio, CatalogoRepositorio>();

            _container.Register<IClienteRepositorio, ClienteRepositorio>();
            _container.Register<IPersonaRepositorio, PersonaRepositorio>();

            _container.Register<ISeguridadRepositorio, SeguridadRepositorio>();            
            _container.Register<ISettingRepository, SettingRepository>();
            _container.Register<IItemRepositorio, ItemRepositorio>();
            _container.Register<IPrecioRepositorio, PrecioRepositorio>();
            
            _container.Register<IProveedorRepositorio, ProveedorRepositorio>();
            _container.Register<IGaleriaRepositorio, GaleriaRepositorio>();
            _container.Register<ITablasBddRepositorio, TablasBddRepositorio>();

            _container.Register<IPerfilMenuRepositorio, PerfilMenuRepositorio>();

            _container.Register<IOrdenRepositorio, OrdenRepositorio>();
            _container.Register<IOrdenDetalleCompraRepositorio, OrdenDetalleCompraRepositorio>();
            _container.Register<IOrdenEstadoRepositorio, OrdenEstadoRepositorio>();

            _container.Register<IStockResumenRepositorio, StockResumenRepositorio>();

            // Services - by default, TinyIoC will register interface registrations as singletons.            
            _container.Register<IDialogService, DialogService>();

            _container.Register<IServicioAdministracion_Empresa, ServicioAdministracion_Empresa>();
            _container.Register<IServicioAdministracion_Catalogo, ServicioAdministracion_Catalogo>();            
            _container.Register<IServicioAdministracion_Proveedor, ServicioAdministracion_Proveedor>();

            _container.Register<IServicioClientes_Cliente, ServicioClientes_Cliente>();
            _container.Register<IServicioClientes_Persona, ServicioClientes_Persona>();

            _container.Register<IServicioIteraccion_Galeria, ServicioIteraccion_Galeria>();
            _container.Register<IServicioIteraccion_TablasBdd, ServicioIteraccion_TablasBdd>();

            _container.Register<IServicioSeguridad_Usuario, ServicioSeguridad_Usuario>();
            _container.Register<IAuthenticationService, AuthenticationService>();
            _container.Register<ISettingsService, SettingsService>();
            _container.Register<INavigationService, NavigationService>();            
            //_container.Register<IServicioAdministracion, CategoryService>();
            _container.Register<IRequestService, RequestService>();                        

            _container.Register<IServicioSeguridad_PerfilMenu, ServicioSeguridad_PerfilMenu>();
            _container.Register<IServicioProductos_Item, ServicioProductos_Item>();
            _container.Register<IServicioProductos_Precio, ServicioProductos_Precio>();

            _container.Register<IServicioCompras_Orden, ServicioCompras_Orden>();
            _container.Register<IServicioCompras_OrdenDetalleCompra, ServicioCompras_OrdenDetalleCompra>();
            _container.Register<IServicioCompras_OrdenEstado, ServicioCompras_OrdenEstado>();

            _container.Register<IServicioInventario_StockResumen, ServicioInventario_StockResumen>();

        }

        private static Type FindViewModel(Type viewType)
        {
            string viewName = string.Empty;

            if (viewType.FullName.EndsWith("Page"))
            {
                viewName = viewType.FullName
                    .Replace("Page", string.Empty)
                    .Replace("Views", "ViewModels");
            }

            var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
            var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}ViewModel, {1}", viewName, viewAssemblyName);

            return Type.GetType(viewModelName);
        }
        public static void UpdateDependencies(bool useMockServices)
        {
        }

        public static void RegisterSingleton<TInterface, T>() where TInterface : class where T : class, TInterface
        {
            _container.Register<TInterface, T>().AsSingleton();
        }

        public static T Resolve<T>() where T : class
        {
            return _container.Resolve<T>();
        }

        private static void OnAutoWireViewModelChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as Element;
            if (view == null)
            {
                return;
            }

            var viewType = view.GetType();
            var viewName = viewType.FullName.Replace(".Views.", ".ViewModels.");
            var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
            var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}Model, {1}", viewName, viewAssemblyName);

            var viewModelType = Type.GetType(viewModelName);
            if (viewModelType == null)
            {
                return;
            }
            var viewModel = _container.Resolve(viewModelType);
            view.BindingContext = viewModel;
        }
    }
}
