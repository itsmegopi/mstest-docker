FROM mcr.microsoft.com/dotnet/core/sdk:3.1
WORKDIR /app
COPY . .
RUN dotnet build "MSTestWithDocker.csproj" -c Release -o /app
ENTRYPOINT ["dotnet", "test", "--logger:trx"]
