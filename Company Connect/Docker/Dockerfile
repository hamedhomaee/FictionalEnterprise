# Restore and build steps
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source

COPY CompanyConnect/*.sln .
COPY CompanyConnect/*.csproj ./aspnetapp/
RUN dotnet restore

COPY aspnetapp/. ./aspnetapp/
WORKDIR /source/aspnetapp
RUN dotnet publish -c release -o /app --no-restore

# Running the app
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app ./
EXPOSE 3000
ENTRYPOINT ["dotnet", "aspnetapp.dll"]