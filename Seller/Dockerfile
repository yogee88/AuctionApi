#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Seller/Seller.csproj", "Seller/"]
COPY ["EventBus.Messages/EventBus.Messages.csproj", "EventBus.Messages/"]
RUN dotnet restore "Seller/Seller.csproj"
COPY . .
WORKDIR "/src/Seller"
RUN dotnet build "Seller.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Seller.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Seller.dll"]