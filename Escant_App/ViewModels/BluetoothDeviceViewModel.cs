using System;
using Escant_App.ViewModels.Base;

namespace Escant_App.ViewModels
{
    public class BluetoothDeviceViewModel : ViewModelBase
    {
        public string Id { get; set; }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        private string _address;
        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                RaisePropertyChanged(() => Address);
            }
        }

        public string DisplayName
        {
            get
            {
                return $"{Name} - {Address}";
            }
        }
    }
}
