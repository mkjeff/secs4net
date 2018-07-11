FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Devices/WebHost/WebHost.csproj Devices/WebHost/
RUN dotnet restore Devices/WebHost/WebHost.csproj
COPY . .
WORKDIR /src/Devices/WebHost
RUN dotnet build WebHost.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish WebHost.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebHost.dll"]
