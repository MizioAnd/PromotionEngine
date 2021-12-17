# PromotionEngine
A simple promotion engine for a checkout process.\
Prerequisites: .NET 6.0 SDK C# 10 and Docker (installed using Docker below).

Contents:
- [How the start of the project was created](#How-the-start-of-the-project-was-created)
- [Create NuGet pkg for PromotionEngineLibrary](#Create-NuGetpkg-for-PromotionEngineLibrary)
- [Run and publish console app](#Run-and-publish-console-app)
- [Create webapi](#Create-webapi)
- [Deploy project into a Docker container](#Deploy-project-into-a-Docker-container)
- [Output from running PromotionEngineConsoleApp](#Output-from-running-PromotionEngineConsoleApp)

## How the start of the project was created

Create new solution,

$ dotnet new sln

Create a class library project,

$ dotnet new classlib -o PromotionEngineLibrary

Add projects to solution,

$ dotnet sln add PromotionEngineLibrary/PromotionEngineLibrary.csproj

Create unit tests with NUnit,

$ dotnet new nunit -o PromotionEngineLibraryTest

The template "NUnit 3 Test Project" was created successfully.

Add project to solution,

$ dotnet sln add PromotionEngineLibraryTest/PromotionEngineLibraryTest.csproj

Add project reference in unit test project to PromotionEngineLibrary,

$ dotnet add PromotionEngineLibraryTest/PromotionEngineLibraryTest.csproj reference PromotionEngineLibrary/PromotionEngineLibrary.csproj

Build project,

$ dotnet build

Run unit tests,

$ dotnet test PromotionEngineLibraryTest/PromotionEngineLibraryTest.csproj

## Create NuGet pkg for PromotionEngineLibrary

Create NuGet package Promotion.Engine.Library (same name as namespace) assuming you have set up an account on nuget.org with and API key. Go into the PromotionEngineLibrary folder and add to .csproj file inside existing `<PropertyGroup>` tag,

`<PackageId>Promotion.Engine.Library</PackageId>`\
`<Version>1.0.0</Version>`\
`<Authors>your_name</Authors>`\
`<Company>your_company</Company>`

Then create the NuGet by running,

$ dotnet pack

Then upload the .nupkg file created in /bin/Debug/,

$ dotnet nuget push Promotion.Engine.Library.1.0.0.nupkg --api-key your_key --source https://api.nuget.org/v3/index.json

Add new NuGet pkg to your console app .csproj,

$ dotnet add package Promotion.Engine.Library

Which will download the package from https://www.nuget.org/packages/Promotion.Engine.Library/

## Run and publish console app

Run the console app (assuming you have a running PromotionEngineAPI found [here](#Create-webapi)),

$ dotnet run --project PromotionEngineConsoleApp/PromotionEngineConsoleApp.csproj

Publish the console app in order to create a .dll and an executable file that are both framework-dependent (.NET 6.0) but also cross-platform,

$ dotnet publish PromotionEngineConsoleApp/PromotionEngineConsoleApp.csproj --configuration Release

The console app can be run by either using the executable file like this,

$ PromotionEngineConsoleApp/bin/Release/net6.0/publish/PromotionEngineConsoleApp

or by dotnet cmd to run the .dll,

$ dotnet PromotionEngineConsoleApp/bin/Release/net6.0/PromotionEngineConsoleApp.dll

Publish a Linux 64-bit self-contained (no need for .NET runtime installed) platform-dependent executable using -r `<RID>` together with dotnet publish,

$ dotnet publish PromotionEngineConsoleApp/PromotionEngineConsoleApp.csproj --configuration Release -r linux-x64

To run executable,

$ PromotionEngineConsoleApp/bin/Release/net6.0/linux-x64/PromotionEngineConsoleApp

For Windows 64-bit executable,

$ dotnet publish PromotionEngineConsoleApp/PromotionEngineConsoleApp.csproj --configuration Release -r win-x64

To run it,

$ PromotionEngineConsoleApp/bin/Release/net6.0/win-x64/publish/PromotionEngineConsoleApp.exe

## Create webapi

List all .NET templates,

$ dotnet new -l

Create webapi using template,

$ dotnet new webapi -o PromotionEngineAPI

Launch API service by F5 or by first cd into folder PromotionEngineAPI and then,

$ dotnet watch run

will also launch Swagger UI in your browser showing all endpoints at following url,

`http://localhost:<port>/swagger/index.html`

Or just run the API like if it was an app,

$ dotnet run --project PromotionEngine/PromotionEngineAPI/PromotionEngineAPI.csproj &

Test API service by sending http request using curl in terminal and getting an API return 200 code,

$ curl -i http://localhost:`<port>`/api/promotionengineitems

where -k would be included for insecure and no certificate validation during request when using https. Otherwise, test the running service using Swagger UI by clicking button "Try it out".

Scaffold a controller,

$ dotnet aspnet-codegenerator controller -name PromotionEngineItemsController -async -api -m PromotionEngineItem -dc PromotionEngineContext -outDir Controllers

Assuming you have done following steps,

`dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design --prerelease`\
`dotnet add package Microsoft.EntityFrameworkCore.Design --prerelease`\
`dotnet add package Microsoft.EntityFrameworkCore.SqlServer --prerelease`\
`dotnet tool install -g dotnet-aspnet-codegenerator`

Try out the API with in-memory (DI),

$ curl -X POST -H "Content-Type: application/json" http://localhost:`<port>`/api/promotionengineitems -d '{ "TotalPrice": "150", "InputSKU": "A,A,A", "PromotionRules": "none" }' | jq '.'

Check that the new item was added with the POST API request in browser or just with curl cmd,

$ curl -i http://localhost:`<port>`/api/promotionengineitems

There are two GET endpoints one gets all entries in the in-memory database `/api/promotionengineitems` and the other `/api/promotionengineitems/id_of_entry` returns only the entry with value for id_of_entry e.g. `/api/promotionengineitems/1` returns the one with id=1.

## Deploy project into a Docker container

For Docker deploy place yourself one level above the folder of the this cloned project and run,

$ bash PromotionEngine/DockerDeploy/deploy.sh

## Output from running PromotionEngineConsoleApp

![Output from running PromotionEngineConsoleApp](OutputPromotionEngineConsoleApp.png)