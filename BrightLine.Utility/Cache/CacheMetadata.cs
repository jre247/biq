using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Utility
{
    public class CacheMetadata : ICloneable
    {
        public string Key;
        public string Type;
        public double ExpirationMinutes;
        public DateTime Expires;
        public DateTime LastAccessTime;
        public DateTime LastUpdated;
        public object StringVal;
        public int AccessCount;
        public Func<object> Fetcher;

        public bool IsAutoRefreshed()
        {
            return Fetcher != null;
        }


        public object Clone()
        {
            var meta = new CacheMetadata();
            meta.Key = Key;
            meta.Type = Type;
            meta.ExpirationMinutes = ExpirationMinutes;
            meta.Expires = Expires;
            meta.LastUpdated = LastUpdated;
            meta.LastAccessTime = LastAccessTime;
            meta.StringVal = StringVal;
            meta.Fetcher = Fetcher;
            meta.AccessCount = AccessCount;
            return meta;
        }


        public double GetMinutesLeftForExpiration(bool useUtc)
        {
            var now = useUtc ? DateTime.UtcNow : DateTime.Now;
            if (Expires > now)
            {
                var diff = (Expires - now);
                return Math.Round(diff.TotalMinutes);
            }

            return -1;
        }
    }
}
