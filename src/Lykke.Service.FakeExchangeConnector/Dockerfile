FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
COPY . ./
WORKDIR /src/Lykke.Service.FakeExchangeConnector/Lykke.Service.FakeExchangeConnector.csproj
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS final
WORKDIR /app
COPY --from=build /app .
# Required for Time Zone database lookups
RUN apk add --no-cache tzdata
ENTRYPOINT ["dotnet", "Lykke.Service.FakeExchangeConnector.dll"]
