using System;
using HannerLabApp.Models;
using TinyMvvm;

namespace HannerLabApp.ViewModels.ActivityViewModels
{
    /// <summary>
    /// Activity Export History List item
    /// </summary>
    public class ActivityExportViewModel : ViewModelBase, IValidableViewModel<Export>
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }

        private Export _model;
        public Export Model
        {
            get => _model;
            set => Set(ref _model, value);
        }

        public static readonly string TitleBaseStatic = "Activity Export";
        public string TitleBase => TitleBaseStatic;
        public bool IsAdvancedShown { get; set; }
        public bool IsValid => true;

        public string Timestamp => _model.Activity?.Timestamp.ToString() ?? string.Empty;
        public string Name => _model.Activity?.Name ?? string.Empty;
        public string UserSpecifiedId => _model.Activity?.UserSpecifiedId ?? string.Empty;

        public bool Validate()
        {
            throw new NotImplementedException();
        }

    }
}
