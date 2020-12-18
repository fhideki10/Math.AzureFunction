using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Math.AzureFunction
{
    public static class Function1
    {
        [FunctionName("Math")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Operacao data = JsonConvert.DeserializeObject<Operacao>(requestBody);
            string responseMessage = "";
            double resultado = 0;

            switch (data?.Operador)
            {
                case "+":
                    resultado = data.N1 + data.N2;
                    break;

                case "-":
                    resultado = data.N1 - data.N2;
                    break;

                case "/":
                    resultado = data.N1 / data.N2;
                    break;

                case "*":
                    resultado = data.N1 * data.N2;
                    break;

                default:
                    responseMessage = "Invalid Operation";
                    break;
            }

            if (string.IsNullOrEmpty(responseMessage))
            {
                responseMessage = string.IsNullOrEmpty(data?.Operador)
               ? "This HTTP triggered function executed successfully. Pass an operator, n1 and n2 in the request body."
               : $"Result: {resultado}";
            }

            return new OkObjectResult(responseMessage);
        }
    }
}
