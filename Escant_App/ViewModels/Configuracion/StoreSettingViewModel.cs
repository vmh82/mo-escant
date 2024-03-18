using System;
using System.Collections.Generic;
using System.Text;
using Escant_App.AppSettings;
using Escant_App.ViewModels.Base;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Escant_App.Helpers;
using Escant_App.Interfaces;
using DataModel.Enums;
using System.IO;
using ManijodaServicios.Offline.Interfaz;
using DataModel;
using DataModel.DTO;
using DataModel.DTO.Configuracion;
using DataModel.Helpers;
using Newtonsoft.Json;
using Plugin.Media;
using ManijodaServicios.Resources.Texts;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using ManijodaServicios.Switch;
using DataModel.DTO.Iteraccion;
using ManijodaServicios.AppSettings;
using DataModel.DTO.Administracion;

namespace Escant_App.ViewModels.Configuracion
{
    public class StoreSettingViewModel : ViewModelBase
    {
        private ImageSource _storeImageSource = ImageSource.FromStream(() =>
        {
            var stream = new MemoryStream();
            return stream;
        });
        private Generales.Generales generales = new Generales.Generales();
        private SwitchConfiguracion _switchConfiguracion;
        private SwitchAdministracion _switchAdministracion;
        private SwitchIteraccion _switchIteraccion;
        private readonly ISettingsService _settingsService;
        private readonly IServicioAdministracion_Empresa _empresaServicio;        
        private readonly IServicioIteraccion_Galeria _galeriaServicio;
        private bool _isSettingExists;
        private string _storeSettingId;
        private Galeria _galeria;
        private string _imagen;

        public StoreSettingViewModel(ISettingsService settingService
                                  , IServicioAdministracion_Empresa empresaServicio                                  
                                  , IServicioIteraccion_Galeria galeriaServicio)
        {
            _settingsService = settingService;
            _empresaServicio = empresaServicio;
            _galeriaServicio = galeriaServicio;
            _switchConfiguracion = new SwitchConfiguracion(_settingsService);
            _switchAdministracion = new SwitchAdministracion(_empresaServicio);
            _switchIteraccion = new SwitchIteraccion(_galeriaServicio);
        }

        public async Task LoadSetting()
        {
            IsBusy = true;
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            var setting = await _switchConfiguracion.ConsultaConfiguracion(new Setting() { SettingType = (int)SettingType.StoreSetting }, estaConectado);            
            if(setting != null)
            {
                _isSettingExists = true;
                _storeSettingId = setting.Id;

                StoreSettings storeSettings = JsonConvert.DeserializeObject<StoreSettings>(setting.Data);
                if(storeSettings != null)
                {
                    StoreName = storeSettings.StoreName;
                    StoreOwner = storeSettings.StoreOwner;
                    Ruc = storeSettings.Ruc;
                    StoreAddress = storeSettings.Address;
                    StoreEmail = storeSettings.Email;
                    StorePhoneNumer = storeSettings.Mobile;
                    IsPrintBillNotIncludedTax = storeSettings.IsPrintBillNotIncludedTax;
                    IsDisplayTotalPriceBeforeTax = storeSettings.IsDisplayTotalPriceBeforeTax;
                    VAT = storeSettings.VAT;
                    TaxNumber = storeSettings.TaxNumber;

                }                

                if (setting.Logo != null)
                    StoreImageSource = ImageSource.FromStream(() => new MemoryStream(setting.Logo));
            }
            else
            {
                _isSettingExists = false;
            }
            IsBusy = false;
        }

        private string _storeName;
        public string StoreName
        {
            get => _storeName;
            set
            {
                _storeName = value;
                RaisePropertyChanged(() => StoreName);
            }
        }

        private string _storeOwner;
        public string StoreOwner
        {
            get => _storeOwner;
            set
            {
                _storeOwner = value;
                RaisePropertyChanged(() => StoreOwner);
            }
        }

        private string _ruc;
        public string Ruc
        {
            get => _ruc;
            set
            {
                _ruc = value;
                RaisePropertyChanged(() => Ruc);
            }
        }

        private string _storeAddress;
        public string StoreAddress
        {
            get => _storeAddress;
            set
            {
                _storeAddress = value;
                RaisePropertyChanged(() => StoreAddress);
            }
        }

        private string _storeEmail;
        public string StoreEmail
        {
            get => _storeEmail;
            set
            {
                _storeEmail = value;
                RaisePropertyChanged(() => StoreEmail);
            }
        }

        public string _storePhoneNumber;
        public string StorePhoneNumer
        {
            get => _storePhoneNumber;
            set
            {
                _storePhoneNumber = value;
                RaisePropertyChanged(() => StorePhoneNumer);
            }
        }

        private bool _isPrintBillNotIncludedTax;
        public bool IsPrintBillNotIncludedTax
        {
            get => _isPrintBillNotIncludedTax;
            set
            {
                _isPrintBillNotIncludedTax = value;
                RaisePropertyChanged(() => IsPrintBillNotIncludedTax);
            }
        }

        private bool _isDisplayTotalPriceBeforeTax;
        public bool IsDisplayTotalPriceBeforeTax
        {
            get => _isDisplayTotalPriceBeforeTax;
            set
            {
                _isDisplayTotalPriceBeforeTax = value;
                RaisePropertyChanged(() => IsDisplayTotalPriceBeforeTax);
            }
        }

        private int _vat;
        public int VAT
        {
            get => _vat;
            set
            {
                _vat = value;
                RaisePropertyChanged(() => VAT);
            }
        }

        private string _taxNumber;
        public string TaxNumber
        {
            get => _taxNumber;
            set
            {
                _taxNumber = value;
                RaisePropertyChanged(() => TaxNumber);
            }
        }

        private bool _isStoreNameInValid;
        public bool IsStoreNameInValid
        {
            get => _isStoreNameInValid;
            set
            {
                _isStoreNameInValid = value;
                RaisePropertyChanged(() => IsStoreNameInValid);
            }
        }

        private bool _isOwnerNameInValid;
        public bool IsOwnerNameInValid
        {
            get => _isOwnerNameInValid;
            set
            {
                _isOwnerNameInValid = value;
                RaisePropertyChanged(() => IsOwnerNameInValid);
            }
        }
        private bool _isRucInValid;
        public bool IsRucInValid
        {
            get => _isRucInValid;
            set
            {
                _isRucInValid = value;
                RaisePropertyChanged(() => IsRucInValid);
            }
        }

        private bool _isAddressInValid;
        public bool IsAddressInValid
        {
            get => _isAddressInValid;
            set
            {
                _isAddressInValid = value;
                RaisePropertyChanged(() => IsAddressInValid);
            }
        }

        private bool _isMobileInValid;
        public bool IsMobileInValid
        {
            get => _isMobileInValid;
            set
            {
                _isMobileInValid = value;
                RaisePropertyChanged(() => IsMobileInValid);
            }
        }

        private bool _isEmailInValid;
        public bool IsEmailInValid
        {
            get => _isEmailInValid;
            set
            {
                _isEmailInValid = value;
                RaisePropertyChanged(() => IsEmailInValid);
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

        public ICommand BrowseGalleryCommand => new Command(async () => await BrowseGalleryAsync());
        public ICommand OpenCameraCommand => new Command(async () => await OpenCameraAsync());
        public ICommand RemoveStoreImageCommand => new Command(() => RemoveStoreImageAsync());
        public ICommand SaveStoreSettingCommand => new Command(async () => await SaveStoreSettingAsync());
        public ICommand DiscardCommand => new Command(async () => await DiscardAsync());

        private async Task BrowseGalleryAsync()
        {
            /*
            try
            {
                var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
                if (storageStatus != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                    {
                        await DialogService.ShowAlertAsync(TextsTranslateManager.Translate("StorageNotAllowed"), TextsTranslateManager.Translate("Warning"), "OK");
                    }

                    storageStatus = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                }

                if (storageStatus == PermissionStatus.Granted)
                {
                    await CrossMedia.Current.Initialize();

                    if (!CrossMedia.Current.IsPickPhotoSupported)
                    {
                        return;
                    }
                    var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                    {
                        SaveMetaData = false,
                        CompressionQuality = 30,
                        MaxWidthHeight = 1024
                    });
                    if (file != null)
                    {
                        StoreImageSource = ImageSource.FromFile(file.Path);
                    }
                }
                else if (storageStatus != PermissionStatus.Unknown)
                {
                    await DialogService.ShowAlertAsync(TextsTranslateManager.Translate("StorageNotAllowed"), TextsTranslateManager.Translate("Warning"), "OK");
                }
            }
            catch (Exception ex)
            {
                await DialogService.ShowAlertAsync(TextsTranslateManager.Translate("SomethingWrong"), TextsTranslateManager.Translate("Warning"), "OK");
            }*/
            var galeria = new Galeria()
            {
                Id = Generator.GenerateKey()
                                    ,
                Directorio = "Setting",
                GuardarStorage = 1
            };
            galeria.Nombre = "Negocio";
            _galeria = await generales.PickAndShowFile(galeria);
            await CargaImagen(5);
        }

        private async Task OpenCameraAsync()
        {

            // Obsoleted
            //var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Camera);
            //if (cameraStatus != PermissionStatus.Granted)
            //{
            //    var results = await CrossPermissions.Current.RequestPermissionsAsync(Plugin.Permissions.Abstractions.Permission.Camera);
            //    if (results.ContainsKey(Plugin.Permissions.Abstractions.Permission.Camera))
            //        cameraStatus = results[Plugin.Permissions.Abstractions.Permission.Camera];
            //}
            //var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Storage);
            //if (storageStatus != PermissionStatus.Granted)
            //{
            //    var results = await CrossPermissions.Current.RequestPermissionsAsync(Plugin.Permissions.Abstractions.Permission.Storage);
            //    if (results.ContainsKey(Plugin.Permissions.Abstractions.Permission.Storage))
            //        storageStatus = results[Plugin.Permissions.Abstractions.Permission.Storage];
            //}

            //if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
            //{
            //    await CrossMedia.Current.Initialize();
            //    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            //    {
            //        await DialogService.ShowAlertAsync(TextsTranslateManager.Translate("CamNotReady"), TextsTranslateManager.Translate("Warning"), "OK");
            //        return;
            //    }
            //    var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            //    {
            //        PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small,
            //        CompressionQuality = 30
            //    });

            //    if (file == null)
            //        return;
            //    StoreImageSource = ImageSource.FromStream(() =>
            //    {
            //        var stream = file.GetStream();
            //        return stream;
            //    });
            //}

            try
            {
                var camStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<CameraPermission>();
                if (camStatus != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera))
                    {
                        await DialogService.ShowAlertAsync(TextsTranslateManager.Translate("CamNotReady"), TextsTranslateManager.Translate("Warning"), "OK");
                    }

                    camStatus = await CrossPermissions.Current.RequestPermissionAsync<CameraPermission>();
                }

                if (camStatus == PermissionStatus.Granted)
                {
                    //Once Camera access granted -> check Storage permission
                    var storageStat = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
                    if (storageStat != PermissionStatus.Granted)
                    {
                        if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                        {
                            await DialogService.ShowAlertAsync(TextsTranslateManager.Translate("StorageNotAllowed"), TextsTranslateManager.Translate("Warning"), "OK");
                        }

                        storageStat = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                    }

                    if (storageStat == PermissionStatus.Granted)
                    {
                        await CrossMedia.Current.Initialize();
                        if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                        {
                            await DialogService.ShowAlertAsync(TextsTranslateManager.Translate("CamNotReady"), TextsTranslateManager.Translate("Warning"), "OK");
                            return;
                        }
                        var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                        {
                            PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small,
                            CompressionQuality = 30
                        });

                        if (file == null)
                            return;
                        StoreImageSource = ImageSource.FromStream(() =>
                        {
                            var stream = file.GetStream();
                            return stream;
                        });
                    }

                    else if (storageStat != PermissionStatus.Unknown)
                    {
                        await DialogService.ShowAlertAsync(TextsTranslateManager.Translate("StorageNotAllowed"), TextsTranslateManager.Translate("Warning"), "OK");
                    }

                }
                else if (camStatus != PermissionStatus.Unknown)
                {
                    await DialogService.ShowAlertAsync(TextsTranslateManager.Translate("CamNotAllowed"), TextsTranslateManager.Translate("Warning"), "OK");
                }
            }
            catch (Exception e)
            {
                await DialogService.ShowAlertAsync(TextsTranslateManager.Translate("SomethingWrong"), TextsTranslateManager.Translate("Warning"), "OK");
            }
        }

        private void RemoveStoreImageAsync()
        {
            StoreImageSource = null;
        }

        private async Task SaveStoreSettingAsync()
        {
            IsBusy = true;
            bool result = Validate();
            if(!result)
            {
                IsBusy = false;
                return;
            }
            var storeSettings = new StoreSettings
            {
                StoreName = StoreName,
                StoreOwner = StoreOwner,
                Ruc=Ruc,
                Address = StoreAddress,
                Email = StoreEmail,
                Mobile = StorePhoneNumer,
                IsPrintBillNotIncludedTax = IsPrintBillNotIncludedTax,
                IsDisplayTotalPriceBeforeTax = IsDisplayTotalPriceBeforeTax,
                VAT = VAT,
                TaxNumber = TaxNumber
            };
            var setting = new Setting
            {
                Id = Generator.GenerateKey(),
                SettingType = (int)SettingType.StoreSetting,
                Data = JsonConvert.SerializeObject(storeSettings)
            };
            if (_isSettingExists)
            {
                setting.Id = _storeSettingId;
            }
            if (StoreImageSource != null)
            {
                setting.Logo = StoreImageSource.GetByteFromImageSource();
                /*Actualizar datos generales*/
                SettingsOnline.oAplicacion.Id = setting.Id;
                SettingsOnline.oAplicacion.Nombre = StoreName;
                SettingsOnline.oAplicacion.Telefono = StorePhoneNumer;
                SettingsOnline.oAplicacion.IdentificacionAdministrador = Ruc;
                SettingsOnline.oAplicacion.Logo = setting.Logo;
            }
            string informedMessage = "";
            /*if (_isSettingExists)
            {
                setting.Id = _storeSettingId;
                await _settingsService.UpdateSettingAsync(setting);
                informedMessage = TextsTranslateManager.Translate("StoreUpdateSuccess");
            }
            else
            {
                await _settingsService.InsertSettingAsync(setting);
                informedMessage = TextsTranslateManager.Translate("StoreInfoSaved");
            }*/
            
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            informedMessage = await _switchConfiguracion.GuardaConfiguracion(setting, estaConectado, _isSettingExists);
            _isSettingExists = false;
            var iregistro = new Empresa
            {
                Id = setting.Id,
                Codigo = StoreName.ToLower().Replace(" ", string.Empty),
                Descripcion = StoreName,
                Identificacion = Ruc,
                Direccion = StoreAddress,
                Celular = StorePhoneNumer,
                Email = StoreEmail,
                TelefonoFijo = "",
                Representante = StoreOwner,
                EsProveedor = 0,
                PorDefecto = 1,
                IdGaleria = "",
                Imagen = Convert.ToBase64String(StoreImageSource.GetByteFromImageSource()),
                NoEsConsulta = 1
            };
            iregistro.EsEdicion = _isSettingExists ? 1 : 0;            
            var resultado = await _switchAdministracion.GuardaRegistro_Empresa(iregistro, estaConectado, _isSettingExists);
            IsBusy = false;
            MessagingCenter.Send<string>("", "StoreLogoChanged");
            await NavigationService.RemoveLastFromBackStackAsync();
            DialogService.ShowToast(informedMessage);
        }

        private bool Validate()
        {
            IsStoreNameInValid = string.IsNullOrEmpty(StoreName);
            IsOwnerNameInValid = string.IsNullOrEmpty(StoreOwner);
            IsRucInValid = string.IsNullOrEmpty(Ruc)|| !Escant_App.AppSettings.Settings.NumerosRegex.IsMatch(Ruc) || Ruc.Length<13;
            IsAddressInValid = string.IsNullOrEmpty(StoreAddress);
            IsMobileInValid = string.IsNullOrEmpty(StorePhoneNumer) || !Escant_App.AppSettings.Settings.MobileRegex.IsMatch(StorePhoneNumer);
            IsEmailInValid = !string.IsNullOrEmpty(StoreEmail) && !Escant_App.AppSettings.Settings.EmailRegex.IsMatch(StoreEmail);

            if (IsStoreNameInValid || IsOwnerNameInValid || IsAddressInValid || IsMobileInValid
                || IsEmailInValid)
                return false;
            else
                return true;
        }

        private async Task DiscardAsync()
        {
            await NavigationService.RemoveLastFromBackStackAsync();
        }

        public ImageSource StoreImageSource
        {
            get => _storeImageSource;
            set
            {
                _storeImageSource = value;
                RaisePropertyChanged(() => StoreImageSource);
            }
        }

        private async Task CargaImagen(int Origen)
        {
            switch (Origen)
            {
                case 1:
                    StoreImageSource = ImageSource.FromStream(() =>
                    {
                        var stream = generales.ByteArrayToStream(Convert.FromBase64String(_galeria.Image));
                        return stream;
                    });
                    break;
                case 2:
                    StoreImageSource = ImageSource.FromStream(() =>
                    {
                        var stream = generales.ByteArrayToStream(Convert.FromBase64String(_galeria.Image));
                        return stream;
                    });
                    break;
                case 3:
                    StoreImageSource = ImageSource.FromStream(() =>
                    {
                        var stream = new MemoryStream();
                        return stream;
                    });
                    break;
                case 4:
                    StoreImageSource = ImageSource.FromStream(() =>
                    {
                        var stream = generales.ByteArrayToStream(Convert.FromBase64String(Imagen));
                        return stream;
                    });
                    break;
                case 5:
                    StoreImageSource = ImageSource.FromStream(() => new MemoryStream(_galeria.fByte));
                    break;


            }
        }

    }
}
