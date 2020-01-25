using System.Text;

using HTTPRequest.Model;

namespace HTTPRequest.Exception
{
    public class HttpException : System.Exception
    {
        public HttpResponseErrorModel HttpResponse;

        public HttpException(HttpResponseErrorModel response, string message) : base(message)
        {
            HttpResponse = response;
        }

        public HttpException(HttpResponseErrorModel response, string message, System.Exception innerException) : base(message, innerException)
        {
            HttpResponse = response;
        }

        public string GetParsedMessage()
        {
            StringBuilder message = new StringBuilder();

            message.AppendLine($"Status HTTP: {HttpResponse.Status}\n");

            if (!string.IsNullOrWhiteSpace(HttpResponse.TraceId))
                message.AppendLine($"Trace ID: {HttpResponse.TraceId}\n");

            if (!string.IsNullOrWhiteSpace(HttpResponse.Title))
                message.AppendLine($"Título: {HttpResponse.Title}\n");

            if (!string.IsNullOrWhiteSpace(HttpResponse.Message))
                message.AppendLine($"Mensagem: {HttpResponse.Message}\n");

            if (!string.IsNullOrWhiteSpace(HttpResponse.Path))
                message.AppendLine($"Caminho: {HttpResponse.Path}\n");

            if (!string.IsNullOrWhiteSpace(HttpResponse.Error))
                message.AppendLine($"Erro: {HttpResponse.Error}\n");

            if (HttpResponse.ErrorsDictionary != null && HttpResponse.ErrorsDictionary.Count > 0)
            {
                message.AppendLine($"Erros:\n");

                foreach (var erro in HttpResponse.ErrorsDictionary)
                    message.AppendLine($"{erro.Key} => '{erro.Value.ToString()}'");

                message.AppendLine();
            }

            return message.ToString();
        }
    }
}
