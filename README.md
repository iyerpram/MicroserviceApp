# MicroserviceApp

A .NET microservices reference architecture demonstrating service
decomposition, pluggable asynchronous messaging, and database-per-service
design — built to explore practical microservices patterns rather than a
single-service "microservice in name only" demo.

## Overview

Each bounded context is its own ASP.NET Core service. Services do not call
each other over HTTP. They persist to their own store through
`IDbProvider` / `IExtendedDbProvider` and talk through
`IMessagingProvider` / `IMessagingProviderFactory`.

```
Client → Cart / Customers / Orders / Payment / Inventory / Fulfilment / Notification
              ↓
         Event bus (pluggable: Azure Service Bus, AWS SNS, …)
              ↓
         Each service’s own database
```

There is currently no API Gateway — clients call each service directly.

## Architecture

Each service follows the same layers:

| Project | Role |
|---|---|
| **Api** | Controllers, `Program`, `MessageObserver` hosted subscriber |
| **Application** | MediatR handlers, DTOs, repository interfaces, AutoMapper |
| **Domain** | Aggregate models owned by that service |
| **Infrastructure** | DI: wires `IMessagingProvider`, `IDbProvider`, repositories |
| **Abstractions** | Service-specific contracts (reserved for repositories/ports) |

Shared infrastructure lives under `src/common`:

- `IMessagingProvider` — publish / subscribe / read
- `IMessagingProviderFactory` — resolve a broker by `MessagingProviderType` and target app
- `IDbProvider<T>` / `IExtendedDbProvider<T>` — Cosmos and Dynamo implementations

Cross-service contracts live in `MicroserviceApp.Common.Application.Events`
so services stay decoupled from each other’s application models.

### Checkout flow (async)

```
Cart checkout
  → Orders (CreateOrder) publishes OrderCreated
    → Payment processes payment, publishes PaymentProcessed
      → Inventory reserves stock, publishes InventoryReserved
        → Fulfilment creates a shipment, publishes FulfilmentUpdated
Notification consumes NotificationRequested from each of the above.
Orders also consumes PaymentProcessed and FulfilmentUpdated to update order status.
Customers owns customer records and observes OrderCreated.
```

### Services

| Service | Responsibility | Typical datastore wiring |
|---|---|---|
| Cart | Basket items and checkout (publishes to Orders) | DynamoDB |
| Customers | Customer records | Cosmos DB |
| Orders | Order creation and order state | Cosmos DB |
| Payment | Charge an order after `OrderCreated` | Cosmos DB |
| Inventory | Stock levels and reservations after paid orders | DynamoDB |
| Fulfilment | Shipments after successful reservation | Cosmos DB |
| Notification | Persist/send notifications from domain events | Cosmos DB |

## Tech stack

- .NET / C# — ASP.NET Core Web API per service
- MediatR for in-process use cases
- Pluggable event-provider abstraction (Azure Service Bus and AWS SNS stubs)
- Database-per-service via `IDbProvider`

## Project structure

```
MicroserviceApp/
├── src/
│   ├── common/
│   ├── cart/
│   ├── customers/
│   ├── orders/
│   ├── payment/
│   ├── inventory/
│   ├── notification/
│   └── fulfilment/
├── tests/
│   └── MicroserviceApp.Tests/
├── MicroserviceApp.sln
└── README.md
```

## Getting started

### Prerequisites
- .NET SDK
- Docker (if running a message broker locally via container)

### Run locally
```bash
git clone https://github.com/iyerpram/MicroserviceApp.git
cd MicroserviceApp
dotnet restore
dotnet build
```

Run each service individually, for example:
```bash
dotnet run --project src/orders/MicroserviceApp.Orders.Api
dotnet run --project src/customers/MicroserviceApp.Customers.Api
dotnet run --project src/cart/MicroserviceApp.Cart.Api
dotnet run --project src/payment/MicroserviceApp.Payment.Api
dotnet run --project src/inventory/MicroserviceApp.Inventory.Api
dotnet run --project src/notification/MicroserviceApp.Notification.Api
dotnet run --project src/fulfilment/MicroserviceApp.Fulfilment.Api
```

Since there's no API Gateway, call each service's own endpoints directly
during local testing.

## Design decisions worth discussing

- **Why a pluggable event provider instead of coding directly against one
  broker?** Keeps the services decoupled from any single messaging
  technology, avoids vendor lock-in, and makes local development/testing
  easier since a lightweight in-memory implementation can stand in for a
  real broker without infrastructure setup.
- **Why separate bounded contexts into different services?** Each owns
  its own data and its own reasons to change independently.
- **Why database-per-service here, rather than a shared database?** Keeps
  each service's internal data model private. Consistency between services
  is handled through events rather than a database transaction.
- **Why shared events in Common.Application?** Services should not
  reference each other’s Application/Domain projects. They share event
  shapes only.

## Known gaps / honest limitations

This is a reference/learning project, not a production system:

- **No automated tests yet.** The `tests/MicroserviceApp.Tests` project
  exists as scaffolding.
- **No API Gateway.**
- **Broker and database providers are stubs** (`NotImplementedException`
  / placeholder publish-subscribe). The wiring and service boundaries are
  the point of the sample.
- **No explicit failure-handling strategy yet** — retries, circuit
  breakers, or dead-letter queues around the event provider aren't
  implemented.
- **Cross-service consistency** is a choreography of events, not a
  formal Saga/outbox implementation.

## What this demonstrates

- Service decomposition around bounded contexts
- Asynchronous, message-driven communication via `IMessagingProvider`
- Database-per-service through `IDbProvider`
- Consistent vertical slice per service: controller → MediatR handler → repository
