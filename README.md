## HTTPRequest
Biblioteca para realizar requisições via HTTP

**Dependências**

* .NET Standard 2.0.3
* Newtonsoft.Json 12.0.3

**Utilização**

```csharp
var httpRequest = new HttpRequest();
await httpRequest.GETAsync<List<Employer>>("https://api.server.com/employer");
```

**Métodos disponíveis**
```csharp
//Envia uma solicitação GET
async Task<T> GETAsync<T>(string url);

//Envia uma solicitação POST
async Task<T> POSTAsync<T>(string url);

//Envia uma solicitação PUT
async Task<T> PUTAsync<T>(string url);

//Envia uma solicitação DELETE
async Task DELETEAsync<T>(string url);

//Adiciona dados ao header
void AddHeader(string key, string value);

//Remove dados do header
void RemoveHeader(string key);

//Altera o farmato de conversão de DATA e HORA 
void ChangeISODateTimeFormat(string newFormat);

//Reinicia a instância HttpClient e descarta os headers adiconados
void ResetHTTPClientAndDiscardHeaders();

//Reinicia a instância HttpClient e mantém os headers adicionados
void ResetHTTPClientAndKeepHeaders();
```

**Exceções**

As requisições estão programadas para lançar uma exceção do tipo *HttpException* em caso de 
status HTTP maior que 300.

O objeto *HttpException* fornece uma propriedade do tipo *HttpResponseDynamic*, a qual
é um objecto do tipo *dynamic* construído a partir do json da resposta.
