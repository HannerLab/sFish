using HannerLabApp.Extensions;
using HannerLabApp.Models;
using TinyMvvm;

namespace HannerLabApp.ViewModels.EquipmentViewModels
{
    public class UnitViewModel : ViewModelBase
    {
        private UnitType _unitType;
        private string _unitValue = string.Empty;
        public string Description => this.UnitType.GetDescription();

        public UnitType UnitType
        {
            get => _unitType;
            set => Set(ref _unitType, value);
        }

        public string UnitValue
        {
            get => _unitValue;
            set => Set(ref _unitValue, value);
        }
    }
}
