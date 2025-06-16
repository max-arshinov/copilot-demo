# 2. Hosting Provider Selection

Date: 2025-06-17

## Status

Accepted

## Context

Our URL shortener system requires a reliable, scalable, and cost-effective hosting solution that can meet the following requirements:

- Support for 1 million users creating 100 links each annually (100 million links over 3 years)
- Fast redirect performance (<0.2 seconds latency)
- High availability with fault tolerance (failure of two nodes shouldn't degrade performance >0.1s)
- Support for multiple technologies (.NET 9, ScyllaDB, PostgreSQL, KeyCloak)
- Ability to scale to handle varying traffic patterns
- Support for seamless, zero-downtime deployments
- Cost-effective for both development and production environments

The selected hosting provider must accommodate our C#/.NET 9 application stack, database services (ScyllaDB and PostgreSQL), and identity management services (KeyCloak).

## Decision

We evaluated several cloud hosting providers based on their ability to meet our technical requirements, scalability needs, reliability standards, and cost efficiency.

### Hosting Options Considered

#### Option 1: AWS (Amazon Web Services)
- **Services**: ECS/EKS for containerized applications, RDS for PostgreSQL, ElastiCache for caching, Route 53 for DNS, CloudFront for CDN
- **ScyllaDB Option**: Managed ScyllaDB Cloud service or self-managed on EC2

#### Option 2: Azure
- **Services**: AKS for containers, Azure SQL/PostgreSQL, Redis Cache, Azure DNS, Azure CDN
- **ScyllaDB Option**: Self-managed on Azure VMs

#### Option 3: Google Cloud Platform (GCP)
- **Services**: GKE for containers, Cloud SQL for PostgreSQL, Memorystore for caching, Cloud DNS, Cloud CDN
- **ScyllaDB Option**: Self-managed on GCE VMs

#### Option 4: DigitalOcean
- **Services**: Kubernetes, Managed PostgreSQL, Managed Redis, Load Balancers
- **ScyllaDB Option**: Self-managed on Droplets

### Comparison Table

| Criteria | AWS | Azure | GCP | DigitalOcean |
|----------|-----|-------|-----|--------------|
| **.NET Support** | Strong | Excellent (Microsoft platform) | Good | Good |
| **ScyllaDB Support** | Official partnership with ScyllaDB Cloud | VM-based deployment | VM-based deployment | VM-based deployment |
| **PostgreSQL Support** | RDS (fully managed) | Azure Database for PostgreSQL | Cloud SQL | Managed PostgreSQL |
| **Global Presence** | Excellent (25+ regions) | Excellent (60+ regions) | Good (20+ regions) | Limited (14 regions) |
| **Reliability/SLA** | 99.99% for most services | 99.95-99.99% | 99.95-99.99% | 99.95% |
| **Scalability** | Excellent | Excellent | Excellent | Good |
| **CI/CD Integration** | CodePipeline, CodeBuild | Azure DevOps, GitHub Actions | Cloud Build | Limited native options |
| **Monitoring/Observability** | CloudWatch, X-Ray | Azure Monitor | Cloud Monitoring, Cloud Trace | Basic monitoring |
| **Cost** | Higher, complex pricing | Medium-high | Medium, with good free tier | Lower, simpler pricing |
| **Managed Kubernetes** | EKS ($73/month/cluster + node costs) | AKS (free control plane) | GKE ($73/month/cluster after free tier) | $12/month/node |

### Cost Estimation (Monthly, Production Environment)

| Provider | Compute | Databases | Networking | Other Services | Estimated Total |
|----------|---------|-----------|------------|----------------|----------------|
| AWS | $1,500 | $800 | $200 | $300 | $2,800 |
| Azure | $1,400 | $750 | $180 | $250 | $2,580 |
| GCP | $1,300 | $700 | $180 | $220 | $2,400 |
| DigitalOcean | $900 | $450 | $150 | $100 | $1,600 |

We have decided to use **Azure** as our hosting provider for the following reasons:

1. **Native .NET Integration**: As a C#/.NET 9 application, our system will benefit from Azure's first-class support for Microsoft technologies.
2. **Comprehensive Service Offering**: Azure provides all the necessary services for our architecture components.
3. **Global Presence**: Azure's extensive regional presence supports our latency requirements and potential future global expansion.
4. **Zero-Downtime Deployment Support**: Azure's deployment slots and seamless scaling support our requirement for zero-downtime releases.
5. **Identity Management**: Azure AD integrates well with KeyCloak and provides additional security options.
6. **Managed Database Services**: Azure provides robust managed services for PostgreSQL, reducing operational overhead.
7. **Developer Familiarity**: Our development team has significant experience with Azure.

For ScyllaDB, we will deploy a self-managed cluster on Azure VMs, as there's no managed ScyllaDB service on Azure. We'll implement appropriate monitoring and automated failover mechanisms to ensure reliability.

## Consequences

### Positive
- Excellent integration with our .NET technology stack
- Comprehensive set of services that meet all our requirements
- Strong SLAs that align with our reliability needs
- Good developer experience with familiar tools and documentation
- Scalable infrastructure that can grow with our user base

### Negative
- Higher cost compared to DigitalOcean (though with more comprehensive services)
- Need to self-manage ScyllaDB cluster, which requires additional operational expertise
- Potential vendor lock-in with certain Azure-specific services

### Mitigation Strategies
- We will use containerization and Kubernetes to maintain some level of provider portability
- We will implement Infrastructure as Code using Terraform to document our infrastructure and facilitate potential future migrations
- We will establish a dedicated DevOps team member to manage the ScyllaDB cluster and automate maintenance tasks
