using System.IO;
using HannerLabApp.Configuration;
using LiteDB;

namespace HannerLabApp.Services.Repositorys
{
    public class DbContext : IDbContext
    {
        /// <summary>
        /// Returns the LiteDB Connection string used for the app
        /// </summary>
        /// <param name="readOnly">Whether or not this connection may write</param>
        /// <returns></returns>
        public ConnectionString GetConnectionString(bool readOnly)
        {
            var fileFullPath = Path.Combine(Constants.AppDataDirectory, Constants.DatabaseFilename);

            var conString = new ConnectionString()
            {
                Filename = fileFullPath,
                Connection = ConnectionType.Direct,
                ReadOnly = readOnly
            };

            return conString;
        }
    }
}
