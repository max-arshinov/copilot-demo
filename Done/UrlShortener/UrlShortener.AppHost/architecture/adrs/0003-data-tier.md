# 3. Data Tier Decision

Date: 2025-06-17

## Status

Accepted

## Context

The URL shortener system requires a data tier that can handle:

- Storage of 100 million shortened URLs annually (1M users × 100 links per user)
- Retention of URLs for 3 years (total of 300M URLs at capacity)
- Fast retrieval for redirects (< 0.2s latency requirement)
- Tracking of click statistics (10 clicks/day/link on average)
- High availability with fault tolerance (failure of 2 nodes must not degrade performance by more than 0.1s)
- Automated cleanup of expired links after 3 years

### Workload Calculations

| Metric | Calculation | Result |
|--------|-------------|--------|
| Annual URL creation | 1M users × 100 URLs/user | 100M URLs/year |
| Total URLs after 3 years | 100M URLs/year × 3 years | 300M URLs |
| Daily URL lookups | 100M URLs × 10 clicks/day | 1B lookups/day |
| Lookups per second | 1B lookups/day ÷ 86,400 seconds | ~11,574 lookups/second |
| Write operations per second | 100M URLs/year ÷ (365 days × 86,400 seconds) | ~3.17 writes/second |
| Click statistics writes per second | 1B clicks/day ÷ 86,400 seconds | ~11,574 stats writes/second |

### Storage Requirements

| Data Point | Size | Total Size |
|------------|------|------------|
| URL record (ID, original URL, short code, user ID, created date, expiry date) | ~300 bytes | 300M URLs × 300 bytes = ~90GB |
| Click statistics (timestamp, URL ID, referrer, user agent summary) | ~100 bytes | 1B clicks/day × 100 bytes = ~100GB/day |
| Click statistics aggregated daily | ~20 bytes | 300M URLs × 20 bytes = ~6GB |

## Decision

After evaluating multiple data storage options, we've decided to use **ScyllaDB** as our primary database solution for the URL shortener system.

### Database Solution: ScyllaDB

ScyllaDB is a highly performant, distributed NoSQL database that is compatible with Apache Cassandra but offers significantly better performance and lower latency. We've selected ScyllaDB as our single database solution for the following key components:

- URL mappings and metadata storage
- Click statistics and analytics
- User account information
- Automated data expiry through TTL mechanisms

### Comparison of Database Options

| Feature | ScyllaDB | PostgreSQL + Redis + TimescaleDB | MongoDB | DynamoDB | Cassandra |
|---------|----------|----------------------------------|---------|----------|-----------|
| Query performance | Excellent | Good (requires coordination) | Good | Excellent | Good |
| Scalability | Excellent | Good (complex) | Excellent | Excellent | Excellent |
| High throughput | Excellent | Good | Good | Good | Good |
| Consistency | Tunable | Strong (PostgreSQL), Eventual (Redis) | Configurable | Configurable | Tunable |
| Low latency | Excellent (<1ms) | Good (depends on cache hit ratio) | Good | Good | Good |
| Schema flexibility | Good | Limited | Excellent | Good | Good |
| Time-series support | Good (with wide rows) | Excellent (with TimescaleDB) | Limited | Limited | Good |
| TTL support | Native | Complex (requires jobs) | Limited | Yes | Yes |
| Operational complexity | Moderate | High (multiple systems) | Moderate | Low | Moderate |
| Multi-datacenter replication | Built-in | Complex | Available | AWS-specific | Built-in |
| Cost | Moderate | High (multiple systems) | Moderate | High | Moderate |
| Failure resilience | Excellent | Good (complex coordination) | Good | Excellent | Good |

### ScyllaDB Advantages for URL Shortener Requirements

| Requirement | How ScyllaDB Addresses It |
|-------------|---------------------------|
| High read throughput (~11,574 reads/sec) | ScyllaDB's shard-per-core architecture optimizes for parallel processing |
| Low latency (<0.2s requirement) | ScyllaDB consistently delivers p99 latencies under 10ms |
| High write throughput (~11,574 stats writes/sec) | Log-structured merge trees (LSM) optimize for write-heavy workloads |
| URL expiration after 3 years | Native TTL support at both row and column level |
| Click statistics tracking | Wide row model allows efficient time-series data storage |
| Fault tolerance (2-node failure resilience) | Configurable replication factor (RF=3) ensures data availability even with 2 node failures |

### Data Model

```
urls TABLE:
- short_code (partition key)
- original_url
- user_id
- created_at
- expires_at (with TTL)
- is_active

url_clicks_by_day TABLE:
- short_code (partition key)
- day (clustering key)
- hour (clustering key)
- click_count
- unique_visitors

url_clicks_detail TABLE:
- short_code (partition key)
- timestamp (clustering key)
- referrer
- user_agent
- ip_region
- device_type
- ttl (set to 90 days for detailed analytics)

users TABLE:
- user_id (partition key)
- email
- name
- created_at
```

### Data Tier Architecture

1. **Read Path**:
   - URL lookups use the short_code as the partition key for direct hash-based lookups
   - ScyllaDB's shard-per-core architecture enables consistent sub-millisecond reads
   - Read path is optimized with custom read consistency levels (LOCAL_ONE for ultra-fast lookups)

2. **Write Path**:
   - URL creation uses prepared statements for optimal performance
   - Batch writes for click statistics to optimize write throughput
   - Consistency level of LOCAL_QUORUM ensures durability without excessive latency

3. **Analytics Path**:
   - Two-tier approach for analytics:
     - Detailed click data stored with TTL (90 days) in url_clicks_detail
     - Aggregated daily statistics stored long-term in url_clicks_by_day
   - Counters for efficient incrementing of click statistics

4. **Cleanup Process**:
   - Automatic TTL-based expiration of URLs after 3 years
   - Automatic cleanup of detailed click statistics after 90 days
   - Background compaction optimizes storage and cleanup operations

### Deployment Architecture

- Minimum 6-node ScyllaDB cluster across 3 availability zones (2 nodes per AZ)
- Replication Factor of 3 (data replicated to all 3 AZs)
- Consistency level of LOCAL_QUORUM for writes (ensures durability in one AZ)
- Consistency level of LOCAL_ONE for reads (ensures fastest possible lookups)

## Consequences

### Advantages

1. **Performance**:
   - Consistent sub-millisecond latency for URL lookups
   - Linear scalability for both reads and writes
   - No need for separate caching layer - ScyllaDB's performance eliminates this requirement

2. **Scalability**:
   - Horizontal scaling by adding nodes
   - Vertical scaling through ScyllaDB's efficient use of server resources
   - Automatic data rebalancing when nodes are added or removed

3. **Reliability**:
   - Configurable replication factor ensures data durability
   - Multi-AZ deployment protects against zone failures
   - Tunable consistency levels allow balancing between performance and consistency

4. **Operational Simplicity**:
   - Single database system to maintain vs. multiple systems
   - Built-in monitoring with Prometheus integration
   - Compatible with Cassandra tools and drivers
   - Automatic data management through TTL

### Challenges

1. **Learning Curve**:
   - Team may need to develop expertise in NoSQL data modeling
   - Different query patterns compared to traditional SQL
   - Understanding consistency levels and their implications

2. **Limited Transaction Support**:
   - Lightweight transactions only (conditional updates)
   - Multi-row/multi-table transactions not available
   - May require application-level transaction handling

3. **Query Flexibility**:
   - Data access patterns must be known in advance
   - Secondary indexes have performance implications
   - Complex queries may require data duplication

### Mitigation Strategies

1. **Data Modeling**:
   - Design tables around query patterns
   - Use denormalization and duplicate data when necessary
   - Implement materialized views for alternative access patterns

2. **Consistency Management**:
   - Use LOCAL_QUORUM for writes to ensure durability
   - Use LOCAL_ONE for reads to optimize latency
   - Implement application-level retry logic for edge cases

3. **Monitoring and Operations**:
   - Deploy with Prometheus and Grafana for comprehensive monitoring
   - Implement automated backup solutions
   - Regular maintenance and compaction strategy optimization

4. **Cost Optimization**:
   - Start with right-sized cluster based on calculations
   - Implement auto-scaling based on load patterns
   - Optimize instance types for read/write patterns
