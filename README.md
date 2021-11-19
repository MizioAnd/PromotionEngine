# PromotionEngine
A simple promotion engine for a checkout process

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