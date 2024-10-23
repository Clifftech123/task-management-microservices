
# RealWorld Project Management System (Microservices Architecture)

This repository contains a real-world project management system built with ASP.NET Core microservices architecture. The system allows users to manage projects and tasks with advanced features like authentication, authorization, and filtering by tags. The project demonstrates how to design, implement, and integrate microservices using cutting-edge tools and technologies.

## Key Features
- **Microservices Architecture**: Each service handles a distinct domain—UserService, TaskService, and ProjectService.
- **YARP API Gateway**: Central gateway for routing and load balancing, securing microservices with JWT tokens.
- **gRPC Communication**: Internal service-to-service communication using gRPC for high performance.
- **Authentication & Authorization**: User management and token issuance using Microsoft.AspNetCore.Identity and JWT authentication.
- **MassTransit & RabbitMQ**: Event-driven communication and message queuing between services.
- **Serilog & Seq**: Centralized logging and structured log management.
- **dotnet aspire**: Streamlined deployment and orchestration of services.

## Technologies
- **ASP.NET Core** (Microservices, gRPC, Identity)
- **MassTransit** (for RabbitMQ integration)
- **YARP** (API Gateway)
- **gRPC** (for internal communication)
- **JWT** (Authentication and Authorization)
- **dotnet aspire** (Containerization and orchestration)
- **Serilog & Seq** (Logging)

## Learning Objectives
This project is designed to help developers learn about:
- Designing and building microservices with ASP.NET Core.
- Integrating RabbitMQ for event-driven communication.
- Using gRPC for efficient service-to-service communication.
- Implementing security across microservices with JWT and YARP.
- Deploying and orchestrating microservices with dotnet aspire.
