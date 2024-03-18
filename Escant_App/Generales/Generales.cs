using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
/*using FBGroupingApp.ServiceOffline.Services;
using FBGroupingApp.Switch;
*/
using Xamarin.Essentials;
using System.Collections.ObjectModel;
using System.Threading;
using Escant_App.Interfaces;
using DataModel.DTO.Iteraccion;
using System.Data;
using Newtonsoft.Json;
using System.ComponentModel;
using DataModel.DTO.Administracion;
using ManijodaServicios.Switch;
using ManijodaServicios.Offline.Interfaz;
using Plugin.Permissions;
using ManijodaServicios.Resources.Texts;
using Plugin.Permissions.Abstractions;
using Plugin.Media;
using Escant_App.ViewModels.Base;
using System.IO;
using Plugin.Media.Abstractions;
using DataModel.Helpers;
using Plugin.FilePicker;
using DataModel.DTO.Seguridad;
using ManijodaServicios.AppSettings;
using System.Linq;
using ManijodaServicios.Interfaces;
using DataModel.DTO;
using DataModel.DTO.Configuracion;
using DataModel.Enums;

namespace Escant_App.Generales
{
    public class Generales : ViewModelBase
    {
        private SwitchAdministracion _switchAdministracion;
        private SwitchIteraccion _switchIteraccion;
        private SwitchSeguridad _switchSeguridad;
        private SwitchConfiguracion _switchConfiguracion;
        private readonly IServicioAdministracion_Catalogo _catalogoServicio;
        private readonly IServicioIteraccion_Galeria _galeriaServicio;
        private readonly IServicioFirestore _firestoreServicio;
        private readonly ISettingsService _settingsServicio;
        public Generales() {
            _switchSeguridad = new SwitchSeguridad();
        }
        public Generales(IServicioAdministracion_Catalogo catalogoServicio)
        {
            _catalogoServicio = catalogoServicio;
            _switchAdministracion = new SwitchAdministracion(_catalogoServicio);
        }
        public Generales(IServicioIteraccion_Galeria galeriaServicio)
        {
            _galeriaServicio = galeriaServicio;
            _switchIteraccion = new SwitchIteraccion(_galeriaServicio);
        }
        public Generales(IServicioAdministracion_Catalogo catalogoServicio, IServicioIteraccion_Galeria galeriaServicio)
        {
            _catalogoServicio = catalogoServicio;
            _galeriaServicio = galeriaServicio;
            _switchAdministracion = new SwitchAdministracion(_catalogoServicio);
            _switchIteraccion = new SwitchIteraccion(_galeriaServicio);
        }
        public Generales(ISettingsService settingsServicio, IServicioFirestore servicioFirestore)
        {
            _settingsServicio = settingsServicio;
            _firestoreServicio = servicioFirestore;
            _switchConfiguracion = new SwitchConfiguracion(_settingsServicio, _firestoreServicio);
        }
        public Generales(IServicioAdministracion_Catalogo catalogoServicio, ISettingsService settingsServicio, IServicioFirestore servicioFirestore)
        {
            _catalogoServicio = catalogoServicio;
            _settingsServicio = settingsServicio;
            _firestoreServicio = servicioFirestore;
            _switchAdministracion = new SwitchAdministracion(_catalogoServicio);
            _switchConfiguracion = new SwitchConfiguracion(_settingsServicio, _firestoreServicio);
        }

        public void GuardaSesion(string nombreSesion, string valorSesion)
        {
            if (!Application.Current.Properties.ContainsKey(nombreSesion))
                App.Current.Properties.Add(nombreSesion, valorSesion);
            else
                App.Current.Properties[nombreSesion] = valorSesion;
        }
        public string ConsultaSesion(string nombreSesion)
        {
            string valor = "";
            if (Application.Current.Properties.ContainsKey(nombreSesion))
                valor = App.Current.Properties[nombreSesion].ToString();

            return valor;
        }

        public string GetKeyFromValue(Dictionary<string, string> dictionaryVar, string valueVar)
        {
            foreach (string keyVar in dictionaryVar.Keys)
            {
                /*if (dictionaryVar[keyVar] == valueVar)
                {
                    return keyVar;
                }*/
                if (keyVar == valueVar)
                {
                    return dictionaryVar[keyVar];
                }
            }
            return null;
        }
        /*
        public static async Task<bool> sincronizarDatos(int ejecutaSincroniza)
        {
            bool sincronizar = false;
            Base.IMessageService _messageService = DependencyService.Get<Base.IMessageService>();
            FBGroupingApp.Generales.Generales generales = new FBGroupingApp.Generales.Generales();
            //await _messageService.ShowAsync("Mensaje :" + generales.ConsultaSesion("EstaConectado"));
            if (generales.ConsultaSesion("EstaConectado") == "true") //Si está online validar sincronización a Base Real
            {
                //1. Validar sincronización desde Local o Online
                var x = await SwitchSincroniza.SincronizaOfflineToOnline(ejecutaSincroniza);
                if (ejecutaSincroniza == 0 && generales.GetKeyFromValue(x, "PorSincronizar") == "1" && generales.GetKeyFromValue(x, "CantidadSincroniza")!="0")
                {
                    await _messageService.ShowAsync("Mensaje Por sincronizar :" + generales.GetKeyFromValue(x, "CantidadSincroniza"));
                }
                //2. Validar sincronización desde Online a Local
                var y = await SwitchSincroniza.SincronizaOnlineToOffline(ejecutaSincroniza);
                if (ejecutaSincroniza == 0 && generales.GetKeyFromValue(y, "PorSincronizar") == "1" && generales.GetKeyFromValue(y, "CantidadSincroniza") != "0")
                {
                    await _messageService.ShowAsync("Mensaje Por sincronizar :" + generales.GetKeyFromValue(y, "CantidadSincroniza"));
                }
                sincronizar = true;

            }

            return sincronizar;
        }
        */
        public static async Task<List<Contact>> todosContactos()
        {
            ObservableCollection<Contact> contactsCollect = new ObservableCollection<Contact>();

            try
            {
                // cancellationToken parameter is optional
                var cancellationToken = default(CancellationToken);
                var contacts = await Contacts.GetAllAsync(cancellationToken);

                if (contacts == null)
                    contactsCollect = null;

                foreach (var contact in contacts)
                    contactsCollect.Add(contact);
            }
            catch (Exception ex)
            {
                // Handle exception here.
            }
            return new List<Contact>(contactsCollect);
        }

        public void enviaMensajeSMS(DatosSms datosSms)
        {
            DependencyService.Get<ISendSms>().Send(datosSms.NumeroTelefono, datosSms.Mensaje);

        }

        public DataTable GetDataTableFromDynamic(dynamic objects)
        {
            return objects != null ? (DataTable)JsonConvert.DeserializeObject(objects, (typeof(DataTable))) : null;
        }

        public DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
        public byte[] streamToByteArray(Stream input)
        {
            MemoryStream ms = new MemoryStream();
            input.CopyTo(ms);
            return ms.ToArray();
        }

        public Stream ByteArrayToStream(byte[] input)
        {
            return new MemoryStream(input);
        }

        public ObservableCollection<T> ToObservableCollection<T>(List<T> data)
        {
            ObservableCollection<T> retorno = new ObservableCollection<T>();
            data.ForEach(x => retorno.Add(x));
            return retorno;
        }

        public ObservableCollection<T> ConvertObservable<T>(IEnumerable<T> original)
        {
            return new ObservableCollection<T>(original);
        }

        public async Task<List<Catalogo>> LoadListaCatalogo(Catalogo iregistro)
        {
            bool estaConectado = Convert.ToBoolean(ConsultaSesion("EstaConectado") == "" ? "false" : ConsultaSesion("EstaConectado"));
            return await _switchAdministracion.ConsultaCatalogos(iregistro, estaConectado);
        }


        private async Task<ImageSource> OpenCameraAsync()
        {
            ImageSource _fuenteImagenRegistro = ImageSource.FromStream(() =>
            {
                var stream = new MemoryStream();
                return stream;
            });
            try
            {
                var camStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<CameraPermission>();
                if (camStatus != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera))
                    {
                        await DialogService.ShowAlertAsync(TextsTranslateManager.Translate("CamNotReady"), TextsTranslateManager.Translate("Warning"), "OK");
                    }

                    camStatus = await CrossPermissions.Current.RequestPermissionAsync<CameraPermission>();
                }

                if (camStatus == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    //Once Camera access granted -> check Storage permission
                    var storageStat = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
                    if (storageStat != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                    {
                        if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                        {
                            await DialogService.ShowAlertAsync(TextsTranslateManager.Translate("StorageNotAllowed"), TextsTranslateManager.Translate("Warning"), "OK");
                        }

                        storageStat = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                    }

                    if (storageStat == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                    {
                        await CrossMedia.Current.Initialize();
                        if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                        {
                            await DialogService.ShowAlertAsync(TextsTranslateManager.Translate("CamNotReady"), TextsTranslateManager.Translate("Warning"), "OK");
                            return null;
                        }
                        var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                        {
                            //SaveToAlbum = true,
                            //Directory = DateTime.Now.Date.ToShortDateString(),
                            PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small,
                            CompressionQuality = 30
                            //Name = $"{DateTime.Now.Date.ToShortDateString()}.jpg"
                        });

                        if (file == null)
                            return null;
                        _fuenteImagenRegistro = ImageSource.FromStream(() =>
                        {
                            var stream = file.GetStream();
                            return stream;
                        });
                    }

                    else if (storageStat != Plugin.Permissions.Abstractions.PermissionStatus.Unknown)
                    {
                        await DialogService.ShowAlertAsync(TextsTranslateManager.Translate("StorageNotAllowed"), TextsTranslateManager.Translate("Warning"), "OK");
                    }

                }
                else if (camStatus != Plugin.Permissions.Abstractions.PermissionStatus.Unknown)
                {
                    await DialogService.ShowAlertAsync(TextsTranslateManager.Translate("CamNotAllowed"), TextsTranslateManager.Translate("Warning"), "OK");
                }
            }
            catch (Exception e)
            {
                await DialogService.ShowAlertAsync(TextsTranslateManager.Translate("SomethingWrong"), TextsTranslateManager.Translate("Warning"), "OK");
            }
            return _fuenteImagenRegistro;
        }

        public async Task<Galeria> TakePhoto(Galeria iregistro)
        {
            string retorno = "";
            byte[] imagebytes;
            ImageSource _fuenteImagenRegistro = ImageSource.FromStream(() =>
            {
                var stream = new MemoryStream();
                return stream;
            });

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DialogService.ShowAlertAsync(TextsTranslateManager.Translate("CamNotReady"), TextsTranslateManager.Translate("Warning"), "OK");
                return null;
            }
            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = iregistro.Directorio,
                SaveToAlbum = true,
                CompressionQuality = 75,
                CustomPhotoSize = 50,
                PhotoSize = PhotoSize.MaxWidthHeight,
                MaxWidthHeight = 2000,
                DefaultCamera = CameraDevice.Front,
                Name = iregistro.Nombre
            });

            if (file == null)
                return null;

            imagebytes = streamToByteArray(file.GetStream());
            iregistro.Extension = ".png";
            iregistro.Image = Convert.ToBase64String(imagebytes);
            if (iregistro.GuardarStorage == 1)
            {
                retorno = await PerformStorage(iregistro);
            }
            _fuenteImagenRegistro = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
            return iregistro;
        }

        public async Task<Galeria> GuardaArchivo(Galeria iregistro)
        {
            string retorno = "";

            if (iregistro.fStream == null)
                return null;

            if (iregistro.GuardarStorage == 1)
            {
                retorno = await PerformStorage(iregistro);
                iregistro.UrlImagen = retorno;
            }

            return iregistro;
        }

        private async Task<string> PerformStorage(Galeria iregistro)
        {
            string url = "";
            try
            {
                bool estaConectado = Convert.ToBoolean(ConsultaSesion("EstaConectado") == "" ? "false" : ConsultaSesion("EstaConectado"));
                url = await _switchIteraccion.GuardaObjetoGaleria(iregistro, estaConectado);
            }
            catch (Exception ex)
            {
                await DialogService.ShowAlertAsync(TextsTranslateManager.Translate("CamNotReady"), TextsTranslateManager.Translate("Warning"), "OK");
            }
            return url;
        }

        public async Task<Galeria> PickAndShowFile(Galeria iregistro)
        {
            string[] fileTypes = null;

            if (Device.RuntimePlatform == Device.Android)
            {
                fileTypes = new string[] { "image/png", "image/jpeg" };
            }

            if (Device.RuntimePlatform == Device.iOS)
            {
                fileTypes = new string[] { "public.image" }; // same as iOS constant UTType.Image
            }

            if (Device.RuntimePlatform == Device.UWP)
            {
                fileTypes = new string[] { ".jpg", ".png" };
            }

            if (Device.RuntimePlatform == Device.WPF)
            {
                fileTypes = new string[] { "JPEG files (*.jpg)|*.jpg", "PNG files (*.png)|*.png" };
            }
            try
            {
                var pickedFile = await CrossFilePicker.Current.PickFile(fileTypes);

                if (pickedFile != null)
                {
                    if (pickedFile.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase)
                        || pickedFile.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                    {

                        iregistro.UrlImagenOffline = pickedFile.FilePath;
                        iregistro.Image = Convert.ToBase64String(pickedFile.DataArray);
                        iregistro.fStream = pickedFile.GetStream();
                        iregistro.fByte = pickedFile.DataArray;


                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }

            return iregistro;
        }

        public async Task<List<PerfilMenu>> devuelveMenu(int nivel, string nombrePadre)
        {
            List<PerfilMenu> resultado = new List<PerfilMenu>();
            if (SettingsOnline.oPerfilMenu == null)
                resultado = null;
            else
            {
                resultado = SettingsOnline.oPerfilMenu.Where(x => x.Nivel == nivel && nivel > 0
                                                        || (SettingsOnline.oPerfilMenu.Where(y => y.NombreMenu == nombrePadre
                                                                                            && y.IdMenu == x.IdMenuPadre).Any()
                                                             && nombrePadre != "")
                                                        ).ToList();
                resultado.Where(d => (SettingsOnline.oLCatalogo.Where(y => y.Id == d.IdMenu).Count() > 0)).ToList().ForEach(x => x.Orden = ((SettingsOnline.oLCatalogo.Where(y => y.Id == x.IdMenu).Sum(l => l.Orden))));
            }
            return resultado;
        }

        /// <summary>
        /// Obtener Secuencial Orden Compra
        /// </summary>
        /// <param name="TipoSecuencial"></param>
        /// <returns></returns>
        public async Task<SecuencialNumerico> obtieneSecuenciales(string TipoSecuencial)
        {
            SecuencialNumerico resultado = new SecuencialNumerico();
            bool estaConectado = Convert.ToBoolean(ConsultaSesion("EstaConectado") == "" ? "false" : ConsultaSesion("EstaConectado"));
            var setting = await _switchConfiguracion.ConsultaConfiguracion(new Setting() { SettingType = (int)SettingType.SecuencialesSetting }, estaConectado);
            if (setting != null)
            {
                SecuencialSetttings comprobantesSettings = JsonConvert.DeserializeObject<SecuencialSetttings>(setting.Data);
                if (comprobantesSettings != null)
                {
                    resultado = await _switchConfiguracion.ActualizaConfiguracionSecuencial(TipoSecuencial);
                    if (!string.IsNullOrEmpty(resultado.SecuencialReciboIncrementado))
                    {
                        switch(TipoSecuencial)
                        {
                            case "OrdenVenta":
                                comprobantesSettings.SecuencialOrdenVenta = resultado.SecuencialReciboIncrementado;
                                break;
                            case "OrdenCompra":
                                comprobantesSettings.SecuencialOrdenCompra = resultado.SecuencialReciboIncrementado;
                                break;
                            case "NumeroPago":
                                comprobantesSettings.SecuencialReciboPago = resultado.SecuencialReciboIncrementado;
                                break;                                
                        }
                        
                        setting.Data = JsonConvert.SerializeObject(comprobantesSettings);
                        await _switchConfiguracion.GuardaConfiguracion(setting, estaConectado, true);
                    }
                }
            }
            return resultado;
        }

        

    }
}