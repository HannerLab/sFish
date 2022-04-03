using System;
using HannerLabApp.Models;
using TinyMvvm;

namespace HannerLabApp.ViewModels
{
    public interface IValidableViewModel<T> : IViewModelBase where T :ISavable
    {
        Guid Id { get; set; }
        bool IsAdvancedShown { get; set; }
        string TitleBase { get; }
        T Model { get; set; }
        bool IsValid { get; }
        bool Validate();
    }
}
