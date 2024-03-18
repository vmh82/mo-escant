using System.IO;
using System.Reflection;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using System.Collections.Generic;
using Syncfusion.SfPdfViewer.XForms;
using DataModel.DTO.Iteraccion;
using Escant_App.ViewModels.Base;
using System.Threading.Tasks;
using System;
using DataModel.DTO.Configuracion;
using Newtonsoft.Json;
using ManijodaServicios.Switch;
using ManijodaServicios.Offline.Interfaz;
using DataModel.DTO;
using DataModel.Enums;
using DataModel.DTO.Compras;
using System.Linq;
using System.Data;
using Syncfusion.Pdf;
using DataModel.Helpers;
using Syncfusion.Pdf.Graphics;
using ManijodaServicios.AppSettings;
using Syncfusion.Drawing;
using ManijodaServicios.Resources.Texts;
using Syncfusion.Pdf.Grid;
using Color = Syncfusion.Drawing.Color;
using DataModel.DTO.Productos;
using System.Collections.ObjectModel;
using DataModel.DTO.Facturacion;

namespace Escant_App.ViewModels.Documentos
{
    [Preserve(AllMembers = true)]
    public class PdfViewerViewModel : ViewModelBase
    {
        private Generales.Generales generales = new Generales.Generales();
        private SwitchConfiguracion _switchConfiguracion;
        private SwitchIteraccion _switchIteraccion;
        private SwitchCompras _switchCompras;
        private Stream m_pdfDocumentStream;
        private bool m_isToolbarVisible = true;
        private bool m_isPickerVisible = false;
        private bool m_isSearchbarVisible = false;
        private bool m_isBottomToolbarVisible = true;
        private bool m_isSecondaryAnnotationBarVisible = false;
        private bool m_isHightlightBarVisible = false;
        private bool m_isUnderlineBarVisible = false;
        private bool m_isStrikeThroughBarVisible = false;
        private bool m_isEditAnnotationBarVisible = false;
        private bool m_isColorBarVisible = false;
        private int m_annotationRowHeight = 50, m_colorRowHeight = 0;
        private int m_annotationGridHeightRequest = 0;
        private Color m_toggleColor = Color.LightGray;
        private Xamarin.Forms.Color m_highlightColor;
        private Xamarin.Forms.Color m_underlineColor;
        private Xamarin.Forms.Color m_strikeThroughColor;
        private Xamarin.Forms.Color m_deleteButtonColor;

        #region 1.2 Variables de Instancia de Objetos para la Clase        
        private DataTable _ordenDetalleCompraSeleccionado;
        private Orden _ordenSeleccionado;
        private ParametroComprobante _parametroComprobante;
        private int count;
        #endregion 1.2 Variables de Instancia de Objetos para la Clase

        private readonly ISettingsService _settingsService;
        private readonly IServicioCompras_Orden _servicioOrden;
        private readonly IServicioIteraccion_Galeria _galeriaServicio;



        public TextSearchSettings m_pdfViewerTextSearchSettings;
        private string m_searchedText = string.Empty;
        private bool m_isCancelVisible = false;
        private Document m_selectedItem;
        private IList<Document> m_pdfDocumentCollection;

        internal SfPdfViewer pdfViewer { get; set; }

        #region 2.InstanciaAsignaVariablesControles
        #region 2.2 Objetos para la Clase
        public Orden OrdenSeleccionado
        {
            get => _ordenSeleccionado;
            set
            {
                _ordenSeleccionado = value;
                RaisePropertyChanged(() => OrdenSeleccionado);

            }
        }
        public ParametroComprobante ParametroComprobanteSeleccionado
        {
            get => _parametroComprobante;
            set
            {
                _parametroComprobante = value;
                RaisePropertyChanged(() => ParametroComprobanteSeleccionado);

            }
        }
        public DataTable OrdenDetalleCompraSeleccionado
        {
            get => _ordenDetalleCompraSeleccionado;
            set
            {
                _ordenDetalleCompraSeleccionado = value;
                RaisePropertyChanged(() => OrdenDetalleCompraSeleccionado);

            }
        }
        public Stream PdfDocumentStream
        {
            get => m_pdfDocumentStream;
            set
            {
                m_pdfDocumentStream = value;
                RaisePropertyChanged(() => PdfDocumentStream);

            }
        }
       
        public TextSearchSettings PDFViewerTextSearchSettings
        {
            get => m_pdfViewerTextSearchSettings;
            set
            {
                m_pdfViewerTextSearchSettings = value;
                RaisePropertyChanged(() => PDFViewerTextSearchSettings);

            }            
        }

        public int AnnotationGridHeightRequest
        {
            get => m_annotationGridHeightRequest;
            set
            {
                m_annotationGridHeightRequest = value;
                RaisePropertyChanged(() => AnnotationGridHeightRequest);

            }            
        }
        public Xamarin.Forms.Color DeleteButtonColor
        {
            get => m_deleteButtonColor;
            set
            {
                m_deleteButtonColor = value;
                RaisePropertyChanged(() => DeleteButtonColor);

            }            
        }

        public Color ToggleColor
        {
            get => m_toggleColor;
            set
            {
                m_toggleColor = value;
                RaisePropertyChanged(() => ToggleColor);

            }            
        }

        public bool IsColorBarVisible
        {
            get => m_isColorBarVisible;
            set
            {
                m_isColorBarVisible = value;
                RaisePropertyChanged(() => IsColorBarVisible);

            }            
        }

        public bool IsToolbarVisible
        {
            get => m_isToolbarVisible;
            set
            {
                if (m_isToolbarVisible == value)
                    return;
                m_isToolbarVisible = value;
                RaisePropertyChanged(() => IsToolbarVisible);

            }            
        }

        public bool IsEditAnnotationBarVisible
        {
            get => m_isEditAnnotationBarVisible;
            set
            {
                m_isEditAnnotationBarVisible = value;
                RaisePropertyChanged(() => IsEditAnnotationBarVisible);

            }            
        }

        public int AnnotationRowHeight
        {
            get => m_annotationRowHeight;
            set
            {
                m_annotationRowHeight = value;
                RaisePropertyChanged(() => AnnotationRowHeight);

            }            
        }

        public int ColorRowHeight
        {
            get => m_colorRowHeight;
            set
            {
                m_colorRowHeight = value;
                RaisePropertyChanged(() => ColorRowHeight);

            }            
        }

        public bool IsSecondaryAnnotationBarVisible
        {
            get => m_isSecondaryAnnotationBarVisible;
            set
            {
                m_isSecondaryAnnotationBarVisible = value;
                RaisePropertyChanged(() => IsSecondaryAnnotationBarVisible);

            }            
        }

        public bool IsHighlightBarVisible
        {
            get => m_isHightlightBarVisible;
            set
            {
                if (m_isHightlightBarVisible == value) return;
                m_isHightlightBarVisible = value;
                RaisePropertyChanged(() => IsHighlightBarVisible);

            }            
        }

        public bool IsUnderlineBarVisible
        {
            get => m_isUnderlineBarVisible;
            set
            {
                if (m_isUnderlineBarVisible == value) return;
                m_isUnderlineBarVisible = value;
                RaisePropertyChanged(() => IsUnderlineBarVisible);

            }            
        }

        public bool IsStrikeThroughBarVisible
        {
            get => m_isStrikeThroughBarVisible;
            set
            {
                if (m_isStrikeThroughBarVisible == value) return;
                m_isStrikeThroughBarVisible = value;
                RaisePropertyChanged(() => IsStrikeThroughBarVisible);

            }            
        }
        public Xamarin.Forms.Color HighlightColor
        {
            get => m_highlightColor;
            set
            {
                if (m_highlightColor == value) return;
                m_highlightColor = value;
                RaisePropertyChanged(() => HighlightColor);

            }            
        }

        public Xamarin.Forms.Color UnderlineColor
        {
            get => m_underlineColor;
            set
            {
                if (m_underlineColor == value) return;
                m_underlineColor = value;
                RaisePropertyChanged(() => UnderlineColor);

            }            
        }

        public Xamarin.Forms.Color StrikeThroughColor
        {
            get => m_strikeThroughColor;
            set
            {
                if (m_strikeThroughColor == value) return;
                m_strikeThroughColor = value;
                RaisePropertyChanged(() => StrikeThroughColor);

            }            
        }
        public bool IsSearchbarVisible
        {
            get => m_isSearchbarVisible;
            set
            {
                if (m_isSearchbarVisible == value) return;
                m_isSearchbarVisible = value;
                RaisePropertyChanged(() => IsSearchbarVisible);

            }
            
        }

        public bool IsBottomToolbarVisible
        {
            get => m_isBottomToolbarVisible;
            set
            {
                if (m_isBottomToolbarVisible == value) return;
                m_isBottomToolbarVisible = value;
                RaisePropertyChanged(() => IsBottomToolbarVisible);

            }
            
        }
        public bool IsPickerVisible
        {
            get => m_isPickerVisible;
            set
            {
                m_isPickerVisible = value;
                RaisePropertyChanged(() => IsPickerVisible);

            }            
        }

        
        public string SearchedText
        {
            get { return m_searchedText; }
            set
            {
                m_searchedText = value;
                if (m_searchedText != string.Empty)
                {
                    IsCancelVisible = true;
                }
                else
                {
                    IsCancelVisible = false;
                }
                RaisePropertyChanged(() => SearchedText);
            }
        }

        
        public bool IsCancelVisible
        {
            get { return m_isCancelVisible; }
            set
            {
                if (m_isCancelVisible == value)
                    return;
                m_isCancelVisible = value;
                RaisePropertyChanged(() => IsCancelVisible);
            }
        }
        
        public Document SelectedItem
        {
            get
            {
                return m_selectedItem;
            }
            set
            {
                if (m_selectedItem == value)
                {
                    IsPickerVisible = false;
                    return;
                }
                m_selectedItem = value;
                PdfDocumentStream = typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("GettingStarted_PDFViewer.Assets." + m_selectedItem.FileName + ".pdf");
                RaisePropertyChanged(() => SelectedItem);
                IsPickerVisible = false;
            }
        }


        
        public IList<Document> PdfDocumentCollection
        {
            get
            {
                if (m_pdfDocumentCollection == null)
                {
                    m_pdfDocumentCollection = new List<Document> { new Document("F# Succinctly"), new Document("GIS Succinctly"), new Document("HTTP Succinctly"), new Document("JavaScript Succinctly") };
                }
                return m_pdfDocumentCollection;
            }
            set
            {
                if (m_pdfDocumentCollection == value)
                    return;
                m_pdfDocumentCollection = value;
                RaisePropertyChanged(() => PdfDocumentCollection);
            }
        }
        #endregion 2.2 Objetos para la Clase
        #endregion 2.InstanciaAsignaVariablesControles
        public PdfViewerViewModel(ISettingsService settingService, IServicioIteraccion_Galeria galeriaServicio, IServicioCompras_Orden servicioOrden)
        {
            m_pdfDocumentStream = typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("GettingStarted_PDFViewer.Assets.GIS Succinctly.pdf");
            SearchAndToolbarToggleCommand = new Command<object>(OnSearchAndToolbarToggleCommand, CanExecute);
            FileOpenCommand = new Command<object>(OnFileOpenedCommand, CanExecute);
            AnnotationCommand = new Command<object>(OnAnnotationIconClickedCommand, CanExecute);
            HighlightCommand = new Command<object>(OnHighlightCommand, CanExecute);
            UnderlineCommand = new Command<object>(OnUnderlineCommand, CanExecute);
            StrikeThroughCommand = new Command<object>(OnStrikeThroughCommand, CanExecute);
            AnnotationBackCommand = new Command<object>(OnAnnotationBackCommand, CanExecute);
            ColorButtonClickedCommand = new Command<object>(OnColorButtonClickedCommand, CanExecute);
            ColorCommand = new Command<object>(OnColorCommand, CanExecute);
            TextMarkupSelectedCommand = new Command<object>(OnTextMarkupSelectedCommand, CanExecute);
            TextMarkupDeselectedCommand = new Command<object>(OnTextMarkupDeselectedCommand, CanExecute);
            DeleteCommand = new Command<object>(OnDeleteCommand, CanExecute);
            DocumentLoadedCommand = new Command<object>(OnDocumentLoadedCommand, CanExecute);
            _settingsService = settingService;
            _galeriaServicio = galeriaServicio;
            _servicioOrden = servicioOrden;
            _switchConfiguracion = new SwitchConfiguracion(_settingsService);
            _switchCompras = new SwitchCompras(_servicioOrden);
            generales = new Generales.Generales(_galeriaServicio);
        }
        #region 4.DefinicionMetodos
        public ICommand SearchAndToolbarToggleCommand { get; set; }
        public ICommand AnnotationCommand { get; set; }
        public ICommand FileOpenCommand { get; set; }
        public ICommand HighlightCommand { get; set; }
        public ICommand UnderlineCommand { get; set; }
        public ICommand StrikeThroughCommand { get; set; }
        public ICommand ColorButtonClickedCommand { get; set; }
        public ICommand ColorCommand { get; set; }
        public ICommand AnnotationBackCommand { get; set; }
        public ICommand TextMarkupSelectedCommand { get; set; }
        public ICommand TextMarkupDeselectedCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand DocumentLoadedCommand { get; set; }
        #endregion 4.DefinicionMetodos

        #region 5.Métodos de Administración de Datos
        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            try { 
            if (navigationData is Orden item && item != null)
            {
                OrdenSeleccionado = item;
                Stream fileStream = new MemoryStream();
                if (OrdenSeleccionado.TipoOrden.Contains("Compra"))
                {
                                        
                        switch(OrdenSeleccionado.CodigoEstadoEnConsulta != null?OrdenSeleccionado.CodigoEstadoEnConsulta: OrdenSeleccionado.CodigoEstado)
                        {
                            case "Ingresado":
                                var ordenDetalleSerializado = JsonConvert.DeserializeObject<List<OrdenDetalleCompraSerializado>>(OrdenSeleccionado.OrdenDetalleCompraSerializado);
                                OrdenDetalleCompraSeleccionado = generales.ToDataTable<OrdenDetalleCompra>(
                                                                 ordenDetalleSerializado.Select(x => new OrdenDetalleCompra
                                                                 {
                                                                     IdItem = x.IdItem
                                                                                                  ,
                                                                     NombreItem = x.NombreItem
                                                                                                  ,
                                                                     IdUnidad = x.IdUnidad
                                                                                                  ,
                                                                     Unidad = x.Unidad
                                                                                                  ,
                                                                     Cantidad = float.Parse(x.Cantidad)
                                                                                                  ,
                                                                     ValorUnitario = float.Parse(x.ValorUnitario)
                                                                                                  ,
                                                                     ValorFinal = float.Parse(x.ValorFinal)

                                                                 }).ToList());
                                break;
                            case "Recibido":
                                var ordenDetalleSerializado1 = JsonConvert.DeserializeObject<List<OrdenDetalleCompraSerializado>>(OrdenSeleccionado.OrdenDetalleCompraRecibidoSerializado);
                                OrdenDetalleCompraSeleccionado = generales.ToDataTable<OrdenDetalleCompraDet>(
                                                                 ordenDetalleSerializado1.Select(x => new OrdenDetalleCompraDet
                                                                 {
                                                                     IdItem = x.IdItem
                                                                                                  ,
                                                                     NombreItem = x.NombreItem
                                                                                                  ,
                                                                     TipoDescuento = x.TipoDescuento
                                                                                                  ,
                                                                     IdUnidad = x.IdUnidad
                                                                                                  ,
                                                                     Unidad = x.Unidad
                                                                                                  ,
                                                                     Cantidad = float.Parse(x.Cantidad)
                                                                                                  ,
                                                                     ValorCompra = float.Parse(x.ValorCompra)
                                                                                                  ,
                                                                     ValorUnitario = float.Parse(x.ValorUnitario)
                                                                                                  ,
                                                                     ValorDescuento = float.Parse(x.ValorDescuento)
                                                                                                  ,
                                                                     ValorFinal = float.Parse(x.ValorFinal)

                                                                 }).ToList());
                                break;
                        }
                    fileStream = await CreateOrdenCompraPdf();
                }
                if (OrdenSeleccionado.TipoOrden.Contains("Venta"))
                {
                        if (OrdenSeleccionado.CodigoEstadoEnConsulta != null && OrdenSeleccionado.CodigoEstadoEnConsulta.ToUpper().Contains("ERTIFICA"))
                           fileStream= await CreateCertificadoClientePdf();
                            
                    switch (OrdenSeleccionado.CodigoEstadoEnConsulta != null ? OrdenSeleccionado.CodigoEstadoEnConsulta : OrdenSeleccionado.CodigoEstado)
                    {
                        case "Ingresado":
                            var ordenDetalleSerializado = JsonConvert.DeserializeObject<List<OrdenDetalleCompraSerializado>>(OrdenSeleccionado.OrdenDetalleCompraSerializado);
                            OrdenDetalleCompraSeleccionado = generales.ToDataTable<OrdenDetalleCompra>(
                                                             ordenDetalleSerializado.Select(x => new OrdenDetalleCompra
                                                             {
                                                                 IdItem = x.IdItem
                                                                                              ,
                                                                 NombreItem = x.NombreItem
                                                                                              ,
                                                                 IdUnidad = x.IdUnidad
                                                                                              ,
                                                                 Unidad = x.Unidad
                                                                                              ,
                                                                 Cantidad = float.Parse(x.Cantidad)
                                                                                              ,
                                                                 ValorUnitario = float.Parse(x.ValorUnitario)
                                                                 ,
                                                                 ValorCompra = float.Parse(x.ValorCompra)
                                                                 ,
                                                                 ValorIncremento = float.Parse(x.ValorIncremento)
                                                                 ,
                                                                 ValorDescuento = float.Parse(x.ValorDescuento)
                                                                 ,
                                                                 ValorImpuesto = float.Parse(x.ValorImpuesto)
                                                                                              ,
                                                                 ValorFinal = float.Parse(x.ValorFinal)

                                                             }).ToList());
                                fileStream = await CreateOrdenVentaPdf();
                            break;
                            case "Cancelado":
                                var ordenDetalleSerializado1 = JsonConvert.DeserializeObject<List<OrdenDetalleCompraSerializado>>(OrdenSeleccionado.OrdenTablaPagoSerializado);
                                //var ordenDetalleSerializado11=ordenDetalleSerializado1.Where(x => x.EstadoCuota == "Cancelada").ToList().LastOrDefault();
                                OrdenDetalleCompraSeleccionado = generales.ToDataTable<OrdenDetalleCompra>(
                                                                 ordenDetalleSerializado1.Where(y => y.EstadoCuota == "CANCELADA").Select(x => new OrdenDetalleCompra
                                                                 {
                                                                     IdItem = x.IdItem
                                                                                                  ,
                                                                     NombreItem = x.NombreItem
                                                                                                  ,
                                                                     IdUnidad = x.IdUnidad
                                                                                                  ,
                                                                     Unidad = string.IsNullOrEmpty(x.Unidad) ? "" : x.Unidad
                                                                                                  ,
                                                                     Cantidad = float.Parse(string.IsNullOrEmpty(x.Cantidad)?"1":x.Cantidad)
                                                                                                  ,
                                                                     ValorUnitario = float.Parse(string.IsNullOrEmpty(x.ValorUnitario) ? x.ValorFinal : x.ValorUnitario)
                                                                     ,
                                                                     ValorCompra = float.Parse(string.IsNullOrEmpty(x.ValorCompra) ? x.ValorFinal : x.ValorCompra)
                                                                     ,
                                                                     ValorDescuento = float.Parse(string.IsNullOrEmpty(x.ValorDescuento) ? "0" : x.ValorDescuento)
                                                                                                  ,
                                                                     ValorFinal = float.Parse(x.ValorFinal)

                                                                 }).ToList());
                                fileStream = await CreateOrdenVentaPdf();
                                break;
                            case "Entregado":
                                var ordenDetalleSerializado2 = JsonConvert.DeserializeObject<List<ItemEntrega>>(OrdenSeleccionado.OrdenDetalleCompraEntregadoSerializado);
                                OrdenDetalleCompraSeleccionado = generales.ToDataTable<OrdenDetalleCompra>(
                                                                 ordenDetalleSerializado2.Select(x => new OrdenDetalleCompra
                                                                 {
                                                                     IdItem = x.IdItem
                                                                                                  ,
                                                                     NombreItem = x.NombreItem
                                                                                                  ,
                                                                     EsServicio=x.EsServicio      ,
                                                                     Unidad = x.Unidad
                                                                                                  ,
                                                                     Cantidad = x.cantidad
                                                                                                  ,
                                                                     CantidadEntregada = x.cantidadEntregada
                                                                     ,
                                                                     CantidadSaldoEntrega = x.cantidadSaldoEntrega                                                                     

                                                                 }).ToList());
                                fileStream = await CreateOrdenEntregaPdf();
                                break;
                        }
                    
                }

                 
                //Load the PDF
                pdfViewer.LoadDocument(fileStream);
            }
            }
            catch(Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Importante", ex.Message.ToString(), "OK");
            }
            IsBusy = false;
        }

        public async Task GuardaArchivo(string filePath)
        {
            Galeria iRegistro = new Galeria()
            {
                Id = OrdenSeleccionado.Id
                ,Directorio="Orden"
               ,
                Nombre = OrdenSeleccionado.NumeroOrden + "-" + OrdenSeleccionado.CodigoEstado
               ,
                Extension = ".pdf"
               ,
                GuardarStorage = 1
               ,
                fStream = File.Open(filePath, FileMode.Open)
            };
            await generales.GuardaArchivo(iRegistro);
            
        }

        #endregion 5.Métodos de Administración de Datos
        private void OnDeleteCommand(object parameter)
        {
            IsEditAnnotationBarVisible = false;
            AnnotationGridHeightRequest = 0;
        }

        private void OnDocumentLoadedCommand(object parameter)
        {
            AnnotationGridHeightRequest = 0;
            IsSecondaryAnnotationBarVisible = false;
            IsColorBarVisible = false;
            IsEditAnnotationBarVisible = false;
            IsHighlightBarVisible = false;
            IsUnderlineBarVisible = false;
            IsStrikeThroughBarVisible = false;
        }
        private void OnTextMarkupSelectedCommand(object parameter)
        {
            ColorRowHeight = 1;
            IsColorBarVisible = false;
            AnnotationGridHeightRequest = 50;
            AnnotationRowHeight = 50;
            IsSecondaryAnnotationBarVisible = false;
            IsHighlightBarVisible = false;
            IsStrikeThroughBarVisible = false;
            IsUnderlineBarVisible = false;
            IsEditAnnotationBarVisible = true;
        }

        private void OnTextMarkupDeselectedCommand(object parameter)
        {
            if (IsEditAnnotationBarVisible)
            {
                AnnotationGridHeightRequest = 0;
                IsEditAnnotationBarVisible = false;
            }
        }       

        private bool CanExecute(object parameter)
        {
            return true;
        }

        private void OnSearchAndToolbarToggleCommand(object destinationPageParam)
        {
            IsToolbarVisible = !IsToolbarVisible;
            IsSearchbarVisible = !IsSearchbarVisible;
            IsPickerVisible = false;
            IsSecondaryAnnotationBarVisible = false;
            IsColorBarVisible = false;
            IsHighlightBarVisible = IsUnderlineBarVisible = IsStrikeThroughBarVisible = false;
            IsEditAnnotationBarVisible = false;
            AnnotationGridHeightRequest = 0;
            SearchedText = string.Empty;
        }

        private void OnFileOpenedCommand(object openedFile)
        {
            IsPickerVisible = !IsPickerVisible;
        }

        private void OnAnnotationIconClickedCommand(object parameter)
        {
            IsSearchbarVisible = false;
            if (AnnotationGridHeightRequest == 0)
            {
                AnnotationGridHeightRequest = 50;
                IsSecondaryAnnotationBarVisible = true;
            }
            else
            {
                if (!IsEditAnnotationBarVisible)
                {
                    AnnotationGridHeightRequest = 0;
                    IsSecondaryAnnotationBarVisible = false;
                }
                else
                {
                    AnnotationGridHeightRequest = 50;
                    IsSecondaryAnnotationBarVisible = true;
                }
            }

            AnnotationRowHeight = 50;
            ColorRowHeight = 1;
            IsColorBarVisible = false;

            IsHighlightBarVisible = false;
            IsUnderlineBarVisible = false;
            IsStrikeThroughBarVisible = false;
            IsEditAnnotationBarVisible = false;
        }

        private void OnHighlightCommand(object parameter)
        {
            IsSecondaryAnnotationBarVisible = false;
            IsUnderlineBarVisible = false;
            IsStrikeThroughBarVisible = false;
            IsHighlightBarVisible = true;
        }

        private void OnUnderlineCommand(object parameter)
        {
            IsSecondaryAnnotationBarVisible = false;
            IsHighlightBarVisible = false;
            IsStrikeThroughBarVisible = false;
            IsUnderlineBarVisible = true;
        }

        private void OnStrikeThroughCommand(object parameter)
        {
            IsSecondaryAnnotationBarVisible = false;
            IsHighlightBarVisible = false;
            IsUnderlineBarVisible = false;
            IsStrikeThroughBarVisible = true;
        }

        private void OnAnnotationBackCommand(object parameter)
        {
            AnnotationGridHeightRequest = 50;
            AnnotationRowHeight = 50;
            ColorRowHeight = 1;
            IsColorBarVisible = false;
            IsHighlightBarVisible = false;
            IsUnderlineBarVisible = false;
            IsStrikeThroughBarVisible = false;
            IsSecondaryAnnotationBarVisible = true;
        }

        private void OnColorButtonClickedCommand(object parameter)
        {
            if (AnnotationGridHeightRequest == 50)
            {
                AnnotationGridHeightRequest = 100;
                AnnotationRowHeight = 50;
                ColorRowHeight = 50;
                IsColorBarVisible = true;
            }
            else
            {
                AnnotationGridHeightRequest = 50;
                AnnotationRowHeight = 50;
                ColorRowHeight = 1;
                IsColorBarVisible = false;
            }
        }

        private void OnColorCommand(object parameter)
        {
            switch (parameter.ToString())
            {
                case "Cyan":
                    {
                        if (IsHighlightBarVisible)
                            HighlightColor = Xamarin.Forms.Color.FromHex("#00FFFF");
                        if (IsStrikeThroughBarVisible)
                            StrikeThroughColor = Xamarin.Forms.Color.FromHex("#00FFFF");
                        if (IsUnderlineBarVisible)
                            UnderlineColor = Xamarin.Forms.Color.FromHex("#00FFFF");
                        if (IsEditAnnotationBarVisible)
                            DeleteButtonColor = Xamarin.Forms.Color.FromHex("00FFFF");
                    }
                    break;

                case "Green":
                    {
                        if (IsHighlightBarVisible)
                            HighlightColor = Xamarin.Forms.Color.Green;
                        if (IsStrikeThroughBarVisible)
                            StrikeThroughColor = Xamarin.Forms.Color.Green;
                        if (IsUnderlineBarVisible)
                            UnderlineColor = Xamarin.Forms.Color.Green;
                        if (IsEditAnnotationBarVisible)
                            DeleteButtonColor = Xamarin.Forms.Color.Green;
                    }
                    break;

                case "Yellow":
                    {
                        if (IsHighlightBarVisible)
                            HighlightColor = Xamarin.Forms.Color.Yellow;
                        if (IsStrikeThroughBarVisible)
                            StrikeThroughColor = Xamarin.Forms.Color.Yellow;
                        if (IsUnderlineBarVisible)
                            UnderlineColor = Xamarin.Forms.Color.Yellow;
                        if (IsEditAnnotationBarVisible)
                            DeleteButtonColor = Xamarin.Forms.Color.Yellow;
                    }
                    break;

                case "Magenta":
                    {
                        if (IsHighlightBarVisible)
                            HighlightColor = Xamarin.Forms.Color.FromHex("#FF00FF");
                        if (IsStrikeThroughBarVisible)
                            StrikeThroughColor = Xamarin.Forms.Color.FromHex("#FF00FF");
                        if (IsUnderlineBarVisible)
                            UnderlineColor = Xamarin.Forms.Color.FromHex("#FF00FF");
                        if (IsEditAnnotationBarVisible)
                            DeleteButtonColor = Xamarin.Forms.Color.FromHex("#FF00FF");
                    }
                    break;

                case "Black":
                    {
                        if (IsHighlightBarVisible)
                            HighlightColor = Xamarin.Forms.Color.Black;
                        if (IsStrikeThroughBarVisible)
                            StrikeThroughColor = Xamarin.Forms.Color.Black;
                        if (IsUnderlineBarVisible)
                            UnderlineColor = Xamarin.Forms.Color.Black;
                        if (IsEditAnnotationBarVisible)
                            DeleteButtonColor = Xamarin.Forms.Color.Black;
                    }
                    break;

                case "White":
                    {
                        if (IsHighlightBarVisible)
                            HighlightColor = Xamarin.Forms.Color.White;
                        if (IsStrikeThroughBarVisible)
                            StrikeThroughColor = Xamarin.Forms.Color.White;
                        if (IsUnderlineBarVisible)
                            UnderlineColor = Xamarin.Forms.Color.White;
                        if (IsEditAnnotationBarVisible)
                            DeleteButtonColor = Xamarin.Forms.Color.White;
                    }
                    break;

            }
            AnnotationGridHeightRequest = 50;
            AnnotationRowHeight = 50;
            ColorRowHeight = 1;
            IsColorBarVisible = false;
        }
        #region FormatosPdf
        /// <summary>
        /// Esquema para Orden de compra
        /// </summary>
        /// <returns></returns>
        public async Task<Stream> CreateOrdenCompraPdf()
        {
            MemoryStream memoryStream = new MemoryStream();
            //1. Obtener Datos a Presentarse de la Orden 
            string id = Generator.GenerarOrdenNo("OC", OrdenSeleccionado.NumeroOrden);
                string proveedor = OrdenSeleccionado.NombreProveedor;
            //2. Crear un nuevo documento PDF
            PdfDocument document = new PdfDocument();
            //3. Añadir las configuraciones respectivas (Vertical, con márgen de 30)
            document.PageSettings.Orientation = PdfPageOrientation.Portrait;
            document.PageSettings.Margins.All = 30;
            //4. Añadir una página al documento
            PdfPage page = document.Pages.Add();
            //5. Declarar una variable de graficación
            PdfGraphics graphics = page.Graphics;
            //6. Almacenar el estado actual de las configuraciones
            PdfGraphicsState state = graphics.Save();



            //Loads the image as stream
            //string[] names = typeof(App).GetTypeInfo().Assembly.GetManifestResourceNames();
            //7. Obtener la imagen del logo del Negocio
            Stream imageStream = new MemoryStream(SettingsOnline.oAplicacion.Logo);//typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("GettingStarted.Assets.bg_header.png");
            PdfImage image = PdfImage.FromStream(imageStream);
            //8. Implementar en el documento como marca de agua
            graphics.SetTransparency(0.25f);
            graphics.DrawImage(image, new PointF(0, 0), page.Graphics.ClientSize);
            //9. Restaurar las configuraciones inciales del documento para graficación
            graphics.Restore(state);
            //10. Declarar una variable de lienzo donde se dibujará el objeto siguiente
            RectangleF bounds = new RectangleF(140, 0, 300, 130);
            //11. Dibujar el logo del Negocio en la cabecera del documento
            page.Graphics.DrawImage(image, bounds);

            //12. Definición de variables gráficas
            PdfFont timesRoman_18 = new PdfStandardFont(PdfFontFamily.TimesRoman, 18);
            PdfFont timesRoman_16_Re = new PdfStandardFont(PdfFontFamily.TimesRoman, 16f, PdfFontStyle.Regular);
            PdfFont timesRoman_15 = new PdfStandardFont(PdfFontFamily.TimesRoman, 15f);
            PdfColor colorNegro = new PdfColor(0, 0, 0);
            PdfColor colorAzulMarino = new PdfColor(52, 77, 117);
            PdfBrush solidBrush = new PdfSolidBrush(colorAzulMarino);
            //12.. Fuente para la Cabecera del documento
            PdfFont subHeadingFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 20);

            PdfFont timesRoman = new PdfStandardFont(PdfFontFamily.TimesRoman, 20);

            //13.Datos del Negocio.(Nombre, Dirección, Teléfono)
            PdfTextElement element = new PdfTextElement(SettingsOnline.oAplicacion.Nombre, timesRoman_18);
            element.Brush = new PdfSolidBrush(colorNegro);
            PdfLayoutResult result = element.Draw(page, new PointF(10, bounds.Bottom + 20));
            element = new PdfTextElement(SettingsOnline.oAplicacion.Direccion, timesRoman_18);
            element.Brush = new PdfSolidBrush(colorNegro);
            result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 5));
            element = new PdfTextElement(SettingsOnline.oAplicacion.Telefono, timesRoman_18);
            element.Brush = new PdfSolidBrush(colorNegro);
            result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 5));

            //14. Dibujar la cabecera del documento (Orden Fecha))
            bounds = new RectangleF(0, bounds.Bottom + 90, graphics.ClientSize.Width, 30);
            //14.1 Dibujar un lugar para la cabecera del documento.
            graphics.DrawRectangle(solidBrush, bounds);
            //14.2 Crear el texto del número de Orden            
            element = new PdfTextElement(TextsTranslateManager.Translate("Orden") + " : " + id.ToString(), subHeadingFont);
            element.Brush = PdfBrushes.White;
            //14.2.1 Dibujar en la cabecera
            result = element.Draw(page, new PointF(10, bounds.Top + 8));

            //14.3 Crear el texto de la Fecha de la Orden
            string currentDate = TextsTranslateManager.Translate("Fecha") + " : " + DateTime.Now.ToString("dd/MM/yyyy");
            //14.3.1 Medir el ancho del texto para localizarlo en el lugar correcto
            SizeF textSize = subHeadingFont.MeasureString(currentDate);
            PointF textPosition = new PointF(graphics.ClientSize.Width - textSize.Width - 10, result.Bounds.Y);
            //14.3.2 Dibujar la fecha en la cabecera
            graphics.DrawString(currentDate, subHeadingFont, element.Brush, textPosition);

            //15. Crear los elemenstos entre la cabecera y el detalle del documento(Proveedor)            
            element = new PdfTextElement(TextsTranslateManager.Translate("Proveedor") + " : " + proveedor, timesRoman);
            element.Brush = new PdfSolidBrush(new PdfColor(126, 155, 203));
            result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 25));

            //15.0 Definir configuraciones para dibujar línea debajo de datos del Proveedor
            PdfPen linePen = new PdfPen(new PdfColor(126, 151, 173), 0.70f);
            PointF startPoint = new PointF(0, result.Bounds.Bottom + 3);
            PointF endPoint = new PointF(graphics.ClientSize.Width, result.Bounds.Bottom + 3);
            //15.1 Dibujar la línea
            graphics.DrawLine(linePen, startPoint, endPoint);

            //16. Crear la fuente de datos de la tabla a visualizarse con los detalles
                DataTable uploadTable = new DataTable();
                switch (OrdenSeleccionado.CodigoEstadoEnConsulta != null ? OrdenSeleccionado.CodigoEstadoEnConsulta : OrdenSeleccionado.CodigoEstado)
                {
                    case "Ingresado":
                        uploadTable = OrdenDetalleCompraSeleccionado.DefaultView.ToTable(false, "IdItem", "NombreItem", "Unidad", "Cantidad", "ValorUnitario", "ValorFinal");
                        uploadTable.Columns["IdItem"].ColumnName = "Id";
                        uploadTable.Columns["NombreItem"].ColumnName = "Item";
                        uploadTable.Columns["Cantidad"].ColumnName = "Cant.";
                        uploadTable.Columns["ValorUnitario"].ColumnName = "Val.Unit.";
                        uploadTable.Columns["ValorFinal"].ColumnName = "Subtotal";
                        break;
                    case "Recibido":
                        uploadTable = OrdenDetalleCompraSeleccionado.DefaultView.ToTable(false, "IdItem", "NombreItem", "TipoDescuento", "Unidad", "Cantidad", "ValorUnitario", "ValorCompra", "ValorDescuento", "ValorFinal");
                        uploadTable.Columns["IdItem"].ColumnName = "Id";
                        uploadTable.Columns["NombreItem"].ColumnName = "Item";
                        uploadTable.Columns["TipoDescuento"].ColumnName = "Tipo Descto.";
                        uploadTable.Columns["Cantidad"].ColumnName = "Cant.";
                        uploadTable.Columns["ValorUnitario"].ColumnName = "Val.Unit.";
                        uploadTable.Columns["ValorCompra"].ColumnName = "Val.Compra.";
                        uploadTable.Columns["ValorDescuento"].ColumnName = "Val.Descto.";
                        uploadTable.Columns["ValorFinal"].ColumnName = "Subtotal";
                        break;
                }

                DataTable invoiceDetails = uploadTable;//CreateOrdersTable();
                                                       //Creates a PDF grid
                PdfGrid grid = new PdfGrid();
                //Adds the data source
                grid.DataSource = invoiceDetails; //formateaCuerpo(invoiceDetails);
                //Creates the grid cell styles
                PdfGridCellStyle cellStyle = new PdfGridCellStyle();
                cellStyle.Borders.All = PdfPens.White;
                PdfGridRow header = grid.Headers[0];
                //Creates the header style
                PdfGridCellStyle headerStyle = new PdfGridCellStyle();
                headerStyle.Borders.All = new PdfPen(new PdfColor(126, 151, 173));
                headerStyle.BackgroundBrush = new PdfSolidBrush(colorAzulMarino);
                headerStyle.TextBrush = PdfBrushes.White;
                headerStyle.Font = timesRoman_16_Re;
                //Adds cell customizations
                for (int i = 0; i < header.Cells.Count; i++)
                {
                    if (i == 0 || i == 1)
                        header.Cells[i].StringFormat = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);
                    else
                        header.Cells[i].StringFormat = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);
                }
                //Applies the header style
                header.ApplyStyle(headerStyle);


            PdfGridRowCollection cuerpo = grid.Rows;
                cellStyle.Borders.Bottom = new PdfPen(new PdfColor(217, 217, 217), 0.70f);
                cellStyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 17f);
                cellStyle.TextBrush = new PdfSolidBrush(colorNegro);
                cellStyle.StringFormat = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);

            cuerpo.ApplyStyle(cellStyle);
            //Creates the layout format for grid
            PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat();
            // Creates layout format settings to allow the table pagination
            layoutFormat.Layout = PdfLayoutType.Paginate;
            //Draws the grid to the PDF page.
            PdfGridLayoutResult gridResult = grid.Draw(page, new RectangleF(new PointF(0, result.Bounds.Bottom + 40), new SizeF(graphics.ClientSize.Width, graphics.ClientSize.Height - 100)), layoutFormat);            

            //Save the PDF document to stream.
            document.Save(memoryStream);
            //Close the document.
            document.Close(true);

            //Save the stream as a file in the device and invoke it for viewing
            //Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("Output.pdf", "application/pdf", stream);

            return memoryStream;
        }

        public async Task<Stream> CreateOrdenVentaPdf()
        {
            MemoryStream memoryStream = new MemoryStream();
            //1. Obtener Datos a Presentarse de la Orden
            string id = Generator.GenerarOrdenNo("OV", OrdenSeleccionado.NumeroOrden);
            string proveedor = OrdenSeleccionado.NombreCliente;
            //2. Crear un nuevo documento PDF
            PdfDocument document = new PdfDocument();
            //3. Añadir las configuraciones respectivas (Vertical, con márgen de 30)
            document.PageSettings.Orientation = PdfPageOrientation.Portrait;
            document.PageSettings.Margins.All = 30;
            //4. Añadir una página al documento
            PdfPage page = document.Pages.Add();
            //5. Declarar una variable de graficación
            PdfGraphics graphics = page.Graphics;
            //6. Almacenar el estado actual de las configuraciones
            PdfGraphicsState state = graphics.Save();
            
            //Loads the image as stream
            //string[] names = typeof(App).GetTypeInfo().Assembly.GetManifestResourceNames();
            //7. Obtener la imagen del logo del Negocio
            Stream imageStream = new MemoryStream(SettingsOnline.oAplicacion.Logo);//typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("GettingStarted.Assets.bg_header.png");
            PdfImage image = PdfImage.FromStream(imageStream);
            //8. Implementar en el documento como marca de agua
            graphics.SetTransparency(0.25f);
            graphics.DrawImage(image, new PointF(0, 0), page.Graphics.ClientSize);
            //9. Restaurar las configuraciones inciales del documento para graficación
            graphics.Restore(state);

            //10. Declarar una variable de lienzo donde se dibujará el objeto siguiente
            RectangleF bounds = new RectangleF(140, 0, 300, 130);
            //11. Dibujar el logo del Negocio en la cabecera del documento
            page.Graphics.DrawImage(image, bounds);

            //12. Definición de variables gráficas
            PdfFont timesRoman_18 = new PdfStandardFont(PdfFontFamily.TimesRoman, 18);
            PdfFont timesRoman_16_Re = new PdfStandardFont(PdfFontFamily.TimesRoman, 16f, PdfFontStyle.Regular);
            PdfFont timesRoman_15 = new PdfStandardFont(PdfFontFamily.TimesRoman, 15f);
            PdfColor colorNegro = new PdfColor(0, 0, 0);
            PdfColor colorAzulMarino = new PdfColor(52, 77, 117);
            PdfBrush solidBrush = new PdfSolidBrush(colorAzulMarino);
            //12.. Fuente para la Cabecera del documento
            PdfFont subHeadingFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 20);

            PdfFont timesRoman = new PdfStandardFont(PdfFontFamily.TimesRoman, 20);

            //13.Datos del Negocio.(Nombre, Dirección, Teléfono)
            PdfTextElement element = new PdfTextElement(SettingsOnline.oAplicacion.Nombre, timesRoman_18);
            element.Brush = new PdfSolidBrush(colorNegro);
            PdfLayoutResult result = element.Draw(page, new PointF(10, bounds.Bottom + 20));
            element = new PdfTextElement(SettingsOnline.oAplicacion.Direccion, timesRoman_18);
            element.Brush = new PdfSolidBrush(colorNegro);
            result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 5));
            element = new PdfTextElement(SettingsOnline.oAplicacion.Telefono, timesRoman_18);
            element.Brush = new PdfSolidBrush(colorNegro);
            result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 5));

            //14. Dibujar la cabecera del documento (Orden Fecha))
            bounds = new RectangleF(0, bounds.Bottom + 90, graphics.ClientSize.Width, 30);
            //14.1 Dibujar un lugar para la cabecera del documento.
            graphics.DrawRectangle(solidBrush, bounds);            
            //14.2 Crear el texto del número de Orden            
            element = new PdfTextElement(TextsTranslateManager.Translate("Orden") + " : " + id.ToString(), subHeadingFont);
            element.Brush = PdfBrushes.White;
            //14.2.1 Dibujar en la cabecera
            result = element.Draw(page, new PointF(10, bounds.Top + 8));

            //14.3 Crear el texto de la Fecha de la Orden
            string currentDate = TextsTranslateManager.Translate("Fecha") + " : " + DateTime.Now.ToString("dd/MM/yyyy");
            //14.3.1 Medir el ancho del texto para localizarlo en el lugar correcto
            SizeF textSize = subHeadingFont.MeasureString(currentDate);
            PointF textPosition = new PointF(graphics.ClientSize.Width - textSize.Width - 10, result.Bounds.Y);
            //14.3.2 Dibujar la fecha en la cabecera
            graphics.DrawString(currentDate, subHeadingFont, element.Brush, textPosition);

            
            //15. Crear los elemenstos entre la cabecera y el detalle del documento(Cliente)
            element = new PdfTextElement(TextsTranslateManager.Translate("Cliente") + " : " + proveedor, timesRoman);
            element.Brush = new PdfSolidBrush(colorAzulMarino);
            result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 25));

            PdfPen linePen = new PdfPen(new PdfColor(126, 151, 173), 0.70f);
            PointF startPoint = new PointF(0, result.Bounds.Bottom + 3);
            PointF endPoint = new PointF(graphics.ClientSize.Width, result.Bounds.Bottom + 3);
            //Draws a line at the bottom of the address
            graphics.DrawLine(linePen, startPoint, endPoint);

            //16. Crear la fuente de datos de la tabla a visualizarse con los detalles
            DataTable uploadTable = new DataTable();
            switch (OrdenSeleccionado.CodigoEstadoEnConsulta != null ? OrdenSeleccionado.CodigoEstadoEnConsulta : OrdenSeleccionado.CodigoEstado)
            {
                case "Ingresado":
                    uploadTable = OrdenDetalleCompraSeleccionado.DefaultView.ToTable(false, "IdItem", "NombreItem", "Unidad", "Cantidad", "ValorUnitario","ValorIncremento", "ValorFinal");
                    uploadTable.Columns["IdItem"].ColumnName = "Id";
                    uploadTable.Columns["NombreItem"].ColumnName = "Item";
                    uploadTable.Columns["Cantidad"].ColumnName = "Cant.";
                    uploadTable.Columns["ValorUnitario"].ColumnName = "Val.Unit.";
                    uploadTable.Columns["ValorIncremento"].ColumnName = "Val.Inc.";
                    uploadTable.Columns["ValorFinal"].ColumnName = "Subtotal";                    
                    break;
                case "Cancelado":
                    uploadTable = OrdenDetalleCompraSeleccionado.DefaultView.ToTable(false, "IdItem", "NombreItem", "Unidad", "Cantidad", "ValorUnitario", "ValorFinal");
                    uploadTable.Columns["IdItem"].ColumnName = "Id";
                    uploadTable.Columns["NombreItem"].ColumnName = "Item";
                    uploadTable.Columns["Cantidad"].ColumnName = "Cant.";
                    uploadTable.Columns["ValorUnitario"].ColumnName = "Val.Unit.";
                    uploadTable.Columns["ValorFinal"].ColumnName = "Subtotal";
                    break;
                case "Recibido":
                    uploadTable = OrdenDetalleCompraSeleccionado.DefaultView.ToTable(false, "IdItem", "NombreItem", "TipoDescuento", "Unidad", "Cantidad", "ValorUnitario", "ValorCompra", "ValorDescuento", "ValorFinal");
                    uploadTable.Columns["IdItem"].ColumnName = "Id";
                    uploadTable.Columns["NombreItem"].ColumnName = "Item";
                    uploadTable.Columns["TipoDescuento"].ColumnName = "Tipo Descto.";
                    uploadTable.Columns["Cantidad"].ColumnName = "Cant.";
                    uploadTable.Columns["ValorUnitario"].ColumnName = "Val.Unit.";
                    uploadTable.Columns["ValorCompra"].ColumnName = "Val.Compra.";
                    uploadTable.Columns["ValorDescuento"].ColumnName = "Val.Descto.";
                    uploadTable.Columns["ValorFinal"].ColumnName = "Subtotal";
                    break;
            }
            //DataTable dtNew = dataTableColsToOtherType(uploadTable, typeof(string), new List<string>() { "ValorFinal" });

            DataTable invoiceDetails = uploadTable;//CreateOrdersTable();
                                                   //Creates a PDF grid
            PdfGrid grid = new PdfGrid();
            //Adds the data source
            grid.DataSource = formateaCuerpo(invoiceDetails);
            //Creates the grid cell styles
            PdfGridCellStyle cellStyle = new PdfGridCellStyle();
            cellStyle.Borders.All = PdfPens.White;
            PdfGridRow header = grid.Headers[0];
            //Creates the header style
            PdfGridCellStyle headerStyle = new PdfGridCellStyle();
            headerStyle.Borders.All = new PdfPen(new PdfColor(126, 151, 173));
            headerStyle.BackgroundBrush = new PdfSolidBrush(colorAzulMarino);
            headerStyle.TextBrush = PdfBrushes.White;
            headerStyle.Font = timesRoman_16_Re;// new PdfStandardFont(PdfFontFamily.TimesRoman, 18f, PdfFontStyle.Regular);

            //Adds cell customizations
            for (int i = 0; i < header.Cells.Count; i++)
            {
                if (i == 0 || i == 1)
                    header.Cells[i].StringFormat = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);
                else
                    header.Cells[i].StringFormat = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);
            }

            //Applies the header style
            header.ApplyStyle(headerStyle);
            PdfGridRowCollection cuerpo = grid.Rows;
            cellStyle.Borders.Bottom = new PdfPen(new PdfColor(217, 217, 217), 0.70f);
            cellStyle.Font = timesRoman_15;// new PdfStandardFont(PdfFontFamily.TimesRoman, 17f);
            cellStyle.TextBrush = new PdfSolidBrush(colorNegro);
            cellStyle.StringFormat = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);

            cuerpo.ApplyStyle(cellStyle);
            //Creates the layout format for grid
            PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat();
            // Creates layout format settings to allow the table pagination
            layoutFormat.Layout = PdfLayoutType.Paginate;
            //Draws the grid to the PDF page.
            PdfGridLayoutResult gridResult = grid.Draw(page, new RectangleF(new PointF(0, result.Bounds.Bottom + 40), new SizeF(graphics.ClientSize.Width, graphics.ClientSize.Height - 100)), layoutFormat);

            //17. Dibujar Resumen o Totales
            //17.1 Generar Info            
            List<object> data = new List<object>();
            OrdenSeleccionado.OrdenTablaPago = !string.IsNullOrEmpty(OrdenSeleccionado.OrdenTablaPagoSerializado) ? JsonConvert.DeserializeObject<ObservableCollection<TablaPago>>(OrdenSeleccionado.OrdenTablaPagoSerializado) : new ObservableCollection<TablaPago>();
            Object row1 = new { ID = "Subtotal", Valor = String.Format("{0:0.00}", OrdenSeleccionado.OrdenTablaPago.Sum(x=>x.ValorPagado)) }; //#.##
            Object row2 = new { ID = "Descuento", Valor = String.Format("{0:0.00}", OrdenSeleccionado.OrdenTablaPago.Where(y=>y.EstadoCuota=="CANCELADA").Sum(x => x.ValorDescuento)) };
            Object row3 = new { ID = "Impuesto", Valor = String.Format("{0:0.00}", OrdenSeleccionado.OrdenTablaPago.Where(y => y.EstadoCuota == "CANCELADA").Sum(x => x.ValorImpuesto)) };
            Object row4 = new { ID = "Total", Valor = String.Format("{0:0.00}", OrdenSeleccionado.OrdenTablaPago.Where(y => y.EstadoCuota == "CANCELADA").Sum(x => x.ValorFinal)) };

            data.Add(row1);
            data.Add(row2);
            data.Add(row3);
            data.Add(row4);
            //Add list to IEnumerable
            IEnumerable<object> dataTable = data;
            
            //Create a PdfGrid.
            PdfGrid pdfGridTotales = new PdfGrid();
            //Assign data source.
            pdfGridTotales.DataSource = dataTable;
            pdfGridTotales.BeginCellLayout += PdfGrid_BeginCellLayout;            
            cuerpo = pdfGridTotales.Rows;
            /*
            foreach(var fila in cuerpo)
            {
                //Adds cell customizations
                for (int i = 0; i < fila.Cells.Count; i++)
                {
                    if (i == 1)
                        fila.Cells[i]. = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);                    
                }
            }
            */

            cellStyle.Borders.Bottom = new PdfPen(new PdfColor(217, 217, 217), 0.70f);
            cellStyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 17f);
            cellStyle.TextBrush = new PdfSolidBrush(colorNegro);
            cellStyle.StringFormat = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);

            cuerpo.ApplyStyle(cellStyle);
            //Draw grid to the page of PDF document.
            pdfGridTotales.Draw(page, new PointF(380, gridResult.Bounds.Bottom + 10));


            //Save the PDF document to stream.
            document.Save(memoryStream);
            //Close the document.
            document.Close(true);

            //Save the stream as a file in the device and invoke it for viewing
            //Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("Output.pdf", "application/pdf", stream);

            return memoryStream;
        }

        public async Task<Stream> CreateOrdenEntregaPdf()
        {
            MemoryStream memoryStream = new MemoryStream();
            //1. Obtener Datos a Presentarse de la Orden
            string id = Generator.GenerarOrdenNo("OV", OrdenSeleccionado.NumeroOrden);
            string proveedor = OrdenSeleccionado.NombreCliente;
            //2. Crear un nuevo documento PDF
            PdfDocument document = new PdfDocument();
            //3. Añadir las configuraciones respectivas (Vertical, con márgen de 30)
            document.PageSettings.Orientation = PdfPageOrientation.Portrait;
            document.PageSettings.Margins.All = 30;
            //4. Añadir una página al documento
            PdfPage page = document.Pages.Add();
            //5. Declarar una variable de graficación
            PdfGraphics graphics = page.Graphics;
            //6. Almacenar el estado actual de las configuraciones
            PdfGraphicsState state = graphics.Save();

            //Loads the image as stream
            //string[] names = typeof(App).GetTypeInfo().Assembly.GetManifestResourceNames();
            //7. Obtener la imagen del logo del Negocio
            Stream imageStream = new MemoryStream(SettingsOnline.oAplicacion.Logo);//typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("GettingStarted.Assets.bg_header.png");
            PdfImage image = PdfImage.FromStream(imageStream);
            //8. Implementar en el documento como marca de agua
            graphics.SetTransparency(0.25f);
            graphics.DrawImage(image, new PointF(0, 0), page.Graphics.ClientSize);
            //9. Restaurar las configuraciones inciales del documento para graficación
            graphics.Restore(state);

            //10. Declarar una variable de lienzo donde se dibujará el objeto siguiente
            RectangleF bounds = new RectangleF(140, 0, 300, 130);
            //11. Dibujar el logo del Negocio en la cabecera del documento
            page.Graphics.DrawImage(image, bounds);

            //12. Definición de variables gráficas
            PdfFont timesRoman_18 = new PdfStandardFont(PdfFontFamily.TimesRoman, 18);
            PdfFont timesRoman_16_Re = new PdfStandardFont(PdfFontFamily.TimesRoman, 16f, PdfFontStyle.Regular);
            PdfFont timesRoman_15 = new PdfStandardFont(PdfFontFamily.TimesRoman, 15f);
            PdfColor colorNegro = new PdfColor(0, 0, 0);
            PdfColor colorAzulMarino = new PdfColor(52, 77, 117);
            PdfBrush solidBrush = new PdfSolidBrush(colorAzulMarino);
            //12.. Fuente para la Cabecera del documento
            PdfFont subHeadingFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 20);

            PdfFont timesRoman = new PdfStandardFont(PdfFontFamily.TimesRoman, 20);

            //13.Datos del Negocio.(Nombre, Dirección, Teléfono)
            PdfTextElement element = new PdfTextElement(SettingsOnline.oAplicacion.Nombre, timesRoman_18);
            element.Brush = new PdfSolidBrush(colorNegro);
            PdfLayoutResult result = element.Draw(page, new PointF(10, bounds.Bottom + 20));
            element = new PdfTextElement(SettingsOnline.oAplicacion.Direccion, timesRoman_18);
            element.Brush = new PdfSolidBrush(colorNegro);
            result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 5));
            element = new PdfTextElement(SettingsOnline.oAplicacion.Telefono, timesRoman_18);
            element.Brush = new PdfSolidBrush(colorNegro);
            result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 5));

            //14. Dibujar la cabecera del documento (Orden Fecha))
            bounds = new RectangleF(0, bounds.Bottom + 90, graphics.ClientSize.Width, 30);
            //14.1 Dibujar un lugar para la cabecera del documento.
            graphics.DrawRectangle(solidBrush, bounds);
            //14.2 Crear el texto del número de Orden            
            element = new PdfTextElement(TextsTranslateManager.Translate("Orden") + " : " + id.ToString(), subHeadingFont);
            element.Brush = PdfBrushes.White;
            //14.2.1 Dibujar en la cabecera
            result = element.Draw(page, new PointF(10, bounds.Top + 8));

            //14.3 Crear el texto de la Fecha de la Orden
            string currentDate = TextsTranslateManager.Translate("Fecha") + " : " + DateTime.Now.ToString("dd/MM/yyyy");
            //14.3.1 Medir el ancho del texto para localizarlo en el lugar correcto
            SizeF textSize = subHeadingFont.MeasureString(currentDate);
            PointF textPosition = new PointF(graphics.ClientSize.Width - textSize.Width - 10, result.Bounds.Y);
            //14.3.2 Dibujar la fecha en la cabecera
            graphics.DrawString(currentDate, subHeadingFont, element.Brush, textPosition);


            //15. Crear los elemenstos entre la cabecera y el detalle del documento(Cliente)
            element = new PdfTextElement(TextsTranslateManager.Translate("Cliente") + " : " + proveedor, timesRoman);
            element.Brush = new PdfSolidBrush(colorAzulMarino);
            result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 25));

            PdfPen linePen = new PdfPen(new PdfColor(126, 151, 173), 0.70f);
            PointF startPoint = new PointF(0, result.Bounds.Bottom + 3);
            PointF endPoint = new PointF(graphics.ClientSize.Width, result.Bounds.Bottom + 3);
            //Draws a line at the bottom of the address
            graphics.DrawLine(linePen, startPoint, endPoint);

            //16. Crear la fuente de datos de la tabla a visualizarse con los detalles
            DataTable uploadTable = new DataTable();
            switch (OrdenSeleccionado.CodigoEstadoEnConsulta != null ? OrdenSeleccionado.CodigoEstadoEnConsulta : OrdenSeleccionado.CodigoEstado)
            {
                case "Entregado":
                    uploadTable = OrdenDetalleCompraSeleccionado.DefaultView.ToTable(false, "IdItem", "NombreItem", "Unidad", "Cantidad", "CantidadEntregada", "CantidadSaldoEntrega");
                    uploadTable.Columns["IdItem"].ColumnName = "Id";
                    uploadTable.Columns["NombreItem"].ColumnName = "Item";
                    uploadTable.Columns["Unidad"].ColumnName = "Unidad";
                    uploadTable.Columns["Cantidad"].ColumnName = "Cant.";
                    uploadTable.Columns["CantidadEntregada"].ColumnName = "Cant. Entregada";
                    uploadTable.Columns["CantidadSaldoEntrega"].ColumnName = "Cant. Saldo";                    
                    break;                
            }
            //DataTable dtNew = dataTableColsToOtherType(uploadTable, typeof(string), new List<string>() { "ValorFinal" });

            DataTable invoiceDetails = uploadTable;//CreateOrdersTable();
                                                   //Creates a PDF grid
            PdfGrid grid = new PdfGrid();
            //Adds the data source
            grid.DataSource = formateaCuerpo(invoiceDetails);
            //Creates the grid cell styles
            PdfGridCellStyle cellStyle = new PdfGridCellStyle();
            cellStyle.Borders.All = PdfPens.White;
            PdfGridRow header = grid.Headers[0];
            //Creates the header style
            PdfGridCellStyle headerStyle = new PdfGridCellStyle();
            headerStyle.Borders.All = new PdfPen(new PdfColor(126, 151, 173));
            headerStyle.BackgroundBrush = new PdfSolidBrush(colorAzulMarino);
            headerStyle.TextBrush = PdfBrushes.White;
            headerStyle.Font = timesRoman_16_Re;// new PdfStandardFont(PdfFontFamily.TimesRoman, 18f, PdfFontStyle.Regular);

            //Adds cell customizations
            for (int i = 0; i < header.Cells.Count; i++)
            {
                if (i == 0 || i == 1)
                    header.Cells[i].StringFormat = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);
                else
                    header.Cells[i].StringFormat = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);
            }

            //Applies the header style
            header.ApplyStyle(headerStyle);
            PdfGridRowCollection cuerpo = grid.Rows;
            cellStyle.Borders.Bottom = new PdfPen(new PdfColor(217, 217, 217), 0.70f);
            cellStyle.Font = timesRoman_15;// new PdfStandardFont(PdfFontFamily.TimesRoman, 17f);
            cellStyle.TextBrush = new PdfSolidBrush(colorNegro);
            cellStyle.StringFormat = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);

            cuerpo.ApplyStyle(cellStyle);
            //Creates the layout format for grid
            PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat();
            // Creates layout format settings to allow the table pagination
            layoutFormat.Layout = PdfLayoutType.Paginate;
            //Draws the grid to the PDF page.
            PdfGridLayoutResult gridResult = grid.Draw(page, new RectangleF(new PointF(0, result.Bounds.Bottom + 40), new SizeF(graphics.ClientSize.Width, graphics.ClientSize.Height - 100)), layoutFormat);

            //2.Incluir para entrega
            // 2.1.Añadir una página al documento
            var detalleCompra=JsonConvert.DeserializeObject<ObservableCollection<OrdenDetalleCompra>>(OrdenSeleccionado.OrdenDetalleCompraSerializado);
            if (detalleCompra.Where(x=>x.EsServicio==1 && x.NombreItem.ToUpper().Contains("ERTIFIC")).ToList().Count()>0)
            {
                document = await CreateCertificadoServicioPdf(document);
            }
            if (detalleCompra.Where(x => x.EsServicio == 1 && !x.NombreItem.ToUpper().Contains("ERTIFIC")).ToList().Count() > 0)
            {
                document = await CreateContratoServicioPdf(document, detalleCompra.Where(x => x.EsServicio == 1 && !x.NombreItem.ToUpper().Contains("ERTIFIC")).FirstOrDefault().NombreItem);
            }

            //Save the PDF document to stream.
            document.Save(memoryStream);
            //Close the document.
            document.Close(true);

            //Save the stream as a file in the device and invoke it for viewing
            //Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("Output.pdf", "application/pdf", stream);

            return memoryStream;
        }
        public async Task<PdfDocument> CreateCertificadoServicioPdf(PdfDocument entrada)
        {
            PdfDocument document=entrada;
            //4. Añadir una página al documento
            PdfPage page = document.Pages.Add();
            //5. Declarar una variable de graficación
            PdfGraphics graphics = page.Graphics;
            //6. Almacenar el estado actual de las configuraciones
            PdfGraphicsState state = graphics.Save();

            //Loads the image as stream
            //string[] names = typeof(App).GetTypeInfo().Assembly.GetManifestResourceNames();
            //7. Obtener la imagen del logo del Negocio
            Stream imageStream = new MemoryStream(SettingsOnline.oAplicacion.Logo);//typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("GettingStarted.Assets.bg_header.png");
            PdfImage image = PdfImage.FromStream(imageStream);
            //8. Implementar en el documento como marca de agua
            graphics.SetTransparency(0.25f);
            graphics.DrawImage(image, new PointF(0, 0), page.Graphics.ClientSize);
            //9. Restaurar las configuraciones inciales del documento para graficación
            graphics.Restore(state);

            //10. Declarar una variable de lienzo donde se dibujará el objeto siguiente
            RectangleF bounds = new RectangleF(140, 0, 300, 130);
            //11. Dibujar el logo del Negocio en la cabecera del documento
            page.Graphics.DrawImage(image, bounds);

            //12. Definición de variables gráficas            
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 12);
            PdfFont timesRoman_18 = new PdfStandardFont(PdfFontFamily.TimesRoman, 18);
            PdfFont timesRoman_16_Re = new PdfStandardFont(PdfFontFamily.TimesRoman, 16f, PdfFontStyle.Regular);
            PdfFont timesRoman_15 = new PdfStandardFont(PdfFontFamily.TimesRoman, 15f);
            PdfColor colorNegro = new PdfColor(0, 0, 0);
            PdfColor colorAzulMarino = new PdfColor(52, 77, 117);
            PdfBrush solidBrush = new PdfSolidBrush(colorAzulMarino);
            //12.. Fuente para la Cabecera del documento
            PdfFont subHeadingFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 20);

            PdfFont timesRoman = new PdfStandardFont(PdfFontFamily.TimesRoman, 20);

            //13.Datos del Negocio.(Nombre, Dirección, Teléfono)
            PdfTextElement element = new PdfTextElement(SettingsOnline.oAplicacion.Nombre, font);
            element.Brush = new PdfSolidBrush(colorNegro);
            PdfLayoutResult result = element.Draw(page, new PointF(10, bounds.Bottom + 20));
            element = new PdfTextElement(SettingsOnline.oAplicacion.Direccion, font);
            element.Brush = new PdfSolidBrush(colorNegro);
            result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 5));
            element = new PdfTextElement(SettingsOnline.oAplicacion.Telefono, font);
            element.Brush = new PdfSolidBrush(colorNegro);
            result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 5));

            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            var ordenes = await _switchCompras.ConsultaPeriodoClienteOrdenes(new Orden()
            {
                Identificacion = OrdenSeleccionado.Identificacion
                                                                                    ,
                FechaDesde = DateTime.Now.AddMonths(-3)
                                                                                    ,
                FechaHasta = DateTime.Now
            }, estaConectado);
            var montoPromedio = Math.Round(ordenes.Average(x => x.ValorFinal),2);
            string monto = String.Format("{0:0.00}", montoPromedio);
            string text = "Quien suscribe, " + SettingsOnline.oAplicacion.Administrador + " con CI: 1308940442 con domicilio en " + SettingsOnline.oAplicacion.Direccion + " Administrador de " + SettingsOnline.oAplicacion.Nombre + " hace constar que el/la Señor/a " + OrdenSeleccionado.NombreCliente + "  con CI: " + OrdenSeleccionado.Identificacion + " ha venido realizando acuerdos comerciales como cliente de ésta Institución en los últimos 3 meses ";
            text = text + " con valores que en promedio ascienden a los " + monto + " dólares mensuales de manera regular.";
            text += "\n" + "El Cliente puede hacer uso del presente documento acorde a sus requerimientos comerciales y financieros.";
            text += "\n\n" + "Atentamente.";
            text += "\n\n" + "_________________________";
            text += "\n" + SettingsOnline.oAplicacion.Administrador;
            text += "\n" + SettingsOnline.oAplicacion.IdentificacionAdministrador;

            
            //Create a new PDF string format instance.
            PdfStringFormat format = new PdfStringFormat();
            //Set the text alignment.
            format.Alignment = PdfTextAlignment.Justify;
            //Draw string to PDF page.
            graphics.DrawString(text, font, PdfBrushes.Black, new RectangleF(10, result.Bounds.Bottom + 5, page.GetClientSize().Width-30, page.GetClientSize().Height), format);

            /*
            //Create a text element with the text and font
            PdfTextElement textElement = new PdfTextElement(text, new PdfStandardFont(PdfFontFamily.TimesRoman, 14));
            //Draw the text in the first column
            textElement.Draw( page, new RectangleF(10, result.Bounds.Bottom + 5, page.GetClientSize().Width, page.GetClientSize().Height),);
            */

            return document;
        }
        /// <summary>
        /// Crear un certificado del cliente
        /// </summary>
        /// <returns></returns>
        public async Task<Stream> CreateCertificadoClientePdf()
        {
            MemoryStream memoryStream = new MemoryStream();
            PdfDocument document = new PdfDocument();
            //4. Añadir una página al documento
            PdfPage page = document.Pages.Add();
            //5. Declarar una variable de graficación
            PdfGraphics graphics = page.Graphics;
            //6. Almacenar el estado actual de las configuraciones
            PdfGraphicsState state = graphics.Save();

            //Loads the image as stream
            //string[] names = typeof(App).GetTypeInfo().Assembly.GetManifestResourceNames();
            //7. Obtener la imagen del logo del Negocio
            Stream imageStream = new MemoryStream(SettingsOnline.oAplicacion.Logo);//typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("GettingStarted.Assets.bg_header.png");
            PdfImage image = PdfImage.FromStream(imageStream);
            //8. Implementar en el documento como marca de agua
            graphics.SetTransparency(0.25f);
            graphics.DrawImage(image, new PointF(0, 0), page.Graphics.ClientSize);
            //9. Restaurar las configuraciones inciales del documento para graficación
            graphics.Restore(state);

            //10. Declarar una variable de lienzo donde se dibujará el objeto siguiente
            RectangleF bounds = new RectangleF(140, 0, 300, 130);
            //11. Dibujar el logo del Negocio en la cabecera del documento
            page.Graphics.DrawImage(image, bounds);

            //12. Definición de variables gráficas            
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 12);
            PdfFont timesRoman_18 = new PdfStandardFont(PdfFontFamily.TimesRoman, 18);
            PdfFont timesRoman_16_Re = new PdfStandardFont(PdfFontFamily.TimesRoman, 16f, PdfFontStyle.Regular);
            PdfFont timesRoman_15 = new PdfStandardFont(PdfFontFamily.TimesRoman, 15f);
            PdfColor colorNegro = new PdfColor(0, 0, 0);
            PdfColor colorAzulMarino = new PdfColor(52, 77, 117);
            PdfBrush solidBrush = new PdfSolidBrush(colorAzulMarino);
            //12.. Fuente para la Cabecera del documento
            PdfFont subHeadingFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 20);

            PdfFont timesRoman = new PdfStandardFont(PdfFontFamily.TimesRoman, 20);

            //13.Datos del Negocio.(Nombre, Dirección, Teléfono)
            PdfTextElement element = new PdfTextElement(SettingsOnline.oAplicacion.Nombre, font);
            element.Brush = new PdfSolidBrush(colorNegro);
            PdfLayoutResult result = element.Draw(page, new PointF(10, bounds.Bottom + 20));
            element = new PdfTextElement(SettingsOnline.oAplicacion.Direccion, font);
            element.Brush = new PdfSolidBrush(colorNegro);
            result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 5));
            element = new PdfTextElement(SettingsOnline.oAplicacion.Telefono, font);
            element.Brush = new PdfSolidBrush(colorNegro);
            result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 5));

            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            var ordenes = await _switchCompras.ConsultaPeriodoClienteOrdenes(new Orden()
            {
                TipoOrden="Venta",
                Identificacion = OrdenSeleccionado.Identificacion
                                                                                    ,
                FechaDesde = DateTime.Now.AddMonths(-3)
                                                                                    ,
                FechaHasta = DateTime.Now
            }, estaConectado);
            var montoPromedio = Math.Round(ordenes.Average(x => x.ValorFinal), 2);
            string monto = String.Format("{0:0.00}", montoPromedio);
            string text = "Quien suscribe, " + SettingsOnline.oAplicacion.Administrador + " con CI: 1308940442 con domicilio en " + SettingsOnline.oAplicacion.Direccion + " Administrador de " + SettingsOnline.oAplicacion.Nombre + " hace constar que el/la Señor/a " + OrdenSeleccionado.NombreCliente + "  con CI: " + OrdenSeleccionado.Identificacion + " ha venido realizando acuerdos comerciales como cliente de ésta Institución en los últimos 3 meses ";
            text = text + " con valores que en promedio ascienden a los " + monto + " dólares mensuales de manera regular.";
            text += "\n" + "El Cliente puede hacer uso del presente documento acorde a sus requerimientos comerciales y financieros.";
            text += "\n\n" + "Atentamente.";
            text += "\n\n" + "_________________________";
            text += "\n" + SettingsOnline.oAplicacion.Administrador;
            text += "\n" + SettingsOnline.oAplicacion.IdentificacionAdministrador;


            //Create a new PDF string format instance.
            PdfStringFormat format = new PdfStringFormat();
            //Set the text alignment.
            format.Alignment = PdfTextAlignment.Justify;
            //Draw string to PDF page.
            graphics.DrawString(text, font, PdfBrushes.Black, new RectangleF(10, result.Bounds.Bottom + 5, page.GetClientSize().Width - 30, page.GetClientSize().Height), format);

            /*
            //Create a text element with the text and font
            PdfTextElement textElement = new PdfTextElement(text, new PdfStandardFont(PdfFontFamily.TimesRoman, 14));
            //Draw the text in the first column
            textElement.Draw( page, new RectangleF(10, result.Bounds.Bottom + 5, page.GetClientSize().Width, page.GetClientSize().Height),);
            */

            //Save the PDF document to stream.
            document.Save(memoryStream);
            //Close the document.
            document.Close(true);

            //Save the stream as a file in the device and invoke it for viewing
            //Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("Output.pdf", "application/pdf", stream);

            return memoryStream;
        }


        public async Task<PdfDocument> CreateContratoServicioPdf(PdfDocument entrada,string conceptoServicio)
        {
            PdfDocument document = entrada;
            //4. Añadir una página al documento
            PdfPage page = document.Pages.Add();
            //5. Declarar una variable de graficación
            PdfGraphics graphics = page.Graphics;
            //6. Almacenar el estado actual de las configuraciones
            PdfGraphicsState state = graphics.Save();

            //Loads the image as stream
            //string[] names = typeof(App).GetTypeInfo().Assembly.GetManifestResourceNames();
            //7. Obtener la imagen del logo del Negocio
            Stream imageStream = new MemoryStream(SettingsOnline.oAplicacion.Logo);//typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("GettingStarted.Assets.bg_header.png");
            PdfImage image = PdfImage.FromStream(imageStream);
            //8. Implementar en el documento como marca de agua
            graphics.SetTransparency(0.25f);
            graphics.DrawImage(image, new PointF(0, 0), page.Graphics.ClientSize);
            //9. Restaurar las configuraciones inciales del documento para graficación
            graphics.Restore(state);

            //10. Declarar una variable de lienzo donde se dibujará el objeto siguiente
            RectangleF bounds = new RectangleF(140, 0, 300, 130);
            //11. Dibujar el logo del Negocio en la cabecera del documento
            page.Graphics.DrawImage(image, bounds);

            //12. Definición de variables gráficas            
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 12);
            PdfFont timesRoman_18 = new PdfStandardFont(PdfFontFamily.TimesRoman, 18);
            PdfFont timesRoman_16_Re = new PdfStandardFont(PdfFontFamily.TimesRoman, 16f, PdfFontStyle.Regular);
            PdfFont timesRoman_15 = new PdfStandardFont(PdfFontFamily.TimesRoman, 15f);
            PdfColor colorNegro = new PdfColor(0, 0, 0);
            PdfColor colorAzulMarino = new PdfColor(52, 77, 117);
            PdfBrush solidBrush = new PdfSolidBrush(colorAzulMarino);
            //12.. Fuente para la Cabecera del documento
            PdfFont subHeadingFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 20);

            PdfFont timesRoman = new PdfStandardFont(PdfFontFamily.TimesRoman, 20);

            //13.Datos del Negocio.(Nombre, Dirección, Teléfono)
            PdfTextElement element = new PdfTextElement(SettingsOnline.oAplicacion.Nombre, font);
            element.Brush = new PdfSolidBrush(colorNegro);
            PdfLayoutResult result = element.Draw(page, new PointF(10, bounds.Bottom + 20));
            element = new PdfTextElement(SettingsOnline.oAplicacion.Direccion, font);
            element.Brush = new PdfSolidBrush(colorNegro);
            result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 5));
            element = new PdfTextElement(SettingsOnline.oAplicacion.Telefono, font);
            element.Brush = new PdfSolidBrush(colorNegro);
            result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 5));


            string text = "El presente documento, representa la constancia sobre la relación de Servicios entre " + SettingsOnline.oAplicacion.Nombre + " ,representada por " + SettingsOnline.oAplicacion.Administrador + " y el Señor/a " + OrdenSeleccionado.NombreCliente + " por concepto de " + conceptoServicio + ".";
            text += "\n" + "El Cliente puede hacer uso del presente documento acorde a sus requerimientos comerciales y financieros.";
            text += "\n\n";
            text += "\n\n" + "_________________________";
            text += "\n" + SettingsOnline.oAplicacion.Administrador;
            text += "\n" + SettingsOnline.oAplicacion.IdentificacionAdministrador;


            //Create a new PDF string format instance.
            PdfStringFormat format = new PdfStringFormat();
            //Set the text alignment.
            format.Alignment = PdfTextAlignment.Justify;
            //Draw string to PDF page.
            graphics.DrawString(text, font, PdfBrushes.Black, new RectangleF(10, result.Bounds.Bottom + 5, page.GetClientSize().Width - 30, page.GetClientSize().Height), format);

            /*
            //Create a text element with the text and font
            PdfTextElement textElement = new PdfTextElement(text, new PdfStandardFont(PdfFontFamily.TimesRoman, 14));
            //Draw the text in the first column
            textElement.Draw( page, new RectangleF(10, result.Bounds.Bottom + 5, page.GetClientSize().Width, page.GetClientSize().Height),);
            */

            return document;
        }


        private List<object> formateaCuerpo(DataTable dataTable)
        {
            List<object> data = new List<object>();
            switch (OrdenSeleccionado.CodigoEstadoEnConsulta != null ? OrdenSeleccionado.CodigoEstadoEnConsulta : OrdenSeleccionado.CodigoEstado)
            {
                case "Ingresado":
                                foreach (DataRow x in dataTable.Rows)
                                {
                                    Object row = new { Id= x.ItemArray[0]
                                                     , Item= x.ItemArray[1]
                                                     , Unidad= x.ItemArray[2]
                                                     , Cant= String.Format("{0:0.00}", x.ItemArray[3])
                                                     , Precio = String.Format("{0:0.00}", x.ItemArray[4])
                                                     , Incremento = String.Format("{0:0.00}", x.ItemArray[5])
                                                     , Subtotal= String.Format("{0:0.00}", x.ItemArray[6])};
                                    data.Add(row);
                                }                    

                    break;
                case "Cancelado":
                    foreach (DataRow x in dataTable.Rows)
                    {
                        Object row = new
                        {
                            Id = x.ItemArray[0]
                                         ,
                            Item = x.ItemArray[1]
                                         ,
                            Unidad = x.ItemArray[2]
                                         ,
                            Cant = String.Format("{0:0.00}", x.ItemArray[3])
                                         ,
                            Precio = String.Format("{0:0.00}", x.ItemArray[4])
                            //             ,
                            //Incremento = String.Format("{0:0.00}", x.ItemArray[5])
                                         ,
                            Subtotal = String.Format("{0:0.00}", x.ItemArray[5])
                        };
                        data.Add(row);
                    }
                    break;
                case "Entregado":
                    foreach (DataRow x in dataTable.Rows)
                    {
                        Object row = new
                        {
                            Id = x.ItemArray[0]
                                         ,
                            Item = x.ItemArray[1]
                                         ,
                            Unidad = x.ItemArray[2]
                                         ,
                            Cant = String.Format("{0:0.00}", x.ItemArray[3])
                                         ,
                            Entregada = String.Format("{0:0.00}", x.ItemArray[4])
                            //             ,
                            //Incremento = String.Format("{0:0.00}", x.ItemArray[5])
                                         ,
                            Saldo = String.Format("{0:0.00}", x.ItemArray[5])
                        };
                        data.Add(row);
                    }
                    break;
            }
            return data;
        }

        private void PdfGrid_BeginCellLayout(object sender, PdfGridBeginCellLayoutEventArgs args)
        {
            count++;
            PdfGrid grid = (sender as PdfGrid);
            if (count <= grid.Headers.Count * grid.Columns.Count)
            {
                args.Skip = true;
            }
        }

        public DataTable dataTableColsToOtherType(DataTable dt, Type type, List<string> colsForTypeChange = default(List<string>))
        {
            var dt2 = new DataTable();
            foreach (DataColumn c in dt.Columns)
            {
                if (colsForTypeChange != null && colsForTypeChange.Count > 0)
                {
                    if (colsForTypeChange.Contains(c.ColumnName))
                        dt2.Columns.Add(c.ColumnName, type);//Change column type if found in list "colsForTypeChange"
                    else dt2.Columns.Add(c.ColumnName, c.DataType);//No change in Column Type
                }
                else
                {
                    dt2.Columns.Add(c.ColumnName, type);//change all columns type to provided type
                }

            }
            dt2.Load(dt.CreateDataReader(), System.Data.LoadOption.OverwriteChanges);
            return dt2;
        }



        /*
         public async Task<Stream> CreateOrdenVentaPdf()
        {
            MemoryStream memoryStream = new MemoryStream();

            string id = Generator.GenerarOrdenNo("OV", OrdenSeleccionado.NumeroOrden);
            string proveedor = OrdenSeleccionado.NombreCliente;
            //Creates a new PDF document
            PdfDocument document = new PdfDocument();
            //Adds page settings
            document.PageSettings.Orientation = PdfPageOrientation.Portrait;
            document.PageSettings.Margins.All = 30;
            //Adds a page to the document
            PdfPage page = document.Pages.Add();
            PdfGraphics graphics = page.Graphics;

            //Loads the image as stream
            //string[] names = typeof(App).GetTypeInfo().Assembly.GetManifestResourceNames();
            Stream imageStream = new MemoryStream(SettingsOnline.oAplicacion.Logo);//typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("GettingStarted.Assets.bg_header.png");
            RectangleF bounds = new RectangleF(140, 0, 300, 130);
            PdfImage image = PdfImage.FromStream(imageStream);
            //Draws the image to the PDF page
            page.Graphics.DrawImage(image, bounds);

            PdfBrush solidBrush = new PdfSolidBrush(new PdfColor(126, 151, 173));
            bounds = new RectangleF(0, bounds.Bottom + 90, graphics.ClientSize.Width, 30);
            //Draws a rectangle to place the heading in that region.
            graphics.DrawRectangle(solidBrush, bounds);
            //Creates a font for adding the heading in the page
            PdfFont subHeadingFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 20);
            //Creates a text element to add the invoice number            
            PdfTextElement element = new PdfTextElement(TextsTranslateManager.Translate("Orden") + " : " + id.ToString(), subHeadingFont);
            element.Brush = PdfBrushes.White;

            //Draws the heading on the page
            PdfLayoutResult result = element.Draw(page, new PointF(10, bounds.Top + 8));
            string currentDate = TextsTranslateManager.Translate("Fecha") + " : " + DateTime.Now.ToString("dd/MM/yyyy");
            //Measures the width of the text to place it in the correct location
            SizeF textSize = subHeadingFont.MeasureString(currentDate);
            PointF textPosition = new PointF(graphics.ClientSize.Width - textSize.Width - 10, result.Bounds.Y);
            //Draws the date by using DrawString method
            graphics.DrawString(currentDate, subHeadingFont, element.Brush, textPosition);
            PdfFont timesRoman = new PdfStandardFont(PdfFontFamily.TimesRoman, 20);
            //Creates text elements to add the address and draw it to the page.
            element = new PdfTextElement(TextsTranslateManager.Translate("Cliente") + " : " + proveedor, timesRoman);
            element.Brush = new PdfSolidBrush(new PdfColor(126, 155, 203));
            result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 25));
            PdfPen linePen = new PdfPen(new PdfColor(126, 151, 173), 0.70f);
            PointF startPoint = new PointF(0, result.Bounds.Bottom + 3);
            PointF endPoint = new PointF(graphics.ClientSize.Width, result.Bounds.Bottom + 3);
            //Draws a line at the bottom of the address
            graphics.DrawLine(linePen, startPoint, endPoint);

            //Creates the datasource for the table
            DataTable uploadTable = new DataTable();
            switch (OrdenSeleccionado.CodigoEstadoEnConsulta != null ? OrdenSeleccionado.CodigoEstadoEnConsulta : OrdenSeleccionado.CodigoEstado)
            {
                case "Ingresado":
                    uploadTable = OrdenDetalleCompraSeleccionado.DefaultView.ToTable(false, "IdItem", "NombreItem", "Unidad", "Cantidad", "ValorUnitario", "ValorFinal");
                    uploadTable.Columns["IdItem"].ColumnName = "Id";
                    uploadTable.Columns["NombreItem"].ColumnName = "Item";
                    uploadTable.Columns["Cantidad"].ColumnName = "Cant.";
                    uploadTable.Columns["ValorUnitario"].ColumnName = "Val.Unit.";
                    uploadTable.Columns["ValorFinal"].ColumnName = "Subtotal";
                    break;
                case "Recibido":
                    uploadTable = OrdenDetalleCompraSeleccionado.DefaultView.ToTable(false, "IdItem", "NombreItem", "TipoDescuento", "Unidad", "Cantidad", "ValorUnitario", "ValorCompra", "ValorDescuento", "ValorFinal");
                    uploadTable.Columns["IdItem"].ColumnName = "Id";
                    uploadTable.Columns["NombreItem"].ColumnName = "Item";
                    uploadTable.Columns["TipoDescuento"].ColumnName = "Tipo Descto.";
                    uploadTable.Columns["Cantidad"].ColumnName = "Cant.";
                    uploadTable.Columns["ValorUnitario"].ColumnName = "Val.Unit.";
                    uploadTable.Columns["ValorCompra"].ColumnName = "Val.Compra.";
                    uploadTable.Columns["ValorDescuento"].ColumnName = "Val.Descto.";
                    uploadTable.Columns["ValorFinal"].ColumnName = "Subtotal";
                    break;
            }

            DataTable invoiceDetails = uploadTable;//CreateOrdersTable();
                                                   //Creates a PDF grid
            PdfGrid grid = new PdfGrid();
            //Adds the data source
            grid.DataSource = invoiceDetails;
            //Creates the grid cell styles
            PdfGridCellStyle cellStyle = new PdfGridCellStyle();
            cellStyle.Borders.All = PdfPens.White;
            PdfGridRow header = grid.Headers[0];
            //Creates the header style
            PdfGridCellStyle headerStyle = new PdfGridCellStyle();
            headerStyle.Borders.All = new PdfPen(new PdfColor(126, 151, 173));
            headerStyle.BackgroundBrush = new PdfSolidBrush(new PdfColor(126, 151, 173));
            headerStyle.TextBrush = PdfBrushes.White;
            headerStyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 18f, PdfFontStyle.Regular);

            //Adds cell customizations
            for (int i = 0; i < header.Cells.Count; i++)
            {
                if (i == 0 || i == 1)
                    header.Cells[i].StringFormat = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);
                else
                    header.Cells[i].StringFormat = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);
            }

            //Applies the header style
            header.ApplyStyle(headerStyle);
            PdfGridRowCollection cuerpo = grid.Rows;
            cellStyle.Borders.Bottom = new PdfPen(new PdfColor(217, 217, 217), 0.70f);
            cellStyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 17f);
            cellStyle.TextBrush = new PdfSolidBrush(new PdfColor(131, 130, 136));
            cellStyle.StringFormat = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);

            cuerpo.ApplyStyle(cellStyle);
            //Creates the layout format for grid
            PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat();
            // Creates layout format settings to allow the table pagination
            layoutFormat.Layout = PdfLayoutType.Paginate;
            //Draws the grid to the PDF page.
            PdfGridLayoutResult gridResult = grid.Draw(page, new RectangleF(new PointF(0, result.Bounds.Bottom + 40), new SizeF(graphics.ClientSize.Width, graphics.ClientSize.Height - 100)), layoutFormat);

            //Save the PDF document to stream.
            document.Save(memoryStream);
            //Close the document.
            document.Close(true);

            //Save the stream as a file in the device and invoke it for viewing
            //Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("Output.pdf", "application/pdf", stream);

            return memoryStream;
        }

         */

        public static string ReturnFormattedPrice(string priceWithOutCurrencySymbol)
        {

            string[] values = new string[2];
            List<string> valueList = new List<string>();

            valueList.Add("");
            valueList.Add("");

            // cause it will be either dot or comma 
            if (priceWithOutCurrencySymbol.Contains(","))
                values = priceWithOutCurrencySymbol.Split( ',' );
            else
                values = priceWithOutCurrencySymbol.Split('.');

            valueList[0] = values[0];


            if (values.Length == 1)
                valueList[1] = ",00";
            else if (values[1].Length == 1)
                valueList[1] = "," + values[1] + "0";
            else
                valueList[1] = "," + values[1];



            return valueList[0] + valueList[1];

        }


        #endregion FormtosPdf

    }

}
