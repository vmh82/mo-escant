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
using SkiaSharp;
using Syncfusion.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

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


        public async Task ObtenerOrdenesReporte(Cliente cliente)
        {
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            var registros = await _switchCompras.ConsultaOrdenes(new Orden()
            {
                TipoOrden = "Venta"
          ,
                Identificacion = SettingsOnline.oAplicacion.PerfilConectado.ToUpper().Contains("SITANT") || SettingsOnline.oAplicacion.PerfilConectado.ToUpper().Contains("LIENTE")
                          ? SettingsOnline.oAplicacion.IdentificacionConectado : cliente.Identificacion
            }, estaConectado);

            List<string> tablaPagoSerializado = registros.Select(q => q.OrdenTablaPagoSerializado).ToList();
            List<string> ordenSerializado = registros.Select(q => q.OrdenPagoSerializado).ToList();
            List<List<OrdenPago>> tablaOrdenPago = new List<List<OrdenPago>>();
            List<List<TablaPago>> tablaPago = new List<List<TablaPago>>();
            List<List<OrdenDetalleCompraSerializado>> detalleCompra = new List<List<OrdenDetalleCompraSerializado>>();

            List<OrdenDetalleCompraSerializado> lista = new List<OrdenDetalleCompraSerializado>();

            List<string> detalleCompraSerializado = registros.Select(q => q.OrdenDetalleCompraSerializado).ToList();

            foreach (var compra in detalleCompraSerializado)
            {
                detalleCompra.Add(JsonConvert.DeserializeObject<List<OrdenDetalleCompraSerializado>>(compra));
            }

            foreach (var tablaOrdenPagoDeserializado in tablaPagoSerializado)
            {
                tablaPago.Add(JsonConvert.DeserializeObject<List<TablaPago>>(tablaOrdenPagoDeserializado));
            }

            List<List<CompraPago>> comprasPago = new List<List<CompraPago>>();

            foreach (string pagoOrden in ordenSerializado)
            {
               List<OrdenPago> pagosRealizados = JsonConvert.DeserializeObject<List<OrdenPago>>(pagoOrden);
               foreach(var codigoOrden in pagosRealizados)
               {
                   foreach(var itemCompra in detalleCompra)
                   {
                        List<CompraPago> compras = itemCompra.Where(q => q.IdOrden.Equals(codigoOrden.IdOrden)).Select(q => new CompraPago()
                        {
                            NumeroOrden = codigoOrden.NumeroOrden,
                            NombreItem = q.NombreItem,
                            ValorCompra = q.ValorCompra,
                            InstitucionFinanciera = codigoOrden.InstitucionFinanciera,
                            FormaCancelacion = codigoOrden.FormaCancelacion,
                            ValorPago = codigoOrden.ValorPago,
                            EstadoCancelado = codigoOrden.EstadoCancelado,
                            Cuota = codigoOrden.Cuotas
                        }).ToList();
                        comprasPago.Add(compras);
                   }
                
               }
            }


            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ReporteVentas.pdf");
            await CreatePDF(filePath, comprasPago, cliente.Nombre);
            await Launcher.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(filePath)
            });

        }
        #endregion 5.Métodos de Administración de Datos
        public async Task MuestraDetalle()
        {
            TablaPagoSeleccionado = OrdenSeleccionado.OrdenTablaPago;
        }

        public async Task CreatePDF(string filePath, List<List<TablaPago>> tablaPago, string clienteNombre)
        {
            using (SKDocument document = SKDocument.CreatePdf(filePath))
            {
                int rowCount = tablaPago.Count + 1; // Add one for the header row
                int colCount = 8; // For product, price, and quantity

                float cellWidth = 595 / colCount;
                float cellHeight = 842 / rowCount;

                var paint = new SKPaint
                {
                    IsAntialias = true,
                    Style = SKPaintStyle.Fill,
                    Color = SKColors.Black,
                    TextSize = 12,
                    FakeBoldText = true
                };

                string clientName = $"Cliente: {clienteNombre}";
                string reportTitle = "Reporte de Transacciones";

                SKCanvas canvas = document.BeginPage(595, 842);
                float currentY = 0;

                // Draw the client name and report title on the first page
                canvas.DrawText(clientName, 20, 40, paint); // Adjust the position as needed
                canvas.DrawText(reportTitle, 595 / 2, 80, paint); // Adjust the position as needed
                currentY = 120; // Adjust this value based on where you want to start drawing the table

                // Draw the headers
                string[] headers = { "Orden", "Cuota", "Estado", "F. Ven", "Val. Compra", "Impto", "Desct", "A Cancelar" };
                for (int i = 0; i < headers.Length; i++)
                {
                    string header = headers[i];
                    float textWidth = paint.MeasureText(header);
                    float textHeight = paint.FontMetrics.Descent - paint.FontMetrics.Ascent;
                    float x = cellWidth * i + (cellWidth - textWidth) / 2;
                    canvas.DrawText(header, x, currentY + textHeight, paint);
                }

                currentY += cellHeight; // Move to the next row for the data

                // Draw the data
                paint.FakeBoldText = false;
                for (int i = 0; i < tablaPago.Count; i++)
                {
                    for (int k = 0; k < tablaPago[i].Count; k++)
                    {
                        string[] values = { tablaPago[i][k].NumeroOrden, tablaPago[i][k].NumeroCuota.ToString(), tablaPago[i][k].EstadoCuota.ToString(), tablaPago[i][k].FechaVencimiento.ToString(), tablaPago[i][k].ValorCompra.ToString(), tablaPago[i][k].ValorImpuesto.ToString(), tablaPago[i][k].ValorDescuento.ToString(), tablaPago[i][k].ValorFinal.ToString() };

                        for (int j = 0; j < values.Length; j++)
                        {
                            string value = values[j];

                            // If the value is too long, adjust the text size
                            while (paint.MeasureText(value) > cellWidth && paint.TextSize > 0)
                            {
                                paint.TextSize--;
                            }

                            // Calculate the text width and height
                            float textWidth = paint.MeasureText(value);
                            float textHeight = paint.FontMetrics.Descent - paint.FontMetrics.Ascent;

                            // Check if we need to start a new page
                            if (currentY + textHeight > 842)
                            {
                                document.EndPage();
                                canvas = document.BeginPage(595, 842);
                                currentY = 0;
                            }

                            // Draw the text
                            float x = j * cellWidth + (cellWidth - textWidth) / 2;
                            canvas.DrawText(value, x, currentY + textHeight, paint);

                            // Reset the text size for the next cell
                            paint.TextSize = 14;
                        }

                        currentY += cellHeight; // Move to the next row
                    }
                }
                // End the last page
                document.EndPage();
            }
        }

        public async Task CreatePDF(string filePath, List<List<CompraPago>> tablaPago, string clienteNombre)
        {
            using (SKDocument document = SKDocument.CreatePdf(filePath))
            {
                int rowCount = tablaPago.Count + 1; // Add one for the header row
                int colCount = 8; // For product, price, and quantity

                float cellWidth = 595 / colCount;
                float cellHeight = 842 / rowCount;

                var paint = new SKPaint
                {
                    IsAntialias = true,
                    Style = SKPaintStyle.Fill,
                    Color = SKColors.Black,
                    TextSize = 12,
                    FakeBoldText = true
                };

                string clientName = $"Cliente: {clienteNombre}";
                string reportTitle = "Reporte de Transacciones";

                SKCanvas canvas = document.BeginPage(595, 842);
                float currentY = 0;

                // Draw the client name and report title on the first page
                canvas.DrawText(clientName, 20, 40, paint); // Adjust the position as needed
                canvas.DrawText(reportTitle, 595 / 2, 80, paint); // Adjust the position as needed
                currentY = 120; // Adjust this value based on where you want to start drawing the table

                // Draw the headers
                string[] headers = { "Orden", "Cuota", "Plan", "F. Pago", "Int. Fin", "Val. Compra", "Val. Pago", "Estado" };
                for (int i = 0; i < headers.Length; i++)
                {
                    string header = headers[i];
                    float textWidth = paint.MeasureText(header);
                    float textHeight = paint.FontMetrics.Descent - paint.FontMetrics.Ascent;
                    float x = cellWidth * i + (cellWidth - textWidth) / 2;
                    canvas.DrawText(header, x, currentY + textHeight, paint);
                }

                currentY += cellHeight; // Move to the next row for the data

                // Draw the data
                paint.FakeBoldText = false;
                for (int i = 0; i < tablaPago.Count; i++)
                {
                    for (int k = 0; k < tablaPago[i].Count; k++)
                    {
                        string[] values = { tablaPago[i][k].NumeroOrden.ToString(), tablaPago[i][k].Cuota.ToString().Replace(",", string.Empty),  tablaPago[i][k].NombreItem.ToString(), tablaPago[i][k].FormaCancelacion.ToString(), tablaPago[i][k].InstitucionFinanciera.ToString(), tablaPago[i][k].ValorCompra.ToString(), tablaPago[i][k].ValorPago.ToString(), tablaPago[i][k].EstadoCancelado.ToString() };

                        for (int j = 0; j < values.Length; j++)
                        {
                            string value = values[j];

                            // If the value is too long, adjust the text size
                            while (paint.MeasureText(value) > cellWidth && paint.TextSize > 0)
                            {
                                paint.TextSize--;
                            }

                            // Calculate the text width and height
                            float textWidth = paint.MeasureText(value);
                            float textHeight = paint.FontMetrics.Descent - paint.FontMetrics.Ascent;

                            // Check if we need to start a new page
                            if (currentY + textHeight > 842)
                            {
                                document.EndPage();
                                canvas = document.BeginPage(595, 842);
                                currentY = 0;
                            }

                            // Draw the text
                            float x = j * cellWidth + (cellWidth - textWidth) / 2;
                            canvas.DrawText(value, x, currentY + textHeight, paint);

                            // Reset the text size for the next cell
                            paint.TextSize = 12;
                        }

                        currentY += cellHeight; // Move to the next row
                    }
                }
                // End the last page
                document.EndPage();
            }
        }


    }

    public class CompraPago
    {
        public object NumeroOrden { get; set;}
        public object NombreItem { get; set; }
        public object ValorCompra { get; set; }
        public object InstitucionFinanciera { get; set; }
        public object FormaCancelacion { get; set; }
        public object ValorPago { get; set; }
        public object EstadoCancelado { get; set; }

        public object Cuota { get; set; }

    }
}
