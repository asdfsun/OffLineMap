using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
namespace NUCTECH.SUBWAY.Common
{
    public class CacheHelper
    {
        private static List<string> _allCacheKey = new List<string>();
        private static ObjectCache _cache = MemoryCache.Default;
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">Key 唯一</param>
        /// <param name="value">值</param>
        /// <param name="cacheOffset">超时时间</param>
        public static void Add(string key, object value, DateTimeOffset cacheOffset)
        {
            if (_allCacheKey.Contains(key))
            {
                Remove(key);
            }
            _allCacheKey.Add(key);
            _cache.Add(key, value, cacheOffset);
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">Key</param>
        public static void Remove(string key)
        {
            if (_allCacheKey.Contains(key))
            {
                _allCacheKey.Remove(key);
            }
            _cache.Remove(key);
        }

        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns></returns>
        public static object Read(string key)
        {
            if (_allCacheKey.Contains(key))
                return _cache[key];
            return null;
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public static void Clear()
        {
            foreach (string value in _allCacheKey)
            {
                _cache.Remove(value);
            }
            _allCacheKey.Clear();
        }
    }
}
