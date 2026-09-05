# MicroserviceApp

A .NET microservices reference architecture demonstrating service
decomposition, inter-service messaging, and independent deployability —
built to show practical, production-oriented microservices patterns rather
than a single-service "microservice in name only" demo.

## Overview

<!-- TODO: Name the actual services in this solution and what each owns,
e.g.:
- OrderService - owns order creation, cancellation, and status
- PaymentService - owns payment processing and refunds
- InventoryService - owns stock levels and reservations
This is the most important missing piece for a reader - without it, a
recruiter/interviewer can't tell what problem the system actually solves. -->

## Architecture

```
Client → API Gateway → [Service A] → Message Broker → [Service B]
                            ↓                              ↓
                       Database A                     Database B
```

<!-- TODO: Replace with your actual topology. Key things worth showing in
this diagram if they apply to your implementation:
- Is there an API Gateway / BFF, or do clients call services directly?
- Does each service own its own database (database-per-service), or is
  there a shared database?
- Synchronous calls (HTTP) vs asynchronous messaging (RabbitMQ/Kafka) -
  which operations use which, and why? -->

### Services

| Service | Responsibility | Communicates via |
|---|---|---|
| <!-- TODO --> | <!-- TODO --> | <!-- TODO: HTTP / RabbitMQ / Kafka --> |
| <!-- TODO --> | <!-- TODO --> | <!-- TODO --> |

### Messaging

This project uses <!-- TODO: RabbitMQ and/or Kafka - specify which, and for
which interactions -->. Asynchronous messaging is used for
<!-- TODO: e.g. "cross-service events like OrderPlaced, so downstream
services (Inventory, Notifications) react without the Order service
blocking on their availability" -->, which decouples services from each
other's uptime and avoids cascading failures from synchronous call chains.

## Tech stack

- .NET / C# — ASP.NET Core Web API per service
- <!-- TODO: RabbitMQ and/or Kafka --> for asynchronous messaging
- <!-- TODO: SQL Server / other persistence, and whether it's
  database-per-service -->
- Docker / Docker Compose for local orchestration
- <!-- TODO: any API gateway, e.g. Ocelot/YARP, if used -->

## Project structure

```
MicroserviceApp/
├── src/
│   ├── <!-- TODO: ServiceA -->/
│   ├── <!-- TODO: ServiceB -->/
│   └── <!-- TODO: Shared/Common libraries, if any -->/
├── docker-compose.yml
└── README.md
```

## Getting started

### Prerequisites
- .NET SDK <!-- TODO: version -->
- Docker & Docker Compose
- <!-- TODO: any other prerequisite -->

### Run locally
```bash
git clone https://github.com/iyerpram/MicroserviceApp.git
cd MicroserviceApp
docker compose up -d       # starts message broker, databases, etc.
dotnet restore
dotnet build
```

Then run each service, either individually:
```bash
dotnet run --project src/<!-- TODO: ServiceA -->
```
or all together via the provided `docker-compose.yml` if it includes the
services themselves, not just their infrastructure dependencies.

### Verifying it's running
<!-- TODO: A quick smoke-test example - e.g. "POST to
http://localhost:5000/orders to create an order, then check
http://localhost:5001/inventory to see the stock level drop after the
message is consumed." This kind of concrete example is what makes a
README demonstrable rather than just descriptive. -->

## Design decisions worth calling out

<!-- TODO: This section is where you turn the README into interview
material. A few prompts to fill in based on what you actually did: -->
- **Why separate services here, rather than a modular monolith?**
  <!-- TODO -->
- **How do services handle failure in a dependency?** (retries, circuit
  breakers, dead-letter queues, etc.) <!-- TODO -->
- **How is data consistency handled across services** (given no
  distributed transactions)? — e.g. eventual consistency via events, the
  Saga pattern, outbox pattern <!-- TODO -->
- **How would this scale under real load?** — which service is the
  bottleneck, and how would you address it? <!-- TODO -->

## What this demonstrates

- Service decomposition and bounded contexts
- Asynchronous, message-driven communication between services
- Independent deployability of each service
- <!-- TODO: add anything else specific to your implementation -->

## Roadmap / possible extensions
- Add distributed tracing (e.g. OpenTelemetry) across service calls
- Add a circuit breaker (e.g. Polly) around inter-service HTTP calls
- Deploy to Azure (Container Apps / AKS) with Application Insights for
  observability — see the [IncidentsAi](https://github.com/iyerpram/IncidentsAi)
  project for a related direction on the ops/observability side
