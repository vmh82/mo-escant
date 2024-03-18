using Escant_App.ViewModels.Base;
using ManijodaServicios.Offline.Interfaz;
using ManijodaServicios.AppSettings;
using System.Threading.Tasks;
using System;
using DataModel.DTO.Iteraccion;
using System.Collections.Generic;
using ManijodaServicios.Switch;
using DataModel.DTO.Seguridad;
using System.Linq;
using DataModel.DTO.Clientes;
using DataModel.DTO.Administracion;
using DataModel.DTO.Productos;
using System.Windows.Input;
using Xamarin.Forms;
using Escant_App.ViewModels.Seguridad;
using DataModel.DTO.Compras;

namespace Escant_App.ViewModels.Principal
{
    public class PrincipalViewModel:ViewModelBase
    {
        private string _imagenUsuario;
        private string _nombreUsuario;
        private string _perfilUsuario;
        private string _fechaActual;
        private SwitchAdministracion _switchAdministracion;
        private SwitchSeguridad _switchSeguridad;
        private SwitchCliente _switchCliente;
        private SwitchProductos _switchProductos;
        private SwitchCompras _switchCompras;

        private Generales.Generales generales = new Generales.Generales();
        private readonly IServicioProductos_Item _itemServicio;
        private readonly IServicioSeguridad_Usuario _seguridadServicio;
        private readonly IServicioClientes_Cliente _clienteServicio;
        private readonly IServicioAdministracion_Empresa _empresaServicio;
        private readonly IServicioCompras_Orden _ordenServicio;
        private List<Usuario> RegistroUsuario;
        private List<Cliente> RegistroCliente;
        private List<Empresa> RegistroEmpresa;
        private List<Item> RegistroProducto;
        private List<Orden> RegistroOrden;

        public string ImagenUsuario
        {
            get
            {
                return _imagenUsuario;
            }
            set
            {
                _imagenUsuario = value;
                RaisePropertyChanged(() => ImagenUsuario);
            }
        }
        public string NombreUsuario
        {
            get
            {
                return _nombreUsuario;
            }
            set
            {
                _nombreUsuario = value;
                RaisePropertyChanged(() => NombreUsuario);
            }
        }
        public string PerfilUsuario
        {
            get
            {
                return _perfilUsuario;
            }
            set
            {
                _perfilUsuario = value;
                RaisePropertyChanged(() => PerfilUsuario);
            }
        }
        public string FechaActual
        {
            get
            {
                return _fechaActual;
            }
            set
            {
                _fechaActual = value;
                RaisePropertyChanged(() => FechaActual);
            }
        }
        public ICommand ViewUsuario => new Command(async () => await ViewUsuarioAsync());
        public PrincipalViewModel(IServicioSeguridad_Usuario usuarioServicio, IServicioClientes_Cliente clienteServicio
                                , IServicioAdministracion_Empresa empresaServicio
                                , IServicioProductos_Item itemServicio
                                , IServicioCompras_Orden ordenServicio)
        {
            _seguridadServicio = usuarioServicio;
            _clienteServicio = clienteServicio;
            _empresaServicio = empresaServicio;
            _itemServicio = itemServicio;
            _ordenServicio = ordenServicio;
            _switchCompras = new SwitchCompras(_ordenServicio);
            _switchAdministracion = new SwitchAdministracion(_empresaServicio);
            _switchSeguridad = new SwitchSeguridad(_seguridadServicio);
            _switchCliente = new SwitchCliente(_clienteServicio);
            _switchProductos = new SwitchProductos(_itemServicio);
            //cargaImagen();
        }
        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            //Bind data for edit mode
            cargaImagen();
            //await ConsultaDatos();
            IsBusy = false;
        }
        private void cargaImagen()
        {
            ImagenUsuario = SettingsOnline.oAplicacion != null ? SettingsOnline.oAplicacion.ImagenUsuario : "";
            NombreUsuario = SettingsOnline.oAplicacion != null ? SettingsOnline.oAplicacion.NombreConectado : "";
            PerfilUsuario = SettingsOnline.oAplicacion != null ? SettingsOnline.oAplicacion.PerfilConectado : "";
            FechaActual = DateTime.Now.ToString("ddd, dd MMMM yyyy");
        }

        public async Task ConsultaDatos()
        {
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            //1.Consulta Usuario
            RegistroUsuario = await _switchSeguridad.ConsultaUsuarios(new Usuario() { Deleted = 0 }, estaConectado);
            RegistroCliente = await _switchCliente.ConsultaClientes(new Cliente() { Deleted = 0 }, estaConectado);
            RegistroEmpresa = await _switchAdministracion.ConsultaEmpresas(new Empresa() { Deleted = 0 }, estaConectado);
            RegistroProducto = await _switchProductos.ConsultaItems(new Item() { Deleted = 0,PoseeStock=-1,PoseeInventarioInicial=-1 }, estaConectado);
            RegistroOrden = await _switchCompras.ConsultaOrdenes(new Orden()
            {               
                Identificacion = SettingsOnline.oAplicacion != null && (SettingsOnline.oAplicacion.PerfilConectado.ToUpper().Contains("SITANT") || SettingsOnline.oAplicacion.PerfilConectado.ToUpper().Contains("LIENTE"))
                            ? SettingsOnline.oAplicacion.IdentificacionConectado : null
            }, estaConectado);
        }

        public async Task<List<Transaccion>> Get()
        {
            List<Transaccion> list = new List<Transaccion>();
            list.Add(new Transaccion { Nombre = "Usuarios", Descripcion = "Usuarios del Sistema", CantidadTotal = await devuelveCantidad("Usuario", 0), CantidadHoy = await devuelveCantidad("Usuario", 1), Imagen = "https://img.icons8.com/cotton/2x/transfer-money.png" });
            list.Add(new Transaccion { Nombre = "Clientes", Descripcion = "Clientes Registrados", CantidadTotal = await devuelveCantidad("Cliente", 0), CantidadHoy = await devuelveCantidad("Cliente", 1), Imagen = "https://img.icons8.com/cotton/2x/transfer-money.png" });
            //list.Add(new Transaccion { Nombre = "Proveedores", Descripcion = "Proveedores disponibles", CantidadTotal = await devuelveCantidad("Proveedor", 0), CantidadHoy = await devuelveCantidad("Proveedor", 1), Imagen = "https://img.icons8.com/cotton/2x/transfer-money.png" });
            list.Add(new Transaccion { Nombre = "Productos", Descripcion = "Productos / Servicios registrados", CantidadTotal = await devuelveCantidad("Producto", 0), CantidadHoy = await devuelveCantidad("Producto", 1), Imagen = "https://img.icons8.com/cotton/2x/transfer-money.png" });
            //list.Add(new Transaccion { Nombre = "Compras", Descripcion = "Compras realizadas", CantidadTotal = await devuelveCantidad("Compra", 0), CantidadHoy = await devuelveCantidad("Compra", 1), Imagen = "https://img.icons8.com/cotton/2x/transfer-money.png" });
            list.Add(new Transaccion { Nombre = "Ventas", Descripcion = "Ventas realizadas", CantidadTotal = await devuelveCantidad("Venta", 0), CantidadHoy = await devuelveCantidad("Venta", 1), Imagen = "https://img.icons8.com/cotton/2x/transfer-money.png" });
            return list;
        }

        public async Task<string> devuelveCantidad(string dato, int tipo)
        {
            string respuesta = "0";
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            switch (dato)
            {
                case "Usuario":                    
                    //var registros = await _switchSeguridad.ConsultaUsuarios(new Usuario() { Deleted = 0 }, estaConectado);
                    if (tipo == 0)
                        respuesta = RegistroUsuario.Count.ToString();
                    else
                        respuesta = RegistroUsuario.Where(x => (Convert.ToDateTime(x.CreatedDateFormato) - DateTime.Now).TotalDays > 0).Count().ToString();
                    break;
                case "Cliente":                    
                    
                    if (tipo == 0)
                        respuesta = RegistroCliente.Count.ToString();
                    else
                        respuesta = RegistroCliente.Where(x => (Convert.ToDateTime(x.CreatedDateFormato) - DateTime.Now).TotalDays > 0).Count().ToString();
                    break;
                case "Proveedor":

                    if (tipo == 0)
                        respuesta = RegistroEmpresa.Where(x=>x.EsProveedor==1).ToList().Count.ToString();
                    else
                        respuesta = RegistroEmpresa.Where(x => x.EsProveedor==1 && (Convert.ToDateTime(x.CreatedDateFormato) - DateTime.Now).TotalDays > 0).Count().ToString();
                    break;
                case "Producto":

                    if (tipo == 0)
                        respuesta = RegistroProducto.ToList().Count.ToString();
                    else
                        respuesta = RegistroProducto.Where(x => (Convert.ToDateTime(x.CreatedDateFormato) - DateTime.Now).TotalDays > 0).Count().ToString();
                    break;
                case "Compra":

                    if (tipo == 0)
                        respuesta = RegistroOrden.Where(x=>x.TipoOrden.ToUpper()=="COMPRA").ToList().Count.ToString();
                    else
                        respuesta = RegistroOrden.Where(x => x.TipoOrden.ToUpper() == "COMPRA" && (Convert.ToDateTime(x.CreatedDateFormato) - DateTime.Now).TotalDays > 0).Count().ToString();
                    break;
                case "Venta":

                    if (tipo == 0)
                        respuesta = RegistroOrden.Where(x => x.TipoOrden.ToUpper() == "VENTA").ToList().Count.ToString();
                    else
                        respuesta = RegistroOrden.Where(x => x.TipoOrden.ToUpper() == "VENTA" && (Convert.ToDateTime(x.CreatedDateFormato) - DateTime.Now).TotalDays > 0).Count().ToString();
                    break;
                default:
                    respuesta = "0";
                    break;
            }
            return respuesta;
        }

        private async Task ViewUsuarioAsync()
        {
            var registro = RegistroUsuario.Where(x => x.Identificacion == SettingsOnline.oAplicacion.IdentificacionConectado).FirstOrDefault();
            registro.EsEdicion = 1; 
            await NavigationService.NavigateToAsync<AddUsuarioViewModel>(registro);            
        }

    }
}
