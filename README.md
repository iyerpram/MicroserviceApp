# MicroserviceApp

A .NET microservices reference architecture demonstrating service
decomposition, pluggable asynchronous messaging, and database-per-service
design — built to explore practical microservices patterns rather than a
single-service "microservice in name only" demo.

## Overview

Two services model a simple e-commerce-style domain:

- **Orders** — owns order creation and order state
- **Customers** — owns customer records

The two communicate asynchronously rather than through direct synchronous
calls, so each service can evolve, fail, and scale independently of the
other.

## Architecture

```
Client → [Orders Service]  →  Event Bus  →  [Customers Service]
              ↓                                      ↓
         Orders DB                             Customers DB
```

There is currently no API Gateway in front of the services — this is a
local/architectural reference project rather than something deployed as a
production system, so clients call each service directly.

### Services

| Service | Responsibility | Communicates via |
|---|---|---|
| Orders | Order creation and order state | Publishes/consumes events via the pluggable event provider |
| Customers | Customer records | Publishes/consumes events via the pluggable event provider |

### Messaging — pluggable by design

Rather than hard-coding a single message broker, this project defines an
event provider abstraction that any concrete broker can implement. The
services depend only on that abstraction, not on RabbitMQ or Kafka
directly — swapping the underlying broker (or adding a new one) means
writing a new implementation of the interface, not changing any service
logic. This is the main architectural decision worth discussing in an
interview: it trades a small amount of up-front abstraction for avoiding
vendor lock-in to a specific broker, and it makes it straightforward to run
the services against a lightweight in-memory provider for local
development or testing without standing up real infrastructure.

RabbitMQ and Kafka are the two concrete providers this project targets.

## Tech stack

- .NET / C# — ASP.NET Core Web API per service
- A pluggable event-provider abstraction, with RabbitMQ and Kafka as
  concrete implementations
- Database-per-service — each service owns its own data store, with no
  shared database between Orders and Customers
- Docker support (`.dockerignore` present; see Setup below)

## Project structure

```
MicroserviceApp/
├── src/
│   ├── Orders/          # Orders service
│   └── Customers/       # Customers service
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

Run each service individually:
```bash
dotnet run --project src/Orders
dotnet run --project src/Customers
```

Since there's no API Gateway, call each service's own endpoints directly
during local testing.

## Design decisions worth discussing

- **Why a pluggable event provider instead of coding directly against one
  broker?** Keeps the services decoupled from any single messaging
  technology, avoids vendor lock-in, and makes local development/testing
  easier since a lightweight in-memory implementation can stand in for a
  real broker without infrastructure setup.
- **Why separate Orders and Customers into different services?** Each owns
  a distinct bounded context with its own data and its own reasons to
  change independently.
- **Why database-per-service here, rather than a shared database?** Keeps
  each service's internal data model private and lets each evolve its
  schema without coordinating with the other — the trade-off is that any
  data consistency between Orders and Customers has to be handled through
  events rather than a database transaction.

## Known gaps / honest limitations

This is a reference/learning project, not a production system, and a few
things are intentionally or currently not yet in place:

- **No automated tests yet.** The `tests/MicroserviceApp.Tests` project
  exists as scaffolding but doesn't currently have meaningful unit or
  integration test coverage — a natural next step before presenting this
  as evidence of testing discipline.
- **No API Gateway.** Clients call each service directly; adding a gateway
  (or an aggregation layer) would be a reasonable extension if this moved
  toward looking like a deployable system.
- **No explicit failure-handling strategy yet** — retries, circuit
  breakers, or dead-letter queues around the event provider aren't
  implemented. Worth having a point of view on this even before
  implementing it, since it's a very likely interview follow-up question
  for any message-driven architecture.
- **Data consistency across services** (given no shared database or
  distributed transactions) isn't yet handled via a specific pattern
  (e.g., Saga, outbox) — currently an open design question rather than an
  implemented solution.

## What this demonstrates

- Service decomposition around bounded contexts (Orders, Customers)
- Asynchronous, message-driven communication decoupled from any single
  broker via a pluggable event-provider abstraction
- Database-per-service design

## Roadmap / possible extensions
- Add real unit and integration test coverage
- Implement a concrete failure-handling strategy (retries, circuit
  breakers, dead-letter queues) around the event provider
- Address cross-service data consistency explicitly (e.g., Saga or outbox
  pattern)
- Add an API Gateway if this moves toward a deployable system
- Deploy to Azure with observability (Application Insights) — see the
  [IncidentsAi](https://github.com/iyerpram/IncidentsAi) project for a
  related direction on the ops/observability side
