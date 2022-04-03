using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.Api.Files;

namespace HannerLabApp.Services.Cloud.DropBox
{
    public class DropboxFileHandler : ICloudFileHandler
    {

        // The user access token. This belongs to the user who logs into the app, and not the developer token.
        private string _accessToken;
        private bool _isAuthenticated;

        public string AccessToken
        {
            get => _accessToken;
            set
            {
                _accessToken = value;
                _isAuthenticated = true;
            }
        }

        public DropboxFileHandler(String userAccessToken)
        {
            this._accessToken = userAccessToken;
            _isAuthenticated = true;
        }

        public DropboxFileHandler()
        {
            _isAuthenticated = false;
        }

        // Creates a new Dropbox connection using the access token.
        private DropboxClient GetClient()
        {
            // HTTP Request workaround: see: https://github.com/dropbox/dropbox-sdk-dotnet/issues/77#issuecomment-487972169
            return new DropboxClient(this._accessToken,
                new DropboxClientConfig() { HttpClient = new HttpClient(new HttpClientHandler()) });
        }

        /// <summary>
        /// Gets the Dropbox file list
        /// </summary>
        /// <returns>An asynchronous task</returns>
        public async Task<IList<Metadata>> ListFiles()
        {
            try
            {
                using (var client = this.GetClient())
                {
                    var list = await client.Files.ListFolderAsync(string.Empty, true);
                    return list?.Entries;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Reads the file from Dropbox and returns it as byte array.
        /// </summary>
        /// <returns>An asynchronous task</returns>
        public async Task<byte[]> ReadFileAsync(string filePath)
        {
            // Due to bugs in Xamarin Dropbox API. The file must be obtained as a steam, and converted into byte array. The built in method will not work.
            // For more information see: https://github.com/dropbox/dropbox-sdk-dotnet/issues/77#issuecomment-487972169

            if (!_isAuthenticated)
            {
                Console.WriteLine("ERROR: DropBoxFileHandlerService: Not authenticated");
            }

            // Check if the filepath provided has a leading `/`. If not then add one.
            if (!filePath.StartsWith("/"))
            {
                filePath = "/" + filePath;
            }


            try
            {
                using (var client = this.GetClient())
                {
                    using (var response = await client.Files.DownloadAsync(filePath))
                    {
                        using (var stream = await response.GetContentAsStreamAsync())
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                await stream.CopyToAsync(ms);
                                return ms.ToArray();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Writes the file to the Dropbox
        /// </summary>
        /// <returns>An asynchronous task</returns>
        public async Task<bool> WriteFileAsync(byte[] fileContent, string remoteFileName, string remoteFilePath = "/")
        {
            if (!_isAuthenticated)
            {
                Console.WriteLine("ERROR: DropBoxFileHandlerService: Not authenticated");
            }

            // Dropbox API is picky with path strings
            var remotePathFull = "/" + remoteFilePath + "/" + remoteFileName;
            remotePathFull = remotePathFull.Replace(@"//", @"/");

            try
            {
                using (var client = this.GetClient())
                {
                    using (MemoryStream stream = new MemoryStream(fileContent))
                    {
                        var response = await client.Files.UploadAsync(remotePathFull, WriteMode.Overwrite.Instance,
                            body: stream);
                        return response != null;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("DEBUG: " + e);
                return false;
            }
        }

        /// <summary>
        /// Writes the file to the Dropbox
        /// </summary>
        /// <returns>An asynchronous task</returns>
        public async Task<FileMetadata> WriteFile(MemoryStream fileContent, string fileName)
        {
            if (!_isAuthenticated)
            {
                Console.WriteLine("ERROR: DropBoxFileHandlerService: Not authenticated");
            }

            try
            {
                var commitInfo = new CommitInfo(fileName, WriteMode.Overwrite.Instance, false, DateTime.Now);

                using (var client = this.GetClient())
                {
                    var metadata = await client.Files.UploadAsync((UploadArg)commitInfo, fileContent);
                    return metadata;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the account name associated with the access token.
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAccountAsync()
        {
            var account = string.Empty;

            if (!_isAuthenticated)
            {
                Console.WriteLine("ERROR: DropBoxFileHandlerService: Not authenticated");
                return account;
            }

            try
            {
                using (var client = this.GetClient())
                {
                    var fullAccount = await client.Users.GetCurrentAccountAsync();
                    account = fullAccount.Email;
                }
            }
            catch (Exception)
            {
                account = string.Empty;
            }

            return account;

        }
    }
}

