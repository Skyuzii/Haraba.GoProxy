# Haraba.GoProxy
Обертка для удобной работы с <a href="https://github.com/Skyuzii/SpoofingTlsFingerprint">прокси сервером Golang</a> для обхода TLS Fingerprint

# Пример Haraba.GoProxy
Вы можете запустить этот тест в <a href="https://github.com/Skyuzii/Haraba.GoProxy/blob/main/Haraba.GoProxy.Tests/MainTests.cs">Haraba.GoProxy.Tests/MainTests.cs</a>
```C#

const string GoProxyUrl = "http://localhost:8000/handle";
const string ChromeUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Safari/537.36";
const string ChromeJa3 = "771,4865-4866-4867-49195-49199-49196-49200-52393-52392-49171-49172-156-157-47-53,0-23-65281-10-11-35-16-5-13-18-51-45-43-27-21,29-23-24,0";

var response = await GoHttpRequest.Create(GoProxyUrl)
                .WithJa3(ChromeJa3)
                .WithHeader("Accept", "*")
                .WithProxy("http://IP:PORT" | "http://LOGIN:PASS@IP:PORT")
                .WithUserAgent(ChromeUserAgent)
                .GetResponseAsync("https://ja3er.com/json");            
```

# Основные классы Haraba.GoProxy
<a href="https://github.com/Skyuzii/Haraba.GoProxy/blob/main/Haraba.GoProxy/GoHttpRequest.cs">GoHttpRequest</a>
```C#
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
/// http://IP:PORT
/// http://LOGIN:PASS@IP:PORT
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
```

<a href="https://github.com/Skyuzii/Haraba.GoProxy/blob/main/Haraba.GoProxy/GoHttpResponse.cs">GoHttpResponse</a>
```C#
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
```

<a href="https://github.com/Skyuzii/Haraba.GoProxy/blob/main/Haraba.GoProxy/GoHttpResponse.cs">GoHttpResponsePayload</a>
```C#
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
```
