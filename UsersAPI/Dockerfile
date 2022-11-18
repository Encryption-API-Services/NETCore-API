#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["UsersAPI/UsersAPI.csproj", "UsersAPI/"]
COPY ["Common/Common.csproj", "Common/"]
COPY ["Libraries/Encryption/Encryption.csproj", "Libraries/Encryption/"]
COPY ["Libraries/DataLayer/DataLayer.csproj", "Libraries/DataLayer/"]
COPY ["Libraries/Models/Models.csproj", "Libraries/Models/"]
COPY ["Libraries/Validation/Validation.csproj", "Libraries/Validation/"]
RUN dotnet restore "UsersAPI/UsersAPI.csproj"
COPY . .
WORKDIR "/src/UsersAPI"
RUN dotnet build "UsersAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UsersAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UsersAPI.dll"]