## Description

The application takes a list of categories in JSON format, passes it to the Gemini API (AI API), and outputs a JSON containing the three most popular attributes for every subcategory in the category structure.

## Prerequisites

- .NET Core SDK 8

## Configuration

Before running the application, you need to configure the API key. Follow these steps:

1. **Create the apikey** from this link https://aistudio.google.com/app/apikey. You can select any relevant project to create the API key.

2. **Now in the project folder, locate the `appsettings.json` file** within the `CategoryProcessor` directory.

3. **Open `appsettings.json`** and find the `"GeminiAi"` section.

4. **Add your API key** by replacing the empty string in the `ApiKey` field. The configuration should look like this:

```
 {
   "GeminiAi": {
     "ApiKey": "your-api-key-here",
     "Url": "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent"
   }
 }
```

Replace "your-api-key-here" with your actual API key.

Save the file after making the changes.

## Execution

To restore dependencies and run the application, use the following commands in the command window:

```
dotnet restore
dotnet run --launch-profile https
```

## Usage

1. Once the application runs successfully, in the web browser, go to the URL: https://localhost:7046/.
2. Enter the list of categories in the input field.
3. Click on the submit button.
4. The list of attributes will be populated after a few seconds in the output field.

## Note

An empty list of attributes may be returned for some categories because the Gemini API can become exhausted due to too many requests in a short time.

## Troubleshoot

For troubleshooting, you can view the logs in the command window where you ran the application.