## HTTPRequest
Simple library to make HTTP requests

**Dependências**

* .NET Standard 2.x
* Newtonsoft.Json 13.x

**Example with basic auth**

```csharp
//basic auth
var config = new HttpRequestConfig();
config.AddBasicAuthorization("username", "password");
var httpRequest = new HttpRequest(config);
await httpRequest.GETAsync<IEnumerable<Employer>>("https://api.server.com/employer");
```

**Methods**
```csharp
void AppendHeader(string key, string value);

void RemoveHeader(string key);

void ClearHeader();

void ChangeTimeoutFromSeconds(60);

async Task<T> GETAsync<T>(string url);

async Task<T> POSTAsync<T>(string url, string jsonData);

async Task<T> POSTAsync<T>(string url, HttpContent httpContent);

async Task<T> PUTAsync<T>(string url);

async Task<T> PUTAsync<T>(string url, string jsonData);

async Task DELETEAsync<T>(string url);
```
