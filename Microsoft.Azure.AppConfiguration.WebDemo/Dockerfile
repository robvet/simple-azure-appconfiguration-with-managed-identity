#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Microsoft.Azure.AppConfiguration.WebDemo/Microsoft.Azure.AppConfiguration.WebDemo.csproj", "Microsoft.Azure.AppConfiguration.WebDemo/"]
RUN dotnet restore "Microsoft.Azure.AppConfiguration.WebDemo/Microsoft.Azure.AppConfiguration.WebDemo.csproj"
COPY . .
WORKDIR "/src/Microsoft.Azure.AppConfiguration.WebDemo"
RUN dotnet build "Microsoft.Azure.AppConfiguration.WebDemo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Microsoft.Azure.AppConfiguration.WebDemo.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Microsoft.Azure.AppConfiguration.WebDemo.dll"]