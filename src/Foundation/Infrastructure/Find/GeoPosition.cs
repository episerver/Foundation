﻿using EPiServer.Find;
using EPiServer.Logging;
using EPiServer.Personalization;
using System.IO;
using System.Text;

namespace Foundation.Infrastructure.Find
{
    public static class GeoPosition
    {
        private static readonly Lazy<IGeolocationProvider> GeoLocationProvider = new Lazy<IGeolocationProvider>(() => ServiceLocator.Current.GetInstance<IGeolocationProvider>());
        private static readonly ILogger _logger = LogManager.GetLogger(typeof(GeoPosition));

        public static GeoLocation ToFindLocation(this IGeolocationResult geoLocationResult)
        {
            return new GeoLocation(geoLocationResult.Location.Latitude, geoLocationResult.Location.Longitude);
        }

        public static GeoCoordinate GetUsersPositionOrNull()
        {
            try
            {
                var requestIp = GetRequestIp();
                var ip = IPAddress.Parse(requestIp);
                var result = GeoLocationProvider.Value.Lookup(ip);
                return result?.Location;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
            }
        }

        public static GeoCoordinate GetUsersPosition()
        {
            var requestIp = GetRequestIp();
            //requestIp = "146.185.31.213";//Temp, provoke error
            var ip = IPAddress.Parse(requestIp);
            IGeolocationResult result;
            try
            {
                result = GeoLocationProvider.Value.Lookup(ip);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                try
                {
                    result = GeoLocationProvider.Value.Lookup(IPAddress.Parse("8.8.8.8"));
                }

                catch (Exception e)
                {
                    _logger.Error(e.Message, e);
                    return null;
                }
            }

            return result != null ? result.Location : null;
        }

        public static IGeolocationResult GetUsersLocation()
        {
            try
            {
                var requestIp = GetRequestIp();

                var ip = IPAddress.Parse(requestIp);
                var result = GeoLocationProvider.Value.Lookup(ip);
                return result ?? GeoLocationProvider.Value.Lookup(IPAddress.Parse("8.8.8.8"));
            }

            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                try
                {
                    return GeoLocationProvider.Value.Lookup(IPAddress.Parse("8.8.8.8"));
                }

                catch (Exception e)
                {
                    _logger.Error(e.Message, e);
                    return null;
                }
            }

        }

        private static string GetRequestIp()
        {
            var accessor = ServiceLocator.Current.GetInstance<IHttpContextAccessor>();
            if (accessor.HttpContext == null)
            {
                return string.Empty;
            }
            var requestIp = accessor.HttpContext.Request.Headers["HTTP_X_FORWARDED_FOR"].ToString();

            if (string.IsNullOrWhiteSpace(requestIp))
            {
                requestIp = accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            if (requestIp.Contains(":"))
            {
                //Port number is included, disregard it
                requestIp = requestIp.Substring(0, requestIp.IndexOf(':'));
            }
            if (!requestIp.Contains(".") || requestIp == "127.0.0.1")
            {
                requestIp = GetLocalRequestIp();
            }
            return requestIp;
        }

        private static string GetLocalRequestIp()
        {
            var requestIp = CacheManager.Get("local_ip") as string;
            if (!string.IsNullOrWhiteSpace(requestIp))
            {
                return requestIp;
            }
            var lookupRequest = WebRequest.Create("http://ipinfo.io/ip/");
            var webResponse = lookupRequest.GetResponse();
            using (var responseStream = webResponse.GetResponseStream())
            {
                var streamReader = new StreamReader(responseStream, Encoding.UTF8);
                requestIp = streamReader.ReadToEnd().Trim();
            }
            webResponse.Close();
            CacheManager.Insert("local_ip", requestIp);
            return requestIp;
        }

    }
}
