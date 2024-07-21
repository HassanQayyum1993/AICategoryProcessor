Description
A brief description of what the project does, its purpose, and any key features.

Prerequisites
.NET Core SDK 8
Installation
Clone the repository:

sh
git clone <repository-url>
Navigate to the project directory:

sh
cd <project-directory>
Restore dependencies:

sh
dotnet restore
Run the application:

sh
dotnet run
Configuration
Before running the application, you need to configure the API key. Follow these steps:

Locate the appsettings.json file within the CategoryProcessor directory:

sh
cd CategoryProcessor
Open appsettings.json and find the "GeminiAi" section.

Add your API key by replacing the empty string in the ApiKey field. The configuration should look like this:

json
{
  "GeminiAi": {
    "ApiKey": "your-api-key-here",
    "Url": "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent"
  }
}
Replace "your-api-key-here" with your actual API key.

Save the file after making the changes.

Usage
Describe how to use the application, including any command-line arguments, environment variables, or user actions required.