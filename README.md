# NETCore-API

# Environment Description
This is a .NET Core 3.1 Web API that has issues already included to be upgraded as a learning exercise and potentially make it's way to a long term production environment for use by other developers. If you are not a developer, this is probably not the project you are looking for. Authentication is hand written using JWT Tokens signed with a RSA 4096 key pair and the storage mechanism is a MongoDB database to allow for easy schema changes during development.

# Environment Setup
If for some reason your installation of Visual Studio didn't install the correct SDK or you are using VSCode, you will need to have the .NET Core [SDK](https://dotnet.microsoft.com/en-us/download/dotnet/3.1) installed.

There are 4 environment variables that need to be define on the machine you are developing on for this project to work properly. 
  - Connection
  - Database
  - UserCollectionName
  - Email (for SMTP)

# Swagger API Documentation
The API documentation for the API endpoints are configured through Swagger and can be found at /swagger/index.html
  - Local Environment [here](https://localhost:44380/swagger/index.html).
  
# Contribution Rules
There is a [main](https://github.com/Encryption-API-Services/NETCore-API) branch and a [development](https://github.com/Encryption-API-Services/NETCore-API/tree/development) branch. When submitting taking on an issue make a new branch based off of "development" i.e. (#35-make-development-branch).

This project is maintained using [Apache 2.0 License](https://github.com/Encryption-API-Services/NETCore-API/blob/main/LICENSE).
