<!--
Sync Impact Report

- Version change: 1.1.0 → 1.1.1
- Modified principles:
	- Development Environment (Mandatory Dev Container, Monorepo Support)
- Added sections: none
- Removed sections: none
- Templates requiring updates:
	- ⚠ .specify/templates/plan-template.md
	- ⚠ .specify/templates/tasks-template.md
- Deferred items: none
-->

# Tuvi API Constitution (Azure Functions Backend for Mobile & SPA)

## Core Principles

### I. Clean Architecture Boundaries
- Functions are **thin HTTP triggers only**: parse/validate input, call services, format response.
- Business logic lives in `Services/` and is framework-agnostic (no Azure Functions types).
- Shared request/response DTOs live in `Models/` and are used by both triggers and services.
- Repository structure is standardized:
	- `Functions/`: HTTP triggers only
	- `Services/`: orchestration and business logic (including AI orchestration)
	- `Models/`: DTOs and contracts

Rationale: predictable separation enables testing, security review, and future trigger types.

### II. Zero-Trust Secrets & Managed Identity
- No API keys, secrets, or connection strings are ever hardcoded or checked into source control.
- Azure resource access uses `DefaultAzureCredential` by default.
	- Production uses **system-assigned Managed Identity**.
	- Local development may use `local.settings.json` and developer identity, but the code path remains
		credential-based (no “dev-only” auth implementations).
- Any required secrets belong in Azure Key Vault and are accessed via Managed Identity.

Rationale: least-privilege and secret hygiene are non-negotiable for public-facing APIs.

### III. Stable API Contract (Envelope + Problem Details)
- All successful responses return a JSON envelope:

	```json
	{
		"data": { },
		"success": true,
		"error": null
	}
	```

- All error responses return RFC 7807 **Problem Details** with appropriate HTTP status codes
	(e.g., 400 for validation errors, 401/403 for auth failures, 500 for unexpected errors).
- JSON serialization uses `System.Text.Json` and **camelCase** property naming.
- All timestamps/dates are **UTC** in ISO 8601 format.

Rationale: mobile + SPA clients require consistent parsing and predictable error handling.

### IV. Async, DI-First, Stateless Functions
- All external I/O (OpenAI calls, Storage, Cosmos DB, Key Vault) MUST be `async`.
- All dependencies are registered in `Program.cs` and injected (including `OpenAIClient` or
	Microsoft.Extensions.AI abstractions).
- Functions are stateless. If persistence is required, use Azure Storage or Cosmos DB.
- CORS must allow only specific origins for the SPA; never `*` in production.
- Authorization defaults to **Function** level auth for HTTP triggers, unless explicitly deployed
	behind APIM / an auth gateway where `Anonymous` is acceptable.
- OpenAPI (Swagger) must be enabled via `Microsoft.Azure.Functions.Worker.Extensions.OpenApi`.

Rationale: DI + statelessness improves scalability; async prevents thread starvation.

### V. AI Safety, Observability, and Cost Discipline
- AI calls MUST enforce reasonable timeouts and cancellation.
- Token usage (prompt/completion/total) MUST be logged for cost tracking.
- Logs must be structured and avoid leaking secrets or sensitive user content.
- Errors must be captured with enough context to debug without logging confidential payloads.

Rationale: AI is both a reliability and cost risk; observability is required to operate safely.

## Technology Stack
- Azure Functions v4, **.NET 8 Isolated Worker**
- Language: C# (latest stable language features)
- AI integration: `Azure.AI.OpenAI` and/or `Microsoft.Extensions.AI`
- Identity/auth to Azure resources: `DefaultAzureCredential` + system-assigned Managed Identity
- OpenAPI: `Microsoft.Azure.Functions.Worker.Extensions.OpenApi`
- JSON transport: `System.Text.Json`
- Testing: xUnit + Moq

## Development Environment
1. **Mandatory Dev Container**: All development, including for the Azure Function API and the frontend React SPA, MUST be capable of running inside a single `.devcontainer` configuration.
2. **Base Image**: The Dev Container MUST use the `.NET 8` base image.
3. **Features**: The Dev Container MUST include `Node.js` (LTS) and `Azure CLI` as installed features.
4. **Monorepo Support**: The container MUST forward ports for both the API (7071) and Web (5173), and the Mobile bundler (8081).

## Workflow & Quality Gates
- Every new/changed service method that contains business logic MUST have unit tests.
- Integration tests focus on the AI service layer by mocking OpenAI responses.
- All HTTP triggers MUST:
	- validate inputs
	- return the standard success envelope on success
	- return Problem Details on errors
- PR review MUST explicitly verify compliance with the security non-negotiables and API contract.

## Governance
- This constitution supersedes other conventions in this repository.
- Amendments:
	- Must be proposed via PR updating this file.
	- Must include any necessary template updates under `.specify/templates/`.
	- Must include a short migration note if behavior or contracts change.
- Versioning follows semantic versioning:
	- MAJOR: breaking governance or API contract changes
	- MINOR: new principle/section or materially expanded guidance
	- PATCH: clarifications, wording fixes, non-semantic refinements
- Compliance expectations:
	- Reviews treat “Security Non-Negotiables” and “API Contract” as blocking.
	- Violations require explicit justification and an approved amendment.

**Version**: 1.1.1 | **Ratified**: 2026-02-16 | **Last Amended**: 2026-02-17
