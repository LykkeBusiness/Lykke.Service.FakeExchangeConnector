FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine3.8
RUN apk add --no-cache tzdata
WORKDIR /app
COPY . .
ENTRYPOINT ["dotnet", "Lykke.Service.FakeExchangeConnector.dll"]
