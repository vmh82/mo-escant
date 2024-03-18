using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Escant_App.AppSettings;
using Escant_App.ViewModels.Base;
using Xamarin.Forms;
using System.Linq;
using Escant_App.Helpers;
using Escant_App.Interfaces;
using DataModel.Enums;
using Escant_App.Services;
using Newtonsoft.Json;
using DataModel;
using DataModel.DTO;
using DataModel.DTO.Configuracion;
using DataModel.Helpers;
using System.Collections.ObjectModel;
using Syncfusion.XForms.ComboBox;
using ManijodaServicios.Resources.Texts;
using ManijodaServicios.Switch;
using ManijodaServicios.Offline.Interfaz;
using ManijodaServicios.AppSettings;

namespace Escant_App.ViewModels.Configuracion
{
    public class ReceiptPrinterViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private SwitchConfiguracion _switchConfiguracion;
        private Generales.Generales generales = new Generales.Generales();
        private bool _isSettingExists;
        private string _settingId;

        internal SfComboBox ComboBox { get; set; }

        public ReceiptPrinterViewModel(IServicioSeguridad_Usuario seguridadServicio
                                  , ISettingsService settingService)
        {
            BindingContext = new ConectividadModelo(seguridadServicio);
            _settingsService = settingService;
            _switchConfiguracion = new SwitchConfiguracion(_settingsService);            
        }

        private bool _isEnableConnectToPrinter;
        public bool IsEnableConnectToPrinter
        {
            get => _isEnableConnectToPrinter;
            set
            {
                _isEnableConnectToPrinter = value;
                RaisePropertyChanged(() => IsEnableConnectToPrinter);
            }
        }

        private bool _isEnablePrint;
        public bool IsEnablePrint
        {

            get => _isEnablePrint;
            set
            {
                _isEnablePrint = value;
                RaisePropertyChanged(() => IsEnablePrint);
            }
        }

        private bool _isBluetoothChecked;
        public bool IsBluetoothChecked
        {
            get => _isBluetoothChecked;
            set
            {
                _isBluetoothChecked = value;
                RaisePropertyChanged(() => IsBluetoothChecked);
            }
        }

        private bool _isWifiChecked;
        public bool IsWifiChecked
        {
            get => _isWifiChecked;
            set
            {
                _isWifiChecked = value;
                RaisePropertyChanged(() => IsWifiChecked);
            }
        }

        private bool _isSizeAChecked;
        public bool IsSizeAChecked
        {
            get => _isSizeAChecked;
            set
            {
                _isSizeAChecked = value;
                RaisePropertyChanged(() => IsSizeAChecked);
            }
        }

        private bool _isSizeBChecked;
        public bool IsSizeBChecked
        {
            get => _isSizeBChecked;
            set
            {
                _isSizeBChecked = value;
                RaisePropertyChanged(() => IsSizeBChecked);
            }
        }

        private bool _isEnableConfirmBeforePrint;
        public bool IsEnableConfirmBeforePrint
        {
            get => _isEnableConfirmBeforePrint;
            set
            {
                _isEnableConfirmBeforePrint = value;
                RaisePropertyChanged(() => IsEnableConfirmBeforePrint);
            }
        }

        private bool _isEnablePrintCopy;
        public bool IsEnablePrintCopy
        {
            get => _isEnablePrintCopy;
            set
            {
                _isEnablePrintCopy = value;
                RaisePropertyChanged(() => IsEnablePrintCopy);
            }
        }

        private string _saleOrderHeader;
        public string SaleOrderHeader
        {
            get => _saleOrderHeader;
            set
            {
                _saleOrderHeader = value;
                RaisePropertyChanged(() => SaleOrderHeader);
            }
        }

        private string _saleOrderFooter;
        public string SaleOrderFooter
        {
            get => _saleOrderFooter;
            set
            {
                _saleOrderFooter = value;
                RaisePropertyChanged(() => SaleOrderFooter);
            }
        }

        private string _purchaseOrderHeader;
        public string PurchaseOrderHeader
        {
            get => _purchaseOrderHeader;
            set
            {
                _purchaseOrderHeader = value;
                RaisePropertyChanged(() => PurchaseOrderHeader);
            }
        }

        private bool _isShownCostPrice;
        public bool IsShownCostPrice
        {
            get => _isShownCostPrice;
            set
            {
                _isShownCostPrice = value;
                RaisePropertyChanged(() => IsShownCostPrice);
            }
        }

        private bool _isShownAddress;
        public bool IsShownAddress
        {
            get => _isShownAddress;
            set
            {
                _isShownAddress = value;
                RaisePropertyChanged(() => IsShownAddress);
            }
        }

        private bool _isShownPhoneNumber;
        public bool IsShownPhoneNumber
        {
            get => _isShownPhoneNumber;
            set
            {
                _isShownPhoneNumber = value;
                RaisePropertyChanged(() => IsShownPhoneNumber);
            }
        }

        private bool _isShownBarcode;
        public bool IsShownBarcode
        {
            get => _isShownBarcode;
            set
            {
                _isShownBarcode = value;
                RaisePropertyChanged(() => IsShownBarcode);
            }
        }

        private bool _isShownPrintDate;
        public bool IsShownPrintDate
        {
            get => _isShownPrintDate;
            set
            {
                _isShownPrintDate = value;
                RaisePropertyChanged(() => IsShownPrintDate);
            }
        }

        private string _defaultDevice;
        public string DefaultDevice
        {
            get => _defaultDevice;
            set
            {
                _defaultDevice = value;
                RaisePropertyChanged(() => DefaultDevice);
            }
        }

        private ObservableCollection<BluetoothDeviceViewModel> _devicesList;
        public ObservableCollection<BluetoothDeviceViewModel> DeviceList
        {
            get => _devicesList;
            set
            {
                _devicesList  = value;
                RaisePropertyChanged(() => DeviceList);
            }
        }

        private BluetoothDeviceViewModel _selectedDevice;
        public BluetoothDeviceViewModel SelectedDevice
        {
            get => _selectedDevice;
            set
            {
                _selectedDevice = value;
                RaisePropertyChanged(() => SelectedDevice);
            }
        }

        public ICommand SavePrinterSettingCommand => new Command(async () => await SavePrinterSettingAsync());
        public ICommand DiscardCommand => new Command(async () => await DiscardAsync());

        private async Task DiscardAsync()
        {
            await NavigationService.RemoveLastFromBackStackAsync();
        }

        private async Task SavePrinterSettingAsync()
        {
            IsBusy = true;
            var printerSettings = new PrinterSettings
            {
                IsEnableConnectToPrinter = IsEnableConnectToPrinter,
                DefaultDevice = SelectedDevice?.Address,
                SaleOrderHeader = SaleOrderHeader,
                SaleOrderFooter = SaleOrderFooter,
                PurchaseOrderHeader = PurchaseOrderHeader,
                IsShownCostPrice = IsShownCostPrice,
                IsShownAddress = IsShownAddress,
                IsShownPhoneNumber = IsShownPhoneNumber,
                IsShownBarcode = IsShownBarcode,
                IsShownPrintDate = IsShownPrintDate,
                IsEnableConfirmBeforePrint = IsEnableConfirmBeforePrint,
                IsEnablePrintCopy = IsEnablePrintCopy
            };

            if (IsWifiChecked)
                printerSettings.ConnectionType = ConnectionType.Wifi;
            else if (IsBluetoothChecked)
                printerSettings.ConnectionType = ConnectionType.Bluetooth;

            if (IsSizeAChecked)
                printerSettings.PaperSize = PaperSize.Size_58;
            else if (IsSizeBChecked)
                printerSettings.PaperSize = PaperSize.Size_80;

            var setting = new Setting
            {
                SettingType = (int)SettingType.PrinterSetting,
                Data = JsonConvert.SerializeObject(printerSettings)
            };            
            string informedMessage = "";
            /*if (_isSettingExists)
            {
                setting.Id = _settingId;
                await _settingsService.UpdateSettingAsync(setting);
                informedMessage = TextsTranslateManager.Translate("SettingUpdatedSuccess");
            }
            else
            {
                setting.Id = Generator.GenerateKey();
                await _settingsService.InsertSettingAsync(setting);
                informedMessage = TextsTranslateManager.Translate("SettingSaved");
            }*/
            if (_isSettingExists)
            {
                setting.Id = _settingId;
            }
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            informedMessage = await _switchConfiguracion.GuardaConfiguracion(setting, estaConectado, _isSettingExists);
            SettingsOnline.oAplicacion.ParametrosImpresion = JsonConvert.SerializeObject(printerSettings);
            IsBusy = false;
            await NavigationService.RemoveLastFromBackStackAsync();
            DialogService.ShowToast(informedMessage);
        }

        public async Task LoadSettings()
        {
            IsBusy = true;
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            var setting = await _switchConfiguracion.ConsultaConfiguracion(new Setting() { SettingType = (int)SettingType.PrinterSetting }, estaConectado);
            //var setting = await _settingsService.GetSettingAsync(x => x.SettingType == (int)SettingType.PrinterSetting);
            if(setting != null)
            {
                _isSettingExists = true;
                _settingId = setting.Id;
                var printerSettings = JsonConvert.DeserializeObject<PrinterSettings>(setting.Data);
                if(printerSettings != null)
                {
                    IsEnableConnectToPrinter = printerSettings.IsEnableConnectToPrinter;
                    switch(printerSettings.ConnectionType)
                    {
                        case ConnectionType.Bluetooth:
                            IsBluetoothChecked = true;
                            break;
                        case ConnectionType.Wifi:
                            IsWifiChecked = true;
                            break;
                        default:
                            break; 
                    }
                    switch(printerSettings.PaperSize)
                    {
                        case PaperSize.Size_58:
                            IsSizeAChecked = true;
                            break;
                        case PaperSize.Size_80:
                            IsSizeBChecked = true;
                            break;
                        default:
                            break;
                    }
                    DefaultDevice = printerSettings.DefaultDevice;
                    SaleOrderHeader = printerSettings.SaleOrderHeader;
                    SaleOrderFooter = printerSettings.SaleOrderFooter;
                    PurchaseOrderHeader = printerSettings.PurchaseOrderHeader;
                    IsShownCostPrice = printerSettings.IsShownCostPrice;
                    IsShownAddress = printerSettings.IsShownAddress;
                    IsShownPhoneNumber = printerSettings.IsShownPhoneNumber;
                    IsShownBarcode = printerSettings.IsShownBarcode;
                    IsShownPrintDate = printerSettings.IsShownPrintDate;
                    IsEnableConfirmBeforePrint = printerSettings.IsEnableConfirmBeforePrint;
                    IsEnablePrintCopy = printerSettings.IsEnablePrintCopy;
                    //Load bluetooth devices
                    LoadPrinters(DefaultDevice);
                }
            }
            else
            {
                _isSettingExists = false;
            }
            IsBusy = false;
        }

        public void LoadPrinters(string defaultPrinter = "")
        {
            var bluetooth = App.BluetoothAdaper;
            if (bluetooth.IsBluetoothTurnOn)
            {
                var devices = bluetooth.GetPairedDevices();
                if (devices != null)
                {
                    DeviceList = new ObservableCollection<BluetoothDeviceViewModel>(bluetooth.GetPairedDevices().Select(x => new BluetoothDeviceViewModel
                    {
                        Address = x.Address,
                        Name = x.Name
                    }).ToList());
                }
                if (DeviceList != null)
                {
                    if (!string.IsNullOrWhiteSpace(DefaultDevice))
                        ComboBox.SelectedItem = DeviceList.FirstOrDefault(x => x.Address == DefaultDevice);
                    else
                        ComboBox.SelectedItem = DeviceList.FirstOrDefault();
                }
            }
        }
    }
}
