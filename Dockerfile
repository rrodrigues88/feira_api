# Estágio 1: Imagem base para runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Estágio 2: Build da aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia apenas os arquivos de projeto para restaurar dependências (melhora cache)
COPY ["API/API.csproj", "API/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Core/Core.csproj", "Core/"]
RUN dotnet restore "API/API.csproj"

# Copia todo o restante do código (incluindo os .cs atualizados)
COPY . .

# Compila o projeto
RUN dotnet build "API/API.csproj" -c Release -o /app/build

# Estágio 3: Publicação
FROM build AS publish
RUN dotnet publish "API/API.csproj" -c Release -o /app/publish

# Estágio 4: Runtime final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "API.dll"]
