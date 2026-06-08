FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/Donnum.DonorService.Presentation.API/Donnum.DonorService.Presentation.API.csproj", "src/Donnum.DonorService.Presentation.API/"]
COPY ["src/Donnum.DonorService.Application/Donnum.DonorService.Application.csproj", "src/Donnum.DonorService.Application/"]
COPY ["src/Donnum.DonorService.Domain/Donnum.DonorService.Domain.csproj", "src/Donnum.DonorService.Domain/"]
COPY ["src/Donnum.DonorService.Infrastructure/Donnum.DonorService.Infrastructure.csproj", "src/Donnum.DonorService.Infrastructure/"]

RUN dotnet restore "src/Donnum.DonorService.Presentation.API/Donnum.DonorService.Presentation.API.csproj"
COPY . .
WORKDIR "/src/src/Donnum.DonorService.Presentation.API"
RUN dotnet build "Donnum.DonorService.Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Donnum.DonorService.Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Donnum.DonorService.Presentation.API.dll"]
