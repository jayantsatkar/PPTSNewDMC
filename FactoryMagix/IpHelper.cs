using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FactoryMagix
{
    public static class IpHelper
    {
        public static string GetClientIpAddress(HttpRequestBase request)
        {
            string ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ip))
            {
                ip = request.ServerVariables["REMOTE_ADDR"];
            }

            return ip;
        }
    }

}