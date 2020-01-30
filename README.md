## HTTPRequest
Biblioteca para realizar requisições via HTTP
- Suporte inicial somente para Basic Auth

**Dependências**

* .NET Standard 2.0.3
* Newtonsoft.Json 12.0.3

**Inicialização**

```csharp
var config = new HttpRequestConfig();
config.AddBasicAuthorization('foo', 'bar');
config.AddApplicationInfo('myappname', 'myappversioin');

var httpRequest = new HttpRequest(config);
```

**Requisições**

A biblioteca dispõe de 4 métodos estáticos e genéricos que, representam 
os principais verbos de comunicação via HTTP, que são:

```csharp
async Task<T> HttpRequest.GETAsync<T>(string url);

async Task<T> HttpRequest.POSTAsync<T>(string url);

async Task<T> HttpRequest.PUTAsync<T>(string url);

async Task HttpRequest.DELETEAsync<T>(string url);
```

Tais métodos já realizam a conversão de JSON para o objeto indicado
entre as tags < e >.

**Exceções**

As requisições estão programadas para lançar uma exceção do tipo *HttpException* em caso de 
status HTTP maior que 300.

O objeto *HttpException* fornece uma propriedade do tipo *HttpResponseDynamic*, a qual
é um objecto do tipo *dynamic* construído a partir do json da resposta.
