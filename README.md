## HTTPRequest
[![NuGet package](https://buildstats.info/nuget/HTTPRequest)](https://www.nuget.org/packages/HTTPRequest)
 
Simple library to make HTTP requests using JSON

**Dependecies**

* .NET Standard 2.x
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
    MaxRequestAttempts = 3
});
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
