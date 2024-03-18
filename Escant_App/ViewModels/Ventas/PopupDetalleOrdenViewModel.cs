using DataModel.DTO.Administracion;
using DataModel.DTO.Clientes;
using DataModel.DTO.Compras;
using DataModel.DTO.Iteraccion;
using DataModel.DTO.Productos;
using Escant_App.Generales;
using Escant_App.ViewModels.Base;
using ManijodaServicios.AppSettings;
using ManijodaServicios.Interfaces;
using ManijodaServicios.Offline.Interfaz;
using ManijodaServicios.Switch;
using Newtonsoft.Json;
using Syncfusion.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escant_App.ViewModels.Ventas
{
    public class PopupDetalleOrdenViewModel : ViewModelBase
    {
        #region 1. Declaración de Variables
        #region 1.1 Variables de Integración    
        private SwitchCompras _switchCompras;
        private Generales.Generales generales = new Generales.Generales();
        private readonly IServicioCompras_Orden _ordenServicio;
        #endregion 1.1 Variables de Integración
        #region 1.2 Variables de Instancia de Objetos para la Clase        
        private Cliente _clienteSeleccionado;
        private Orden _ordenSeleccionado;
        private ObservableCollection<TablaPago> _tablaPagoSeleccionado;
        private ObservableCollection<Orden> _listaOrden;
        #endregion 1.2 Variables de Instancia de Objetos para la Clase
        #region 1.3 Variables de Vinculación de Controles de Vista y Modelo        
        #endregion 1.3 Variables de Vinculación de Controles de Vista y Modelo
        #region 1.4 Variables de Control de Datos        
        #endregion 1.4 Variables de Control de Datos
        #region 1.4 Variables de Control de Errores
        private bool _isVisibleOrden;
        #endregion 1.4 Variables de Control de Errores
        #region 1.6 Acciones        
        #endregion 1.6 Acciones
        #endregion 1. Declaración de Variables

        #region 2.InstanciaAsignaVariablesControles
        public Cliente ClienteSeleccionado
        {
            get => _clienteSeleccionado;
            set
            {
                _clienteSeleccionado = value;
                RaisePropertyChanged(() => ClienteSeleccionado);
            }

        }
        public Orden OrdenSeleccionado
        {
            get => _ordenSeleccionado;
            set
            {
                _ordenSeleccionado = value;
                RaisePropertyChanged(() => OrdenSeleccionado);
            }

        }
        public ObservableCollection<TablaPago> TablaPagoSeleccionado
        {
            get => _tablaPagoSeleccionado;
            set
            {
                _tablaPagoSeleccionado = value;
                RaisePropertyChanged(() => TablaPagoSeleccionado);
            }

        }
        public ObservableCollection<Orden> ListaOrden
        {
            get => _listaOrden;
            set
            {
                _listaOrden = value;
                RaisePropertyChanged(() => ListaOrden);
            }

        }
        public bool IsVisibleOrden
        {
            get => _isVisibleOrden;
            set
            {
                _isVisibleOrden = value;
                RaisePropertyChanged(() => IsVisibleOrden);
            }

        }
        #endregion 2.InstanciaAsignaVariablesControles

        #region 3.Constructor
        /// <summary>
        /// 1. Declaración de constructor
        /// </summary>
        /// <param name="catalogoServicio"></param>
        public PopupDetalleOrdenViewModel(IServicioSeguridad_Usuario seguridadServicio, IServicioCompras_Orden ordenServicio)
        {
            //Fijo en cada Model
            BindingContext = new ConectividadModelo(seguridadServicio);
            //Instanciación Acorde al model
            _ordenServicio = ordenServicio;
            _switchCompras = new SwitchCompras(_ordenServicio);            

        }
        #endregion 3.Constructor       

        #region 5.Métodos de Administración de Datos
        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            try
            {
                if (navigationData is Cliente item && item != null)
                {
                    ClienteSeleccionado = item;                    
                    await ConsultaOrdenes();
                }
            }
            catch (NullReferenceException ex)
            {

            }
            IsBusy = false;
        }
        private async Task ConsultaOrdenes()
        {
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            var registros = await _switchCompras.ConsultaOrdenes(new Orden()
            {
                TipoOrden = "Venta"
            ,
                Identificacion = SettingsOnline.oAplicacion.PerfilConectado.ToUpper().Contains("SITANT") || SettingsOnline.oAplicacion.PerfilConectado.ToUpper().Contains("LIENTE")
                            ? SettingsOnline.oAplicacion.IdentificacionConectado : ClienteSeleccionado.Identificacion
            }, estaConectado);
            if (registros != null && registros.Count > 0)
            {
                IsVisibleOrden = true;
                ListaOrden = generales.ToObservableCollection<Orden>(registros.OrderByDescending(x => Convert.ToInt32(x.NumeroOrden)).ToList());
                ListaOrden.ForEach(x => x.OrdenTablaPago = !string.IsNullOrEmpty(x.OrdenTablaPagoSerializado) ? JsonConvert.DeserializeObject<ObservableCollection<TablaPago>>(x.OrdenTablaPagoSerializado) : new ObservableCollection<TablaPago>());
            }
        }
        #endregion 5.Métodos de Administración de Datos
        public async Task MuestraDetalle()
        {
            TablaPagoSeleccionado = OrdenSeleccionado.OrdenTablaPago;
        }
    }
}
