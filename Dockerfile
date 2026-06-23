FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["donor-service/src/Donnum.DonorService.Presentation.API/Donnum.DonorService.Presentation.API.csproj", "donor-service/src/Donnum.DonorService.Presentation.API/"]
COPY ["donor-service/src/Donnum.DonorService.Application/Donnum.DonorService.Application.csproj", "donor-service/src/Donnum.DonorService.Application/"]
COPY ["donor-service/src/Donnum.DonorService.Domain/Donnum.DonorService.Domain.csproj", "donor-service/src/Donnum.DonorService.Domain/"]
COPY ["donor-service/src/Donnum.DonorService.Infrastructure/Donnum.DonorService.Infrastructure.csproj", "donor-service/src/Donnum.DonorService.Infrastructure/"]
COPY ["building-blocks/BuildingBlocks.Messaging/Donnum.BuildingBlocks.Messaging.csproj", "building-blocks/BuildingBlocks.Messaging/"]
COPY ["building-blocks/BuildingBlocks.Messaging.RabbitMQ/Donnum.BuildingBlocks.Messaging.RabbitMQ.csproj", "building-blocks/BuildingBlocks.Messaging.RabbitMQ/"]
COPY ["building-blocks/Directory.Build.props", "building-blocks/"]

RUN dotnet restore "donor-service/src/Donnum.DonorService.Presentation.API/Donnum.DonorService.Presentation.API.csproj"
COPY . .
WORKDIR "/src/donor-service/src/Donnum.DonorService.Presentation.API"
RUN dotnet build "Donnum.DonorService.Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Donnum.DonorService.Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Donnum.DonorService.Presentation.API.dll"]
