FROM microsoft/aspnetcore:2.0
RUN apk add --no-cache tzdata
WORKDIR /app
COPY . .
ENTRYPOINT ["dotnet", "Lykke.Service.FakeExchangeConnector.dll"]
