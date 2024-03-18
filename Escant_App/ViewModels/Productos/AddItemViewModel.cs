using DataModel.DTO.Administracion;
using DataModel.DTO.Iteraccion;
using DataModel.DTO.Productos;
using DataModel.Helpers;
using ManijodaServicios.AppSettings;
using ManijodaServicios.Offline.Interfaz;
using ManijodaServicios.Resources.Texts;
using ManijodaServicios.Switch;
using Escant_App.Validations;
using Escant_App.ViewModels.Administracion;
using Escant_App.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Escant_App.ViewModels.Productos
{
    class AddItemViewModel : ViewModelBase
    {
        #region 0.DeclaracionVariables
        private SwitchIteraccion _switchIteraccion;
        private SwitchProductos _switchProductos;
        private Generales.Generales generales;

        private readonly IServicioIteraccion_Galeria _galeriaServicio;
        private readonly IServicioProductos_Item _itemServicio;
        private readonly IServicioProductos_Precio _precioServicio;


        /// 0.1 Referencias a instancias de objetos a utilizarse en la Clase        
        private Galeria _galeria;
        private Galeria _galeria1;
        private Item _registro;
        private Precio _registro1;

        private CultureInfo _cultura;//=new System.Globalization.CultureInfo("ja-JP")

        private bool _isEditMode;

        /// <summary>
        /// 0.2 Declaración de las variables de lectura y escritura para los controles
        /// </summary>
        private ImageSource _fuenteImagenRegistro = ImageSource.FromStream(() =>
        {
            var stream = new MemoryStream();
            return stream;
        });
        private ImageSource _fuenteImagenRegistro1 = ImageSource.FromStream(() =>
        {
            var stream = new MemoryStream();
            return stream;
        });
        private string _id;
        private bool _esServicio;
        private string _codigo;
        private ValidatableObject<string> _nombre;
        private ValidatableObject<string> _idMarca;
        private string _marca;
        private ValidatableObject<string> _idCategoria;
        private string _categoria;
        private ValidatableObject<string> _idUnidad;
        private string _unidad;
        private string _idTipoImpuesto;
        private string _tipoImpuesto;
        private float _porcentajeImpuesto;
        private string _idTipoAplicacionImpuesto;
        private string _tipoAplicacionImpuesto;
        private string _idPeriodoIncremento;
        private string _periodoIncremento;
        private float _valorPeriodoIncremento;
        private string _idPeriodoServicio;
        private string _periodoServicio;
        private float _cantidadPeriodoServicio;
        private float _valorIncremento;
        private float _valorUnidadOriginal;
        private bool _esParticionable;
        private string _idGaleria;
        private string _imagen;
        private string _galeriaImagen;
        private int _stockMinimo;
        private ValidatableObject<float> _porcentajeRentabilidad;
        private ValidatableObject<float> _porcentajeGastoOperativo;
        private ValidatableObject<float> _precioCompra;
        private ValidatableObject<float> _precioVenta;
        private ValidatableObject<float> _precioBase;
        private float _valorGastoOperativo;
        private string _valorGastoOperativoFormato;
        private float _valorRentabilidad;
        private float _valorUtilidad;
        private float _valorImpuesto;
        private float _precioVentaImpuesto;
        private bool _poseeStock;
        private string _descripcion;
        private string _itemStockRegistradoSerializado;

        /// <summary>
        /// 0.3 Declaración variables control de datos
        /// </summary>

        private bool _isRegistroExiste;
        private bool _isRegistroExiste1;

        private bool _isVisibleCondicionesServicio;
        private bool _isVisibleMarca;
        private bool _isVisibleCategoria;
        private bool _isVisibleEsParticionable;        
        private bool _isVisibleStockMinimo;
        private bool _isVisiblePorcentajeRentabilidad;
        private bool _isVisiblePorcentajeGastoOperativo;
        private bool _isVisiblePrecioCompra;
        private bool _isVisiblePrecioVenta;
        private bool _isVisibleValorGastoOperativo;
        private bool _isVisibleValorRentabilidad;
        private bool _isVisibleValorUtilidad;

        private bool _isEnabledPrecio;

        /// <summary>
        /// 0.4 Declaración de las variables de control de errores
        /// </summary>
        private bool _isNombreValid;
        private bool _isValorUnidadOriginalValid;
        private bool _isStockMinimoValid;
        private bool _isPrecioBaseValid;
        private bool _isPrecioCompraValid;
        private bool _isPrecioVentaValid;

        /// 0.5 Referencias a objetos obtenidos externamente
        private Empresa _marcaSeleccionada;
        private Catalogo _unidadSeleccionada;
        private Catalogo _categoriaSeleccionada;
        private Catalogo _tipoImpuestoSeleccionado;
        private Catalogo _tipoAplicacionImpuestoSeleccionado;
        private Catalogo _periodoIncrementoSeleccionado;
        private Catalogo _periodoServicioSeleccionado;

        #endregion 0.DeclaracionVariables
        #region 1.InstanciaAsignaVariablesControles
        public ImageSource FuenteImagenRegistro
        {
            get
            {
                return _fuenteImagenRegistro;
            }
            set
            {
                _fuenteImagenRegistro = value;
                RaisePropertyChanged(() => FuenteImagenRegistro);
            }
        }
        public ImageSource FuenteImagenRegistro1
        {
            get
            {
                return _fuenteImagenRegistro1;
            }
            set
            {
                _fuenteImagenRegistro1 = value;
                RaisePropertyChanged(() => FuenteImagenRegistro1);
            }
        }
        public string Id
        {
            get => _id;
            set
            {
                _id = value;
                RaisePropertyChanged(() => Id);
            }
        }
        public bool EsServicio
        {
            get => _esServicio;
            set
            {
                _esServicio = value;
                RaisePropertyChanged(() => EsServicio);
                eventoEsServicio();
            }
        }
        public string Codigo
        {
            get => _codigo;
            set
            {
                _codigo = value;
                RaisePropertyChanged(() => Codigo);
            }
        }
        public ValidatableObject<string> Nombre
        {
            get => _nombre;
            set
            {
                _nombre = value;
                RaisePropertyChanged(() => Nombre);
            }
        }
        public ValidatableObject<string> IdMarca
        {
            get => _idMarca;
            set
            {
                _idMarca = value;
                RaisePropertyChanged(() => IdMarca);
            }
        }
        public string Marca
        {
            get => _marca;
            set
            {
                _marca = value;
                RaisePropertyChanged(() => Marca);
            }
        }
        public ValidatableObject<string> IdCategoria
        {
            get => _idCategoria;
            set
            {
                _idCategoria = value;
                RaisePropertyChanged(() => IdCategoria);
            }
        }
        public string Categoria
        {
            get => _categoria;
            set
            {
                _categoria = value;
                RaisePropertyChanged(() => Categoria);
            }
        }
        public ValidatableObject<string> IdUnidad
        {
            get => _idUnidad;
            set
            {
                _idUnidad = value;
                RaisePropertyChanged(() => IdUnidad);
            }
        }
        public string Unidad
        {
            get => _unidad;
            set
            {
                _unidad = value;
                RaisePropertyChanged(() => Unidad);
            }
        }
        public string IdTipoImpuesto
        {
            get => _idTipoImpuesto;
            set
            {
                _idTipoImpuesto = value;
                RaisePropertyChanged(() => IdTipoImpuesto);
            }
        }
        public string TipoImpuesto
        {
            get => _tipoImpuesto;
            set
            {
                _tipoImpuesto = value;
                RaisePropertyChanged(() => TipoImpuesto);
            }
        }
        public float PorcentajeImpuesto
        {
            get => _porcentajeImpuesto;
            set
            {
                _porcentajeImpuesto = value;
                RaisePropertyChanged(() => PorcentajeImpuesto);
                CalculaValores(1);
            }
        }
        public string IdTipoAplicacionImpuesto
        {
            get => _idTipoAplicacionImpuesto;
            set
            {
                _idTipoAplicacionImpuesto = value;
                RaisePropertyChanged(() => IdTipoAplicacionImpuesto);
            }
        }
        public string TipoAplicacionImpuesto
        {
            get => _tipoAplicacionImpuesto;
            set
            {
                _tipoAplicacionImpuesto = value;
                RaisePropertyChanged(() => TipoAplicacionImpuesto);
                CalculaValores(1);
            }
        }
        public string IdPeriodoIncremento
        {
            get => _idPeriodoIncremento;
            set
            {
                _idPeriodoIncremento = value;
                RaisePropertyChanged(() => IdPeriodoIncremento);
            }
        }
        public string PeriodoIncremento
        {
            get => _periodoIncremento;
            set
            {
                _periodoIncremento = value;
                RaisePropertyChanged(() => PeriodoIncremento);                
            }
        }
        public float ValorPeriodoIncremento
        {
            get => _valorPeriodoIncremento;
            set
            {
                _valorPeriodoIncremento = value;
                RaisePropertyChanged(() => ValorPeriodoIncremento);
            }
        }
        public string IdPeriodoServicio
        {
            get => _idPeriodoServicio;
            set
            {
                _idPeriodoServicio = value;
                RaisePropertyChanged(() => IdPeriodoServicio);
            }
        }
        public string PeriodoServicio
        {
            get => _periodoServicio;
            set
            {
                _periodoServicio = value;
                RaisePropertyChanged(() => PeriodoServicio);
            }
        }
        public float CantidadPeriodoServicio
        {
            get => _cantidadPeriodoServicio;
            set
            {
                _cantidadPeriodoServicio = value;
                RaisePropertyChanged(() => CantidadPeriodoServicio);
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

        public float ValorUnidadOriginal
        {
            get => _valorUnidadOriginal;
            set
            {
                _valorUnidadOriginal = value;
                RaisePropertyChanged(() => ValorUnidadOriginal);
            }
        }
        public bool EsParticionable
        {
            get => _esParticionable;
            set
            {
                _esParticionable = value;
                RaisePropertyChanged(() => EsParticionable);
                eventoEsParticionable();
            }
        }
        public string IdGaleria
        {
            get => _idGaleria;
            set
            {
                _idGaleria = value;
                RaisePropertyChanged(() => IdGaleria);
            }
        }
        public string Imagen
        {
            get => _imagen;
            set
            {
                _imagen = value;
                RaisePropertyChanged(() => Imagen);
            }
        }
        public string GaleriaImagen
        {
            get => _galeriaImagen;
            set
            {
                _galeriaImagen = value;
                RaisePropertyChanged(() => GaleriaImagen);
            }
        }
        public int StockMinimo
        {
            get => _stockMinimo;
            set
            {
                _stockMinimo = value;
                RaisePropertyChanged(() => StockMinimo);
            }
        }
        public ValidatableObject<float> PorcentajeRentabilidad
        {
            get => _porcentajeRentabilidad;
            set
            {
                _porcentajeRentabilidad = value;
                RaisePropertyChanged(() => PorcentajeRentabilidad);
            }
        }
        public ValidatableObject<float> PorcentajeGastoOperativo
        {
            get => _porcentajeGastoOperativo;
            set
            {
                _porcentajeGastoOperativo = value;
                RaisePropertyChanged(() => PorcentajeGastoOperativo);
            }
        }
        public ValidatableObject<float> PrecioCompra
        {
            get => _precioCompra;
            set
            {
                _precioCompra = value;
                RaisePropertyChanged(() => PrecioCompra);
            }
        }
        public ValidatableObject<float> PrecioVenta
        {
            get => _precioVenta;
            set
            {
                _precioVenta = value;
                RaisePropertyChanged(() => PrecioVenta);
            }
        }
        public ValidatableObject<float> PrecioBase
        {
            get => _precioBase;
            set
            {
                _precioBase = value;
                RaisePropertyChanged(() => PrecioBase);
            }
        }
        public float ValorRentabilidad
        {
            get => _valorRentabilidad;
            set
            {
                _valorRentabilidad = value;
                RaisePropertyChanged(() => ValorRentabilidad);
            }
        }
        public float ValorUtilidad
        {
            get => _valorUtilidad;
            set
            {
                _valorUtilidad = value;
                RaisePropertyChanged(() => ValorUtilidad);
            }
        }
        public float ValorImpuesto
        {
            get => _valorImpuesto;
            set
            {
                _valorImpuesto = value;
                RaisePropertyChanged(() => ValorImpuesto);
            }
        }
        public float PrecioVentaImpuesto
        {
            get => _precioVentaImpuesto;
            set
            {
                _precioVentaImpuesto = value;
                RaisePropertyChanged(() => PrecioVentaImpuesto);
            }
        }
        public float ValorGastoOperativo
        {
            get => _valorGastoOperativo;
            set
            {
                _valorGastoOperativo = value;
                RaisePropertyChanged(() => ValorGastoOperativo);
            }
        }
        public bool PoseeStock
        {
            get => _poseeStock;
            set
            {
                _poseeStock = value;
                RaisePropertyChanged(() => PoseeStock);
            }
        }
        public string Descripcion
        {
            get => _descripcion;
            set
            {
                _descripcion = value;
                RaisePropertyChanged(() => Descripcion);
            }
        }
        public string ItemStockRegistradoSerializado
        {
            get => _itemStockRegistradoSerializado;
            set
            {
                _itemStockRegistradoSerializado = value;
                RaisePropertyChanged(() => ItemStockRegistradoSerializado);
            }
        }

        public bool IsNombreValid
        {
            get => _isNombreValid;
            set
            {
                _isNombreValid = value;
                RaisePropertyChanged(() => IsNombreValid);
            }
        }

        public bool IsValorUnidadOriginalValid
        {
            get => _isValorUnidadOriginalValid;
            set
            {
                _isValorUnidadOriginalValid = value;
                RaisePropertyChanged(() => IsValorUnidadOriginalValid);
            }
        }

        public bool IsStockMinimoValid
        {
            get => _isStockMinimoValid;
            set
            {
                _isStockMinimoValid = value;
                RaisePropertyChanged(() => IsStockMinimoValid);
            }
        }
        public bool IsPrecioCompraValid
        {
            get => _isPrecioCompraValid;
            set
            {
                _isPrecioCompraValid = value;
                RaisePropertyChanged(() => IsPrecioCompraValid);
            }
        }
        public bool IsPrecioBaseValid
        {
            get => _isPrecioBaseValid;
            set
            {
                _isPrecioBaseValid = value;
                RaisePropertyChanged(() => IsPrecioBaseValid);
            }
        }
        public bool IsPrecioVentaValid
        {
            get => _isPrecioVentaValid;
            set
            {
                _isPrecioVentaValid = value;
                RaisePropertyChanged(() => IsPrecioVentaValid);
            }
        }

        public Empresa MarcaSeleccionada
        {
            get => _marcaSeleccionada;
            set
            {
                _marcaSeleccionada = value;
                RaisePropertyChanged(() => MarcaSeleccionada);
            }
        }

        public Catalogo UnidadSeleccionada
        {
            get => _unidadSeleccionada;
            set
            {
                _unidadSeleccionada = value;
                RaisePropertyChanged(() => UnidadSeleccionada);
            }
        }
        public Catalogo TipoImpuestoSeleccionado
        {
            get => _tipoImpuestoSeleccionado;
            set
            {
                _tipoImpuestoSeleccionado = value;
                RaisePropertyChanged(() => TipoImpuestoSeleccionado);
            }
        }
        public Catalogo TipoAplicacionImpuestoSeleccionado
        {
            get => _tipoAplicacionImpuestoSeleccionado;
            set
            {
                _tipoAplicacionImpuestoSeleccionado = value;
                RaisePropertyChanged(() => TipoAplicacionImpuestoSeleccionado);
            }
        }
        public Catalogo PeriodoIncrementoSeleccionado
        {
            get => _periodoIncrementoSeleccionado;
            set
            {
                _periodoIncrementoSeleccionado = value;
                RaisePropertyChanged(() => PeriodoIncrementoSeleccionado);
            }
        }
        public Catalogo PeriodoServicioSeleccionado
        {
            get => _periodoServicioSeleccionado;
            set
            {
                _periodoServicioSeleccionado = value;
                RaisePropertyChanged(() => PeriodoServicioSeleccionado);
            }
        }
        public Catalogo CategoriaSeleccionada
        {
            get => _categoriaSeleccionada;
            set
            {
                _categoriaSeleccionada = value;
                RaisePropertyChanged(() => CategoriaSeleccionada);
            }
        }


        public bool IsVisibleCondicionesServicio
        {
            get => _isVisibleCondicionesServicio;
            set
            {
                _isVisibleCondicionesServicio = value;
                RaisePropertyChanged(() => IsVisibleCondicionesServicio);
            }
        }

        public bool IsVisibleMarca
        {
            get => _isVisibleMarca;
            set
            {
                _isVisibleMarca = value;
                RaisePropertyChanged(() => IsVisibleMarca);
            }
        }

        public bool IsVisibleCategoria
        {
            get => _isVisibleCategoria;
            set
            {
                _isVisibleCategoria = value;
                RaisePropertyChanged(() => IsVisibleCategoria);
            }
        }

        public bool IsVisibleEsParticionable
        {
            get => _isVisibleEsParticionable;
            set
            {
                _isVisibleEsParticionable = value;
                RaisePropertyChanged(() => IsVisibleEsParticionable);
            }
        }
        public bool IsVisibleStockMinimo
        {
            get => _isVisibleStockMinimo;
            set
            {
                _isVisibleStockMinimo = value;
                RaisePropertyChanged(() => IsVisibleStockMinimo);
            }
        }
        public bool IsVisiblePrecioCompra
        {
            get => _isVisiblePrecioCompra;
            set
            {
                _isVisiblePrecioCompra = value;
                RaisePropertyChanged(() => IsVisiblePrecioCompra);
            }
        }
        public bool IsVisiblePrecioVenta
        {
            get => _isVisiblePrecioVenta;
            set
            {
                _isVisiblePrecioVenta = value;
                RaisePropertyChanged(() => IsVisiblePrecioVenta);
            }
        }
        public bool IsVisiblePorcentajeRentabilidad
        {
            get => _isVisiblePorcentajeRentabilidad;
            set
            {
                _isVisiblePorcentajeRentabilidad = value;
                RaisePropertyChanged(() => IsVisiblePorcentajeRentabilidad);
            }
        }
        public bool IsVisiblePorcentajeGastoOperativo
        {
            get => _isVisiblePorcentajeGastoOperativo;
            set
            {
                _isVisiblePorcentajeGastoOperativo = value;
                RaisePropertyChanged(() => IsVisiblePorcentajeGastoOperativo);
            }
        }
        public bool IsVisibleValorGastoOperativo
        {
            get => _isVisibleValorGastoOperativo;
            set
            {
                _isVisibleValorGastoOperativo = value;
                RaisePropertyChanged(() => IsVisibleValorGastoOperativo);
            }
        }
        public bool IsVisibleValorRentabilidad
        {
            get => _isVisibleValorRentabilidad;
            set
            {
                _isVisibleValorRentabilidad = value;
                RaisePropertyChanged(() => IsVisibleValorRentabilidad);
            }
        }
        public bool IsVisibleValorUtilidad
        {
            get => _isVisibleValorUtilidad;
            set
            {
                _isVisibleValorUtilidad = value;
                RaisePropertyChanged(() => IsVisibleValorUtilidad);
            }
        }
        public bool IsEnabledPrecio
        {
            get => _isEnabledPrecio;
            set
            {
                _isEnabledPrecio = value;
                RaisePropertyChanged(() => IsEnabledPrecio);
            }
        }
        public string ValorGastoOperativoFormato
        {
            get => _valorGastoOperativoFormato;
            set
            {
                _valorGastoOperativoFormato = value;
                RaisePropertyChanged(() => ValorGastoOperativoFormato);
            }
        }

        public Precio PrecioDefinido
        {
            get => _registro1;
            set
            {
                _registro1 = value;
                RaisePropertyChanged(() => PrecioDefinido);
            }
        }

        public CultureInfo Cultura => Escant_App.AppSettings.Settings.CustomCurrencySymbol=="$"?new System.Globalization.CultureInfo("en-US"): new System.Globalization.CultureInfo("en-US");

        #endregion 1.InstanciaAsignaVariablesControles
        #region 2.Constructor
        /// <summary>
        /// 1. Declaración de constructor
        /// </summary>
        /// <param name="catalogoServicio"></param>
        public AddItemViewModel(IServicioSeguridad_Usuario seguridadServicio
                                  , IServicioProductos_Item itemServicio
                                  , IServicioProductos_Precio precioServicio
                                  , IServicioIteraccion_Galeria galeriaServicio)
        {
            //Fijo en cada Model
            BindingContext = new ConectividadModelo(seguridadServicio);
            //Instanciación Acorde al model            
            _itemServicio = itemServicio;
            _precioServicio = precioServicio;
            _galeriaServicio = galeriaServicio;
            _switchProductos = new SwitchProductos(_itemServicio,_precioServicio);
            _switchIteraccion = new SwitchIteraccion(_galeriaServicio);
            generales = new Generales.Generales(galeriaServicio);
            inicioControlesValidacion();
            AddValidations();
            MessagingCenter.Unsubscribe<Empresa>(this, Escant_App.AppSettings.Settings.EmpresaSeleccionadaAddItemView);
            MessagingCenter.Subscribe<Empresa>(this, Escant_App.AppSettings.Settings.EmpresaSeleccionadaAddItemView, (args) =>
            {
                MarcaSeleccionada = args;
                Marca = MarcaSeleccionada != null && MarcaSeleccionada.Descripcion != null ? MarcaSeleccionada.Descripcion : Marca;
                IdMarca.Value = MarcaSeleccionada != null && MarcaSeleccionada.Id != null ? MarcaSeleccionada.Id : IdMarca.Value;
                eventoNombre();
            });
            MessagingCenter.Unsubscribe<Catalogo>(this, Escant_App.AppSettings.Settings.CatalogoSeleccionadoAddItemView);
            MessagingCenter.Subscribe<Catalogo>(this, Escant_App.AppSettings.Settings.CatalogoSeleccionadoAddItemView, (args) =>
            {
                switch (args.CodigoCatalogo)
                {
                    case "categoria":
                        CategoriaSeleccionada = args.CodigoCatalogo == "categoria" ? args : CategoriaSeleccionada;
                        Categoria = CategoriaSeleccionada != null && CategoriaSeleccionada.Nombre != null ? CategoriaSeleccionada.Nombre : Categoria;
                        IdCategoria.Value = CategoriaSeleccionada != null && CategoriaSeleccionada.Id != null ? CategoriaSeleccionada.Id : IdCategoria.Value;
                        eventoNombre();
                        break;
                    case "unidad":
                        UnidadSeleccionada = args.CodigoCatalogo == "unidad" ? args : UnidadSeleccionada;
                        Unidad = UnidadSeleccionada != null && UnidadSeleccionada.Nombre != null ? UnidadSeleccionada.Nombre : Unidad;
                        IdUnidad.Value = UnidadSeleccionada != null && UnidadSeleccionada.Id != null ? UnidadSeleccionada.Id : IdUnidad.Value;
                        ValorUnidadOriginal = UnidadSeleccionada != null ? UnidadSeleccionada.ValorConversion : ValorUnidadOriginal;
                        break;
                    case "tipoimpuesto":
                        TipoImpuestoSeleccionado = args.CodigoCatalogo == "tipoimpuesto" ? args : TipoImpuestoSeleccionado;
                        TipoImpuesto = TipoImpuestoSeleccionado != null && TipoImpuestoSeleccionado.Nombre != null ? TipoImpuestoSeleccionado.Nombre : TipoImpuesto;
                        IdTipoImpuesto = TipoImpuestoSeleccionado != null && TipoImpuestoSeleccionado.Id != null ? TipoImpuestoSeleccionado.Id : IdTipoImpuesto;
                        PorcentajeImpuesto = TipoImpuestoSeleccionado != null && TipoImpuestoSeleccionado.Nombre != null ? TipoImpuestoSeleccionado.ValorConstanteNumerico : PorcentajeImpuesto;
                        break;
                    case "tipoaplicacionimpuesto":
                        TipoAplicacionImpuestoSeleccionado = args.CodigoCatalogo == "tipoaplicacionimpuesto" ? args : TipoAplicacionImpuestoSeleccionado;
                        TipoAplicacionImpuesto = TipoAplicacionImpuestoSeleccionado != null && TipoAplicacionImpuestoSeleccionado.Nombre != null ? TipoAplicacionImpuestoSeleccionado.Nombre : TipoAplicacionImpuesto;
                        IdTipoAplicacionImpuesto = TipoAplicacionImpuestoSeleccionado != null && TipoAplicacionImpuestoSeleccionado.Id != null ? TipoAplicacionImpuestoSeleccionado.Id : IdTipoAplicacionImpuesto;
                        break;
                    case "periodoincremento":
                        PeriodoIncrementoSeleccionado = args.CodigoCatalogo == "periodoincremento" ? args : PeriodoIncrementoSeleccionado;
                        PeriodoIncremento = PeriodoIncrementoSeleccionado != null && PeriodoIncrementoSeleccionado.Nombre != null ? PeriodoIncrementoSeleccionado.Nombre : PeriodoIncremento;
                        IdPeriodoIncremento = PeriodoIncrementoSeleccionado != null && PeriodoIncrementoSeleccionado.Id != null ? PeriodoIncrementoSeleccionado.Id: IdPeriodoIncremento;
                        ValorPeriodoIncremento = PeriodoIncrementoSeleccionado != null && PeriodoIncrementoSeleccionado.Nombre != null ? PeriodoIncrementoSeleccionado.ValorConstanteNumerico : ValorPeriodoIncremento; 
                        break;
                    case "periodoservicio":
                        PeriodoServicioSeleccionado = args.CodigoCatalogo == "periodoservicio" ? args : PeriodoServicioSeleccionado;
                        PeriodoServicio = PeriodoServicioSeleccionado != null && PeriodoServicioSeleccionado.Nombre != null ? PeriodoServicioSeleccionado.Nombre : PeriodoServicio;
                        IdPeriodoServicio = PeriodoServicioSeleccionado != null && PeriodoServicioSeleccionado.Id != null ? PeriodoServicioSeleccionado.Id : IdPeriodoServicio;
                        CantidadPeriodoServicio = PeriodoServicioSeleccionado != null && PeriodoServicioSeleccionado.Nombre != null ? PeriodoServicioSeleccionado.ValorConstanteNumerico : CantidadPeriodoServicio;
                        break;
                }
                                                                                
                
                
                
            });
        }
        #endregion 2.Constructor       

        #region 3.DefinicionMetodos
        /// <summary>
        /// 3. Definición de métodos de la pantalla
        /// </summary>
        /// <summary>
        public ICommand BrowseGalleryCommand => new Command(async () => await BrowseGalleryAsync());
        public ICommand OpenCameraCommand => new Command(async () => await OpenCameraAsync());
        public ICommand RemoveImageCommand => new Command(async () => await RemoveImageAsync());
        public ICommand BrowseGalleryCommand1 => new Command(async () => await BrowseGallery1Async());
        public ICommand OpenCameraCommand1 => new Command(async () => await OpenCamera1Async());
        public ICommand RemoveImageCommand1 => new Command(async () => await RemoveImage1Async());
        public ICommand Ok_Command => new Command(async () => await Ok_CommandAsync());
        public ICommand Cancel_Command => new Command(async () => await Cancel_CommandAsync());
        public ICommand Precio_Command => new Command(async () => await Precio_CommandAsync());
        public ICommand OpenPeriodoServicioCommand => new Command(async () => await OpenPeriodoServicioAsync());
        public ICommand OpenMarcaCommand => new Command(async () => await OpenMarcaAsync());
        public ICommand OpenCategoriaCommand => new Command(async () => await OpenCategoriaAsync());
        public ICommand OpenUnidadCommand => new Command(async () => await OpenUnidadAsync());
        public ICommand OpenTipoImpuestoCommand => new Command(async () => await OpenTipoImpuestoAsync());
        public ICommand OpenTipoAplicacionImpuestoCommand => new Command(async () => await OpenTipoAplicacionImpuestoAsync());
        public ICommand OpenPeriodoIncrementoCommand => new Command(async () => await OpenPeriodoIncrementoAsync());
        public ICommand NombreUnfocused => new Command(async () => await NombreUnfocused_CommandAsync());
        public ICommand MarcaUnfocused => new Command(async () => await NombreUnfocused_CommandAsync());
        public ICommand CategoriaUnfocused => new Command(async () => await NombreUnfocused_CommandAsync());


        #endregion 3.DefinicionMetodos

        #region 4.MétodosAdministraciónDatos
        /// <summary>
        /// 4. Métodos de administración de datos
        /// </summary>
        /// <returns></returns>
        /// 4.1. Método para Cargar información de pantalla, la llamada realizada desde el frontend al inicializar
        /// </summary>
        /// <returns></returns>
        public async Task Load()
        {
            IsBusy = true;
            await LoadInicio();
            IsBusy = false;
        }
        /// <summary>
        /// 4.2 Método iniciar valores
        /// </summary>
        /// <returns></returns>
        public async Task LoadInicio()
        {
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            ResetForm();
        }
        /// <summary>
        /// 4.3 Método iniciar Pantalla con 1 parámetro
        /// </summary>
        /// <param name="navigationData"></param>
        /// <returns></returns>
        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            PageTitle = TextsTranslateManager.Translate("AddClienteClientesTitle");
            //Bind data for edit mode
            await Task.Run(() =>
            {
                if (navigationData is Item registro && registro != null)
                {
                    _isEditMode = registro.EsEdicion == 1 ? true : false;
                    _registro = registro;
                    PageTitle = TextsTranslateManager.Translate("EditClienteClientesTitle");
                    BindExistingData(registro);
                }
                else
                {
                    Load();
                }
            });
            IsBusy = false;
        }

        /// <summary>
        /// Método para almacenar información
        /// </summary>
        /// <returns></returns>
        private async Task Ok_CommandAsync()
        {
            IsBusy = true;
            //1. Validar información
            IdMarca.Value = EsServicio ? (string.IsNullOrEmpty(IdMarca.Value) ? SettingsOnline.oAplicacion.Id : IdMarca.Value) : IdMarca.Value;
            bool result = Validate();
            if (!result)
            {
                IsBusy = false;
                return;
            }
            _isRegistroExiste = _isEditMode;
            var iregistro = new Item
            {
                Id = _isRegistroExiste ? Id : Generator.GenerateKey(),
                EsServicio = EsServicio ? 1 : 0,
                Codigo = Codigo,
                Nombre = Nombre.Value,
                IdMarca = EsServicio?(string.IsNullOrEmpty(IdMarca.Value)? SettingsOnline.oAplicacion.Id:IdMarca.Value) :IdMarca.Value,
                Marca = EsServicio ? (string.IsNullOrEmpty(IdMarca.Value) ? SettingsOnline.oAplicacion.Nombre : Marca) : Marca,
                IdCategoria = IdCategoria.Value,
                Categoria = Categoria,
                IdUnidad = IdUnidad.Value,
                Unidad = Unidad,
                IdTipoImpuesto = IdTipoImpuesto,
                TipoImpuesto = TipoImpuesto,
                PorcentajeImpuesto=PorcentajeImpuesto,
                IdTipoAplicacionImpuesto = IdTipoAplicacionImpuesto,
                TipoAplicacionImpuesto = TipoAplicacionImpuesto,
                ValorImpuesto=ValorImpuesto,
                IdPeriodoIncremento=IdPeriodoIncremento,
                PeriodoIncremento = PeriodoIncremento,
                ValorPeriodoIncremento = ValorPeriodoIncremento,
                IdPeriodoServicio = IdPeriodoServicio,
                PeriodoServicio = PeriodoServicio,
                CantidadPeriodoServicio = CantidadPeriodoServicio,
                ValorIncremento = ValorIncremento,
                PrecioVentaImpuesto = PrecioVentaImpuesto,
                ValorUnidadOriginal = (float)ValorUnidadOriginal,
                EsParticionable = EsParticionable ? 1 : 0,
                IdGaleria = _galeria != null ? _galeria.Id : IdGaleria,
                Imagen = _galeria != null ? _galeria.Image : Imagen,
                GaleriaImagen = _galeria1 != null ? _galeria1.Image : GaleriaImagen,
                StockMinimo = StockMinimo,
                PorcentajeRentabilidad=PorcentajeRentabilidad.Value,
                PorcentajeGastoOperativo = PorcentajeGastoOperativo.Value,
                PrecioBase = PrecioBase.Value,
                PrecioCompra = PrecioCompra.Value,
                PrecioVenta = PrecioVenta.Value, 
                ValorRentabilidad=ValorRentabilidad,
                ValorGastoOperativo = ValorGastoOperativo,
                ValorUtilidad = ValorUtilidad,
                Descripcion = Descripcion,
                PoseeStock=PoseeStock?1:0,
                PoseeInventarioInicial = 0,
                CantidadInventarioInicial = EsServicio ? (PoseeStock?0:1) : 0,
                CantidadEntrada = EsServicio ? (PoseeStock ? 0 : 1) : 0,
                CantidadSaldoFinal = EsServicio ? (PoseeStock ? 0 : 1) : 0,
                ItemStockRegistradoSerializado=ItemStockRegistradoSerializado,
            };
            iregistro.EsEdicion = _isRegistroExiste ? 1 : 0;
            string informedMessage = "";

            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            //0. Añadir imagen a Galeria
            //2. Definición de información de Galeria
            if (_galeria != null)
            {
                _galeria.Nombre = Nombre.Value;  //Nombre de imagen
                _galeria.GuardarStorage = 1;     //Almacenar Imagen en Storage            
                var resultadoimagen = await _switchIteraccion.GuardaRegistro_Galeria(_galeria, estaConectado, _isRegistroExiste);
            }
            //2. Añadir los datos del Cliente
            var resultado = await _switchProductos.GuardaRegistro_Item(iregistro, estaConectado, _isRegistroExiste);
            if(resultado!=null)
            {
                _isRegistroExiste1 = _isRegistroExiste;
                _registro1 = new Precio()
                {
                    Id = _isRegistroExiste1 ? Id : Generator.GenerateKey(),
                    CodigoItem = Codigo,
                    EstaActivo = 1,
                    IdItem = iregistro.Id,
                    NombreItem = iregistro.Nombre,
                    IdUnidadPadre = "0",
                    IdUnidad = UnidadSeleccionada!=null?UnidadSeleccionada.Id:iregistro.IdUnidad,
                    Unidad = UnidadSeleccionada != null ? UnidadSeleccionada.Nombre : iregistro.Nombre,
                    ValorUnidadOriginal = iregistro.ValorUnidadOriginal,
                    IdUnidadConvertida = UnidadSeleccionada != null ? UnidadSeleccionada.Id : iregistro.IdUnidad,
                    UnidadConvertida = UnidadSeleccionada != null ? UnidadSeleccionada.Nombre : iregistro.Nombre,
                    ValorConversion = 1,
                    PrecioVenta = PrecioVenta.Value,
                    FechaDesde= DateTimeHelpers.GetDate(DateTimeHelpers.GetDateActual()),
                    FechaHasta = DateTimeHelpers.GetDate(DateTimeHelpers.GetDateActual().AddYears(40))
                };
                var resultado1 = await _switchProductos.GuardaRegistro_Precio(_registro1, estaConectado, _isRegistroExiste1);                
            }
            if (_isRegistroExiste)
            {
                await NavigationService.RemoveLastFromBackStackAsync();
            }
            else
            {
                ResetForm();
            }
            IsBusy = false;
            informedMessage = resultado.mensaje;
            DialogService.ShowToast(informedMessage);
            //Si el producto es particionable almacena los precios derivados para definirlos
            if (EsParticionable)
            {
                informedMessage = "Defina los precios derivados pendientes";
            }
            DialogService.ShowToast(informedMessage);
        }
        /// <summary>
        /// Método de Asignación de valores recibidos por parámetros
        /// </summary>
        /// <param name="registro"></param>
        /// <returns></returns>
        private async Task BindExistingData(Item iregistro)
        {
            Id = iregistro.Id;
            EsServicio = iregistro.EsServicio == 1 ? true : false;
            Codigo = iregistro.Codigo;
            Nombre.Value = iregistro.Nombre;
            IdMarca.Value = iregistro.IdMarca;
            Marca = iregistro.Marca;
            IdCategoria.Value = iregistro.IdCategoria;
            Categoria = iregistro.Categoria;
            IdUnidad.Value = iregistro.IdUnidad;
            Unidad = iregistro.Unidad;
            IdTipoImpuesto = iregistro.IdTipoImpuesto;
            TipoImpuesto = iregistro.TipoImpuesto;
            PorcentajeImpuesto= iregistro.PorcentajeImpuesto;
            IdTipoAplicacionImpuesto = iregistro.IdTipoAplicacionImpuesto;
            TipoAplicacionImpuesto = iregistro.TipoAplicacionImpuesto;
            IdPeriodoIncremento = iregistro.IdPeriodoIncremento;
            PeriodoIncremento = iregistro.PeriodoIncremento;
            ValorPeriodoIncremento = iregistro.ValorPeriodoIncremento;
            IdPeriodoServicio = iregistro.IdPeriodoServicio;
            PeriodoServicio = iregistro.PeriodoServicio;
            CantidadPeriodoServicio = iregistro.CantidadPeriodoServicio;
            ValorIncremento = iregistro.ValorIncremento;
            ValorUnidadOriginal = iregistro.ValorUnidadOriginal;
            EsParticionable = iregistro.EsParticionable == 1 ? true : false;
            IdGaleria = iregistro.IdGaleria;
            Imagen = iregistro.Imagen;
            if (Imagen != null)
            {
                CargaImagen(4);
            }
            if (GaleriaImagen != null)
            {
                CargaImagen1(4);
            }
            StockMinimo = iregistro.StockMinimo;
            PorcentajeRentabilidad.Value = iregistro.PorcentajeRentabilidad;
            PorcentajeGastoOperativo.Value= iregistro.PorcentajeGastoOperativo;
            PrecioCompra.Value = iregistro.PrecioCompra;
            PrecioVenta.Value = iregistro.PrecioVenta;
            PrecioBase.Value = iregistro.PrecioBase;
            ValorImpuesto = iregistro.ValorImpuesto;
            PrecioVentaImpuesto = iregistro.PrecioVentaImpuesto;
            ValorGastoOperativo = iregistro.ValorGastoOperativo;
            ValorRentabilidad = iregistro.ValorRentabilidad;
            ValorUtilidad = iregistro.ValorUtilidad;
            Descripcion = iregistro.Descripcion;
            ItemStockRegistradoSerializado = iregistro.ItemStockRegistradoSerializado;
            PoseeStock = iregistro.PoseeStock == 1 ? true : false;

            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            PrecioDefinido = (await _switchProductos.ConsultaPrecios(new Precio()
            {
                IdItem = Id
                ,IdUnidad=IdUnidad.Value
            },estaConectado)).FirstOrDefault();
            
        }

        #endregion 4.MétodosAdministraciónDatos

        #region 5.MétodosGenerales
        /// <summary>
        /// Reiniciar valores de controles
        /// </summary>
        private void ResetForm()
        {

            Id = "";
            EsServicio = false;
            Codigo = "";
            Nombre.Value = "";
            IdMarca.Value = "";
            Marca = TextsTranslateManager.Translate("SelectMarca");
            IdCategoria.Value = "";
            Categoria = TextsTranslateManager.Translate("SelectCategoria");
            IdUnidad.Value = "";
            Unidad = TextsTranslateManager.Translate("SelectUnidad");
            IdTipoImpuesto = "";
            TipoImpuesto = TextsTranslateManager.Translate("SelectTipoImpuesto");
            PorcentajeImpuesto = 0;
            IdTipoAplicacionImpuesto = "";
            TipoAplicacionImpuesto = TextsTranslateManager.Translate("SelectTipoAplicacionImpuesto");
            IdPeriodoIncremento = "";
            PeriodoIncremento = TextsTranslateManager.Translate("SelectPeriodoIncremento");
            ValorPeriodoIncremento = 0;
            IdPeriodoServicio = "";
            PeriodoServicio = TextsTranslateManager.Translate("SelectPeriodoServicio");
            CantidadPeriodoServicio = 0;
            ValorIncremento = 0;
            ValorUnidadOriginal = 0;
            EsParticionable = false;
            IdGaleria = "";
            Imagen = "";
            if (Imagen != null)
            {
                CargaImagen(3);
            }
            GaleriaImagen = "";
            if (GaleriaImagen != null)
            {
                CargaImagen1(3);
            }
            StockMinimo = 0;
            PrecioBase.Value = 0;
            PrecioCompra.Value = 0;
            PrecioVenta.Value = 0;
            ValorGastoOperativo = 0;
            ValorRentabilidad = 0;
            ValorUtilidad = 0;
            ValorImpuesto = 0;
            PrecioVentaImpuesto = 0;
            Descripcion = "";
            PorcentajeGastoOperativo.Value = 0;
            PorcentajeRentabilidad.Value = 0;
            ValorGastoOperativoFormato = ValorGastoOperativo.FormatoPrecioSinSimbolo();
            PoseeStock = true;

            _isEditMode = false;
            _isRegistroExiste = false;
        }
        /// <summary>
        /// Método para iniciar instancia de Controles de Validación
        /// </summary>
        public void inicioControlesValidacion()
        {

            _nombre = new ValidatableObject<string>();
            _idMarca = new ValidatableObject<string>();
            _idCategoria = new ValidatableObject<string>();
            _idUnidad = new ValidatableObject<string>();
            _porcentajeRentabilidad = new ValidatableObject<float>();
            _porcentajeGastoOperativo = new ValidatableObject<float>();
            _precioBase = new ValidatableObject<float>();
            _precioCompra = new ValidatableObject<float>();
            _precioVenta = new ValidatableObject<float>();

        }
        /// <summary>
        /// Método para definir las validaciones en pantalla
        /// </summary>
        private void AddValidations()
        {
            _nombre.Validations.Add(new IsNotNullOrEmptyRule<string>(""));
            _idMarca.Validations.Add(new IsNotNullOrEmptyRule<string>(""));
            _idCategoria.Validations.Add(new IsNotNullOrEmptyRule<string>(""));
            _idUnidad.Validations.Add(new IsNotNullOrEmptyRule<string>(""));
            _porcentajeRentabilidad.Validations.Add(new MontoRule<float>(""));
            _porcentajeGastoOperativo.Validations.Add(new MontoRule<float>(""));
            _precioBase.Validations.Add(new MontoRule<float>(""));
            _precioCompra.Validations.Add(new MontoRule<float>(""));
            _precioVenta.Validations.Add(new MontoRule<float>(""));
        }
        /// <summary>
        /// Método para ejecutar la validación de los controles de pantalla
        /// </summary>
        /// <returns></returns>
        private bool Validate()
        {
            string mensaje = "Debe escoger";
            var IsUnidadValid= !IdUnidad.Validate();
            var IsCategoriaValid = !IdCategoria.Validate();
            var IsMarcaValid = !IdMarca.Validate();
            IsNombreValid = !Nombre.Validate();            
            IsPrecioCompraValid = !PrecioCompra.Validate();
            IsPrecioVentaValid = !PrecioVenta.Validate();
            IsPrecioBaseValid = !PrecioBase.Validate();

            if (IsCategoriaValid)
                mensaje += " una categoría";
            if (IsMarcaValid)
                mensaje += " una empresa o marca";
            if (IsUnidadValid)
                mensaje += " una unidad correspondiente";
            if(IsCategoriaValid || IsMarcaValid || IsUnidadValid)
                App.Current.MainPage.DisplayAlert("Importante", mensaje + " para el ítem", "OK");

            return !IsNombreValid
                && !IsPrecioBaseValid
                && !IsPrecioCompraValid
                && !IsPrecioVentaValid
                && !IsUnidadValid
                && !IsCategoriaValid
                && !IsMarcaValid;
        }
        /// <summary>
        /// Método para salir de la pantalla
        /// </summary>
        /// <returns></returns>
        private async Task Cancel_CommandAsync()
        {
            await NavigationService.RemoveLastFromBackStackAsync();
        }
        private async Task Precio_CommandAsync()
        {
            await NavigationService.NavigateToAsync<PrecioViewModel>(PrecioDefinido);
        }
        /// <summary>
        /// Evento cuando se escoge la opción EsProveedor
        /// </summary>
        /// <returns></returns>
        private async void eventoEsServicio()
        {
            if (EsServicio)
            {
                IsVisibleCondicionesServicio = true;
                IsVisibleMarca = false;
                IsVisibleCategoria = true;
                IsVisibleEsParticionable = false;
                IsVisibleStockMinimo = false;
                IsVisiblePrecioCompra = true;
                IsVisiblePrecioVenta = true;
                IsVisiblePorcentajeRentabilidad = true;
                IsVisiblePorcentajeGastoOperativo = true;
                IsVisibleValorGastoOperativo = true;
                IsVisibleValorRentabilidad = true;
                IsVisibleValorUtilidad = true;
                //IsVisibleGeneroSeleccionado = false;
            }
            else
            {
                IsVisibleCondicionesServicio = false;
                IsVisibleMarca = true;
                IsVisibleCategoria = true;
                IsVisibleEsParticionable = true;
                IsVisibleStockMinimo = true;
                IsVisiblePrecioCompra = true;
                IsVisiblePrecioVenta = true;
                IsVisiblePorcentajeRentabilidad = true;
                IsVisiblePorcentajeGastoOperativo = true;
                IsVisibleValorGastoOperativo = true;
                IsVisibleValorRentabilidad = true;
                IsVisibleValorUtilidad = true;
                //IsVisibleEmpresaSeleccionada = false;
                //IsVisibleGeneroSeleccionado = true;
            }
        }

        private async void eventoEsParticionable()
        {
            if (EsParticionable)
            {
                IsEnabledPrecio = true;
            }
            else
            {
                IsEnabledPrecio = false;
            }
        }

            /// <summary>
            /// Abrir pantalla para selección de género
            /// </summary>
            /// <returns></returns>
            public async Task OpenMarcaAsync()
        {
            await NavigationService.NavigateToAsync<EmpresaViewModel>(MarcaSeleccionada);
        }
        public async Task OpenPeriodoServicioAsync()
        {
            PeriodoServicioSeleccionado = new Catalogo()
            {
                CodigoCatalogo = "periodoservicio"
            ,
                EsJerarquico = 0
            };
            await NavigationService.NavigateToAsync<CatalogoViewModel>(PeriodoServicioSeleccionado);
        }
        public async Task OpenCategoriaAsync()
        {
            CategoriaSeleccionada = new Catalogo()
            {
                CodigoCatalogo = "categoria"
            ,
                EsJerarquico = 0
            };
            await NavigationService.NavigateToAsync<CatalogoViewModel>(CategoriaSeleccionada);
        }
        public async Task OpenUnidadAsync()
        {
            UnidadSeleccionada = new Catalogo()
            {
                CodigoCatalogo = "unidad"
            ,
                EsJerarquico = 0
            };
            await NavigationService.NavigateToAsync<CatalogoViewModel>(UnidadSeleccionada);
        }
        public async Task OpenTipoImpuestoAsync()
        {
            TipoImpuestoSeleccionado = new Catalogo()
            {
                CodigoCatalogo = "tipoimpuesto"
            ,
                EsJerarquico = 0
            };
            await NavigationService.NavigateToAsync<CatalogoViewModel>(TipoImpuestoSeleccionado);
        }
        public async Task OpenTipoAplicacionImpuestoAsync()
        {
            TipoAplicacionImpuestoSeleccionado = new Catalogo()
            {
                CodigoCatalogo = "tipoaplicacionimpuesto"                                  
            ,
                EsJerarquico = 0
            };
            await NavigationService.NavigateToAsync<CatalogoViewModel>(TipoAplicacionImpuestoSeleccionado);
        }
        public async Task OpenPeriodoIncrementoAsync()
        {
            PeriodoIncrementoSeleccionado = new Catalogo()
            {
                CodigoCatalogo = "periodoincremento"
            ,
                EsJerarquico = 0
            };
            await NavigationService.NavigateToAsync<CatalogoViewModel>(PeriodoIncrementoSeleccionado);
        }
        private async Task BrowseGalleryAsync()
        {
            var galeria = new Galeria()
            {
                Id = Generator.GenerateKey()
                                    ,
                Directorio = "Item",
                GuardarStorage = 0
            };
            galeria.Nombre = Nombre.Value == "" ? galeria.Id : Nombre.Value;
            _galeria = await generales.PickAndShowFile(galeria);
            await CargaImagen(2);
        }

        private async Task BrowseGallery1Async()
        {
            var galeria = new Galeria()
            {
                Id = Generator.GenerateKey()
                                    ,
                Directorio = "Galeria",
                GuardarStorage = 0
            };
            galeria.Nombre = Nombre.Value == "" ? galeria.Id : Nombre.Value;
            _galeria1 = await generales.PickAndShowFile(galeria);
            await CargaImagen1(2);
        }

        private async Task OpenCameraAsync()
        {
            var galeria = new Galeria()
            {
                Id = Generator.GenerateKey()
                                    ,
                Directorio = "Item",
                GuardarStorage = 0
            };
            galeria.Nombre = Nombre.Value == "" ? galeria.Id : Nombre.Value;
            _galeria = await generales.TakePhoto(galeria);
            await CargaImagen(1);
        }
        private async Task OpenCamera1Async()
        {
            var galeria = new Galeria()
            {
                Id = Generator.GenerateKey()
                                    ,
                Directorio = "Item",
                GuardarStorage = 0
            };
            galeria.Nombre = Nombre.Value == "" ? galeria.Id : Nombre.Value;
            _galeria1 = await generales.TakePhoto(galeria);
            await CargaImagen1(1);
        }
        private async Task RemoveImageAsync()
        {
            await CargaImagen(3);

        }
        private async Task RemoveImage1Async()
        {
            await CargaImagen1(3);

        }
        private async Task CargaImagen(int Origen)
        {
            switch (Origen)
            {
                case 1:
                    FuenteImagenRegistro = ImageSource.FromStream(() =>
                    {
                        var stream = generales.ByteArrayToStream(Convert.FromBase64String(_galeria.Image));
                        return stream;
                    });
                    break;
                case 2:
                    FuenteImagenRegistro = ImageSource.FromStream(() =>
                    {
                        var stream = generales.ByteArrayToStream(Convert.FromBase64String(_galeria.Image));
                        return stream;
                    });
                    break;
                case 3:
                    FuenteImagenRegistro = ImageSource.FromStream(() =>
                    {
                        var stream = new MemoryStream();
                        return stream;
                    });
                    break;
                case 4:
                    FuenteImagenRegistro = ImageSource.FromStream(() =>
                    {
                        var stream = generales.ByteArrayToStream(Convert.FromBase64String(Imagen));
                        return stream;
                    });
                    break;
            }
        }
        private async Task CargaImagen1(int Origen)
        {
            switch (Origen)
            {
                case 1:
                    FuenteImagenRegistro1 = ImageSource.FromStream(() =>
                    {
                        var stream = generales.ByteArrayToStream(Convert.FromBase64String(_galeria1.Image));
                        return stream;
                    });
                    break;
                case 2:
                    FuenteImagenRegistro1 = ImageSource.FromStream(() =>
                    {
                        var stream = generales.ByteArrayToStream(Convert.FromBase64String(_galeria1.Image));
                        return stream;
                    });
                    break;
                case 3:
                    FuenteImagenRegistro1 = ImageSource.FromStream(() =>
                    {
                        var stream = new MemoryStream();
                        return stream;
                    });
                    break;
                case 4:
                    FuenteImagenRegistro1 = ImageSource.FromStream(() =>
                    {
                        var stream = generales.ByteArrayToStream(Convert.FromBase64String(GaleriaImagen));
                        return stream;
                    });
                    break;
            }
        }

        private async Task NombreUnfocused_CommandAsync()
        {
            await eventoNombre();

        }
        private async Task eventoNombre()
        {            
            string codigoUnificado = "";

            foreach (var part in Marca.ToString().Split(' '))
            {
                codigoUnificado += part.Length<2?part:part.Substring(0, 2);
            }
            foreach (var part in Categoria.ToString().Split(' '))
            {
                codigoUnificado += part.Length < 2 ? part : part.Substring(0, 2);
            }
            foreach (var part in Nombre.Value.ToString().Split(' '))
            {
                codigoUnificado += part.Length < 2 ? part : part.Substring(0, 2);
            }
            Codigo = codigoUnificado.ToLower();
        }

        public async void CalculaValores(int tipo)
        {
            float _precioCompra;
            float _valorUtilidad;
            float _valorGastoOperativo;
            float _valorRentabilidad;
            float _porcentajeImpuesto;
            float _valorImpuesto;
            float _precioBase;
            string _tipoAplicacionImpuesto = "";
            float _porcentajeRentabilidad = PorcentajeRentabilidad.Value;
            float _porcentajeGastoOperativo = PorcentajeGastoOperativo.Value;
            float _precioVenta;

            if (PrecioBase.Value > 0)
            {
                _precioBase = PrecioBase.Value;
                _porcentajeImpuesto = PorcentajeImpuesto;
                _tipoAplicacionImpuesto = !String.IsNullOrEmpty(TipoAplicacionImpuesto) ? TipoAplicacionImpuesto : "";
                _valorImpuesto= !string.IsNullOrEmpty(_tipoAplicacionImpuesto) && _porcentajeImpuesto > 0 //si posee porcentajeimpuesto mayor a cero
                    ? (_tipoAplicacionImpuesto.ToUpper().Contains("INCLUID") //si es impuesto incluido en el valor
                    ? (_porcentajeImpuesto / 100) * ((_precioBase) / (1 + (_porcentajeImpuesto / 100))) //cálculo incluido
                    : (_precioBase) * (_porcentajeImpuesto / 100)) //Adicional
                    : 0; //Caso que no tenga definido Impuesto
                _precioVenta = _valorImpuesto > 0 ? 
                                            (_tipoAplicacionImpuesto.ToUpper().Contains("INCLUID") //si es impuesto incluido en el valor
                                            ?_precioBase-_valorImpuesto
                                            :_precioBase
                                            ): _precioBase;
                PrecioVenta.Value = _precioVenta;
                ValorImpuesto = _valorImpuesto;
                PrecioVentaImpuesto = _precioVenta + _valorImpuesto;
                if (_precioVenta > 0)
                {
                    _precioCompra = tipo==1?(float)Math.Round(((100 - _porcentajeRentabilidad) * _precioVenta) / 100, 2):PrecioCompra.Value;
                    _porcentajeRentabilidad = tipo == 1 ? _porcentajeRentabilidad : (float)Math.Round((100- (float)Math.Round((_precioCompra*100)/_precioVenta,2)),2);
                    _valorRentabilidad = (float)Math.Round((_porcentajeRentabilidad * _precioVenta) / 100, 2);
                    _valorGastoOperativo = (float)Math.Round((_porcentajeGastoOperativo * _valorRentabilidad) / 100, 2);
                    _valorUtilidad = (float)Math.Round((_valorRentabilidad - _valorGastoOperativo), 2);
                    PorcentajeRentabilidad.Value = _porcentajeRentabilidad;
                    PrecioCompra.Value = _precioCompra;
                    ValorRentabilidad = _valorRentabilidad;
                    ValorGastoOperativo = _valorGastoOperativo;
                    ValorGastoOperativoFormato = _valorGastoOperativo.FormatoPrecioSinSimbolo();
                    ValorUtilidad = _valorUtilidad;

                }

            }
        }

        #endregion 5.MétodosGenerales

    }
}
