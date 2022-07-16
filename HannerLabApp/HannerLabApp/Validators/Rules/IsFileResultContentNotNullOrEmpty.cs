using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace HannerLabApp.Validators.Rules
{
    internal class IsFileResultContentNotNullOrEmpty : IValidationRule<FileResult>
    {
        public string ValidationMessage { get; set; }

        public bool Check(FileResult value)
        {
            if (value == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(value.ContentType))
            {
                return false;
            }

            return true;
        }
    }
}
