## Description

This project takes a list of categories in JSON format and outputs the 3 most famous attributes.

## Prerequisites

- .NET Core SDK 8

## Configuration

Before running the application, you need to configure the API key. Follow these steps:

1. **Create the apikey** from this link https://aistudio.google.com/app/apikey. You can select any relevant project.

1. **Locate the `appsettings.json` file** within the `CategoryProcessor` directory.

2. **Open `appsettings.json`** and find the `"GeminiAi"` section.

3. **Add your API key** by replacing the empty string in the `ApiKey` field. The configuration should look like this:
   
   {
     "GeminiAi": {
       "ApiKey": "your-api-key-here",
       "Url": "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent"
     }
   }
   
Replace "your-api-key-here" with your actual API key.

Save the file after making the changes.

## Execution
To restore dependencies and run the application, use the following commands:

dotnet restore
dotnet run

## Usage

1. Enter the list of categories in the input field.
2. Click on the submit button.
3. The list of attributes will be populated after a few seconds in the output field.

## Note
An empty list of attributes may be returned for some categories because the Gemini API can become exhausted due to too many requests in a short time.








