FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ImagesApp/ImagesApp.csproj", "ImagesApp/"]
RUN dotnet restore "ImagesApp/ImagesApp.csproj"
COPY . .
WORKDIR "/src/ImagesApp"
RUN dotnet build "ImagesApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ImagesApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ImagesApp.dll"]
