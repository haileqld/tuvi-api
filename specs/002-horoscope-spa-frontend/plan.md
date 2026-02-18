# Implementation Plan: Tuvi Horoscope API & SPA

**Branch**: `002-horoscope-spa-frontend` | **Date**: 2026-02-18 | **Spec**: [Combined]
**Input**: Merged from plans for `001` and `002`. Updated with a unified constitution and corrected project structure.

## Summary

This master plan outlines the implementation for the complete Tuvi Horoscope system, comprising a C#/.NET 8 backend and a React/TypeScript frontend.

The **backend** will be an Azure Functions API located at `src/api`. It will accept birth details, generate a horoscope chart, use an AI service for interpretation, and return a structured result.

The **frontend** will be a React SPA located at `src/web`. Its primary architectural driver is future code sharing with a mobile app, achieved by isolating all shared, platform-agnostic logic (state, API services, types) into a dedicated `src/shared` directory.

The entire system will be developed within a unified monorepo, orchestrated via the Azure Developer CLI (`azd`), and governed by the single, comprehensive constitution defined within this document.

## Technical Context

| Area | Backend (API) | Frontend (SPA) | Shared |
|---|---|---|---|
| **Language** | C# (.NET 8) | TypeScript, React 18+ | TypeScript |
| **Framework** | Azure Functions v4 | Vite, MUI | Redux Toolkit |
| **Testing** | xUnit, Moq | Vitest, RTL | Vitest |
| **Orchestration** | \multicolumn{3}{c|}{Azure Developer CLI (`azd`)} |
| **Environment** | \multicolumn{3}{c|}{Single Dev Container} |

## Project Structure

The project will be organized in a unified monorepo structure with a clear separation between backend, frontend, and shared code.

```text
/
├── .devcontainer/
│   └── devcontainer.json
├── src/
│   ├── api/               # BACKEND: C# Azure Functions Project
│   │   ├── Functions/
│   │   ├── Services/
│   │   └── TuviApi.csproj
│   ├── web/                 # FRONTEND: React SPA
│   │   ├── src/
│   │   │   └── main.tsx
│   │   └── package.json
│   └── shared/              # SHARED: Platform-agnostic TS logic
│       ├── src/
│       │   ├── state/
│       │   ├── services/
│       │   └── types/
│       └── package.json
├── tests/
│   └── api/               # BACKEND: xUnit tests
│       └── TuviApi.Tests.csproj
├── azure.yaml
└── pnpm-workspace.yaml
```

**Structure Decision**: This `src/api`, `src/web`, and `src/shared` structure provides the cleanest separation of concerns, directly enabling the code-sharing requirement while maintaining a standard monorepo layout compatible with tools like `azd`.

---

## Project Constitution

### I. Core Architectural Principles

1.  **Backend: Clean Architecture.** The API backend code MUST maintain a strict separation of concerns. `Functions/` are for thin HTTP triggers only. All business logic, orchestration, and data access MUST reside in `Services/`. All DTOs and contracts MUST be in `Models/`.
2.  **Frontend: Decoupled Architecture.** The React frontend code MUST be structured for reusability. All platform-agnostic, shareable logic (Redux state, API service calls, type definitions) MUST reside in `src/shared`. UI components in `src/web` should be primarily "presentational," receiving data and callbacks as props from higher-level container components that interact with the shared logic.
3.  **Security: Zero-Trust Secrets.** No API keys, secrets, or connection strings may be hardcoded. Backend access to Azure resources MUST use `DefaultAzureCredential` (and Managed Identity in production). Secrets MUST be stored in Azure Key Vault.
4.  **API: Stable Contract.** All successful API responses MUST return a `{ "data": ... }` envelope. All errors MUST return RFC 7807 Problem Details. JSON serialization MUST be camelCase.
5.  **Observability: Unified & Disciplined.** Both backend and frontend MUST be instrumented for observability (e.g., via Azure Application Insights). Backend AI calls MUST log token usage for cost tracking. Logs MUST be structured and MUST NOT leak sensitive user data.

### II. Technology Stack

-   **Backend**: C# on .NET 8 (Isolated Worker), Azure Functions v4
-   **Frontend**: TypeScript on React 18+
-   **Shared Logic**: TypeScript, Redux Toolkit
-   **Tooling**: Vite (frontend), xUnit/Moq (backend testing), Vitest/RTL (frontend testing)
-   **Azure Integration**: `Azure.AI.OpenAI`, `DefaultAzureCredential`, Azure Developer CLI (`azd`)

### III. Development Environment

1.  **Mandatory Dev Container**: All development MUST be performed within the single, unified `.devcontainer` configuration at the root of the repository.
2.  **Configuration**: The Dev Container will use the `.NET 8` base image and include `Node.js` (LTS), `Azure CLI`, and `azd` as features.
3.  **Ports**: The container will forward ports `7071` (API) and `5173` (Web).

### IV. Workflow & Quality Gates

-   All new business logic (backend or frontend) MUST be accompanied by unit tests.
-   PR reviews MUST explicitly verify compliance with the Core Architectural Principles.
-   Code MUST adhere to existing style and formatting conventions.

### V. Governance

-   This constitution is the single source of truth for all architectural and structural decisions in this repository.
-   Amendments must be proposed via PR.
-   Versioning follows SemVer (MAJOR for breaking changes, MINOR for new principles, PATCH for clarifications).
