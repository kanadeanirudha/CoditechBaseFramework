using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Coditech.DataAccessLayer.Extensions
{
    public static class EntityExtension
    {
        #region Public
        public static TEntity FindExtension<TId, TEntity>(this DbSet<TEntity> source, TId id) where TEntity : class where TId : struct => source.Find(id);

        public static async Task<TEntity> FindExtensionAsync<TId, TEntity>(this DbSet<TEntity> source, TId id) where TEntity : class where TId : struct => await source.FindAsync(id);

        //Get time zone by Id.
        private static TimeZoneInfo GetTimeZoneInformation(string timeZone)
          => TimeZoneInfo.FindSystemTimeZoneById(string.IsNullOrEmpty(timeZone) ? "Central Standard Time" : timeZone);
        #endregion
    }
}
