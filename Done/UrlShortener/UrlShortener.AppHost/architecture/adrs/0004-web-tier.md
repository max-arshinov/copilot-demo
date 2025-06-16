# 4. Web Tier Decision

Date: 2025-06-17

## Status

Accepted

## Context

The URL shortener system requires a web tier that can handle:

- High-volume URL redirection traffic (approximately 11,574 requests per second)
- User authentication and management for 1 million new users per year
- URL creation and management (100 links per user per year)
- Click statistics visualization and reporting
- Consistent performance with redirect latency under 0.2 seconds
- Seamless releases with zero downtime
- Resilience to node failures (failure of any two nodes must not degrade performance by more than 0.1s)

These requirements inform our web tier architecture decisions, particularly in light of our chosen data tier (ScyllaDB).

### Traffic Analysis

| Traffic Type | Volume | Notes |
|--------------|--------|-------|
| URL redirects | ~11,574 requests/second | Primary traffic (1B requests/day) |
| URL creations | ~3.17 requests/second | Low volume, higher complexity |
| Statistics access | ~100 requests/second (estimated) | Dashboard and reporting access |
| User management | ~30 requests/second (estimated) | Authentication, profile management |

## Decision

After evaluating multiple web tier architectures, we've decided to implement a microservices architecture using ASP.NET Core with the following components:

### Web Tier Architecture

1. **Frontend Layer**
   - **User Portal SPA**: React-based single-page application for user management, URL creation, and statistics visualization
   - **Admin Portal SPA**: React-based dashboard for system monitoring and administration

2. **API Gateway Layer**
   - **API Gateway**: YARP (Yet Another Reverse Proxy) for request routing, authentication, and rate limiting

3. **Service Layer**
   - **URL Redirect Service**: Optimized .NET Core service focused solely on URL redirection
   - **URL Management Service**: Service for URL creation, update, and deletion
   - **User Management Service**: Service for user authentication and profile management
   - **Analytics Service**: Service for processing and querying click statistics

4. **Infrastructure**
   - **CDN**: Global content delivery network for static assets and edge caching
   - **Load Balancer**: For distributing traffic across service instances
   - **Auto-scaling Groups**: For dynamically adjusting capacity based on load

### Comparison of Web Tier Architectures

| Architecture | Performance | Scalability | Deployment Complexity | Development Velocity | Cost |
|--------------|-------------|-------------|------------------------|----------------------|------|
| Monolithic ASP.NET Core | Good | Limited | Low | High initially, decreases over time | Low initially, higher at scale |
| Microservices with .NET Core | Excellent | Excellent | Moderate | Moderate initially, increases over time | Moderate initially, optimizable at scale |
| Serverless Functions | Good | Excellent | Low | High | Pay-per-use, can be high with constant load |
| Node.js with Express | Good | Good | Moderate | Moderate | Moderate |

### Technology Stack Selection

| Component | Technology | Justification |
|-----------|------------|---------------|
| API Framework | ASP.NET Core 8+ | High performance, mature ecosystem, excellent integration with cloud services |
| Frontend Framework | React | Widespread adoption, component-based architecture, strong ecosystem |
| API Gateway | YARP | Native .NET integration, high performance, customizable |
| Authentication | JWT with IdentityServer4 | Industry standard, scalable, supports various authentication flows |
| Container Orchestration | Kubernetes | Industry standard, mature ecosystem, excellent scalability |
| Service Mesh | Linkerd | Lightweight, easy to adopt, provides observability and resilience |
| Monitoring | Prometheus + Grafana | Comprehensive metrics collection, visualization, and alerting |
| Logging | OpenTelemetry + Elasticsearch | Distributed tracing, structured logging, searchable |

### Web Service Optimizations

1. **URL Redirect Service Optimizations**:
   - In-memory LRU cache for frequently accessed URLs
   - Asynchronous logging of click statistics
   - Response compression
   - Connection pooling to ScyllaDB
   - Minimal payload size
   - Health checks and circuit breakers

2. **Read/Write Path Separation**:
   - Dedicated high-performance service for URL redirection (read path)
   - Separate services for URL management and analytics (write path)
   - Different scaling policies for read vs. write services

3. **Edge Optimization**:
   - CDN caching for static assets
   - Edge-optimized load balancers
   - Geographic distribution of services

### Deployment Architecture

- Minimum 6 instances of URL Redirect Service across 3 availability zones
- Minimum 2 instances of other services across 2+ availability zones
- Auto-scaling based on CPU utilization and request rate
- Blue/Green deployment for zero-downtime releases

## Consequences

### Advantages

1. **Performance**:
   - Optimized URL Redirect Service ensures latency well below 0.2 seconds
   - Horizontal scaling capability to handle traffic spikes
   - Edge caching reduces latency for global users

2. **Scalability**:
   - Independent scaling of services based on their specific load patterns
   - URL Redirect Service can scale to handle millions of requests per second
   - Stateless design enables simple horizontal scaling

3. **Reliability**:
   - Multiple instances across availability zones ensure resilience
   - Health checks and circuit breakers prevent cascading failures
   - Auto-healing infrastructure recovers from node failures automatically

4. **Maintainability**:
   - Separation of concerns through microservices
   - Independent deployment of services
   - Comprehensive monitoring and observability

### Challenges

1. **Operational Complexity**:
   - Managing a distributed system requires sophisticated DevOps practices
   - Monitoring and debugging across services requires specialized tools
   - Service coordination and communication patterns add complexity

2. **Eventual Consistency**:
   - Statistics updates are asynchronous, leading to eventual consistency
   - Need for careful handling of data consistency across services

3. **Development Overhead**:
   - More complex development environment setup
   - Cross-service testing requirements
   - Need for careful API versioning and backward compatibility

### Mitigation Strategies

1. **DevOps Automation**:
   - Infrastructure as Code (IaC) using Terraform or Pulumi
   - CI/CD pipelines for automated testing and deployment
   - Automated canary deployments and rollback capabilities

2. **Observability Implementation**:
   - Distributed tracing across all services
   - Centralized logging with context correlation
   - Real-time dashboards for system health monitoring

3. **Developer Experience**:
   - Local development environment with Docker Compose
   - Service contracts and API documentation
   - Shared libraries for common functionality

4. **Performance Testing Regime**:
   - Continuous performance testing in the CI pipeline
   - Regular load testing simulating production scenarios
   - Chaos engineering practices to verify fault tolerance
