# NETCore-API

# Environment Description
This is a .NET 6.0 Web API. Authentication is hand written using JWT Tokens which are signed with an individual RSA 4096 key pairs and the storage mechanism is a MongoDB database to allow for easy schema changes during development. This project contains the front-end routes for our [SPA interface](https://github.com/Encryption-API-Services/AngularSPA). Once you are registered for the site, you gain access to the API routes for certain encryption methods. If you are not experienced with encryption and want certain things taken care of outside of your environmet we are here for YOU! 

# Environment Setup
If for some reason your installation of Visual Studio didn't install the correct SDK or you are using VSCode, you will need to have the .NET Core [SDK](https://dotnet.microsoft.com/en-us/download/dotnet/3.1) installed.

There are 9 environment variables that need to be define on the machine you are developing on for this project to work properly. 
  - Connection
  - Database
  - UserCollectionName
  - Email (for SMTP)
  - TWILIO_ACCOUNT_SID
  - TWILIO_AUTH_TOKEN
  - IpInfoToken
  - StripApiKey
  - Domain

# Swagger API Documentation
The API documentation for the API endpoints are configured through Swagger and can be found at /swagger/index.html
  - Local Environment [here](https://localhost:44380/swagger/index.html).
  
# Contribution Rules
When submitting taking on an issue make a new branch based off of "main".


# Supporters / Dontations
:coffee: [Buy Me A Coffee](https://www.buymeacoffee.com/mikemulchrs)

This project is maintained using [Apache 2.0 License](https://github.com/Encryption-API-Services/NETCore-API/blob/main/LICENSE).
