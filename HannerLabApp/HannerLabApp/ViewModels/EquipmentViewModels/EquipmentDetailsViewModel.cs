using System.Collections.Generic;
using System.Windows.Input;
using HannerLabApp.Models;
using HannerLabApp.Services;
using HannerLabApp.Services.Managers;
using TinyMvvm;
using HannerLabApp.Extensions;

namespace HannerLabApp.ViewModels.EquipmentViewModels
{
    public class EquipmentDetailsViewModel : DetailsViewModelBase<Equipment>
    {
        public IList<string> UnitTypes { get; }

        public ICommand AddUnitCommand { get; private set; }
        public ICommand RemoveUnitCommand { get; private set; }

        public EquipmentDetailsViewModel(IValidableViewModel<Equipment> viewModel, IManager<Equipment> manager, IPageService pageService) : base(viewModel, manager, pageService)
        {
            AddUnitCommand = new TinyCommand(AddUnit);
            RemoveUnitCommand = new TinyCommand<UnitViewModel>(RemoveUnit);

            this.UnitTypes = UnitType.Depth.GetDescriptionList();
        }

        private void AddUnit()
        {
            (ViewModel as EquipmentViewModel)?.Units.Value.Add(new UnitViewModel());
        }

        private void RemoveUnit(UnitViewModel vm)
        {
            // If this unit entry viewmodel is the only one, don't let them delete it, just empty its value.
            (ViewModel as EquipmentViewModel)?.Units.Value.Remove(vm);

            if ((ViewModel as EquipmentViewModel)?.Units.Value.Count <= 0)
            {
                (ViewModel as EquipmentViewModel)?.Units.Value.Add(new UnitViewModel());
            }
        }
    }
}
