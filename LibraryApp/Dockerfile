FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["LibraryApp.csproj", "."]
RUN dotnet restore "./LibraryApp.csproj"

COPY . .
WORKDIR "/src/."

RUN dotnet publish "LibraryApp.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

RUN ls -la && pwd

ENTRYPOINT ["dotnet", "./bin/Debug/net8.0/LibraryApp.dll"]