// Implicit using statements are included
using System;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Azure;
using Azure.Identity;

// Add Azure OpenAI package
using Azure.AI.OpenAI;

// Build a config object and retrieve user settings.
IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
string? oaiEndpoint = config["AzureOAIEndpoint"];
string? oaiModelName = config["AzureOAIModelName"];

// Read sample text file into a string
string textToSummarize = System.IO.File.ReadAllText(@"../text-files/sample-text.txt");

// Generate summary from Azure OpenAI
GetSummaryFromOpenAI(textToSummarize);

void GetSummaryFromOpenAI(string text)
{
    Console.WriteLine("\nSending request for summary to Azure OpenAI endpoint...\n\n");

    if (string.IsNullOrEmpty(oaiEndpoint) || string.IsNullOrEmpty(oaiModelName))
    {
        Console.WriteLine("Please check your appsettings.json file for missing or incorrect values.");
        return;
    }

    // Add code to build request...
    // Initialize the Azure OpenAI client with InteractiveBrowserCredential
    OpenAIClient client = new OpenAIClient(new Uri(oaiEndpoint), new DefaultAzureCredential());

    // Build completion options object
    ChatCompletionsOptions chatCompletionsOptions = new ChatCompletionsOptions()
    {
        Messages =
        {
            new ChatMessage(ChatRole.System, "You are a helpful assistant."),
            new ChatMessage(ChatRole.User, "Summarize the following text in 20 words or less:\n" + text),
        },
        MaxTokens = 120,
        Temperature = 0.7f,
        DeploymentName = oaiModelName
    };

    // Send request to Azure OpenAI model
    ChatCompletions response = client.GetChatCompletions(chatCompletionsOptions);
    string completion = response.Choices[0].Message.Content;

    Console.WriteLine("Summary: " + completion + "\n");

}
