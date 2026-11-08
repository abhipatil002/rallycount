FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY BadmintonApi/BadmintonApi.csproj BadmintonApi/
RUN dotnet restore BadmintonApi/BadmintonApi.csproj
COPY . .
RUN dotnet publish BadmintonApi/BadmintonApi.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "BadmintonApi.dll"]
