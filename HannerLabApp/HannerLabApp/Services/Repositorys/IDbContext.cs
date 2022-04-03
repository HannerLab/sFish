using LiteDB;

namespace HannerLabApp.Services.Repositorys
{
    public interface IDbContext
    {
        ConnectionString GetConnectionString(bool readOnly);
    }
}
