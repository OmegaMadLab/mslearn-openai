using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Threading.Tasks;

using Azure;

// Add Azure OpenAI package
using Azure.AI.OpenAI;

namespace generate_image
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // Get config settings from AppSettings
                IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();
                string? oaiEndpoint = config["AzureOAIEndpoint"];
                string? oaiKey = config["AzureOAIKey"];
                string? oaiModelName = config["AzureOAIModelName"];

                if(string.IsNullOrEmpty(oaiEndpoint) || string.IsNullOrEmpty(oaiKey) || string.IsNullOrEmpty(oaiModelName))
                {
                    Console.WriteLine("Please check your appsettings.json file for missing or incorrect values.");
                    return;
                }

                // Get prompt for image to be generated
                Console.Clear();
                Console.WriteLine("Enter a prompt to request an image:");
                string prompt = Console.ReadLine();

                OpenAIClient client = new OpenAIClient(new Uri(oaiEndpoint), new AzureKeyCredential(oaiKey));

                ImageGenerationOptions imgOptions = new ImageGenerationOptions()
                {
                    DeploymentName = oaiModelName,
                    Size = "1024x1024",
                    ImageCount = 1,
                    Prompt = prompt,
                    Style = "vivid",
                    Quality = "standard"
                };

                ImageGenerations img = client.GetImageGenerations(imgOptions);
                Console.WriteLine("Response from Dall-e 3");
                Console.WriteLine("Revised prompt:");
                Console.WriteLine(img.Data[0].RevisedPrompt);
                Console.WriteLine("Image url:");
                Console.WriteLine(img.Data[0].Url.ToString());
                
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

}
