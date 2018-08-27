# Tfl App

This is a .net core 2.1 console application used to retrieve and display the traffic status of a road monitored by Transport for London.

# Getting Started

### System Requirements

Make sure that the dotnet core cli, dotnet core sdk and dotnet core runtime are installed on your pc. See this [link](https://docs.microsoft.com/en-us/dotnet/core/windows-prerequisites?tabs=netcore21#prerequisites-with-visual-studio-2017).

### Download

Download the solution from the  git hub page and place the folder containg the app anywhere on your pc.
The root folder is called 'tflapp-master', go into that folder and you will find another folder called 'tflapp-master' as well. In that folder, you will find two projects folders: 
  - TflApp.Console
  - TflApp.Tests

# How to build and publish the code

Navigate into the project folder TflApp.Console from command prompt. Then run the following command:

dotnet publish -c release -r win-x64 -o \path\to\outputfolder

The command above will publish the exe - TflApp.Console.exe and the accompanying dlls into the output folder you specify in the command above.
So for instance, if you run the above command like this - 

dotnet publish -c release -r win-x64 -o c:\Projects\tflApp. 

The exe will be published to the output folder c:\Projects\tflApp on your computer. 

# How to run the output

### Important Configuration

There is a settings file called AppSettings.json which you will find in the published output folder from the previous step above. The file has the below structure.

{
  "AppSettings": {
    "ApplicationID": "",
    "ApplicationKey": "",
    "BaseApiUrl": ""
  }
}

Your applicationId, application key and BaseApiUrl needs to be applied to these settings. Without theses settings, the solution will not work. 

Below is an example of what the setting should be like.

{
  "AppSettings": {
    "ApplicationID": "9f862777",
    "ApplicationKey": "1234abcd5678efgh4444ijkl",
    "BaseApiUrl": "https://api.tfl.gov.uk/"
  }
}

### Run the solution.

Navigate into the published output folder from windows Powershell and run any of the following example commands.

 .\tflapp.console.exe A13 -should return a valid road status.

  .\tflapp.console.exe A5000 - should return an invalid road message.
  
 ### Error Logs.
 
 If you get the following error message - An unexpected error has occurred - when running the solution, please view the log file called tflapp.log which will be found in the published output folder.
 To test this, you can remove the configuration settings of the AppSettings.json file and try and run the solution. You will receive a custom error message and you can view the log file to see the reason behind the error.

# How to run the tests
Navigate into the project folder TflApp.Tests from command propmpt. Then run the following command:

Dotnet test.

The above command will run all the unit tests in the TflApp.Tests.


# Visual Studio.

Alternatively, the publishing and  and ruuning of unit tests can be done through visual studio. See the links below.

[Publishing with visual studio](https://docs.microsoft.com/en-us/dotnet/core/deploying/deploy-with-vs#self-contained-deployment-with-third-party-dependencies)
