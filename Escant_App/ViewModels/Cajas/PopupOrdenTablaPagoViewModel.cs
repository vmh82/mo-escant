using DataModel.DTO.Administracion;
using DataModel.DTO.Compras;
using ManijodaServicios.AppSettings;
using ManijodaServicios.Offline.Interfaz;
using Escant_App.ViewModels.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Google.Api;
using Xamarin.CommunityToolkit.ObjectModel.Extensions;
using Syncfusion.Data.Extensions;
using System.Diagnostics.SymbolStore;
using System.Collections;

namespace Escant_App.ViewModels.Cajas
{
    public class PopupOrdenTablaPagoViewModel:ViewModelBase
    {
        #region 1. Declaración de Variables
        #region 1.1 Variables de Integración
        private Generales.Generales generales;
        #endregion 1.1 Variables de Integración
        #region 1.2 Variables de Instancia de Objetos para la Clase                
        private Orden _ordenSeleccionado;        
        #endregion 1.2 Variables de Instancia de Objetos para la Clase        
        #region 1.4 Variables de Control de Datos
        private float _valorTotal;
        private float _valorFinal;                
        private float _valorDescuento;
        private float _valorIncremento;
        private bool _esApertura;
        private ObservableCollection<TablaPago> _tablaPagoSeleccionado;
        #endregion 1.4 Variables de Control de Datos
        #endregion 1. Declaración de Variables
        #region 2.InstanciaAsignaVariablesControles
        public Orden OrdenSeleccionado
        {
            get => _ordenSeleccionado;
            set
            {
                _ordenSeleccionado = value;
                RaisePropertyChanged(() => OrdenSeleccionado);
            }

        }                
        public float ValorTotal
        {
            get => _valorTotal;
            set
            {
                _valorTotal = value;
                RaisePropertyChanged(() => ValorTotal);
            }

        }
        public float ValorFinal
        {
            get => _valorFinal;
            set
            {
                _valorFinal = value;
                RaisePropertyChanged(() => ValorFinal);
            }

        }                
        public float ValorDescuento
        {
            get => _valorDescuento;
            set
            {
                _valorDescuento = value;
                RaisePropertyChanged(() => ValorDescuento);
            }

        }
        public float ValorIncremento
        {
            get => _valorIncremento;
            set
            {
                _valorIncremento = value;
                RaisePropertyChanged(() => ValorIncremento);
            }

        }
        public bool EsApertura
        {
            get => _esApertura;
            set
            {
                _esApertura = value;
                RaisePropertyChanged(() => EsApertura);
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
        #endregion 2.InstanciaAsignaVariablesControles

        #region 3.Constructor
        public PopupOrdenTablaPagoViewModel(IServicioSeguridad_Usuario seguridadServicio)
        {
            //Fijo en cada Model
            BindingContext = new ConectividadModelo(seguridadServicio);

            generales = new Generales.Generales();
        }
        #endregion 3.Constructor
        #region 4. Definición de Métodos        
        #endregion 4. Definición de Métodos
        #region 5.Métodos de Administración de Datos
        /// <summary>
        /// Método inicial que recepta parámetro Orden
        /// </summary>
        /// <param name="navigationData"></param>
        /// <returns></returns>
        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            if (navigationData is Orden item && item != null)
            {
                OrdenSeleccionado = item;                                
                var perfil = SettingsOnline.oAplicacion.PerfilConectado.ToUpper();
                var estadoEliminar = "";
                switch (perfil)
                {
                    case string a when perfil.Contains("SITANTE"):
                    case string b when perfil.Contains("IENTE"):
                        estadoEliminar = "ENDIENTE";
                        TablaPagoSeleccionado = !string.IsNullOrEmpty(OrdenSeleccionado.OrdenTablaPagoSerializado) ? JsonConvert.DeserializeObject<ObservableCollection<TablaPago>>(OrdenSeleccionado.OrdenTablaPagoSerializado) : new ObservableCollection<TablaPago>();
                        TablaPagoSeleccionado.ForEach(x => x.Seleccionada = 0);
                        while (TablaPagoSeleccionado.Where(x => !x.EstadoCuota.ToUpper().Contains(estadoEliminar)).Count() > 0)
                        {
                            TablaPagoSeleccionado.Remove(TablaPagoSeleccionado.Where(x => !x.EstadoCuota.ToUpper().Contains(estadoEliminar)).FirstOrDefault());
                        }
                        EsApertura = false;
                        break;                        
                    default:
                        estadoEliminar = "ERIFICAR";
                        TablaPagoSeleccionado = !string.IsNullOrEmpty(OrdenSeleccionado.OrdenTablaPagoSerializado) ? JsonConvert.DeserializeObject<ObservableCollection<TablaPago>>(OrdenSeleccionado.OrdenTablaPagoSerializado) : new ObservableCollection<TablaPago>();
                        while (TablaPagoSeleccionado.Where(x => !x.EstadoCuota.ToUpper().Contains(estadoEliminar)).Count() > 0)
                        {
                            TablaPagoSeleccionado.Remove(TablaPagoSeleccionado.Where(x => !x.EstadoCuota.ToUpper().Contains(estadoEliminar)).FirstOrDefault());
                        }
                        EsApertura = true;
                        break;
                }
                calculaSumatorias();

            }
            IsBusy = false;
        }        
        #endregion 5.Métodos de Administración de Datos
        #region 6.Métodos Generales
        /// <summary>
        /// Método que calcula los valores del grid posterior al evento de selección o deselección de registros
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="valorNumeroCuota"></param>
        /// <returns></returns>
        public bool calculaValores(int tipo,int valorNumeroCuota)
        {
            bool retorno = true;            
            int numeroCuota = Convert.ToInt32(valorNumeroCuota);
            var cuotaAnterior = TablaPagoSeleccionado.Where(x => x.NumeroCuota == numeroCuota - 1).FirstOrDefault();
            var cuotaActual = TablaPagoSeleccionado.Where(x => x.NumeroCuota == numeroCuota).FirstOrDefault();
            var cuotaSiguiente = TablaPagoSeleccionado.Where(x => x.NumeroCuota == numeroCuota + 1).FirstOrDefault();
            if ((cuotaAnterior!=null && cuotaAnterior.Seleccionada == 1)
                //|| (numeroCuota-1==0)
                || (numeroCuota == TablaPagoSeleccionado.FirstOrDefault().NumeroCuota)
                )
            {
                if (cuotaSiguiente!=null && cuotaSiguiente.Seleccionada == 1 && cuotaActual.Seleccionada == 0)
                {
                    TablaPagoSeleccionado.Where(x => x.NumeroCuota == numeroCuota).FirstOrDefault().Seleccionada = 1;
                    retorno = false;
                }
                else
                {
                    _valorFinal = 0;
                }
            }
            else
            {
                TablaPagoSeleccionado.Where(x => x.NumeroCuota == numeroCuota).FirstOrDefault().Seleccionada = 0;                       
                retorno = false;
            }           
            calculaSumatorias();
            return retorno;
        }
        /// <summary>
        /// Método para calcular y mostrar las sumatorias
        /// </summary>
        private void calculaSumatorias()
        {
            float _valorTotal = 0;
            float _valorFinal = 0;
            float _valorDescuento = 0;
            float _valorIncremento = 0;

            foreach (var item in TablaPagoSeleccionado)
            {

                if (item.Seleccionada == 1)
                {
                    _valorTotal += item.ValorCompra;
                    _valorDescuento += item.ValorDescuento;
                    _valorIncremento += item.ValorIncremento;
                    _valorFinal += item.ValorFinal;
                }

            }
            ValorTotal = _valorTotal;
            ValorFinal = _valorFinal;
            ValorDescuento = _valorDescuento;
            ValorIncremento = _valorIncremento;
        }
                 
        public void AlmacenaDatos()
        {
            var perfil = SettingsOnline.oAplicacion.PerfilConectado.ToUpper();
            var ordenTablaPago= !string.IsNullOrEmpty(OrdenSeleccionado.OrdenTablaPagoSerializado) ? JsonConvert.DeserializeObject<ObservableCollection<TablaPago>>(OrdenSeleccionado.OrdenTablaPagoSerializado) : new ObservableCollection<TablaPago>();
            ordenTablaPago.ForEach(x => x.Seleccionada = 0);
            switch (perfil)
            {
                case string a when perfil.Contains("SITANTE"):
                case string b when perfil.Contains("IENTE"):
                    //ordenTablaPago.Where(x=>OrdenSeleccionado.OrdenTablaPago.Where(y=>y.))                    
                    /*var iguales = (ordenTablaPago.Where(b1 => TablaPagoSeleccionado.Any(bi => bi.Id == b1.Id
                                                                                      && bi.Seleccionada == 1)).ToList());
                    iguales.ForEach(b1 => b1.Seleccionada = 1);*/

                    (from k in ordenTablaPago
                     where TablaPagoSeleccionado.Any(bi => bi.IdOrden == k.IdOrden
                                                  && bi.NumeroCuota==k.NumeroCuota
                                                  && bi.Seleccionada == 1)
                     select k).ToList().ForEach(b2 => b2.Seleccionada = 1);
                    //TablaPagoSeleccionado.ForEach(b1 => ordenTablaPago.First(orig => orig.Id == b1.Id).Seleccionada = b1.Seleccionada);
                    break;
                default :                    
                    (from k in ordenTablaPago
                     where TablaPagoSeleccionado.Any(bi => bi.IdOrden == k.IdOrden
                                                  && bi.NumeroCuota == k.NumeroCuota
                                                  && bi.Seleccionada == 1)
                     select k).ToList().ForEach(b2 => b2.Seleccionada = 1);                    
                    break;
            }
            OrdenSeleccionado.OrdenTablaPagoSerializado = JsonConvert.SerializeObject(ordenTablaPago);
        }

        #endregion 6.Métodos Generales
    }
}
