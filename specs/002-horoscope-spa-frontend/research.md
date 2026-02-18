# Phase 0 Research & Decisions

This document records the decisions made to resolve ambiguities and satisfy constitutional requirements identified in the implementation plan.

## 1. Frontend Testing Framework

- **Unknown**: The `spec.md` mandates unit test coverage but does not specify a testing framework.
- **Decision**: **Vitest** with **React Testing Library**.
- **Rationale**:
    - **Vite Integration**: Vitest is designed for Vite and offers a fast, seamless development experience with near-instant test re-runs.
    - **Modern**: It uses modern ES modules and has a Jest-compatible API, making it easy to learn for developers familiar with Jest.
    - **Industry Standard**: React Testing Library is the de-facto standard for testing React components in a way that resembles how users interact with them, leading to more robust and maintainable tests.
- **Alternatives Considered**:
    - **Jest**: While a very popular choice, it requires more complex configuration to work with Vite (`babel-jest`, etc.). Vitest provides a more native and efficient experience in this toolchain.

## 2. Client-Side Observability

- **Violation**: The project violates Constitution Principle V by lacking any client-side logging or error monitoring.
- **Decision**: Integrate **Azure Application Insights**.
- **Rationale**:
    - **Ecosystem Alignment**: The project backend is built on Azure Functions. Using Application Insights for the frontend allows for unified, end-to-end distributed tracing, from a user click in the SPA down to the AI call in the backend.
    - **Comprehensive Features**: It provides robust error tracking, performance monitoring (client-side metrics), and user behavior analytics.
    - **Managed Identity**: It aligns with the Zero-Trust secret management principle, as the instrumentation key can be managed securely.
- **Alternatives Considered**:
    - **Sentry**: An excellent and popular open-source error tracking tool. It's a strong alternative, but Application Insights offers tighter integration with the existing Azure backend stack.
    - **LogRocket**: Provides session replay, which is very powerful, but might be overkill for the initial version of this application. It could be considered in the future if more detailed UX analysis is needed.

## 3. Mandatory Development Environment

- **Violation**: The project is missing the mandatory `.devcontainer` as required by the constitution's "Development Environment" section.
- **Decision**: A `.devcontainer/devcontainer.json` file will be created at the repository root.
- **Rationale**: This is a mandatory requirement of the project constitution to ensure a consistent and reproducible development environment for all contributors. The container will encapsulate all necessary dependencies (.NET SDK, Node.js, Azure CLI).
- **Implementation Details**:
    - **Base Image**: `mcr.microsoft.com/devcontainers/dotnet:8.0`
    - **Features**:
        - `ghcr.io/devcontainers/features/node:1`: To install Node.js (LTS).
        - `ghcr.io/devcontainers/features/azure-cli:1`: To install the Azure CLI.
    - **Forwarded Ports**:
        - `7071`: For the Azure Functions API.
        - `5173`: For the Vite dev server (React SPA).
        - `8081`: Reserved for the React Native packager in the future.
    - **Post-Create Command**: `npm install -g pnpm` to make the chosen package manager available globally in the container.
- **Alternatives Considered**: None. This is a mandatory constitutional requirement.
