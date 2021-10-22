FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine
RUN apk add --no-cache tzdata
WORKDIR /app
COPY . .
ENTRYPOINT ["dotnet", "Lykke.Service.FakeExchangeConnector.dll"]
