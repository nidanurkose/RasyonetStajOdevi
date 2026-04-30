# .NET 8 çalışma zamanını (runtime) temel alıyoruz
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# SDK kullanarak projeyi derliyoruz
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["RasyonetStaj.csproj", "."]
RUN dotnet restore "./RasyonetStaj.csproj"
COPY . .
RUN dotnet build "RasyonetStaj.csproj" -c Release -o /app/build

# Yayınlama (Publish) aşaması
FROM build AS publish
RUN dotnet publish "RasyonetStaj.csproj" -c Release -o /app/publish

# Son aşama: Uygulamayı çalıştırıyoruz
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RasyonetStaj.dll"]