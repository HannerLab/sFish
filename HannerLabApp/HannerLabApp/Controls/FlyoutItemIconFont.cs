using Xamarin.Forms;

namespace HannerLabApp.Controls
{
    /// <summary>
    /// Extension to FlyoutItem with an icon font (Font Awesome et al).
    /// See: https://docs.microsoft.com/en-us/answers/questions/260997/how-do-i-change-the-color-of-an-icon-in-a-flyoutit.html
    ///      https://stackoverflow.com/questions/65400688/xamarin-forms-how-can-i-change-the-flyoutitem-icons-color-when-it-is-selected
    /// </summary>
    public class FlyoutItemIconFont : FlyoutItem
    {
        public static readonly BindableProperty IconGlyphProperty = BindableProperty.Create(nameof(IconGlyphProperty), typeof(string), typeof(FlyoutItemIconFont), string.Empty);
        public string IconGlyph
        {
            get { return (string)GetValue(IconGlyphProperty); }
            set { SetValue(IconGlyphProperty, value); }
        }

        public static readonly BindableProperty IconFontFamilyProperty = BindableProperty.Create(nameof(IconFontFamilyProperty), typeof(string), typeof(FlyoutItemIconFont), string.Empty);
        public string IconFontFamily
        {
            get { return (string)GetValue(IconFontFamilyProperty); }
            set { SetValue(IconFontFamilyProperty, value); }
        }
    }
}
