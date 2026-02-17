# Research & Decisions

This document records the technical decisions made to resolve points marked as "NEEDS CLARIFICATION" in the implementation plan, ensuring alignment with the project constitution.

## Decision 1: Caching Storage

- **Decision**: **Azure Table Storage** will be used for caching generated horoscope interpretations (FR-014).
- **Rationale**: Azure Table Storage is a low-cost, serverless, key-value data store that is highly suitable for the API's caching requirements. It integrates seamlessly with the Azure Functions ecosystem and supports authentication via Managed Identity, aligning with Principle II of the constitution. Its performance is more than adequate for this feature's p90 < 5000ms goal, while being more cost-effective than premium alternatives. The cache key will be the deterministic hash specified in FR-014.
- **Alternatives Considered**:
    - **Azure Blob Storage**: Rejected because it is optimized for unstructured blobs of data (files, images), not for structured, queryable cache entries.
    - **Azure Cosmos DB**: Rejected as it is a premium, multi-model database designed for high-throughput, low-latency scenarios that are beyond the scope and budget of this feature's simple caching needs.
    - **Azure Cache for Redis**: Rejected due to higher cost and management overhead. While extremely fast, its performance benefits are not required to meet the specified 5-second p90 response time and it would introduce unnecessary complexity compared to the native Table Storage integration.

## Decision 2: AI Integration Model

- **Decision**: **Azure OpenAI Service** will be used for the AI interpretation layer (FR-010), accessed via the `Azure.AI.OpenAI` .NET SDK.
- **Rationale**: This decision is mandated by the project constitution (Technology Stack section and Principle V). Using the managed Azure OpenAI service ensures compliance with security requirements (Managed Identity via `DefaultAzureCredential`), observability (integrated logging and token tracking), and reliability (service-level agreements, timeouts, and cancellation). This approach avoids the significant operational burden, security risks, and architectural complexity of deploying, managing, and networking a separate, self-hosted AI model within a serverless architecture.
- **Alternatives Considered**:
    - **Self-hosted Open Source Model (e.g., Llama 3)**: This option was proposed during the clarification phase. It has been **rejected** because it directly conflicts with the project constitution, which mandates the use of the `Azure.AI.OpenAI` stack. The self-hosted approach would require a separate containerized service, complex networking, and custom-built solutions for security and observability, violating the principles of a lean, serverless-first architecture. By adhering to the constitution, we leverage a managed, secure, and fully-integrated solution.
