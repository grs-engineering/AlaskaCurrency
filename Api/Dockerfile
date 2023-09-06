FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Currency/Currency.csproj", "Currency/"]
RUN dotnet restore "Currency/Currency.csproj"
COPY . .
WORKDIR "/src/Currency"
RUN dotnet build "Currency.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Currency.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Currency.dll"]
