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

O objeto *HttpException* fornece uma propriedade do tipo *HttpResponseErrorModel*, a qual
contém detalhes sobre do erro, que são:

```csharp
//O título principal fornecido pela implementação da API
public string Title { get; set; }

//O código de status HTTP
public int Status { get; set; }

//Detalhes do erro
public string Error { get; set; }

//Um conjunto de chave:valor com erros de validação
//A classe HttpException fornece o método GetParsedMessage()
//que exibe de forma legível os dados do erro
public Dictionary<string, object> ErrorsDictionary { get; set; }

//O texto da exceção fornecido pela implemntação da API
public string Exception { get; set; }

//Mensagem com mais dealhes do erro
public string Message { get; set; }

//Caminho da requisição
public string Path { get; set; }

//Identificador da exceção
public string TraceId { get; set; }
```
