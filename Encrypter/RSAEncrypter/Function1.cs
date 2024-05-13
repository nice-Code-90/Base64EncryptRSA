using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace FunctionApp1
{
    public static class Function1
    {
        [Function("Function1")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req)
        {
            string base64EncodedText = req.Query["base64EncodedText"];

            if (string.IsNullOrEmpty(base64EncodedText))
            {
                return new BadRequestObjectResult("Please provide a base64 encoded text in the query string.");
            }

            try
            {
                byte[] decodedBytes = Convert.FromBase64String(base64EncodedText);
                string decodedText = Encoding.UTF8.GetString(decodedBytes);

                using (RSA rsa = RSA.Create())
                {
                    byte[] encryptedBytes = rsa.Encrypt(Encoding.UTF8.GetBytes(decodedText), RSAEncryptionPadding.Pkcs1);
                    string encryptedText = Convert.ToBase64String(encryptedBytes);

                    StringBuilder xmlBuilder = new StringBuilder();

                    xmlBuilder.AppendLine(rsa.ToXmlString(true));




                    return new OkObjectResult($"Encrypted Text: {encryptedText}\n\nRSA Key:\n{xmlBuilder.ToString()}");
                }

            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error: {ex.Message}");
            }
        }
    }
}