#Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

#Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

#Copy all files and build
COPY . ./
RUN dotnet publish -c Release -o /app/publish

#Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Backend.dll"]