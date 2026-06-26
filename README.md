# Donor Service

Microservicio responsable de la gestión del ciclo de vida de los donantes dentro de la plataforma de donación de sangre.

## SQL Server en Docker

La infraestructura compartida debe iniciarse primero desde `building-blocks` con
`docker compose up -d --wait sqlserver rabbitmq` y luego
`docker compose run --rm sqlserver-init`. Este servicio usa la base `DonnumDonorService` y se conecta
desde Docker a `sqlserver,1433`; `localhost,1433` se reserva para conexiones realizadas
desde la maquina host. Solo el SQL Server compartido publica `1433:1433`; esta API no
expone el puerto de base de datos.

## Propósito

Este servicio centraliza toda la información asociada a un donante y actúa como fuente de verdad para:

* Perfil del donante.
* Grupo sanguíneo y factor RH.
* Ubicación geográfica.
* Historial de donaciones.
* Participación en campañas y urgencias.
* Sistema de gamificación e insignias.
* Índice de confiabilidad.

El servicio forma parte de una arquitectura basada en microservicios y se comunica con otros dominios mediante eventos publicados en RabbitMQ.

---

## Funcionalidades principales

### Gestión de Donantes

Permite crear y actualizar el perfil de un donante, incluyendo información relevante para los procesos de búsqueda y convocatoria.

### Historial de Donaciones

Mantiene el registro de todas las donaciones efectivamente realizadas por un donante.

### Participación en Campañas y Urgencias

Registra las inscripciones realizadas por los donantes a campañas o solicitudes de donación.

### Gamificación

Administra las insignias obtenidas por los donantes como mecanismo de reconocimiento y fidelización.

---

## Arquitectura

El proyecto sigue principios de:

* Clean Architecture
* Domain Driven Design (DDD)
* CQRS
* Event Driven Architecture

Estructura principal:

```text
src/

├── Donor.Api
├── Donor.Application
├── Donor.Domain
├── Donor.Infrastructure
└── Donor.SharedKernel
```

---

## Integraciones

| Servicio             | Propósito                               |
| -------------------- | --------------------------------------- |
| Auth Service         | Autenticación y autorización            |
| Request Service      | Gestión de campañas y urgencias         |
| Notification Service | Envío de notificaciones                 |
| Statistics Service   | Generación de métricas globales         |
| RabbitMQ             | Comunicación asincrónica entre dominios |

---

## Tecnologías

* .NET 10
* ASP.NET Core
* Entity Framework Core
* SQL Server
* RabbitMQ
* Docker
* Swagger/OpenAPI

---

## Modelo de Dominio

Principales entidades:

* Donor
* Donation
* DonationRequestParticipation
* Badge
* DonorBadge

---

## Mensajería y Eventos

El servicio utiliza `Donnum.BuildingBlocks` para la integración asincrónica con RabbitMQ basada en `MessageEnvelope`.

### Configuración del Broker de Mensajería (appsettings.json)

```json
"Messaging": {
  "Broker": {
    "HostName": "localhost",
    "Port": 5672,
    "UserName": "",
    "Password": "",
    "VirtualHost": "/",
    "ExchangeName": "donor.events",
    "ClientProvidedName": "donnum-donor-service"
  }
}
```

### Eventos Publicados

* **Inscripción de Donante**
  * **Topic**: `donor.participation.registered`
  * **Payload**: Contiene información local del registro de participación.

### Eventos Consumidos

* **Donación Física Completada**
  * **Topic**: `donation.physical.completed`
  * **Queue**: `donor-service.donation-completed`
  * **Routing Key**: `donation.physical.completed`

---

## Desarrollo Local

### Requisitos

* .NET 10 SDK
* SQL Server
* Docker (opcional)

### Restaurar dependencias

```bash
dotnet restore
```

### Ejecutar migraciones

```bash
dotnet ef database update
```

### Ejecutar la aplicación

```bash
dotnet run --project src/Donor.Api
```

---
