using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DataModel;
using DataModel.DTO;
using DataModel.DTO.Configuracion;
using DataModel.Enums;
using DataModel.Helpers;
using Escant_App.AppSettings;

using Escant_App.Helpers;
using ManijodaServicios.Resources.Texts;
using ManijodaServicios.Offline.Interfaz;
using Escant_App.ViewModels.Base;
using Newtonsoft.Json;
using Plugin.Multilingual;
using Xamarin.Forms;
using ManijodaServicios.Offline.Interfaz;
using ManijodaServicios.Switch;

namespace Escant_App.ViewModels.Configuracion
{
    public class AppSettingsViewModel : ViewModelBase
    {
        private SwitchConfiguracion _switchConfiguracion;
        private Generales.Generales generales = new Generales.Generales();
        private readonly ISettingsService _settingsService;
        private bool _isSettingExists;
        private string _settingId;

        public AppSettingsViewModel(ISettingsService settingService)
        {
            _settingsService = settingService;
            _switchConfiguracion = new SwitchConfiguracion(_settingsService);
        }

        private bool _isEnableSelectCustomer;
        public bool IsEnableSelectCustomer
        {
            get => _isEnableSelectCustomer;
            set
            {
                _isEnableSelectCustomer = value;
                RaisePropertyChanged(() => IsEnableSelectCustomer);
            }
        }

        private bool _isEnableSelectSupplier;
        public bool IsEnableSelectSupplier
        {
            get => _isEnableSelectSupplier;
            set
            {
                _isEnableSelectSupplier = value;
                RaisePropertyChanged(() => IsEnableSelectSupplier);
            }
        }

        private bool _isEnableSelectStaff;
        public bool IsEnableSelectStaff
        {
            get => _isEnableSelectStaff;
            set
            {
                _isEnableSelectStaff = value;
                RaisePropertyChanged(() => IsEnableSelectStaff);
            }
        }
        private bool? _showCustomFormat;
        public bool? ShowCustomFormat
        {
            get => _showCustomFormat;
            set
            {
                _showCustomFormat = value;
                RaisePropertyChanged(() => ShowCustomFormat);
            }
        }
        private bool _isUseDefaultCurrencyFormat;
        public bool IsUseDefaultCurrencyFormat
        {
            get => _isUseDefaultCurrencyFormat;
            set
            {
                _isUseDefaultCurrencyFormat = value;
                RaisePropertyChanged(() => IsUseDefaultCurrencyFormat);
                ShowCustomFormat = !IsUseDefaultCurrencyFormat;
                RaisePropertyChanged(() => ShowCustomFormat);
                Settings.IsUseDefaultCurrencyFormat = value;
            }
        }
        private bool _isEnableDelivery;
        public bool IsEnableDelivery
        {
            get => _isEnableDelivery;
            set
            {
                _isEnableDelivery = value;
                RaisePropertyChanged(() => IsEnableDelivery);
            }
        }

        private bool _isEnableRevenue;
        public bool IsEnableRevenue
        {
            get => _isEnableRevenue;
            set
            {
                _isEnableRevenue = value;
                RaisePropertyChanged(() => IsEnableRevenue);
            }
        }

        private bool _isDisplayStockBalanceQty;
        public bool IsDisplayStockBalanceQty
        {
            get => _isDisplayStockBalanceQty;
            set
            {
                _isDisplayStockBalanceQty = value;
                RaisePropertyChanged(() => IsDisplayStockBalanceQty);
            }
        }

        private bool _isEnableHotKeyWhenSearchProduct;
        public bool IsEnableHotKeyWhenSearchProduct
        {
            get => _isEnableHotKeyWhenSearchProduct;
            set
            {
                _isEnableHotKeyWhenSearchProduct = value;
                RaisePropertyChanged(() => IsEnableHotKeyWhenSearchProduct);
            }
        }

        private bool _isEnableAutoBackup;
        public bool IsEnableAutoBackup
        {
            get => _isEnableAutoBackup;
            set
            {
                _isEnableAutoBackup = value;
                RaisePropertyChanged(() => IsEnableAutoBackup);
            }
        }

        private bool _isEnablePromotionPrice;
        public bool IsEnablePromotionPrice
        {
            get => _isEnablePromotionPrice;
            set
            {
                _isEnablePromotionPrice = value;
                RaisePropertyChanged(() => IsEnablePromotionPrice);
            }
        }

        private bool _isEnableStaffCannotAdjustSalePrice;
        public bool IsEnableStaffCannotAdjustSalePrice
        {
            get => _isEnableStaffCannotAdjustSalePrice;
            set
            {
                _isEnableStaffCannotAdjustSalePrice = value;
                RaisePropertyChanged(() => IsEnableStaffCannotAdjustSalePrice);
            }
        }

        private bool _isEnableStaffCannotAdjustInboundPrice;
        public bool IsEnableStaffCannotAdjustInboundPrice
        {
            get => _isEnableStaffCannotAdjustInboundPrice;
            set
            {
                _isEnableStaffCannotAdjustInboundPrice = value;
                RaisePropertyChanged(() => IsEnableStaffCannotAdjustInboundPrice);
            }
        }

        private bool _isPreventSellWhenNegative;
        public bool IsPreventSellWhenNegative
        {
            get => _isPreventSellWhenNegative;
            set
            {
                _isPreventSellWhenNegative = value;
                RaisePropertyChanged(() => IsPreventSellWhenNegative);
            }
        }
        private List<string> _formatList = new List<string> { "#,##0", "#,##.00" };
        public List<string> FormatList
        {
            get => _formatList;
            set
            {
                _formatList = value;
                RaisePropertyChanged(() => FormatList);
            }
        }
        private List<SelectItem> _languageList = Escant_App.AppSettings.Settings.Languages;
        public List<SelectItem> LanguageList
        {
            get => _languageList;
            set
            {
                _languageList = value;
                RaisePropertyChanged(() => LanguageList);
            }
        }

        private SelectItem _languageSelected;
        public SelectItem LanguageSelected
        {
            get => _languageSelected;
            set
            {
                _languageSelected = value;
                RaisePropertyChanged(() => LanguageSelected);
                Settings.LanguageSelected = value?.Code;
            }
        }
        private string _currentCurrencyCustomFormat;
        public string CurrentCurrencyCustomFormat
        {
            get => _currentCurrencyCustomFormat;
            set
            {
                _currentCurrencyCustomFormat = value;
                RaisePropertyChanged(() => CurrentCurrencyCustomFormat);
                Settings.CurrentCurrencyCustomFormat = value;
            }
        }

        private string _customCurrencySymbol;
        public string CustomCurrencySymbol
        {
            get => _customCurrencySymbol;
            set
            {
                _customCurrencySymbol = value;
                RaisePropertyChanged(() => CustomCurrencySymbol);
                Settings.CustomCurrencySymbol = value;
            }
        }
        public async Task LoadSettings()
        {
            IsBusy = true;
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            var setting = await _switchConfiguracion.ConsultaConfiguracion(new Setting() { SettingType = (int)SettingType.ApplicationSetting }, estaConectado);
            //var setting = await _settingsService.GetSettingAsync(x => x.SettingType == (int)SettingType.ApplicationSetting);
            if(setting != null)
            {
                _isSettingExists = true;
                _settingId = setting.Id ?? Generator.GenerateKey();
                ApplicationSettings applicationSettings = JsonConvert.DeserializeObject<ApplicationSettings>(setting.Data);
                if (applicationSettings != null)
                {
                    IsEnableSelectCustomer = applicationSettings.IsEnableSelectCustomer;
                    IsEnableSelectSupplier = applicationSettings.IsEnableSelectSupplier;
                    IsEnableSelectStaff = applicationSettings.IsEnableSelectStaff;
                    IsEnableDelivery = applicationSettings.IsEnableDelivery;
                    IsEnableRevenue = applicationSettings.IsEnableRevenue;
                    IsUseDefaultCurrencyFormat = applicationSettings.IsUseDefaultCurrencyFormat ?? true;
                    CurrentCurrencyCustomFormat = applicationSettings.CurrentCurrencyCustomFormat;
                    LanguageSelected = !string.IsNullOrEmpty(applicationSettings.LanguageSelected) ? Escant_App.AppSettings.Settings.Languages.FirstOrDefault(x => x.Code == applicationSettings.LanguageSelected) : null;
                    CustomCurrencySymbol = applicationSettings.CustomCurrencySymbol;
                    IsDisplayStockBalanceQty = applicationSettings.IsDisplayStockBalanceQty;
                    IsEnableHotKeyWhenSearchProduct = applicationSettings.IsEnableHotKeyWhenSearchProduct;
                    IsEnableAutoBackup = applicationSettings.IsEnableAutoBackup;
                    IsEnablePromotionPrice = applicationSettings.IsEnablePromotionPrice;
                    IsEnableStaffCannotAdjustSalePrice = applicationSettings.IsEnableStaffCannotAdjustSalePrice;
                    IsEnableStaffCannotAdjustInboundPrice = applicationSettings.IsEnableStaffCannotAdjustInboundPrice;
                    IsPreventSellWhenNegative = applicationSettings.IsPreventSellWhenNegative;
                }
            }
            else
            {
                _isSettingExists = false;
            }
            IsBusy = false;
        }

        public ICommand SaveApplicationSettingCommand => new Command(async () => await SaveApplicationSettingAsync());
        public ICommand DiscardCommand => new Command(async () => await DiscardAsync());

        private async Task SaveApplicationSettingAsync()
        {
            IsBusy = true;
            var applicationSettings = new ApplicationSettings
            {
                IsEnableSelectCustomer = IsEnableSelectCustomer,
                IsEnableSelectSupplier = IsEnableSelectSupplier,
                IsEnableSelectStaff = IsEnableSelectStaff,
                IsEnableDelivery = IsEnableDelivery,
                IsEnableRevenue = IsEnableRevenue,
                IsUseDefaultCurrencyFormat = IsUseDefaultCurrencyFormat,
                CurrentCurrencyCustomFormat = CurrentCurrencyCustomFormat,
                LanguageSelected = LanguageSelected?.Code,
                CustomCurrencySymbol = CustomCurrencySymbol,
                IsDisplayStockBalanceQty = IsDisplayStockBalanceQty,
                IsEnableHotKeyWhenSearchProduct = IsEnableHotKeyWhenSearchProduct,
                IsEnableAutoBackup = IsEnableAutoBackup,
                IsEnablePromotionPrice = IsEnablePromotionPrice,
                IsEnableStaffCannotAdjustSalePrice = IsEnableStaffCannotAdjustSalePrice,
                IsEnableStaffCannotAdjustInboundPrice = IsEnableStaffCannotAdjustInboundPrice,
                IsPreventSellWhenNegative = IsPreventSellWhenNegative
            };
            
            var setting = new Setting
            {
                Id = Generator.GenerateKey(),
                SettingType = (int)SettingType.ApplicationSetting,
                Data = JsonConvert.SerializeObject(applicationSettings)
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
                await _settingsService.InsertSettingAsync(setting);
                informedMessage =TextsTranslateManager.Translate("SettingSaved");
            }*/
            if (_isSettingExists)
            {
                setting.Id = _settingId;
            }
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            informedMessage = await _switchConfiguracion.GuardaConfiguracion(setting, estaConectado, _isSettingExists);
            IsBusy = false;
            await NavigationService.RemoveLastFromBackStackAsync();
            DialogService.ShowToast(informedMessage);
        }

        private async Task DiscardAsync()
        {
            await NavigationService.RemoveLastFromBackStackAsync();
        }
    }
}
