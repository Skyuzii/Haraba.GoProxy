using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Haraba.GoProxy.Extensions
{
    internal static class CookieExtensions
    {
        public static GoCookie ToGoCookie(this Cookie cookie) =>
            new()
            {
                Name = cookie.Name,
                Value = cookie.Value,
                Path = cookie.Path,
                Domain = cookie.Domain,
                Secure = cookie.Secure,
                HTTPOnly = cookie.HttpOnly
            };

        public static List<GoCookie> ToGoCookies(this CookieCollection cookies) =>
            (from Cookie cookie in cookies select cookie.ToGoCookie()).ToList();
    }
}