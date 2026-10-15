# Restore and build steps
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source

COPY CompanyConnect/*.sln .
COPY CompanyConnect/CompanyConnect.Core/*.csproj ./CompanyConnect.Core/
COPY CompanyConnect/CompanyConnect.Infrastructure/*.csproj ./CompanyConnect.Infrastructure/
COPY CompanyConnect/CompanyConnect.WebApi/*.csproj ./CompanyConnect.WebApi/
COPY CompanyConnect/CompanyConnect.IntegrationTests/*.csproj ./CompanyConnect.IntegrationTests/

RUN dotnet restore

COPY CompanyConnect/. .

RUN dotnet publish -c release --no-restore

# Running the app
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /source/CompanyConnect.WebApi/bin/Release/net10.0/publish/. .
EXPOSE 3000
ENTRYPOINT ["dotnet", "CompanyConnect.WebApi.dll"]