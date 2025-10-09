FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY GestaoPessoas.sln ./
COPY GestaoPessoas/GestaoPessoas.csproj GestaoPessoas/
RUN dotnet restore GestaoPessoas/GestaoPessoas.csproj

COPY . .
RUN dotnet publish GestaoPessoas/GestaoPessoas.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app .

ENV ASPNETCORE_URLS=http://+:${PORT:-8080}

EXPOSE 8080

ENTRYPOINT ["dotnet", "GestaoPessoas.dll"]