using System.Collections.Generic;
using System.Net;
using Haraba.GoProxy.Exceptions;

namespace Haraba.GoProxy
{
    public class GoHttpResponse
    {
        /// <summary>
        /// Статус операции
        /// </summary>
        public bool Success { get; set; }
        
        /// <summary>
        /// Текст ошибки
        /// </summary>
        public string Error { get; set; }
        
        /// <summary>
        /// Полезная нагрузка
        /// </summary>
        public GoHttpResponsePayload Payload { get; set; }

        /// <summary>
        /// Выкинуть ошибку, если ответ отрицательный
        /// </summary>
        /// <exception cref="GoHttpException"></exception>
        public void ThrowIfNotSuccessStatusCode()
        {
            if (!Success) throw new GoHttpException($"Success = false -> {Error}");
            if (Payload == null) throw new GoHttpException("Пустой ответ");
            if (Payload.Status < 200 || Payload.Status > 299) throw new GoHttpException($"({Payload.Status}) {Payload.Content}");
        }
    }

    public class GoHttpResponsePayload
    {
        /// <summary>
        /// Статус HTTP запроса
        /// </summary>
        public int Status { get; set; }
        
        /// <summary>
        /// Конечная ссылка
        /// </summary>
        public string Url { get; set; }
        
        /// <summary>
        /// Контент
        /// </summary>
        public string Content { get; set; }
        
        /// <summary>
        /// Список куков с заголовков Set-Cookie
        /// </summary>
        public List<Cookie> Cookies { get; set; }
        
        /// <summary>
        /// Список заголовков
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }
    }
}