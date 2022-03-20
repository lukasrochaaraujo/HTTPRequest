## HTTPRequest
[![NuGet package](https://buildstats.info/nuget/HTTPRequest)](https://www.nuget.org/packages/HTTPRequest)
[![build](https://github.com/lukasrochaaraujo/HTTPRequest/actions/workflows/dotnet.yml/badge.svg)](https://github.com/lukasrochaaraujo/HTTPRequest/actions/workflows/dotnet.yml)
 
Simple library to make HTTP requests using JSON

**Dependecies**

* .NET Standard 2.x
* Microsoft.Extensions.DependencyInjection.Abstractions 6.x
* Newtonsoft.Json 13.x

**Example with bearer auth**

```csharp
var httpRequest = new HttpRequest();
httpRequest.AppendHeader("Authorization", "Bearer token")
await httpRequest.GETAsync<IEnumerable<Employer>>("https://api.server.com/employer");
```

**HttpRequest options**
```csharp
new HttpRequest(new HttpRequestOptions()
{
    DateTimeFormat = "iso_datetime_format",
    TimeOutInSeconds = 30,
    MaxRequestAttempts = 3,
    IntevalBetweenAttemptsInSeconds = 5
});
```

**HttpRequest Dependency Injection**
```csharp
(...)
services.AddHttpRequest(); //simple with default options values
//OR
services.AddHttpRequest(provider => //with custom options values
{
    return new HttpRequest(new HttpRequestOptions
    {
        TimeOutInSeconds = 120,
        MaxRequestAttempts = 5
        (...)
    });
});
(...)
//Constructor DI
(...)
private readonly IHttpRequest _httpRequest;

public MyClass(IHttpRequest httpRequest)
{
    _httpRequest = httpRequest;
}
(...)
```

**Methods**
```csharp
void AppendHeader(string key, string value);

void RemoveHeader(string key);

void ClearHeaders();

async Task<T> GETAsync<T>(string url);

async Task<T> POSTAsync<T>(string url, string jsonData);

async Task<T> PUTAsync<T>(string url);

async Task<T> PUTAsync<T>(string url, string jsonData);

async Task DELETEAsync<T>(string url);
```

