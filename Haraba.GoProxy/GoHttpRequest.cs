using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Haraba.GoProxy.Extensions;
using Newtonsoft.Json;

namespace Haraba.GoProxy
{
    public class GoHttpRequest
    {
        /// <summary>
        /// Ссылка на обработчик запроса
        /// </summary>
        public string GoProxyUrl { get; set; }
        
        /// <summary>
        /// Ссылка на ресурс
        /// </summary>
        public string Url { get; set; }
        
        /// <summary>
        /// Полный отпечаток Ja3
        /// Можно получить свой полный отпечаток на сайте https://ja3er.com/
        /// </summary>
        public string Ja3 { get; set; }
        
        /// <summary>
        /// Тело запроса, если посылается POST запрос
        /// </summary>
        public string Body { get; set; }
        
        /// <summary>
        /// Прокси
        /// IP:PORT
        /// LOGIN:PASS@IP:PORT
        /// </summary>
        public string Proxy { get; set; }
        
        /// <summary>
        /// Метод запроса
        /// </summary>
        public string Method { get; set; }
        
        /// <summary>
        /// Идентификатор клиентского приложения
        /// </summary>
        public string UserAgent { get; set; }
        
        /// <summary>
        /// Таймаут в секундах
        /// </summary>
        public int TimeOut { get; set; }
        
        /// <summary>
        /// Список куков
        /// </summary>
        public List<GoCookie> Cookies { get; set; }
        
        /// <summary>
        /// Список заголовков
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }

        private GoHttpRequest()
        {
        }
        
        /// <summary>
        /// Инициализировать новый класс GoHttpRequest
        /// </summary>
        /// <param name="goProxyUrl">Ссылка на обработчик запроса</param>
        /// <returns></returns>
        public static GoHttpRequest Create(string goProxyUrl)
        {
            return new()
            {
                GoProxyUrl = goProxyUrl,
                TimeOut = 60,
                Cookies = new List<GoCookie>(),
                Headers = new Dictionary<string, string>()
            };
        }
        
        /// <summary>
        /// Получить ответ
        /// </summary>
        /// <param name="url">Ссылка на ресурс</param>
        /// <param name="method">Метод запроса (GET|POST|PUT|HEAD)</param>
        /// <param name="throwIfNotSuccessCode">Выкинуть ошибку, если ответ отрицательный</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<GoHttpResponse> GetResponseAsync(string url, string method = "GET", bool throwIfNotSuccessCode = true)
        {
            try
            {
                var request = (HttpWebRequest) WebRequest.Create(url);
                request.Method = "POST";
                Url = url;
                Method = method;

                var payload = JsonConvert.SerializeObject(this);
                var buffer = Encoding.UTF8.GetBytes(payload);
            
                await using var requestStream = request.GetRequestStream();
                await requestStream.WriteAsync(buffer, 0, buffer.Length);


                using var response = request.GetResponse();
                await using var responseStream = response.GetResponseStream();
                using var responseStreamReader = new StreamReader(requestStream);
                
                var goHttpResponse = JsonConvert.DeserializeObject<GoHttpResponse>(await responseStreamReader.ReadToEndAsync());
                if (throwIfNotSuccessCode)
                    goHttpResponse!.ThrowIfNotSuccessStatusCode();
                
                RefreshCookies(goHttpResponse);
            
                return goHttpResponse;
            }
            catch (Exception ex)
            {
                throw ex.GetBaseException();
            }
        }

        /// <summary>
        /// Получить ответ
        /// </summary>
        /// <param name="url">Ссылка на ресурс</param>
        /// <param name="method">Метод запроса (GET|POST|PUT|HEAD)</param>
        /// <param name="throwIfNotSuccessCode">Выкинуть ошибку, если ответ отрицательный</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public GoHttpResponse GetResponse(string url, string method = "GET", bool throwIfNotSuccessCode = true)
        {
            try
            {
                var request = (HttpWebRequest) WebRequest.Create(GoProxyUrl);
                request.Method = "POST";
                Url = url;
                Method = method;
                
                var payload = JsonConvert.SerializeObject(this);
                var buffer = Encoding.UTF8.GetBytes(payload);
            
                using var requestStream = request.GetRequestStream();
                requestStream.Write(buffer, 0, buffer.Length);

                using var response = request.GetResponse();
                using var responseStream = response.GetResponseStream();
                using var responseStreamReader = new StreamReader(requestStream);
                
                var goHttpResponse = JsonConvert.DeserializeObject<GoHttpResponse>(responseStreamReader.ReadToEnd());
                if (throwIfNotSuccessCode)
                    goHttpResponse!.ThrowIfNotSuccessStatusCode();
                
                RefreshCookies(goHttpResponse);
            
                return goHttpResponse;
            }
            catch (Exception ex)
            {
                throw ex.GetBaseException();
            }
        }

        private void RefreshCookies(GoHttpResponse response)
        {
            if (response.Payload?.Cookies == null) return;

            foreach (var cookie in response.Payload.Cookies)
            {
                var item = Cookies.FirstOrDefault(x => x.Name == cookie.Name);
                if (item != null)
                {
                    item.Value = cookie.Value;
                    continue;                    
                }
                
                Cookies.Add(cookie.ToGoCookie());
            }
        }

        public GoHttpRequest WithHeader(string name, string value)
        {
            Headers.AddOrUpdate(name, value);
            return this;
        }
        
        public GoHttpRequest WithHeaders(Dictionary<string, string> headers)
        {
            foreach (var item in headers)
            {
                Headers.AddOrUpdate(item.Key, item.Value);
            }
            
            return this;
        }

        public GoHttpRequest WithTimeout(int timeout)
        {
            TimeOut = timeout;
            return this;
        }
        
        public GoHttpRequest WithCookie(Cookie cookie)
        {
            Cookies.Add(cookie.ToGoCookie());
            return this;
        }

        public GoHttpRequest WithCookies(CookieCollection cookies)
        {
            Cookies.AddRange(cookies.ToGoCookies());
            return this;
        }

        public GoHttpRequest WithUserAgent(string userAgent)
        {
            UserAgent = userAgent;
            return this;
        }

        public GoHttpRequest WithBody(string body)
        {
            Body = body;
            return this;
        }

        public GoHttpRequest WithProxy(string proxy)
        {
            Proxy = proxy;
            return this;
        }

        public GoHttpRequest WithJa3(string ja3)
        {
            Ja3 = ja3;
            return this;
        }
    }
}