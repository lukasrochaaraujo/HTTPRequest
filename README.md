## HTTPRequest
Biblioteca para realizar requisições via HTTP
- Suporte inicial somente para Basic Auth

**Dependências**

* .NET Standard 2.0.3
* Newtonsoft.Json 12.0.3

**Inicialização**

```csharp
var httpRequest = new HttpRequest();
httpRequest.AppendHeader(key, value); (optional)
httpRequest.ChangeISODateTimeFormat("newFormat"); (optional)
```

**Requisições**

A biblioteca dispõe de 4 métodos genéricos que, representam 
os principais verbos de comunicação via HTTP, que são:

```csharp
async Task<T> GETAsync<T>(string url);

async Task<T> POSTAsync<T>(string url);

async Task<T> PUTAsync<T>(string url);

async Task DELETEAsync<T>(string url);
```

Tais métodos já realizam a conversão de JSON para o objeto indicado
entre as tags < e >.

**Exceções**

As requisições estão programadas para lançar uma exceção do tipo *HttpException* em caso de 
status HTTP maior que 300.

O objeto *HttpException* fornece uma propriedade do tipo *HttpResponseDynamic*, a qual
é um objecto do tipo *dynamic* construído a partir do json da resposta.
