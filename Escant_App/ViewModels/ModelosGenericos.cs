using System;
using Escant_App.ViewModels.Base;

namespace Escant_App.ViewModels
{
    public class OrganizationUnitViewModel : ViewModelBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class GenderViewModel : ViewModelBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class DiscountRateViewModel : ViewModelBase
    {
        public long DiscountRate { get; set; }
        public string Name { get; set; }
    }
}
