# Construindo a aplicação
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copiando os arquivos de projeto e restaurando as dependências
COPY ./*.sln ./
COPY ./TechChallengeAuth/*.csproj ./TechChallengeAuth/
COPY ./Application/*.csproj ./Application/
COPY ./Domain/*.csproj ./Domain/
COPY ./Infra/*.csproj ./Infra/
COPY ./TechChallengeAuth.Tests/*.csproj ./TechChallengeAuth.Tests/
RUN dotnet restore

# Copiando o código-fonte e compilando a aplicação
COPY . ./
RUN dotnet publish -c Release -o out

# Executando a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .

# Expondo a porta da aplicação
EXPOSE 80

# Iniciar a aplicação quando o contêiner for iniciado
ENTRYPOINT ["dotnet", "TechChallengeAuth.dll"]