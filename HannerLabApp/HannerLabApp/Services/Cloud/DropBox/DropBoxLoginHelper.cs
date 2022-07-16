using System;
using System.Threading.Tasks;
using Dropbox.Api;
using Xamarin.Forms;

namespace HannerLabApp.Services.Cloud.DropBox
{
    public class DropboxLoginHelper : ICloudAuthenticator
    {
        private readonly IPageService _pageService;

        private readonly string _clientId;
        private const string RedirectUri = "http://127.0.0.1:52475/authorize";
        private string _oauth2State;

#nullable enable
        private string? _token = null;

        public DropboxLoginHelper(IPageService pageService)
        {
            _clientId = App.AppConfiguration.DropBoxDevToken;
            _pageService = pageService;
        }

        /// <summary>
        /// Authenticates the users dropbox account. Returns the API key. 
        /// </summary>
        /// <returns></returns>
        public async Task<string> Authenticate()
        {
            _token = null;

            this._oauth2State = Guid.NewGuid().ToString("N");
            var authorizeUri = DropboxOAuth2Helper.GetAuthorizeUri(OAuthResponseType.Token, _clientId, new Uri(RedirectUri), this._oauth2State);
            var webView = new WebView { Source = new UrlWebViewSource { Url = authorizeUri.AbsoluteUri } };
            webView.Navigating += this.WebViewOnNavigating;
            var contentPage = new ContentPage { Content = webView };

            //await Application.Current.MainPage.Navigation.PushModalAsync(contentPage);
            await _pageService.NavigateToAsync(contentPage);

            while (_token == null)
            {
                await Task.Delay(1);
            }

            return _token;
        }

        private async void WebViewOnNavigating(object sender, WebNavigatingEventArgs e)
        {
            // we need to ignore all navigation that isn't to the redirect uri.
            if (!e.Url.StartsWith(RedirectUri, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            try
            {
                var result = DropboxOAuth2Helper.ParseTokenFragment(new Uri(e.Url));

                if (result.State != this._oauth2State)
                {
                    return;
                }

                _token = result.AccessToken;
            }
            catch (ArgumentException)
            {
                _token = string.Empty;
            }
            finally
            {
                e.Cancel = true;
                //await Application.Current.MainPage.Navigation.PopModalAsync();
                await _pageService.BackAsync();
            }
        }

    }
}