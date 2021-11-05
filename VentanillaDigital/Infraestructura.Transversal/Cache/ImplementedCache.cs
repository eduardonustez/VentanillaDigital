using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Infraestructura.Transversal.Cache
{
    public class ImplementedCache
    {
        #region Properties
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;
        private const int _lifeTime = 60;
        #endregion

        #region Builder
        public ImplementedCache(IMemoryCache memoryCache,
            IConfiguration configuration)
        {
            _cache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration)); ;
        }
        #endregion

        #region Implementation
        /// <summary>
        /// Obtiene un valor de cache
        /// </summary>
        /// <param name="cacheName"></param>
        /// <returns></returns>
        public object Get(string cacheName)
        {
            ValidateParameters();

            return _cache.Get(cacheName);
        }

        /// <summary>
        /// Establece el valor de la cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, object value)
        {
            ValidateParameters();

            _cache.Set(key, value, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_lifeTime)
            });
        }

        /// <summary>
        /// Obtiene o establece la cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheName"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public Task<T> GetFromCache<T>(string cacheName, Func<Task<T>> func, int timeLife = _lifeTime)
        {
            ValidateParameters();

            var cacheEntry = _cache.GetOrCreate(cacheName, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(timeLife);
                entry.SetPriority(CacheItemPriority.High);

                return func.Invoke();
            });

            return cacheEntry;
        }

        /// <summary>
        /// Obtiene o establece la cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheName"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public Task<T> CetFromCache<T>(string cacheName, Func<T> func)
        {
            ValidateParameters();

            var cacheEntry = _cache.GetOrCreateAsync(cacheName, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_lifeTime);
                entry.SetPriority(CacheItemPriority.High);

                return Task.FromResult(func.Invoke());
            });

            return cacheEntry;
        }
        #endregion

        #region Privates
        /// <summary>
        /// Valida que los parámetros existan en el config
        /// </summary>
        private void ValidateParameters()
        {
            //if (_configuration.GetSection("CacheDefinition:LifeTime") == null
            //    || _configuration.GetSection("CacheDefinition:LifeTime").Value == null)
            //    throw new NotImplementedException("CacheDefinition:LifeTime Not implemented");

            //_lifeTime = int.Parse(_configuration.GetSection("CacheDefinition:LifeTime").Value);
        }
        #endregion
    }
}
